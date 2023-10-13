
var app = new Vue({
    el: '#app',
    data: {
        GroupMasterLetter: Model,
        // Ri Group Slip
        GroupMasterLetterDetails: GroupMasterLetterDetails ? GroupMasterLetterDetails : [],
        GroupMasterLetterDetailMaxIndex: 0,
        GroupMasterLetterDetailData: {
            cedantId: null,
        },
        GroupReferralDetail: null,
        GroupMasterLetterDetailDataValidation: [],
        isCheckAll: false,
        selected: [],
        // Others
        DropDownCedants: DropDownCedants,
    },
    methods: {
        // Ri Group Slip
        removeDetail: function (index) {
            this.GroupMasterLetterDetails.splice(index, 1);
            this.GroupMasterLetterDetailMaxIndex--;

            this.GroupMasterLetter.NoOfRiGroupSlip = this.GroupMasterLetterDetails.length;
        },
        resetModalData: function () {
            this.GroupMasterLetterDetailDataValidation = [];
            this.GroupMasterLetterDetailData.cedantId = this.GroupMasterLetter.CedantId;            

            this.$nextTick(function () {
                $("#cedantId").prop('disabled', true);
                $("#cedantId").selectpicker('refresh');
            });

            this.searchGroupReferral();
        },
        validateDetail: function () {
            this.GroupMasterLetterDetailDataValidation = [];
            if (this.GroupMasterLetterDetailData.cedantId == null || this.GroupMasterLetterDetailData.cedantId == "" || this.GroupMasterLetterDetailData.cedantId == 0)
                this.GroupMasterLetterDetailDataValidation.push("Ceding Company is required");

            return this.GroupMasterLetterDetailDataValidation.length == 0;
        },
        searchGroupReferral: function () {
            if (!this.validateDetail())
                return;
  
            var obj = {
                cedantId: this.GroupMasterLetterDetailData.cedantId,
            };

            var groupReferralBos;
            $.ajax({
                url: GetByCedantIdHasRiGroupSlipUrl ? GetByCedantIdHasRiGroupSlipUrl : null,
                type: "POST",
                data: obj,
                cache: false,
                async: false,
                success: function (data) {
                    console.log(data)
                    groupReferralBos = data.GroupReferrals;
                }
            });

            this.selected = [];
            this.isCheckAll = false;

            if (groupReferralBos == null)
                this.GroupMasterLetterDetailDataValidation.push("No record found");

            var groupReferralIds = this.GroupMasterLetterDetails.map(b => b.TreatyPricingGroupReferralId);
            this.GroupReferralDetail = groupReferralBos.filter(function (bo) {
                return !groupReferralIds.includes(bo.Id);
            });
        },
        addGroupReferral: function () {
            if (this.selected.length == 0) {
                this.GroupMasterLetterDetailDataValidation.push("No Ri Group Slip selected.");
                return;
            }

            var details = new Array();
            details = this.GroupReferralDetail.filter(f => this.selected.includes(f.Id))

            for (var k in details) {
                this.GroupMasterLetterDetails.push({ TreatyPricingGroupReferralId: details[k].Id, TreatyPricingGroupReferralBo: details[k] });
            }
            this.GroupMasterLetterDetailMaxIndex++;

            this.GroupMasterLetter.NoOfRiGroupSlip = this.GroupMasterLetterDetails.length;

            $('#addRiGroupSlipModal').modal('toggle');
        },
        checkAll: function () {
            this.isCheckAll = !this.isCheckAll;
            var selectedCheck = new Array();
            if (this.isCheckAll) { // Check all
                this.GroupReferralDetail.forEach(function (item) {
                    selectedCheck.push(item.Id);
                });
            } else {
                selectedCheck = [];
            }

            this.selected = selectedCheck;
        },
        updateCheckall: function () {
            if (this.selected.length == this.GroupReferralDetail.length) {
                this.isCheckAll = true;
            } else {
                this.isCheckAll = false;
            }
        },
    },
    created: function () {
        this.GroupMasterLetterDetailMaxIndex = (this.GroupMasterLetterDetails) ? this.GroupMasterLetterDetails.length - 1 : -1; 

        if (this.GroupMasterLetter.CedantId == 0)
            this.GroupMasterLetter.CedantId = "";
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});