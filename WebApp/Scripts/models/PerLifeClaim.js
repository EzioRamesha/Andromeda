var app = new Vue({
    el: '#app',
    data: {
        Model: Model,
        isCheckAllClaimRegisterData: false,
        isCheckAllExceptions: false,
        isCheckAllClaimRetroData: false,
        isCheckAllSummaryPaidClaims: false,
        isCheckAllSummaryPendingClaims: false,
        isCheckAllSummaryClaimsRemoved: false,
        StatusHistories: StatusHistories,
        selected: [],
        ClaimRegisterData: [],
        Exceptions: [],
        ClaimRetroData: [],
        SummaryPaidClaims: [],
        SummaryPendingClaims: [],
        SummaryClaimsRemoved: [],
    },
    methods: {
        editPerLifeClaimRetroData: function (claimRetroDataId) {
            var url = EditPerLifeClaimRetroDataUrl + '/' + claimRetroDataId;
            return url
        },
        submitForProcessing: function (confirm) {
            $.ajax({
                url: SubmitForProcessingUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    id: this.Model.Id,
                    model: this.Model,
                    confirm: confirm
                },
                success: function (data) {
                    if (data.errors == "success") {
                        location.reload();
                    }
                    else if (data.errors == "overwrite") {
                        $('#submitForProcessingModal').modal('show');
                    } else {
                        // show error
                    }
                }
            });
        },
        validateData: function () {
            $.ajax({
                url: ValidateDataUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    id: this.Model.Id,
                    model: this.Model
                },
                success: function (data) {
                    if (data.errors == "success") {
                        location.reload();
                    } else if (data.errors == "fail") {
                        $('#validateDataModal').modal('show');
                    }
                    else {
                        // show error
                    }
                }
            });
        },
        processClaimRecovery: function () {
            $.ajax({
                url: ProcessClaimRecoveryUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    id: this.Model.Id,
                    model: this.Model
                },
                success: function (data) {
                    if (data.errors == "success") {
                        location.reload();
                    } else if (data.errors == "fail") {
                        $('#processClaimRecoveryModal').modal('show');
                    }
                    else {
                        // show error
                    }
                }
            });
        },
        processFinalise: function () {
            $.ajax({
                url: ProcessFinaliseUrl,
                type: "POST",
                cache: false,
                async: false,
                data: {
                    id: this.Model.Id,
                    model: this.Model
                },
                success: function (data) {
                    if (data.errors == "success") {
                        location.reload();
                    }
                    else {
                        // show error
                    }
                }
            });
        },
        checkAll: function (dataTable) {
            console.log(dataTable);
            if (dataTable == "ClaimRegisterData") {
                this.isCheckAllClaimRegisterData = !this.isCheckAllClaimRegisterData;
                var selectedCheck = new Array();
                if (this.isCheckAllClaimRegisterData) { // Check all
                    this.ClaimRegisterData.forEach(function (item) {
                        selectedCheck.push(item.Id);
                    });
                } else {
                    selectedCheck = [];
                }

                this.selected = selectedCheck;
            } else if (dataTable == "Exceptions") {
                this.isCheckAllExceptions = !this.isCheckAllExceptions;
                var selectedCheck = new Array();
                if (this.isCheckAllExceptions) { // Check all
                    this.Exceptions.forEach(function (item) {
                        selectedCheck.push(item.Id);
                    });
                } else {
                    selectedCheck = [];
                }

                this.selected = selectedCheck;
            } else if (dataTable == "ClaimRetroData") {
                this.isCheckAllClaimRetroData = !this.isCheckAllClaimRetroData;
                var selectedCheck = new Array();
                if (this.isCheckAllClaimRetroData) { // Check all
                    this.ClaimRetroData.forEach(function (item) {
                        selectedCheck.push(item.Id);
                    });
                } else {
                    selectedCheck = [];
                }

                this.selected = selectedCheck;
            } else if (dataTable == "SummaryPaidClaims") {
                this.isCheckAllSummaryPaidClaims = !this.isCheckAllSummaryPaidClaims;
                var selectedCheck = new Array();
                if (this.isCheckAllSummaryPaidClaims) { // Check all
                    this.ClaimRetroData.forEach(function (item) {
                        selectedCheck.push(item.Id);
                    });
                } else {
                    selectedCheck = [];
                }

                this.selected = selectedCheck;
            } else if (dataTable == "SummaryPendingClaims") {
                this.isCheckAllSummaryPendingClaims = !this.isCheckAllSummaryPendingClaims;
                var selectedCheck = new Array();
                if (this.isCheckAllSummaryPendingClaims) { // Check all
                    this.ClaimRetroData.forEach(function (item) {
                        selectedCheck.push(item.Id);
                    });
                } else {
                    selectedCheck = [];
                }

                this.selected = selectedCheck;
            } else if (dataTable == "SummaryClaimsRemoved") {
                this.isCheckAllSummaryClaimsRemoved = !this.isCheckAllSummaryClaimsRemoved;
                var selectedCheck = new Array();
                if (this.isCheckAllSummaryClaimsRemoved) { // Check all
                    this.ClaimRetroData.forEach(function (item) {
                        selectedCheck.push(item.Id);
                    });
                } else {
                    selectedCheck = [];
                }

                this.selected = selectedCheck;
            }
        },
        updateCheckall: function (dataTable) {
            if (dataTable == "ClaimRegisterData") {
                if (this.selected.length == this.ClaimRegisterData.length) {
                    this.isCheckAll = true;
                } else {
                    this.isCheckAll = false;
                }
            } else if (dataTable == "Exceptions") {
                if (this.selected.length == this.Exceptions.length) {
                    this.isCheckAllExceptions = true;
                } else {
                    this.isCheckAllExceptions = false;
                }
            } else if (dataTable == "ClaimRetroData") {
                if (this.selected.length == this.ClaimRetroData.length) {
                    this.isCheckAllClaimRetroData = true;
                } else {
                    this.isCheckAllClaimRetroData = false;
                }
            } else if (dataTable == "SummaryPaidClaims") {
                if (this.selected.length == this.SummaryPaidClaims.length) {
                    this.isCheckAllSummaryPaidClaims = true;
                } else {
                    this.isCheckAllSummaryPaidClaims = false;
                }
            } else if (dataTable == "SummaryPendingClaims") {
                if (this.selected.length == this.SummaryPendingClaims.length) {
                    this.isCheckAllSummaryPendingClaims = true;
                } else {
                    this.isCheckAllSummaryPendingClaims = false;
                }
            } else if (dataTable == "SummaryClaimsRemoved") {
                if (this.selected.length == this.SummaryClaimsRemoved.length) {
                    this.isCheckAllSummaryClaimsRemoved = true;
                } else {
                    this.isCheckAllSummaryClaimsRemoved = false;
                }
            }
        },
    }
});