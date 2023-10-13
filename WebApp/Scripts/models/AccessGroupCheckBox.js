
function getAllCheckBox(name) {
    return $(`input:checkbox[id$='_${name}']`);
}

function getAdditionalCheckBox() {
    return $("input:checkbox[id*=_]").not("[id$=_C]").not("[id$=_R]").not("[id$=_U]").not("[id$=_D]");
}

function check(name, checked) {
    getAllCheckBox(name).each(function () {
        $("#" + this.id).prop("checked", checked);
    });
}

function checkAdditional(checked) {
    getAdditionalCheckBox().each(function () {
        $("#" + this.id).prop("checked", checked);
    });
}

function checkboxOnChange(nameAll, name) {
    var checked = false;
    var allChecked = true;
    var checkboxes = getAllCheckBox(name);
    checkboxes.each(function (index, element) {
        if (!element.checked) {
            allChecked = false;
        } else {
            checked = true;
        }
    });

    if (checkboxes.length == 0)
        allChecked = false;

    if (allChecked) {
        $('#' + nameAll).prop("checked", true).prop("indeterminate", false);
    } else {
        $('#' + nameAll).prop("checked", false).prop("indeterminate", checked);
    }
}

function checkboxAdditionalOnChange() {
    var checked = false;
    var allChecked = true;
    var checkboxes = getAdditionalCheckBox();
    checkboxes.each(function (index, element) {
        if (!element.checked) {
            allChecked = false;
        } else {
            checked = true;
        }
    });

    if (checkboxes.length == 0)
        allChecked = false;

    if (allChecked) {
        $('#additionalAll').prop("checked", true).prop("indeterminate", false);
    } else {
        $('#additionalAll').prop("checked", false).prop("indeterminate", checked);
    }
}

function initial() {
    checkboxOnChange('createAll', 'C');
    checkboxOnChange('readAll', 'R');
    checkboxOnChange('updateAll', 'U');
    checkboxOnChange('deleteAll', 'D');
    checkboxAdditionalOnChange();
}