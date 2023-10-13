
var app = new Vue({
    el: '#app',
    data: {
        Rate: RateModel,
        RateDetails: RateDetails ? RateDetails : [],
        RateDetailUploads: RateDetailUploads ? RateDetailUploads : [],
        InsuredGenderCodes: InsuredGenderCodes,
        InsuredTobaccoUses: InsuredTobaccoUses,
        InsuredOccupationCodes: InsuredOccupationCodes,
        RateDetailMaxIndex: 0,
        ValuationRate: ValuationRate,
        RateDetailModal: {
            index: -1,
            RateId: "",
            RateDetailId: "",
            InsuredGenderCodeId: "",
            CedingTobaccoUseId: "",
            CedingOccupationCodeId: "",
            AttainedAge: "",
            IssueAge: "",
            PolicyTerm: "",
            PolicyTermRemain: "",
            RateValue: ""
        },

        DisabledGender: false,
        DisabledSmoker: false,
        DisabledOccupation: false,
        DisabledAttainedAge: false,
        DisabledIssueAge: false,
        DisabledPolicyTerm: false,
        DisabledPolicyTermRemain: false,

        RateDetailCount: RateDetailCount,
        StartItemOnPage: 1,
        CurrentPage: 1,
    },
    methods: {
        resetRateDetailModal() {
            this.RateDetailModal.index = -1;
            this.RateDetailModal.RateId = this.Rate.Id;
            this.RateDetailModal.RateDetailId = "";
            this.RateDetailModal.InsuredGenderCodeId = "";
            this.RateDetailModal.CedingTobaccoUseId = "";
            this.RateDetailModal.CedingOccupationCodeId = "";
            this.RateDetailModal.AttainedAge = "";
            this.RateDetailModal.IssueAge = "";
            this.RateDetailModal.PolicyTerm = "";
            this.RateDetailModal.PolicyTermRemain = "";
            this.RateDetailModal.RateValue = "";

            $('#rateTableError').empty();
            $('#rateTableError').hide();
        },
        editRateDetail(index) {
            var bo = this.RateDetails[index];

            this.RateDetailModal.index = index;
            this.RateDetailModal.RateId = this.Rate.Id;
            this.RateDetailModal.RateDetailId = bo.Id;
            this.RateDetailModal.InsuredGenderCodeId = bo.InsuredGenderCodePickListDetailId;
            this.RateDetailModal.CedingTobaccoUseId = bo.CedingTobaccoUsePickListDetailId;
            this.RateDetailModal.CedingOccupationCodeId = bo.CedingOccupationCodePickListDetailId;
            this.RateDetailModal.AttainedAge = bo.AttainedAge;
            this.RateDetailModal.IssueAge = bo.IssueAge;
            this.RateDetailModal.PolicyTerm = bo.PolicyTerm;
            this.RateDetailModal.PolicyTermRemain = bo.PolicyTermRemain;
            this.RateDetailModal.RateValue = bo.RateValueStr;

            this.updateDisabledField();
        },
        saveRateDetail(type, save = false) {
            if (save) {
                $('#rateTableError').empty();
                $('#rateTableError').hide();

                var obj = {
                    Id: this.RateDetailModal.RateDetailId,
                    RateId: this.RateDetailModal.RateId,
                    InsuredGenderCodePickListDetailId: this.RateDetailModal.InsuredGenderCodeId,
                    CedingTobaccoUsePickListDetailId: this.RateDetailModal.CedingTobaccoUseId,
                    CedingOccupationCodePickListDetailId: this.RateDetailModal.CedingOccupationCodeId,
                    AttainedAge: this.RateDetailModal.AttainedAge,
                    IssueAge: this.RateDetailModal.IssueAge,
                    PolicyTerm: this.RateDetailModal.PolicyTerm,
                    PolicyTermRemain: this.RateDetailModal.PolicyTermRemain,
                    RateValueStr: this.RateDetailModal.RateValue,
                    action: type
                };

                var error = '';
                $.ajax({
                    url: ActionUrl,
                    type: "POST",
                    cache: false,
                    async: false,
                    data: obj,
                    success: function (data) {
                        if (data.error !== "") {
                            error = data.error;
                        }
                    }
                });

                if (error !== '') {
                    $('#rateTableError').append(error);
                    $('#rateTableError').show();
                    return;
                }
                else {
                    if (type == 'u')
                        GetPageData(this.CurrentPage);
                    else
                        GetPageData();

                    $('#rateDetailModal').modal('hide');
                }

            } else {
                if (this.RateDetailModal.index !== -1) { // edit
                    var rateDetail = this.RateDetails[this.RateDetailModal.index];
                    rateDetail.InsuredGenderCodePickListDetailId = this.RateDetailModal.InsuredGenderCodeId;
                    rateDetail.CedingTobaccoUsePickListDetailId = this.RateDetailModal.CedingTobaccoUseId;
                    rateDetail.CedingOccupationCodePickListDetailId = this.RateDetailModal.CedingOccupationCodeId;
                    rateDetail.AttainedAge = this.RateDetailModal.AttainedAge;
                    rateDetail.IssueAge = this.RateDetailModal.IssueAge;
                    rateDetail.PolicyTerm = this.RateDetailModal.PolicyTerm;
                    rateDetail.PolicyTermRemain = this.RateDetailModal.PolicyTermRemain;
                    rateDetail.RateValueStr = this.RateDetailModal.RateValue;

                    rateDetail.InsuredGenderCode = this.getDropdownText(this.RateDetailModal.InsuredGenderCodeId, TypeInsuredGenderCode);
                    rateDetail.CedingTobaccoUse = this.getDropdownText(this.RateDetailModal.CedingTobaccoUseId, TypeCedingTobaccoUse);
                    rateDetail.CedingOccupationCode = this.getDropdownText(this.RateDetailModal.CedingOccupationCodeId, TypeCedingOccupationCode);
                }
                else { // add
                    this.RateDetails.push({
                        Id: 0,
                        InsuredGenderCodePickListDetailId: this.RateDetailModal.InsuredGenderCodeId,
                        CedingTobaccoUsePickListDetailId: this.RateDetailModal.CedingTobaccoUseId,
                        CedingOccupationCodePickListDetailId: this.RateDetailModal.CedingOccupationCodeId,
                        AttainedAge: this.RateDetailModal.AttainedAge,
                        IssueAge: this.RateDetailModal.IssueAge,
                        PolicyTerm: this.RateDetailModal.PolicyTerm,
                        PolicyTermRemain: this.RateDetailModal.PolicyTermRemain,
                        RateValueStr: this.RateDetailModal.RateValue,

                        InsuredGenderCode: this.getDropdownText(this.RateDetailModal.InsuredGenderCodeId, TypeInsuredGenderCode),
                        CedingTobaccoUse: this.getDropdownText(this.RateDetailModal.CedingTobaccoUseId, TypeCedingTobaccoUse),
                        CedingOccupationCode: this.getDropdownText(this.RateDetailModal.CedingOccupationCodeId, TypeCedingOccupationCode),
                    });
                    this.RateDetailMaxIndex += 1;
                }

                $('#rateDetailModal').modal('hide');
            }
        },
        removeRateDetail(index, save = false) {
            if (save) {
                var id = this.RateDetails[index].Id;
                var rateDetailBo = this.RateDetails[index];

                $.ajax({
                    url: ActionUrl,
                    type: "POST",
                    cache: false,
                    async: false,
                    data: { rateDetailBo, action: 'd' },
                    success: function (data) {

                    }
                });

                GetPageData(this.RateDetails.length - 1 > 0 ? this.CurrentPage : this.CurrentPage - 1);
            }
            else {
                this.RateDetails.splice(index, 1);
                this.RateDetailMaxIndex -= 1;
            }
        },
        changeValuationRate: function () {
            var valuationRate = $("#ValuationRate").val();
            this.ValuationRate = valuationRate;

            GetPageData(this.CurrentPage);

        },
        emptyDisabledField(valuationRate) {
            switch (valuationRate) {
                case ValuationRate1:
                    this.RateDetails.forEach((value) => {
                        value.InsuredGenderCodePickListDetailId = "";
                        value.CedingTobaccoUsePickListDetailId = "";
                        value.CedingOccupationCodePickListDetailId = "";
                        value.PolicyTermRemain = null;
                        value.InsuredGenderCode = "";
                        value.CedingTobaccoUse = "";
                        value.CedingOccupationCode = "";
                    });
                    break;
                case ValuationRate2:
                    this.RateDetails.forEach((value) => {
                        value.CedingTobaccoUsePickListDetailId = "";
                        value.CedingOccupationCodePickListDetailId = "";
                        value.IssueAge = null;
                        value.PolicyTerm = null;
                        value.PolicyTermRemain = null;
                        value.CedingTobaccoUse = "";
                        value.CedingOccupationCode = "";
                    });
                    break;
                case ValuationRate3:
                    this.RateDetails.forEach((value) => {
                        value.InsuredGenderCodePickListDetailId = "";
                        value.CedingOccupationCodePickListDetailId = "";
                        value.IssueAge = null;
                        value.PolicyTerm = null;
                        value.PolicyTermRemain = null;
                        value.InsuredGenderCode = "";
                        value.CedingOccupationCode = "";
                    });
                    break;
                case ValuationRate4:
                    this.RateDetails.forEach((value) => {
                        value.CedingOccupationCodePickListDetailId = "";
                        value.IssueAge = null;
                        value.PolicyTerm = null;
                        value.PolicyTermRemain = null;
                        value.CedingOccupationCode = "";
                    });
                    break;
                case ValuationRate5:
                    this.RateDetails.forEach((value) => {
                        value.InsuredGenderCodePickListDetailId = "";
                        value.CedingTobaccoUsePickListDetailId = "";
                        value.AttainedAge = null;
                        value.IssueAge = null;
                        value.PolicyTerm = null;
                        value.PolicyTermRemain = null;
                        value.InsuredGenderCode = "";
                        value.CedingTobaccoUse = "";
                    });
                    break;
                case ValuationRate6:
                    this.RateDetails.forEach((value) => {
                        value.CedingOccupationCodePickListDetailId = "";
                        value.IssueAge = null;
                        value.PolicyTerm = null;
                        value.CedingOccupationCode = "";
                    });
                    break;
                case ValuationRate7:
                    this.RateDetails.forEach((value) => {
                        value.InsuredGenderCodePickListDetailId = "";
                        value.CedingTobaccoUsePickListDetailId = "";
                        value.CedingOccupationCodePickListDetailId = "";
                        value.PolicyTerm = null;
                        value.InsuredGenderCode = "";
                        value.CedingTobaccoUse = "";
                        value.CedingOccupationCode = "";
                    });
                    break;
                case ValuationRate8:
                    this.RateDetails.forEach((value) => {
                        value.CedingOccupationCodePickListDetailId = "";
                        value.PolicyTermRemain = null;
                        value.CedingOccupationCode = "";
                    });
                    break;
                case ValuationRate9:
                    this.RateDetails.forEach((value) => {
                        value.CedingTobaccoUsePickListDetailId = "";
                        value.PolicyTerm = null;
                        value.PolicyTermRemain = null;
                        value.CedingTobaccoUse = "";
                    });
                    break;
                case ValuationRate10:
                    this.RateDetails.forEach((value) => {
                        value.PolicyTerm = null;
                        value.PolicyTermRemain = null;
                    });
                    break;
            }
            this.updateDisabledField();
        },
        updateDisabledField() {
            switch (this.ValuationRate) {
                case ValuationRate1:
                    this.DisabledGender = true;
                    this.DisabledSmoker = true;
                    this.DisabledOccupation = true;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = false;
                    this.DisabledPolicyTerm = false;
                    this.DisabledPolicyTermRemain = true;
                    break;
                case ValuationRate2:
                    this.DisabledGender = false;
                    this.DisabledSmoker = true;
                    this.DisabledOccupation = true;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = true;
                    this.DisabledPolicyTerm = true;
                    this.DisabledPolicyTermRemain = true;
                    break;
                case ValuationRate3:
                    this.DisabledGender = true;
                    this.DisabledSmoker = false;
                    this.DisabledOccupation = true;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = true;
                    this.DisabledPolicyTerm = true;
                    this.DisabledPolicyTermRemain = true;
                    break;
                case ValuationRate4:
                    this.DisabledGender = false;
                    this.DisabledSmoker = false;
                    this.DisabledOccupation = true;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = true;
                    this.DisabledPolicyTerm = true;
                    this.DisabledPolicyTermRemain = true;
                    break;
                case ValuationRate5:
                    this.DisabledGender = true;
                    this.DisabledSmoker = true;
                    this.DisabledOccupation = false;
                    this.DisabledAttainedAge = true;
                    this.DisabledIssueAge = true;
                    this.DisabledPolicyTerm = true;
                    this.DisabledPolicyTermRemain = true;
                    break;
                case ValuationRate6:
                    this.DisabledGender = false;
                    this.DisabledSmoker = false;
                    this.DisabledOccupation = true;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = true;
                    this.DisabledPolicyTerm = true;
                    this.DisabledPolicyTermRemain = false;
                    break;
                case ValuationRate7:
                    this.DisabledGender = true;
                    this.DisabledSmoker = true;
                    this.DisabledOccupation = true;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = false;
                    this.DisabledPolicyTerm = true;
                    this.DisabledPolicyTermRemain = false;
                    break;
                case ValuationRate8:
                    this.DisabledGender = false;
                    this.DisabledSmoker = false;
                    this.DisabledOccupation = true;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = false;
                    this.DisabledPolicyTerm = false;
                    this.DisabledPolicyTermRemain = true;
                    break;
                case ValuationRate9:
                    this.DisabledGender = false;
                    this.DisabledSmoker = true;
                    this.DisabledOccupation = false;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = false;
                    this.DisabledPolicyTerm = true;
                    this.DisabledPolicyTermRemain = true;
                    break;
                case ValuationRate10:
                    this.DisabledGender = false;
                    this.DisabledSmoker = false;
                    this.DisabledOccupation = false;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = false;
                    this.DisabledPolicyTerm = true;
                    this.DisabledPolicyTermRemain = true;
                    break;
                default:
                    this.DisabledGender = false;
                    this.DisabledSmoker = false;
                    this.DisabledOccupation = false;
                    this.DisabledAttainedAge = false;
                    this.DisabledIssueAge = false;
                    this.DisabledPolicyTerm = false;
                    this.DisabledPolicyTermRemain = false;
                    break;
            }
        },
        getDropdownText: function (id, type) {
            var stringText = "";
            if (id != "") {
                switch (type) {
                    case TypeInsuredGenderCode:
                        this.InsuredGenderCodes.forEach(function (e) {
                            if (id == e.Value) stringText = e.Text;
                        });
                        break;
                    case TypeCedingTobaccoUse:
                        this.InsuredTobaccoUses.forEach(function (e) {
                            if (id == e.Value) stringText = e.Text;
                        });
                        break;
                    case TypeCedingOccupationCode:
                        this.InsuredOccupationCodes.forEach(function (e) {
                            if (id == e.Value) stringText = e.Text;
                        });
                        break;
                }
            }
            return stringText;
        },
        stringifyRateDetail: function () {
            $('#rateDetailsJsonString').val(JSON.stringify(this.RateDetails));
        },
        stringifyRateDetailUpload: function () {
            console.log(1);
            if (this.Rate.Id == 0)
                $('input[name="rateDetailUploadsJsonString"]').val(JSON.stringify(this.RateDetailUploads));
        },
        downloadError: function (id) {
            window.location.href = DownloadErrorUrl + "?id=" + id;
        }
    },
    created: function () {
        this.RateDetailMaxIndex = (this.RateDetails) ? this.RateDetails.length - 1 : -1;
        //this.updateDisabledField();
        this.stringifyRateDetailUpload();
    },
    updated() {
        //$(this.$refs.select).selectpicker('refresh');
    }
});

$(document).ready(function () {
    $(".dropdown-item").click(function () {
        $("#btnDownload").dropdown("toggle");
    });

    if (RateModel.Id > 0) GetPageData();
});

function uploadFile(form) {
    form.action = UploadFileUrl;
    form.submit();
}

function GetPageData(pageNum = 1) {
    //After every trigger remove previous data and paging
    $("#contentPager").empty();
    $("#contentPagerInfo").empty();

    $.ajax({
        url: FetchUrl,
        type: "POST",
        cache: false,
        async: false,
        data: { id: RateModel.Id, pageNumber: pageNum },
        success: function (data) {
            var rateDetailBos = data.bos;
            if (rateDetailBos !== null) {
                app.RateDetails = rateDetailBos;
                app.emptyDisabledField(app.ValuationRate);
            }
            else {
                app.RateDetails = [];
            }

            if (data.bosTotal > 0)
                PaggingTemplate(data.bosTotal, pageNum);
        }
    });
}

function PaggingTemplate(totalItemCount, pageNum) {
    var template = "";
    var PageNumberArray = Array();

    var TotalItemCount = totalItemCount;
    var PageNumber = pageNum;
    var PageCount = TotalItemCount > 0 ? Math.ceil(TotalItemCount / PageSize) : 0;
    var HasPreviousPage = PageNumber > 1;
    var HasNextPage = PageNumber < PageCount;
    var IsFirstPage = PageNumber == 1;
    var IsLastPage = PageNumber >= PageCount;

    // calculate start and end of range of page info
    var FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
    var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
    var LastItemOnPage = numberOfLastItemOnPage > TotalItemCount ? TotalItemCount : numberOfLastItemOnPage;

    // calculate start and end of range of page numbers
    var maxPageNumbersToDisplay = 10;
    var firstPageToDisplay = PageNumber - maxPageNumbersToDisplay / 2;
    if (firstPageToDisplay < 1)
        firstPageToDisplay = 1;

    var lastPageToDisplay = firstPageToDisplay + maxPageNumbersToDisplay - 1;
    if (lastPageToDisplay > PageCount) {
        firstPageToDisplay = PageCount - maxPageNumbersToDisplay + 1;
        if (firstPageToDisplay < 1)
            firstPageToDisplay = 1;
        lastPageToDisplay = PageCount;
    }

    var countIncr = 0;
    for (var i = firstPageToDisplay; i <= lastPageToDisplay; i++) {
        PageNumberArray[countIncr] = i;
        countIncr++;
    }

    app.CurrentPage = PageNumber;
    app.StartItemOnPage = FirstItemOnPage;

    template = '<div class="pagination-container"><ul class="pagination">'
        + (HasPreviousPage && (PageNumber >= 7 && PageCount > 10) ? '<li class="PagedList-skipToFirst"><a  onclick="GetPageData(1)" href="#">««</a></li>' : '')
        + (HasPreviousPage ? '<li class="PagedList-skipToPrevious"><a  onclick="GetPageData(' + (PageNumber - 1) + ')" href="#" rel="prev">Prev</a></li>' : '')
        + (PageNumber >= 7 && PageCount > 10 ? '<li class="disabled PagedList-ellipses"><a>…</a></li>' : '');

    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        if (PageNumberArray[i] == PageNumber)
            numberingLoop = numberingLoop + '<li class="active"><a>' + PageNumberArray[i] + '</a>';
        else
            numberingLoop = numberingLoop + '<li><a onclick="GetPageData(' + PageNumberArray[i] + ')" href="#">' + PageNumberArray[i] + '</a>';
    }

    template = template + numberingLoop
        + (PageCount > 10 && PageNumberArray.indexOf(PageCount) == -1 ? '<li class="disabled PagedList-ellipses"><a>…</a></li>' : '')
        + (HasNextPage ? '<li class="PagedList-skipToNext"><a onclick="GetPageData(' + (PageNumber + 1) + ')" href="#" rel="next">Next</a></li>' : '')
        + (HasNextPage ? '<li class="PagedList-skipToLast"><a onclick="GetPageData(' + (PageCount) + ')" href="#">»»</a></li>' : '') + '</ul></div>';;
    $("#contentPager").append(template);
    $("#contentPagerInfo").append('<span class="page-info">Showing ' + numberWithCommas(FirstItemOnPage) + ' to ' + numberWithCommas(LastItemOnPage) + ' of ' + numberWithCommas(TotalItemCount) + ' records</span>')

}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
