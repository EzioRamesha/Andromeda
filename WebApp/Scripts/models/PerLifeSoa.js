
var attachHandlers = function () {
    $('#PaidClaimsPager a, #PaidClaimsAct a').click(function () {
        $('#paidClaims').load(this.href, function () {
            attachHandlers();
        });
        return false;
    });
};

var attachHandlers2 = function () {
    $('#PendingClaimsPager a, #PendingClaimsAct a').click(function () {
        $('#pendingClaims').load(this.href, function () {
            attachHandlers2();
        });
        return false;
    });
};

var attachHandlers3 = function () {
    $('#ClaimsRemovedPager a, #ClaimsRemovedAct a').click(function () {
        $('#claimsRemoved').load(this.href, function () {
            attachHandlers3();
        });
        return false;
    });
};

var attachHandlers4 = function () {
    $('#RetroOutputPager a, #RetroOutputAct a').click(function () {
        $('#retroOutput').load(this.href, function () {
            attachHandlers4();
        });
        return false;
    });
};

var attachHandlers5 = function () {
    $('#InforcePolicyPager a, #InforcePolicyAct a').click(function () {
        $('#inforceListing').load(this.href, function () {
            attachHandlers5();
        });
        return false;
    });
};

$(document).ready(function () {
    dateOffAutoComplete();

    $('#Quarter').datepicker({
        format: QuarterFormat,
        minViewMode: 1,
        autoclose: true,
        language: "qtrs",
        forceParse: false,
    })
    .on('show', function (e) {
        var datepickerDropDown = $('.datepicker');
        datepickerDropDown.addClass('quarterpicker');
    })
    .on('changeDate', function () {
        Model.Quarter = $('#Quarter').val();
    });

    $("#RiskYear").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    })
    .on('changeDate', function () {
        app.PremiumSearch.RiskYear = $('#RiskYear').val();
    });

    if (Model.RetroPartyId == 0) {
        $("#RetroPartyId").prop("selectedIndex", 0);
        $('#RetroPartyId').selectpicker('refresh');
    }


    // Search Claims
    $('#CedantDateOfNotification, #InsuredDob, #ReinsEffDatePol, #DateOfEvent').datepicker({
        format: DateFormatDatePickerJs,
        autoclose: true,
    });

    $('#RetroSoaQuarter').datepicker({
        format: QuarterFormat,
        minViewMode: 1,
        autoclose: true,
        language: "qtrs",
        forceParse: false,
    })
    .on('show', function (e) {
        var datepickerDropDown = $('.datepicker');
        datepickerDropDown.addClass('quarterpicker');
    });


    var retroPartyId = $('#RetroPartyId').val();
    getRetroTreatyByRetroParty(retroPartyId);

    attachHandlers();
    attachHandlers2();
    attachHandlers3();
    attachHandlers4();
    attachHandlers5();

    //$("#PaidClaimsForm").submit(function () {
    //    console.log(2)
    //    console.log(this.action)
    //    console.log(this.method)
    //    console.log(this.serialize())

    //    //$("#section1-saving").html("<img src='../../Images/ajax-loader.gif' />");

    //    //$.ajax({
    //    //    url: this.action,
    //    //    type: this.method,
    //    //    data: $(this).serialize(),
    //    //    success: function (result) {
    //    //        $("#section1-saving").html("Saved!");
    //    //        $.ajaxSettings.cache = false;

    //    //        // Refresh the sub section 2 on the Section 2 tab
    //    //        $("#subSection2").load('../../Projects/subSection2/' + $("#ProjectID").val());
    //    //    },
    //    //    error: function (jqXHR, textStatus, errorThrown) {
    //    //        $("#section1-saving").html("Error: " + textStatus + " " + errorThrown);
    //    //    }
    //    //});
    //    return false;
    //});

    $("#PaidClaimsForm").submit(function () {
        if ($(this).valid()) {
            $('#paidClaims').load(this.action + '?' + $(this).serialize());
            $("#paidClaims #PaidClaimsTable .selectpicker").selectpicker('refresh');
        }
        return false;
    });

    $("#PendingClaimsForm").submit(function () {
        if ($(this).valid()) {
            $('#pendingClaims').load(this.action + '?' + $(this).serialize());
            $("#pendingClaims #PendingClaimsTable .selectpicker").selectpicker('refresh');
        }
        return false;
    });

    $("#ClaimsRemovedForm").submit(function () {
        if ($(this).valid()) {
            $('#claimsRemoved').load(this.action + '?' + $(this).serialize());
            $("#claimsRemoved #ClaimsRemovedTable .selectpicker").selectpicker('refresh');
        }
        return false;
    });
})

//function searchClaims(claimCategory) {
//    console.log(claimCategory)
//    console.log(11)
//    $("#" + claimCategory + "Form").submit(function () {
//        console.log(1)
//        if ($(this).valid()) {
//            $.ajax({
//                url: this.action,
//                type: this.method,
//                data: $(this).serialize(),
//                //beforeSend: function () {

//                //},
//                //complete: function () {

//                //},
//                success: function (result) {
//                    console.log(result)
//                    $("#" + claimCategory).html(result);
//                    $("#" + claimCategory + " #" + claimCategory + "Table .selectpicker").selectpicker('refresh');
//                }
//            });
//        }
//        return false;
//    });
//}

function startDownload(download) {
    loadingDiv.removeClass('hide-loading-spinner');
    var downloadToken = (new Date()).getTime();

    if (download.href.includes("downloadToken=")) {
        download.href = download.href.replace(/(downloadToken=).*?/, '$1' + downloadToken + '$2')
    } else {
        download.href += "&downloadToken=" + downloadToken;
    }

    var cookiePattern = new RegExp(("downloadToken=" + downloadToken), "i");
    var cookieTimer = setInterval(checkCookies, 500);
    var refreshSession = setInterval(
        function () {
            $.ajax({
                url: RefreshUrl,
                type: "POST",
                cache: false,
                async: false,
                success: function (data) {
                    if (data.logout == true) {
                        window.location.href = LoginUrl;
                    }
                },
            });
        }, 60 * 1000
    );

    function checkCookies() {
        if (document.cookie.search(cookiePattern) >= 0) {
            loadingDiv.addClass('hide-loading-spinner');
            clearInterval(cookieTimer);
            clearInterval(refreshSession);
        }
    }
}

function getRetroTreatyByRetroParty(retroPartyId) {

    if ((retroPartyId !== null) && (retroPartyId !== '') && ($.isNumeric(retroPartyId))) {
        $.ajax({
            url: GetRetroTreatyUrl,
            type: "POST",
            data: { retroPartyId: retroPartyId },
            cache: false,
            async: false,
            success: function (data) {
                var retroTreatyList = data.retroTreatyBos;
                refreshDropDownItems('RetroTreatyId', retroTreatyList, Model.RetroTreatyId, 'Code');
            }
        });
    }
    else {
        refreshDropDownItems('RetroTreatyId', [], null, 'Code');
    }
}

function validateQuarter(qtr, name) {
    if (qtr == null || qtr == "")
        return;
    if (!/[0-9]{4} Q{1}([1-4]){1}$/.test(qtr)) {
        $("#SoaQuarter").val(null);
    }
}

function focusOnDate(val) {
    $('#' + val).focus();
}

function updateStatus(status) {
    $('#Status').val(status);
    $('#editform').submit();
}

var app = new Vue({
    el: '#app',
    data: {
        PerLifeSoaModel: Model,
        // Soa WMOM
        SoaWMOMs: SoaWMOMs ? SoaWMOMs : [],
        SoaWM: WMWithin,
        SoaOM: OMOutside,
        // Soa
        SoaSummaries: SoaSummaries ? SoaSummaries : [],
        // Retro Summary
        RetroStatements: RetroStatements ? RetroStatements : [],
        // Premium (Summary By Treaty)
        RetroPremiums: SoaSummaryByTreaties ? SoaSummaryByTreaties : [],
        DropDownTreatyCodes: DropDownTreatyCodes,
        PremiumSearch: {
            TreatyCode: null,
            RiskYear: null,
        },
        // Remark        
        Remarks: RemarksList ? RemarksList : [],
        RemarkMaxIndex: 0,
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: StatusPending ? StatusPending : null,
            CreatedAtStr: null,
            RemarkIsPrivate: false,
            Content: null,
            DocumentIsPrivate: false,
            HasFollowUp: false,
            FollowUpAtStr: null,
            FollowUpUserId: null,
        },
        // Document
        Documents: DocumentBos ? DocumentBos : [],
        DocumentTypeItems: DocumentTypeItems,
        DocumentMaxIndex: 0,
        DocumentModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            File: null,
            Type: 0,
            Description: '',
        },
        // Status History
        StatusHistories: StatusHistoriesList ? StatusHistoriesList : [],
        // Auto Expand TextArea
        textAreaWidth: 150,
        textAreaHeight: 21,
    },
    methods: {
        // Soa WMOM
        soaItem: function (rowLabel, object, type) {
            if (this.SoaWMOMs.length) {
                var item = this.SoaWMOMs.filter(function (soa) { return soa.RowLabel === rowLabel && soa.WMOM === parseInt(type) });
                if (item.length) {
                    var objectStr = object + "Str";
                    return item[0][objectStr];
                }
                else return '0.00';
            }            
            else return '0.00';
        },
        calculateNetTotal: function (object, type) {
            var premiumNet = 0;
            var claimNet = 0;

            if (this.SoaWMOMs.length) {
                var premium = this.SoaWMOMs.filter(function (soa) { return soa.RowLabel === 'Total Premium' && soa.WMOM === parseInt(type) });
                var claim = this.SoaWMOMs.filter(function (soa) { return soa.RowLabel === 'Total Claim' && soa.WMOM === parseInt(type) });
                if (premium.length) premiumNet = premium[0][object];
                if (claim.length) claimNet = claim[0][object];

                return this.setAmountFormat(premiumNet - claimNet);
            }
            else return '0.00';
        },
        calculateTotalSum: function (rowLabel, object) {
            if (this.SoaWMOMs.length) {
                var item = this.SoaWMOMs.filter(function (soa) { return soa.RowLabel === rowLabel });
                if (item.length)
                    return this.setAmountFormat(item.reduce((a, b) => a + b[object], 0));
                else return '0.00';
            }
            else return '0.00';
        },
        calculateTotalPayable: function (object) {
            var premiumTotal = 0;
            var claimTotal = 0;

            if (this.SoaWMOMs.length) {
                var premium = this.SoaWMOMs.filter(function (soa) { return soa.RowLabel === 'Total Premium' });
                var claim = this.SoaWMOMs.filter(function (soa) { return soa.RowLabel === 'Total Claim' });
                if (premium.length) premiumTotal = premium.reduce((a, b) => a + b[object], 0);
                if (claim.length) claimTotal = claim.reduce((a, b) => a + b[object], 0);

                return this.setAmountFormat(premiumTotal - claimTotal);
            }
            else return '0.00';
        },
        // Soa
        soaSummariesBy: function (type) {
            if (this.SoaSummaries != null) {
                return this.SoaSummaries.filter(function (summary) {
                    return summary.PremiumClaim == type;
                })
            }
            else {
                return this.SoaSummaries;
            }
        },
        calculateTotal: function (type, object) {
            if (this.SoaSummaries.length) {
                var item = this.SoaSummaries.filter(function (soa) { return soa.PremiumClaim === type });                
                if (item.length)
                    return this.setAmountFormat(item.reduce((a, b) => a + b[object], 0));
                else return '0.00';
            }
            else return '0.00';
        },
        calculateTotalDue: function () {
            var premiumTotal = 0;
            var claimTotal = 0;
            var profitCommTotal = 0;

            if (this.SoaSummaries.length) {
                var premium = this.SoaSummaries.filter(function (soa) { return soa.PremiumClaim === 1 });
                var claim = this.SoaSummaries.filter(function (soa) { return soa.PremiumClaim === 2 });
                var profitComm = this.SoaSummaries.filter(function (soa) { return soa.PremiumClaim === 3 });
                if (premium.length) premiumTotal = premium.reduce((a, b) => a + b['Movement'], 0);
                if (claim.length) claimTotal = claim.reduce((a, b) => a + b['Total'], 0);
                if (profitComm.length) profitCommTotal = profitComm.reduce((a, b) => a + b['Individual'], 0);

                return this.setAmountFormat(premiumTotal - claimTotal - profitCommTotal);
            }
            else return '0.00';
        },
        // Retro Summary
        viewRetroStatementLink: function (index) {
            var item = this.RetroStatements[index];
            if (item != null && item.Id != '') {
                var url = ViewRetroStatementUrl + item.Id;
                return url
            }
        },
        downalodRetroStatementLink: function (index) {
            var item = this.RetroStatements[index];
            if (item != null && item.Id != '') {
                var url = DownloadRetroStatementUrl + item.Id;
                return url
            }
        },
         // Premium (Summary By Treaty)
        RetroPremiumsByMovement: function () {
            if (this.RetroPremiums != null) {
                return this.RetroPremiums.filter(function (premium) {
                    return premium.MovementTotalRetroAmount != 0 || premium.MovementTotalGrossPremium != 0
                        && premium.MovementTotalNetPremium != 0 || premium.MovementTotalDiscount != 0
                        && premium.MovementTotalPolicyCount != 0;
                });
            }
            else {
                this.RetroPremiums = [];
            }
        },
        searchPremium: function () {
            var obj = {
                id: this.PerLifeSoaModel.Id,
                treatyCode: this.PremiumSearch.TreatyCode,
                riskYear: this.PremiumSearch.RiskYear,
            }

            var retroPremiumBos;
            $.ajax({
                url: SearchSummaryByTreatyUrl ? SearchSummaryByTreatyUrl : null,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    console.log(data)
                    retroPremiumBos = data.PremiumSummaryByTreatyBos;
                }
            });

            this.RetroPremiums = retroPremiumBos;
        },
        // Remark
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.PerLifeSoaModel.Status == 0 ? StatusPending : this.PerLifeSoaModel.Status;
            this.RemarkModal.ModuleId = this.PerLifeSoaModel.ModuleId;
            this.RemarkModal.ObjectId = this.PerLifeSoaModel.Id;
            this.RemarkModal.RemarkIsPrivate = false;
            this.RemarkModal.Content = null;
            this.RemarkModal.DocumentIsPrivate = false;
            this.RemarkModal.HasFollowUp = false;
            this.clearFollowUpDetails();

            clearSelectedFiles('remark');
        },
        clearFollowUpDetails: function (check) {
            this.RemarkModal.FollowUpAtStr = null;
            this.RemarkModal.FollowUpUserId = null;

            $('#FollowUpUserId').selectpicker("refresh");

            if ($('#hasFollowUp')[0].checked)
                $('#FollowUpUserId').prop("disabled", false);
            else {
                $('#FollowUpUserId').prop("disabled", true);
            }

            $('#FollowUpUserId').selectpicker("refresh");
        },
        addRemark: function () {
            var remark = null;
            var remarkModal = this.RemarkModal;
            remarkModal.IsPrivate = this.RemarkModal.RemarkIsPrivate;
            if (this.RemarkModal.HasFollowUp) {
                remarkModal.RemarkFollowUpBo = {
                    FollowUpAtStr: this.RemarkModal.FollowUpAtStr,
                    FollowUpUserId: this.RemarkModal.FollowUpUserId,
                    Status: this.RemarkModal.HasFollowUp ? FollowUpStatusPending : FollowUpStatusCompleted,
                    StatusName: this.RemarkModal.HasFollowUp ? FollowUpStatusPendingName : FollowUpStatusCompletedName
                };
            }

            if (!this.RetroRegisterBatch.Id) {
                remark = remarkModal;
                remark.StatusName = StatusPendingName;
                remark.PermissionName = this.RemarkModal.RemarkIsPrivate ? "Private" : "Public";

                remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex);
            } else {
                var remark = createRemark(this.RemarkModal);
                remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex, remark.Id);
            }

            if (remark) {
                this.Remarks.unshift(Object.assign({}, remark));
                this.RemarkMaxIndex++;
            }
        },
        completeFollowUp: function (index) {
            if (this.Remarks[index].RemarkFollowUpBo == null)
                return;

            this.Remarks[index].RemarkFollowUpBo.Status = FollowUpStatusCompleted;
            this.Remarks[index].RemarkFollowUpBo.StatusName = FollowUpStatusCompletedName;
            if (this.ReferralClaim.Id) {
                completeFollowUp(this.Remarks[index].RemarkFollowUpBo.Id);
            }
        },
        // Documents
        saveDocuments(remarkIndex, remarkId = null) {
            var files = getFiles();

            if (!files) return;

            var parentId = this.PerLifeSoaModel.Id;
            var document = {
                ModuleId: this.PerLifeSoaModel.ModuleId,
                ObjectId: parentId,
                RemarkId: remarkId,
                RemarkIndex: remarkIndex,
                IsPrivate: this.RemarkModal.DocumentIsPrivate,
                CreatedByName: AuthUserName,
                CreatedAtStr: this.RemarkModal.CreatedAtStr,
            };

            var save = true;
            if (!parentId)
                save = false;

            var documents = [];

            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var document = saveDocument(document, file, save);

                if (document != null) {
                    document.Index = this.DocumentMaxIndex;

                    this.Documents.push(Object.assign({}, document));
                    documents.push(Object.assign({}, document));

                    this.DocumentMaxIndex++;
                }
            }

            return documents;
        },
        removeDocument(index) {
            var document = this.Documents[index];

            if (deleteDocument(document)) {
                if (document.RemarkId) {
                    var remark = this.Remarks.find(r => { return r.Id == document.RemarkId });
                    remark.DocumentBos = remark.DocumentBos.filter(d => { return d.Id != document.Id });
                } else {
                    for (var i = 0; i < this.Remarks.length; i++) {
                        var remark = this.Remarks[i];
                        if (remark.DocumentBos.filter(d => { return d.HashFileName == document.HashFileName }).length > 0) {
                            remark.DocumentBos = remark.DocumentBos.filter(d => { return d.HashFileName != document.HashFileName });
                            break;
                        }
                    }
                }

                this.Documents.splice(index, 1);
                this.DocumentMaxIndex--;
            }
        },
        // Auto Expand TextArea
        autoExpandTextarea: function (id) {
            var tArea = $('#' + id);
            this.textAreaWidth = tArea.width();
            this.textAreaHeight = tArea.height();
            tArea.autoResize();
            tArea.trigger('keyup');

            tArea.on('keypress', function (evt) {
                var evt = (evt) ? evt : ((event) ? event : null);
                if (evt.keyCode == 13)
                    return false;
            });
        },
        autoCollapseTextarea: function (id) {
            var tArea = $('#' + id);
            tArea.height(this.textAreaHeight);
        },
        openQuarterPicker: function (currentId) {
            var id = "#" + currentId;
            if ($(id).data("datepicker") != null) {
                $(id).datepicker("destroy");
            }

            $(id).datepicker({
                format: QuarterDateFormat,
                minViewMode: 1,
                autoclose: true,
                language: "qtrs",
                forceParse: false
            }).on('show', function (e) {
                var datepickerDropDown = $('.datepicker');
                datepickerDropDown.addClass('quarterpicker');
            });

            var updateQuarterValue = this.updateQuarterValue;
            $(id).on('changeDate', function () {
                updateQuarterValue(currentId);
            });

            $(id).focus();
        },
        openDatePicker: function (field) {
            var config = {
                format: DateFormatDatePickerJs,
                autoclose: true,
            };
            var id = '#' + field;

            if (id.includes("Quarter")) {
                var config = {
                    format: QuarterDateFormat,
                    minViewMode: 1,
                    autoclose: true,
                    language: "qtrs",
                    forceParse: false
                };
            }

            if (typeof $(id).data("datepicker") === 'undefined') {
                $(id).datepicker(config);

                if (id.includes("Quarter")) {
                    $(id).on('show', function (e) {
                        $('.datepicker').addClass('quarterpicker');
                    });
                }

                var setMatchValue = this.setMatchValue;
                $(id).on('changeDate', function () {
                    setMatchValue(field, $(id).val());
                });
            }

            $(id).focus();
        },
        setMatchValue: function (field, value) {
            var model = "RetroRegisterBatch";
            var fieldName = field;
            if (field.startsWith("RemarkModal")) {
                fieldName = field.replace('RemarkModal', '');

                model = "RemarkModal";
            }

            this[model][fieldName] = value;
        },
        updateQuarterValue: function (currentId) {
            var id = "#" + currentId;
            var value = $(id).val();

            this.newRetroData[currentId] = value;
        },
        setDateFormat: function (date) {
            if (date != '')
                return moment(date).format('DD MMM YYYY');
            return '';
        },
        setAmountFormat: function (amount) {
            if (amount == '' || amount == null)
                amount = 0;

            return amount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
        },
    },
    created: function () {
        //console.log(this.SoaWMOMs)
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});
