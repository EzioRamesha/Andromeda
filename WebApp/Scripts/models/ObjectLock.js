$(document).ready(function () {
    if (!CanEnterEditMode) {
        $('.edit-mode-btn').prop('disabled', true);
    }

    if (IsOtherUserEditing) {
        $('#objectLockErrorMessage').text(LockedErrorMessage);
        $('#objectLockErrorModal').modal('show');
    }

    var isActionPost = false;
    var isDownloadFile = false;
    $("form").submit(function () {
        isActionPost = true;
    });

    $('a[href*="Download"]').click(function () {
        isDownloadFile = true;
    });

    $(window).on("beforeunload", function () {
        if (IsEditMode && !isActionPost && !isDownloadFile) {
            return "";
        }
    });

    $(window).on("unload", function () {
        if (IsEditMode && !isActionPost && !isDownloadFile) {
            var data = {
                controller: SubModuleController,
                objectId: Model.Id,
            };

            fetch(ReleaseObjectLockUrl, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                method: 'POST',
                body: JSON.stringify(data),
                keepalive: true
            });
        }
        isDownloadFile = false;
    });

});


function enterEditMode() {
    var url = EditModeUrl += '?Id=' + Model.Id;

    if (IsCalledFromWorkflow) {
        url += '&versionId=' + VersionId +
            '&isCalledFromWorkflow=' + IsCalledFromWorkflow +
            '&isQuotationWorkflow=' + IsQuotationWorkflow +
            '&workflowId=' + Model.WorkflowId;
    }

    url += '&isEditMode=true';

    window.location.href = url;
}