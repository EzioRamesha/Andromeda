
function initTokenfield(index, isAdd = false) {
    var accountForCount1 = 0;

    $(document).ready(function () {

        $('#account' + index + 'TokenField').on('tokenfield:createtoken', function (e) {
            var existingTokens = $(this).tokenfield('getTokens');
            if (accountForCount1 != 0) {
                $.each(existingTokens, function (el, token) {
                    if (token.value === e.attrs.value) {
                        e.preventDefault();
                    }
                });
            }
        })
        .on('tokenfield:createdtoken', function (e) {
            accountForCount1 += 1;
            $("#account" + index + "TokenField-tokenfield").removeAttr('placeholder');
            if (isAdd) {
                app.TreatyCodes[index].AccountFor = $(this).tokenfield('getTokens').map(e => e.value).join(",");
            }
        })
        .on('tokenfield:removedtoken', function (e) {
            accountForCount1 -= 1;
            if (accountForCount1 == 0) {
                $("#account" + index + "TokenField-tokenfield").attr("placeholder", "Type here");
            }
            if (isAdd) {
                app.TreatyCodes[index].AccountFor = $(this).tokenfield('getTokens').map(e => e.value).join(",");
            }
        })
        .tokenfield();

    });
}

var app = new Vue({
    el: '#app',
    data: {
        Treaty: TreatyModel,
        Cedants: CedantList,
        TreatyCodes: TreatyCodes,
        StatusData: StatusData,
        TreatyTypeData: TreatyTypeData,
        TreatyStatusData: TreatyStatusData,
        SortIndex: this.TreatyCodes.length,
        TreatyCodeItems: TreatyCodeItems,
    },
    methods: {
        addNew: function () {
            this.SortIndex++;
            this.TreatyCodes.push({
                SortIndex: this.SortIndex,
                OldTreatyCodeId: 0,
                AccountFor: "",
                Status: "1",
                TreatyTypePickListDetailId: "",
                TreatyStatusPickListDetailId: "",
                LineOfBusinessPickListDetailId: "",
            });
            this.getTreatyCode();
            initTokenfield(this.SortIndex - 1, true);
        },
        removeTreatyCode: function (index) {
            var item = this.TreatyCodes[index];
            var defaultTreatyCodes = this.TreatyCodes;
            if (item != null && item.Id != null && item.Id != 0) {
                $.ajax({
                    url: ValidateTreatyCodeDeleteUrl,
                    type: "POST",
                    data: { treatyCodeId: item.Id },
                    cache: false,
                    async: false,
                    success: function (data) {
                        var valid = data.valid;
                        if (valid) {
                            var arr = [];
                            defaultTreatyCodes.forEach(
                                function (treatyCodes, detailIndex) {
                                    arr.push($('#account' + detailIndex + 'TokenField').tokenfield('getTokens'));
                                }
                            );

                            defaultTreatyCodes.splice(index, 1);
                            this.SortIndex--;

                            var i = 0;
                            arr.forEach(
                                function (a, arrIndex) {
                                    if (arrIndex != index) {
                                        $('#account' + i + 'TokenField').tokenfield('setTokens', a);
                                        defaultTreatyCodes[i].AccountFor = a.map(e => e.value).join(",");
                                        i++;
                                    }
                                }
                            );
                            this.TreatyCodes = defaultTreatyCodes;
                        } else {
                            //alert('The Treaty Code "' + item.Code + '" In Use');                            
                            $("#errorRecordInUsed").css("display", "block");
                            $("#errorRecordInUsed").text('The Treaty Code "' + item.Code + '" In Use');
                        }
                    }
                });
            } else {
                var arr = [];
                this.TreatyCodes.forEach(
                    function (treatyCodes, detailIndex) {
                        arr.push($('#account' + detailIndex + 'TokenField').tokenfield('getTokens'));
                    }
                );

                this.TreatyCodes.splice(index, 1);
                this.SortIndex--;

                var details = this.TreatyCodes;
                var i = 0;
                arr.forEach(
                    function (a, arrIndex) {
                        if (arrIndex != index) {
                            $('#account' + i + 'TokenField').tokenfield('setTokens', a);
                            details[i].AccountFor = a.map(e => e.value).join(",");
                            i++;
                        }
                    }
                );
                this.TreatyCodes = details;
            }
        },
        getTreatyCode: function () {
            var cedantId = this.Treaty.CedantId;
            this.TreatyCodeItems = [];

            $.ajax({
                url: GetTreatyCodeUrl,
                type: "POST",
                data: { cedantId: cedantId },
                cache: false,
                async: false,
                success: function (data) {
                    TreatyCodeItems = data.treatyCodeBos;
                }
            });
            this.TreatyCodeItems = TreatyCodeItems;
        },
        filterTreatyCode: function (index) {
            var item = this.TreatyCodes[index];

            if (item != null && item.Id != null) {
                return this.TreatyCodeItems.filter(function (treatyCodeItem) {
                    return treatyCodeItem.Id != item.Id;
                })
            } else {
                return this.TreatyCodeItems;
            }
        },
        autoExpandTextarea: function (id) {
            var tArea = $('#' + id);
            this.textAreaWidth = tArea.width();
            this.textAreaHeight = tArea.height();
            tArea.autoResize();
            tArea.trigger('keyup');

            tArea.on('keypress', function (evt) {
                var evt = (evt) ? evt : ((event) ? event : null);
                if (evt.keyCode == 13)
                    return false;
            });
        },
        autoCollapseTextarea: function (id) {
            var tArea = $('#' + id);
            tArea.height(this.textAreaHeight);
        },
    },
    created: function () {

        var i = 0;
        if (this.TreatyCodes.length > 0) {
            this.TreatyCodes.forEach(function (treatyCode) {

                if (treatyCode != null) {

                    if (treatyCode.Id != null) {
                        $.ajax({
                            url: GetTreatyOldCodeUrl,
                            type: "POST",
                            data: { treatyCodeId: treatyCode.Id },
                            cache: false,
                            async: false,
                            success: function (data) {
                                if (data.treatyOldCodeIds.length > 0)
                                    treatyCode.OldTreatyCodeId = data.treatyOldCodeIds;
                            }
                        });
                    }

                    if (treatyCode.TreatyTypePickListDetailId == null) {
                        treatyCode.TreatyTypePickListDetailId = "";
                    }
                    if (treatyCode.TreatyStatusPickListDetailId == null) {
                        treatyCode.TreatyStatusPickListDetailId = "";
                    }
                    if (treatyCode.LineOfBusinessPickListDetailId == null) {
                        treatyCode.LineOfBusinessPickListDetailId = "";
                    }

                    initTokenfield(i);
                    i++;
                }
            });
        }
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});