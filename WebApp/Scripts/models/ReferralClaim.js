function submitForAssessment() {
    $('#Status').val(StatusPendingAssessment);
}

function submitForChecklist() {
    $('#Status').val(StatusPendingChecklist);
}

function submitClosed() {
    $('#Status').val(StatusClosed);
}

function toggleRowView() {
    var search = $('#RiDataLabel').val() ?? "";
    var filter = search.toUpperCase();
    var hasValueOnly = $('#HasValue').prop("checked");

    var table = document.getElementById("riDataTable");
    var tr = table.getElementsByTagName("tr");


    for (i = 0; i < tr.length; i++) {
        field = tr[i].getElementsByTagName("td")[0];
        value = tr[i].getElementsByTagName("td")[1];

        if (field) {
            fieldTxt = field.textContent || field.innerText;
            valueTxt = value.textContent || value.innerText;
            if (hasValueOnly == true && (valueTxt == null || valueTxt.trim() == "")) {
                tr[i].style.display = "none";
            } else if (fieldTxt.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

$(document).ready(function () {
    dateOffAutoComplete();

    $('#AssessorComments').autoResize();
    $('#ReviewerComments').autoResize();
    $('#AssessorComments').trigger('keyup');
    $('#ReviewerComments').trigger('keyup');

    if (isClosedRegistered) {
        var readOnlyFields = ["GroupName", "DateOfCommencementStr", "CedingPlanCode", "SumInsuredStr", "SumReinsuredStr", "RiskQuarter", "ReceivedAtStr", "ReceivedAtTime", "RespondedAtStr", "RespondedAtTime"];
        readOnlyFields.forEach(function (field) {
            $('#' + field).attr('readOnly', true);
        });
    }
});

var app = new Vue({
    el: '#app',
    data: {
        ReferralClaim: Model,
        TreatyCodeList: TreatyCodeList,
        ShowNoTreatyCodeSelected: false,
        SearchRiData: {
            TreatyCode: null,
            InsuredName: null,
            PolicyNumber: null,
            CedingPlanCode: null,
            DateOfBirth: null,
            Searched: false
        },
        SearchRiDataValidation: [],
        MatchedRiDatas: [],
        RiData: Model.RiDataWarehouseBo,
        DuplicateFound: false,
        DuplicateList: [],
        SanctionFound: false,
        SanctionList: [],
        SanctionVerificationDetailIds: [],
        SanctionUpdateSuccess: false,
        DropDownUsers: DropDownUsers,
        // Remark
        Remarks: RemarksList ? RemarksList : [],
        RemarkMaxIndex: 0,
        RemarkModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            Status: StatusNewCase ? StatusNewCase : null,
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
        DocumentMaxIndex: 0,
        DocumentModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            File: null,
            Type: 0,
            Description: '',
        },
        // Status History
        StatusHistories: StatusHistoriesList ? StatusHistoriesList : [],
        // Changelog
        UserTrails: UserTrails ? UserTrails : [],
        // Related Claims
        RelatedClaimRegisters: RelatedClaimRegisters,
    },
    methods: {
        // Turn Around Time
        calculateTurnAroundTime(actionType) { /*
            actionType 1 = ReceivedAt or RespondedAt modified
            actionType 2 = DateReceivedFullDocuments or DocRespondedAt modified*/

            if (actionType == 1) {
                this.setTurnAroundTime(this.ReferralClaim.ReceivedAtStr, this.ReferralClaim.RespondedAtStr,
                    this.ReferralClaim.ReceivedAtTime, this.ReferralClaim.RespondedAtTime, 'TurnAroundTime');
            } else if (actionType == 2) {
                this.setTurnAroundTime(this.ReferralClaim.DateReceivedFullDocumentsStr, this.ReferralClaim.DocRespondedAtStr,
                    this.ReferralClaim.DateReceivedFullDocumentsTime, this.ReferralClaim.DocRespondedAtTime, 'DocTurnAroundTime');
            }
        },
        setTurnAroundTime: function(start, end, startTime, endTime, field) {
            if (start == null || end == null)
                return;

            start += ' ' + startTime;
            end += ' ' + endTime;

            var hours = 0;
            var minutes = 0;
            var turnAroundTime = 0;
            $.ajax({
                url: CalculateDateRangeUrl,
                type: "POST",
                data: {
                    startDateStr: start,
                    endDateStr: end,
                    addDayAfterWorkingHour: true,
                },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.error) {
                        alert(data.error);
                    } else {
                        hours = data.hours;
                        minutes = data.minutes;
                        turnAroundTime = data.turnAroundTime;
                        $('#' + field).val(turnAroundTime);
                    }
                }
            });

            var hourField = field + 'Hours';
            var minuteField = field + 'Minutes';

            this.ReferralClaim[field] = turnAroundTime;
            this.ReferralClaim[hourField] = hours;
            this.ReferralClaim[minuteField] = minutes;
        },
        // Retro
        resetRetro: function () {
            this.ReferralClaim.RetrocessionaireName = null;
            this.ReferralClaim.RetrocessionaireShareStr = null;
            this.ReferralClaim.RetroReferralReasonId = null;
            this.ReferralClaim.MlreReferralReasonId = null;
            this.ReferralClaim.RetroReviewedById = null;
            this.ReferralClaim.RetroReviewedAtStr = null;

            if ($('#IsRetro')[0].checked) {
                $('#RetroReferralReasonId').prop("disabled", false);
                $('#MlreReferralReasonId').prop("disabled", false);
                $('#RetroReviewedById').prop("disabled", false);
            } else {
                $('#RetroReferralReasonId').prop("disabled", true);
                $('#MlreReferralReasonId').prop("disabled", true);
                $('#RetroReviewedById').prop("disabled", true);
            }

            $('#RetroReferralReasonId').selectpicker('refresh');
            $('#MlreReferralReasonId').selectpicker('refresh');
            $('#RetroReviewedById').selectpicker('refresh');
        },
        // Treaty
        resetTreatyModal: function () {
            this.ShowNoTreatyCodeSelected = false;
        },
        selectTreatyCode: function () {
            this.ShowNoTreatyCodeSelected = false;
            if (document.querySelector('input[name="treatyCodeOptions"]:checked')) {
                var index = document.querySelector('input[name="treatyCodeOptions"]:checked').value;

                var treatyCode = TreatyCodeList[index];
                this.ReferralClaim.TreatyCode = treatyCode.Code;   
                this.ReferralClaim.TreatyType = treatyCode.TreatyTypeCode;

                $('#treatyCodeModal').modal('toggle');

                this.$nextTick(function () {
                    $("#TreatyType").selectpicker('refresh');
                })

            } else {
                this.ShowNoTreatyCodeSelected = true;
            }
        },
        // Claim Register
        resetRiDataModal: function () {
            this.SearchRiData.PolicyNumber = this.ReferralClaim.PolicyNumber;
            this.SearchRiData.InsuredName = this.ReferralClaim.InsuredName;
            this.SearchRiData.DateOfBirth = this.ReferralClaim.InsuredDateOfBirthStr;
            this.SearchRiData.CedingPlanCode = this.ReferralClaim.CedingPlanCode;
        },
        validateRiData: function () {
            this.SearchRiDataValidation = [];
            if (this.SearchRiData.PolicyNumber == null || this.SearchRiData.PolicyNumber == "")
                this.SearchRiDataValidation.push("Policy Number is required");
            if (this.SearchRiData.InsuredName == null || this.SearchRiData.InsuredName == "")
                this.SearchRiDataValidation.push("Insured Name is required");

            return this.SearchRiDataValidation.length == 0;
        },
        searchRiData: function () {
            if (!this.validateRiData())
                return;

            var obj = {
                treatyCode: this.SearchRiData.TreatyCode,
                insuredName: this.SearchRiData.InsuredName,
                policyNumber: this.SearchRiData.PolicyNumber,
                cedingPlanCode: this.SearchRiData.CedingPlanCode,
                dateOfBirthStr: this.SearchRiData.DateOfBirth,
            };

            var riDataBos = [];
            $.ajax({
                url: SearchRiDataWarehouseUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    riDataBos = data.riDataBos;
                }
            });
            this.MatchedRiDatas = riDataBos;

            if (this.MatchedRiDatas.length == 0)
                this.SearchRiDataValidation.push("No records found");
            else
                this.SearchRiData.Searched = true;

        },
        selectRiData: function () {
            var index = $('input[name="selectRiData"]:checked').val();
            if (typeof index == 'undefined') {
                this.SearchRiDataValidation.push("No RI Data Selected");
                return;
            }

            this.RiData = this.MatchedRiDatas[index];
            $('#RiDataWarehouseId').val(this.RiData.Id);
            this.ReferralClaim.SumReinsuredStr = this.RiData.AarStr;
            this.ReferralClaim.ClaimRecoveryAmountStr = this.RiData.AarStr;
            this.ReferralClaim.TreatyCode = this.RiData.TreatyCode;
            this.ReferralClaim.ReinsBasisCode = this.RiData.ReinsBasisCode;
            this.ReferralClaim.TreatyType = this.RiData.TreatyType;
            this.ReferralClaim.TreatyShareStr = this.RiData.MlreShareStr;

            $('#searchRiDataModal').modal('toggle');
        },
        // Related Claim Register
        linkToRegisterClaim: function () {
            var obj = {
                refId: this.ReferralClaim.ReferralId
            };

            var bo = null;
            $.ajax({
                url: FindClaimRegisterUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    if (data.success) {
                        bo = data.claimRegisterBo;
                    } else if (data.message != "") {
                        alert(data.message);
                    }
                },
            });

            if (bo != null) {
                this.RelatedClaimRegisters.push(bo);
            }
        },
        urlClaimRegisterEditLink: function (index) {
            var item = this.RelatedClaimRegisters[index];
            var url = UrlClaimRegisterLink + '/' + item.Id;
            return url
        },
        // Duplicate
        getDuplicate: function () {
            var obj = { 'bo': this.ReferralClaim };
            var list = [];

            $.ajax({
                url: GetDuplicateUrl,
                type: "POST",
                data: {
                    bo: this.ReferralClaim
                },
                cache: false,
                async: false,
                success: function (data) {
                    list = data.bos;
                }
            });

            if (list.length > 0)
                this.DuplicateFound = true;
            this.DuplicateList = list;
        },
        // Sanction
        matchSanction: function () {
            var obj = { 'bo': this.ReferralClaim };
            var list = [];
            var ids = [];

            $.ajax({
                url: MatchSanctionUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    list = data.bos;
                    ids = data.detailIds;
                }
            });

            if (list.length > 0)
                this.SanctionFound = true;
            this.SanctionList = list;
            this.SanctionVerificationDetailIds = ids;
        },
        sanctionAction: function (action) {
            // 1 = whitelist
            // 2 = exact match
            var obj = {
                'ids': this.SanctionVerificationDetailIds,
                'isWhitelist': action == 1 ? true : false,
                'isExactMatch': action == 2 ? true : false,
            };

            $.ajax({
                url: UpdateSanctionVerificationDetailUrl,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    alert(data.message);
                }
            });

            $('#sanctionModal').modal('hide');
        },
        // Remark & Document
        resetRemarkInfo: function () {
            this.RemarkModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.RemarkModal.Status = this.ReferralClaim.Status == 0 ? StatusNewCase : this.ReferralClaim.Status;
            this.RemarkModal.ModuleId = this.ReferralClaim.ModuleId;
            this.RemarkModal.ObjectId = this.ReferralClaim.Id;
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
            else
                $('#FollowUpUserId').prop("disabled", true);

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

            if (!this.ReferralClaim.Id) {
                remark = remarkModal;
                remark.StatusName = StatusNewCaseName;
                remark.PermissionName = this.RemarkModal.RemarkIsPrivate ? "Private" : "Public";

                remark.DocumentBos = this.saveDocuments(this.RemarkMaxIndex);
            } else {
                var remark = createRemark(this.RemarkModal);
                if (remark)
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
        saveDocuments: function (remarkIndex, remarkId = null) {
            var files = getFiles();

            if (!files) return;

            var parentId = this.ReferralClaim.Id;
            var document = {
                ModuleId: this.ReferralClaim.ModuleId,
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
        // Changelog
        toggleChangelogDataView: function (index) {
            $('#showAll_' + index).toggle();
            $('#showLess_' + index).toggle();
            $('#showAllBtn_' + index).toggle();
            $('#collapseAllBtn_' + index).toggle();
        },
        // Shared
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
        openTimePicker: function (field) {
            var id = '#' + field;

            if (!$(id).hasClass("timepicker")) {
                $(id).timepicker({
                    use12HourClock: true,
                    timeFormat: "%g:%i %A"
                });

                $(id).addClass('timepicker');

                var setMatchValue = this.setMatchValue;
                $(id).on('change', function () {
                    setMatchValue(field, $(id).val());
                });
            }

            this.$nextTick(function () {
                $(".my-timepicker-div").position({
                    of: $(id),
                    my: 'right top',
                    at: 'right bottom',
                });
            })

            $(id).focus();
        },
        setMatchValue: function (field, value) {
            var model = "ReferralClaim";
            var fieldName = field;
            if (field.startsWith("SearchRiData")) {
                fieldName = field.replace('SearchRiData', '');
                model = "SearchRiData";
            } else if (field.startsWith("RemarkModal")) {
                fieldName = field.replace('RemarkModal', '');

                model = "RemarkModal";
            } 

            this[model][fieldName] = value;

            if (field.startsWith("ReceivedAt") || field.startsWith("RespondedAt") || field.startsWith("DateReceivedFullDocuments") || field.startsWith("DocRespondedAt")) {
                var timeField = field.replace("Str", "Time");
                if (this.ReferralClaim[timeField] == null)
                    this.ReferralClaim[timeField] = '00:00';

                if (field.startsWith("ReceivedAt") || field.startsWith("RespondedAt")) {
                    this.calculateTurnAroundTime(1);
                }
                else if (field.startsWith("DateReceivedFullDocuments") || field.startsWith("DocRespondedAt")) {
                    this.calculateTurnAroundTime(2);
                }
            }

            if (field == "DateOfEventStr") {
                this.ReferralClaim.RiskQuarter = dateToQuarter(value);
            }
        },
        // Auto Expand TextArea
        autoExpandTextarea: function (id) {
            var tArea = $('#' + id);
            this.textAreaWidth = tArea.width();
            this.textAreaHeight = tArea.height();

            tArea.autoResize();
            tArea.trigger('keyup');
        },
        autoCollapseTextarea: function (id) {
            var tArea = $('#' + id);
            tArea.height(this.textAreaHeight);
        },
        updateClaimAmount: function () {
            this.ReferralClaim.ClaimRecoveryAmountStr = this.ReferralClaim.SumReinsuredStr;
        }
    }
});

