$(document).ready(function () {
    GetUserByDepartment(CalMLReModel.DepartmentId);
});

function GetUserByDepartment(deptId) {
    var userId = CalMLReModel.UserId;

    $('#UserId option').remove();
    if (!isNaN(deptId)) {

        var userData;
        $.ajax({
            url: GetUsersUrl,
            type: "POST",
            data: { departmentId: deptId },
            cache: false,
            async: false,
            success: function (data) {
                userData = data.users;
            }
        });

        userData.forEach(function (entry) {
            $('#UserId').append(new Option(entry.Text, entry.Value, false, entry.Value == userId));
        });
    }
    else {
        $('#UserId').append(new Option("Please select", ""));
    }
    $('#UserId').selectpicker('refresh');
}

var app = new Vue({
    el: '#app',
    data: {
        CalMLRe: CalMLReModel,
        ClaimCodes: ClaimCodeData,
        CalMLReDetails: CALMLReDetailList ? CALMLReDetailList : [],
        SortIndex: 0,
        ClaimCodeMLRe: [],
    },
    methods: {
        addNew: function () {
            this.CalMLReDetails.push({ ClaimCodeId: "" });
            this.SortIndex += 1;
        },
        addAll: function () {
            var listClaimCodes = [];
            if (this.CalMLReDetails !== null) {
                for (let i = 0; i < this.CalMLReDetails.length; i++) {
                    if (typeof this.CalMLReDetails[i].ClaimCodeId !== "number") {
                        listClaimCodes.push(this.CalMLReDetails[i].ClaimCodeId);
                    } else {
                        listClaimCodes.push(this.CalMLReDetails[i].ClaimCodeId.toString());
                    }
                }
                var ClaimCodesData;
                $.ajax({
                    url: GetClaimCodesUrl,
                    type: "POST",
                    data: {},
                    cache: false,
                    async: false,
                    success: function (data) {
                        ClaimCodesData = data.claimCodes
                    }
                });

                var notInClaimCode = ClaimCodesData.filter(f => !listClaimCodes.includes(f.Value));

                for (let i = 1; i < notInClaimCode.length; i++) {
                    if (notInClaimCode[i].Value !== "") {
                        this.CalMLReDetails.push({ ClaimCodeId: notInClaimCode[i].Value });
                        this.SortIndex += 1;
                    }
                }

            } else {
                var ClaimCodesData;
                $.ajax({
                    url: GetClaimCodesUrl,
                    type: "POST",
                    data: {},
                    cache: false,
                    async: false,
                    success: function (data) {
                        ClaimCodesData = data.claimCodes
                    }
                });
                for (let i = 1; i < ClaimCodesData.length; i++) {
                    setTimeout(this.CalMLReDetails.push({ ClaimCodeId: ClaimCodesData[i].Value }), 5000);
                    this.SortIndex += 1;
                }
                this.$forceUpdate();
            }

        },
        removeDetail: function (index) {
            this.CalMLReDetails.splice(index, 1);
            this.ClaimCodeMLRe.splice(index, 1);
            this.SortIndex -= 1;
        },
        getClaimCodeVal: function (index) {
            var item = this.CalMLReDetails[index];
            this.ClaimCodeMLRe[index] = item.ClaimCodeId;
        },
        filterClaimCodes: function (index) {
            var item = this.CalMLReDetails[index];

            var ccId = null;
            if (item != null && item.Id != null) {
                ccId = item.ClaimCodeId;
            }

            var itemsWithoutCurrent;
            if (item != null) {
                var exceptIndex = index;
                //var itemsWithoutCurrent = this.ClaimCodeMLRe.filter(function (x) { return x !== ccId; });
                itemsWithoutCurrent = this.ClaimCodeMLRe.filter((value, index) => exceptIndex !== index);
            }

            var ClaimCodesData;
            $.ajax({
                url: GetClaimCodesUrl,
                type: "POST",
                data: { claimCodeId: ccId },
                cache: false,
                async: false,
                success: function (data) {
                    ClaimCodesData = data.claimCodes
                }
            });

            return this.ClaimCodes = ClaimCodesData.filter(f => !itemsWithoutCurrent.includes(f.Value));
        },
    },
    created: function () {
        this.SortIndex = (this.CalMLReDetails) ? this.CalMLReDetails.length - 1 : -1;

        var items = new Array();
        if (this.CalMLReDetails.length > 0) {
            this.CalMLReDetails.forEach(function (calMLReDetail) {

                if (calMLReDetail != null) {
                    if (calMLReDetail.ClaimCodeId == 0) {
                        calMLReDetail.ClaimCodeId = "";

                        items.push(calMLReDetail.ClaimCodeId);
                    }
                    else {
                        items.push(calMLReDetail.ClaimCodeId.toString());
                    }
                }
            });
        }

        this.ClaimCodeMLRe = items;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});