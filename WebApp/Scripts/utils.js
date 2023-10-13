
function getDateTimeFormat(datetime) {
    if (datetime.length > 0)
        return moment(datetime).format("DD/MM/YYYY hh:mm:ss A");
    return '';
}

function refreshDropDownItems(id, items, selectedId, first, second = "", style = true, valueField = 'Id', displaySelect = true, isChosen = false) {
    $(`#${id} option`).remove();
    if (displaySelect) {
        $(`#${id}`).append(new Option("Please select", ""));
    }
    items.forEach(function (obj) {
        var text = obj[first];
        if (second && obj[second])
            text += ` - ${obj[second]}`;
        $(`#${id}`).append(new Option(text, obj[valueField], false, obj[valueField] == selectedId));
    });
    if (style) {
        $(`#${id}`).selectpicker('refresh');
    }
    if (isChosen) {
        $(`#${id}`).trigger('chosen:updated');
    }
}

function disableFields() {
    $("input:not('.prevent-disable'):not('[name=__RequestVerificationToken]'):not('[type=hidden]')").prop("disabled", "disabled");
    $("select:not('.prevent-disable')").prop("disabled", true);
    $("button:not('.prevent-disable'):not('.dropdown-toggle')").prop("disabled", true);

    $('.selectpicker').selectpicker('refresh');
}

function openDatePicker(id) {
    id = '#' + id;
    if (typeof $(id).data("datepicker") === 'undefined') {
        $(id).datepicker({
            format: DateFormatDatePickerJs,
            autoclose: true,
        });
    }

    $(id).focus();
}

function openQuarterPicker(id) {
    id = '#' + id;
    if (typeof DateQuarterFormatJs === 'undefined')
        DateQuarterFormatJs = 'yyyy MM';

    if (typeof $(id).data("datepicker") === 'undefined') {
        $(id).datepicker({
            format: DateQuarterFormatJs,
            minViewMode: 1,
            autoclose: true,
            language: "qtrs",
            forceParse: false
        }).on('show', function (e) {
            $('.datepicker').addClass('quarterpicker');
        });
    }

    $(id).focus();
}

function dateOffAutoComplete() {
    $("input[name*='Date']").attr('autocomplete', 'off');
    $("input[name*='Quarter']").attr('autocomplete', 'off');
}

function focusOnElement(id) {
    $('#' + id).focus();
}

function dateToQuarter(date) {
    date = new Date(date);

    var year = date.getUTCFullYear();
    var month = date.getUTCMonth() + 1;

    var quarter = 0;
    if (month <= 3) {
        quarter = 1;
    } else if (month <= 6) {
        quarter = 2;
    } else if (month <= 9) {
        quarter = 3;
    } else if (month <= 12) {
        quarter = 4;
    }

    return year + ' Q' + quarter;
}

function arrayToUnorderedList(list) {
    var content = "<ul>";
    for (i = 0; i < list.length; i++) {
        content += "<li>" + list[i] + "</li>";
    }
    content += "</ul>";

    return content;
}

function initializeTokenField(id, options, countName, isVue = false, listName = '', validate = true) {
    var tokenFieldId = id + '-tokenfield';
    $(id)
        .on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            $.each(existingTokens, function (index, token) {
                if (window[countName] != 0) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                }
            });
        })
        .on('tokenfield:createdtoken', function (e) {
            if (validate && !options.includes(e.attrs.value)) {
                $(e.relatedTarget).addClass('invalid');
            }
            window[countName] += 1;
            $(tokenFieldId).removeAttr('placeholder');

            if (isVue) {
                var idDetails = id.substring(1).split('_');
                if (idDetails.length == 2) {
                    var fieldName = idDetails[0];
                    var index = idDetails[1];
                    if ($(this).tokenfield('getTokens').map(e => e.value).length != 0)
                        app[listName][index][fieldName] = $(this).tokenfield('getTokens').map(e => e.value).join(",");
                }
            }
        })
        .on('tokenfield:edittoken', function (e) {
            if (options.includes(e.attrs.value)) {
                e.preventDefault();
            }
        })
        .on('tokenfield:removedtoken', function (e) {
            window[countName] -= 1;
            if (window[countName] == 0) {
                $(tokenFieldId).attr("placeholder", "Type here");
            }

            if (isVue) {
                var idDetails = id.substring(1).split('_');
                if (idDetails.length == 2) {
                    var fieldName = idDetails[0];
                    var index = idDetails[1];
                    app[listName][index][fieldName] = $(this).tokenfield('getTokens').map(e => e.value).join(",");
                }
            }
        })
        .tokenfield({
            autocomplete: {
                source: options,
                delay: 100
            },
            showAutocompleteOnFocus: true
        });
}

function arrayToUnorderedList(list) {
    var content = "<ul>";
    for (i = 0; i < list.length; i++) {
        content += "<li>" + list[i] + "</li>";
    }
    content += "</ul>";

    return content;
}

var tableFilters = [];

function filterTable(vueListName, module, codeListName = null) {
    if (app[vueListName].length == 0)
        return;

    // Declare variables
    var input, table, tr, i;
    table = document.getElementById(module + "List");
    tr = table.getElementsByTagName("tr");

    var filterRowId = '#' + module + 'FilterRow';
    tableFilters = [];
    var filters = $(filterRowId)[0].children;
    for (var i = 0; i < filters.length; i++) {
        var value = null;
        var selector = filterRowId + ' th:nth-child(' + (i + 1) + ')';

        var input = $(selector).find('input');
        var select = $(selector).find('select');

        if (input.length == 1) {
            value = input.val().toLowerCase();
        } else if (select.length == 1) {
            value = select.val().toLowerCase();
        }

        tableFilters.push(value);
    }

    // Loop through all table rows, and hide those who don't match the search query
    isAllHidden = true;
    for (i = 3; i < tr.length; i++) {
        if (isDisplayRecord(tr[i].getElementsByTagName("td"), codeListName)) {
            tr[i].style.display = "";
            isAllHidden = false;
        } else {
            tr[i].style.display = "none";
        }
    }

    var noDataId = '#' + module + 'NoData';
    if (isAllHidden) {
        $(noDataId).show();
    } else {
        $(noDataId).hide();
    }
}

function isDisplayRecord(cols, codeListName) {
    var codeList = codeListName != null ? window[codeListName] : null;
    for (var i = 0; i < tableFilters.length; i++) {
        var filter = tableFilters[i];

        var col = cols[i];
        var content = (col.textContent || col.innerText).toLowerCase();
        if (i == 0 && codeList != null && !codeList.includes(content)) 
            return false;

        if (filter == null || filter == '' || filter == 'null')
            continue;

        if (content.indexOf(filter) == -1)
            return false;
    }
    return true;
}

function clearTableFilters(vueListName, module, codeListName = null) {
    var condition = "id^='" + module + "Filter-'";
    $("input[" + condition + "]").val("");
    $("select[" + condition + "]").val("null").selectpicker('refresh');
    filterTable(vueListName, module, codeListName);
}
