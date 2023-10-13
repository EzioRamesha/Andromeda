var documentPrefix = '';

function uploadFile(prefix = '', updateList = true) {
    documentPrefix = prefix;

    if (updateList) {
        var selectedFiles = document.querySelector(getSelectedFileId());
        selectedFiles.innerHTML = "";
    }

    var files = getFiles();
    if (!files) return;

    if (updateList) {
        var list = "";
        for (var i = 0; i < files.length; i++) {
            list += "<li>" + files[i].name + "</li>";
        }
        selectedFiles.innerHTML = "<ul>" + list + "</ul>";
    }
}

function getInputId() {
    return '#' + documentPrefix + 'files';
}

function getSelectedFileId() {
    return '#' + documentPrefix + 'selectedFiles';
}

function getUpload() {
    return $(getInputId());
}

function getFiles() {
    var inputId = getInputId();
    if (!$(inputId).length)
        return null;

    return $(inputId)[0].files;
}

function resetDocumentModal() {
    resetAddDocumentError();
    if (app != null)
        app.resetDocumentModal();
}

function resetAddDocumentError() {
    $('#addDocumentError').empty();
    $('#addDocumentError').hide();
}

function validateAddDocument(documentBo, file) {
    if (documentPrefix != '')
        return true;

    resetAddDocumentError();

    var errors = [];

    if (file == null)
        errors.push("File Upload is required");

    if (!hideDocumentDetail) {
        if (!documentBo.Type == null || documentBo.Type == 0)
            errors.push("Type is required");

        if (documentBo.Description == null || documentBo.Description == '')
            errors.push("Description is required");
    }

    if (showDocumentName && (documentBo.Description == null || documentBo.Description == ''))
        errors.push("Document Name is required");

    var fileErrors = validateFiles();
    errors = errors.concat(fileErrors);

    if (errors.length > 0) {
        $('#addDocumentError').append(arrayToUnorderedList(errors));
        $('#addDocumentError').show();
        return false;
    }

    return true;
}

function validateFiles() {
    var files = getFiles();
    var errors = [];

    if (files != null) {
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var fileSize = file.size / 1024 / 1024; // in MB
            if (fileSize > 10)
                errors.push(file.name + ' size exceeded 10 MB');
        }
    }    

    return errors;
}

function saveDocument(document, file, save = false) {
    if (!validateAddDocument(document, file))
        return null;

    var fileName = file.name;
    document.FileName = fileName;

    var formData = new FormData();
    formData.append(fileName, file);
    formData.append('documentBo', JSON.stringify(document));

    $.ajax({
        url: save ? SaveDocumentUrl : SaveTempDocumentUrl,
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        async: false,
        data: formData,
        success: function (data) {
            if (data.error) {
                document = null;
                alert(data.error);
            } else {
                document = data.documentBo;
            }
        }
    });

    $('#documentModal').modal('hide');

    return document;
}

function deleteDocument(documentBo) {
    var success = false;
    $.ajax({
        url: DeleteDocumentUrl,
        type: "POST",
        cache: false,
        async: false,
        data: { documentBo: documentBo },
        success: function (data) {
            if (data.success)
                success = true;
        }
    });

    return success;
}

function clearSelectedFiles(prefix = '') {
    documentPrefix = prefix;

    getUpload().val(null);

    var selectedFiles = document.querySelector(getSelectedFileId());
    if (selectedFiles)
        selectedFiles.innerHTML = "";

    if (typeof hideDocumentDetail != 'undefined' && !hideDocumentDetail) {
        $('#documentType').selectpicker('refresh');
    }
}