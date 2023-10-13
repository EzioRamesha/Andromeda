var selectedVersion = null;
var disableVersion = false;
var ignoreFields = [];
$(document).ready(function () {
    $('#CurrentVersion').val(Model.CurrentVersion);
    setVersionObject(Model.CurrentVersionObject);
    $("#CurrentVersion").change(function () {
        var selectedValue = this.value;
        setVersionObject(Model.VersionObjects.filter(function (obj) { return obj.Version == selectedValue })[0]);
    });

    if (ReadOnly)
        disabledHeaderFields();
});

function resetCreateVersionError() {
    $('#createVersionError').empty();
    $('#createVersionError').hide();
}

function createVersion() {
    resetCreateVersionError();
    var duplicate = $('#duplicatePreviousVersion').is(":checked");
    var bo = null;
    var changelog = null;
    var errors = [];
    $.ajax({
        url: CreateVersionUrl,
        type: "POST",
        data: { bo: Model, duplicatePreviousVersion: duplicate },
        cache: false,
        async: false,
        success: function (data) {
            if (data.errors) {
                errors = data.errors;
            } else {
                bo = data.bo;
                refreshDropDownItems('CurrentVersion', data.versions, bo.CurrentVersion, 'Text', "", true, 'Value', false);
                changelog = data.changelog;
            }
        }
    });

    if (errors && $('#createVersionError').length) {
        $('#createVersionError').append(arrayToUnorderedList(errors));
        $('#createVersionError').show();
    }

    if (bo) {
        $('#EditableVersion').val(bo.EditableVersion);
        Model.EditableVersion = bo.EditableVersion;
        Model.CurrentVersion = bo.CurrentVersion;
        Model.CurrentVersionObject = bo.CurrentVersionObject;
        Model.CurrentVersionObjectId = bo.Id;
        Model.VersionObjects = bo.VersionObjects;
        setVersionObject(Model.CurrentVersionObject);

        if ($('#newVersionModal').length)
            $('#newVersionModal').modal('hide');
    } 

    if (typeof app !== 'undefined' && typeof app.Changelogs !== 'undefined' && changelog != null) {
        app.Changelogs.unshift(changelog);
    }
}

function setVersionObject(bo, hasVue = false, vueObject = "") {
    var disable = bo.Version != Model.EditableVersion || ReadOnly;
    if (bo) {
        for (property in bo) {
            if (property == 'Id')
                continue;

            if (ignoreFields.includes(property))
                continue;

            var id = '#' + property;

            if ($(id).length) {
                var isCheckbox = $(id).is(':checkbox');
                var currentValue = isCheckbox ? $(id).is(':checked') : $(id).val();

                if (selectedVersion != null)
                    selectedVersion[property] = currentValue;

                var value = bo[property];
                if (isCheckbox) {
                    $(id).prop('checked', value);
                } else {
                    $(id).val(value);
                }
                //Model[property] = value;
                if (disable)
                    $(id).not('.prevent-disable').prop('disabled', true);
                else
                    $(id).not('.prevent-disable').prop('disabled', false);
            }
        }
    }
    $('.selectpicker').selectpicker('refresh');

    disableVersion = disable;

    if (typeof setVersionCallBack != 'undefined') {
        setVersionCallBack(bo);
    }

    // Treaty Pricing Profit Commission
    if (typeof profitCommissionCallBack != 'undefined') {
        profitCommissionCallBack(bo.ProfitSharing);
    }

    if (selectedVersion) 
        Model.VersionObjects[Model.VersionObjects.findIndex(obj => obj.Id == selectedVersion.Id)] = selectedVersion;
    selectedVersion = bo;
}

function disabledHeaderFields() {
    $("input:not('.prevent-disable'):not('.tab-content input'):not('[name=__RequestVerificationToken]'):not('[type=hidden]')").prop("disabled", "disabled");
    $("select:not('.prevent-disable'):not('.tab-content select'):not('#CurrentVersion')").prop("disabled", true);
    $("textarea:not('.prevent-disable'):not('.tab-content textarea'):not('.dropdown-toggle')").prop("disabled", true);
    $("button:not('.prevent-disable'):not('.dropdown-toggle')").prop("disabled", true);

    //$('#CreateNewVersionBtn').prop("disabled", true);

    //$("a:not('.prevent-disable')").click(function (e) {
    //    e.preventDefault();
    //});

    $('.selectpicker').selectpicker('refresh');
}