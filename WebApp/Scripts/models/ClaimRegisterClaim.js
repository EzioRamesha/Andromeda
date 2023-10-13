$(document).ready(function () {
    if (!CanUpdateUnderwritingFeedback)
        $("input[name='UnderwriterFeedback']").prop('disabled', 'disabled');

    if (readOnly)
        $(":submit").prop("disabled", true);

    $('#EventChronologyComment').autoResize();
    $('#ClaimAssessorRecommendation').autoResize();
    $('#ClaimCommitteeComment1').autoResize();
    $('#ClaimCommitteeComment2').autoResize();
    $('#CeoComment').autoResize();

    $('#completeBtn').on("click", function () {
        var underwriterFeedback = $("input[name='UnderwriterFeedback']:checked").val();

        if (typeof underwriterFeedback !== 'undefined') {
            var url = $(this).attr('href') + '&underwriterFeedback=' + underwriterFeedback;
            $(this).attr("href", url);
        }
    });

    $('#downloadExGratiaBtn').on("click", function () {
        var textFields = [
            'ClaimTransactionType',
            'InsuredName',
            'ClaimId',
            'InsuredDateOfBirthStr',
            'PolicyNumber',
            'ReinsEffDatePolStr',
            'DateOfEventStr',
            'CauseOfEvent',
            'ClaimRecoveryAmtStr',
            'ClaimAssessorRecommendation',
            'EventChronologyComment',
            'ClaimCommitteeComment1',
            'ClaimCommitteeComment2',
            'CeoComment'
        ];

        var selectFields = [
            'CedingCompany',
            'InsuredGenderCode',
            'MlreBenefitCode',
            'TreatyCode',
        ];

        var committeeUserFields = [
            'ClaimCommitteeUser1',
            'ClaimCommitteeUser2',
        ];

        var params = [];
        textFields.forEach(function (field) {
            params.push(field + '=' + encodeURIComponent($('#' + field).val()));
        });

        selectFields.forEach(function (field) {
            var value = null;
            if ($('#' + field).val() != null && $('#' + field).val() != 'null') {
                value = $('#' + field + ' option:selected').text();
            }

            params.push(field + '=' + encodeURIComponent(value));
        });

        committeeUserFields.forEach(function (field) {
            var idField = '#' + field + 'Id';
            var nameField = '#' + field + 'Name';

            var value = null;
            if ($(idField).val() != null && $(idField).val() != 'null') {
                value = $(idField + ' option:selected').text();
            } else {
                value = $(nameField).val();
            }

            params.push(field + '=' + encodeURIComponent(value));
        });

        var paramStr = params.join('&');

        var url = DownloadExGratiaUrl + '?' + paramStr;
        $(this).attr("href", url);
    });

    $('#assignPicBtn').on("click", function () {
        var picClaimId = $('#PicClaimId').val();

        var params = [];
        params.push('id=' + Model.Id);
        params.push('userId=' + picClaimId);

        var paramStr = params.join('&');

        window.location.href = AssignCaseUrl + '?' + paramStr;
    });

    $('#SubmitForApprovalByLimitBtn').on("click", function () {
        $('#changePicModal').modal('toggle');
        return false;
    });

    $('#confirmApprovalByLimitBtn').on("click", function () {
        var params = [
            'id=' + Model.Id,
            'status=' + ClaimStatusApprovalByLimit,
            'nextPicClaimId=' + $('#NextPicClaimId').val()
        ];

        var paramStr = params.join('&');

        var url = UpdateStatusUrl + '?' + paramStr;
        $(this).attr("href", url);
    });

    $('#OverwriteBtn').on("click", function () {
        saveUrl = this.href;
        $('#overwriteConfirmationModal').modal('show');
        return false;
    });

    toggleClaimCommitteeUser(1);
    toggleClaimCommitteeUser(2);
});

var saveUrl;

function toggleClaimCommitteeUser(index) {
    var idField = '#ClaimCommitteeUser' + index + 'Id';
    var strField = 'ClaimCommitteeUser' + index + 'Name';
    if ($(idField).val() == null || $(idField).val() == '') {
        $('#' + strField).prop('readonly', false);
    } else {
        app.ClaimRegister[strField] = null;
        $('#' + strField).prop('readonly', true);
    }
}

function confirmOverwriteApproval() {
    if (saveUrl != '') {
        window.location.href = saveUrl;
    }
}

function notifyClaimCommittee() {
    var claimId = $('#ClaimId').val();
    var userId1 = $('#ClaimCommitteeUser1Id').val();
    var userId2 = $('#ClaimCommitteeUser2Id').val();

    $('#notifyCommitteeBtn').prop("disabled", true);

    $.ajax({
        url: NotifyClaimCommitteeUrl,
        type: "POST",
        data: {
            claimId: claimId,
            userId1: userId1,
            userId2: userId2,
        },
        cache: false,
        async: false,
        success: function (data) {
            $("#committeeUser1Msg").text(data.messages[0]);
            $("#committeeUser2Msg").text(data.messages[1]);
            $('#notificationResultModal').modal('show');
        }, 
        //error: function (xhr, status, error) {
        //    alert('Error: ' + xhr.statusText);
        //}
    });

    $('#notifyCommitteeBtn').prop("disabled", false);
}
