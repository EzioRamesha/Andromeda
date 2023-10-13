var app = new Vue({
    el: '#app',
    data: {
        RetroTreaty: Model,
        RetroTreatyDetails: RetroTreatyDetails,
        AddNewDetailModal: {
            TreatyCodeId: null,
            TreatyTypeId: null,
            IsToAggregate: null,
            Configs: []
        },
    },
    methods: {
        resetAddNewDetailModal: function () {
            this.AddNewDetailModal.TreatyCodeId = null;
            this.AddNewDetailModal.TreatyTypeId = null;
            this.AddNewDetailModal.IsToAggregate = null;
            this.AddNewDetailModal.Configs = [];

            this.resetAddNewDetailError();
            $('#addNewDetailModal').modal('show');

            this.$nextTick(function () {
                $('#TreatyCodeId').selectpicker('refresh');
                $('#TreatyTypeId').selectpicker('refresh');
                $('#IsToAggregate').selectpicker('refresh');
            })
        },
        resetAddNewDetailError: function () {
            $('#addNewDetailError').empty();
            $('#addNewDetailError').hide();
        },
        searchConfig: function () {
            this.resetAddNewDetailError();

            var treatyCodeId = this.AddNewDetailModal.TreatyCodeId;
            var treatyTypeId = this.AddNewDetailModal.TreatyTypeId;
            var isToAggregate = this.AddNewDetailModal.IsToAggregate;

            var configs = [];
            var errors = [];
            $.ajax({
                url: SearchConfigUrl,
                type: "POST",
                data: { treatyCodeId: treatyCodeId, treatyTypeId: treatyTypeId, isToAggregate: isToAggregate, retroTreatyId: Model.Id },
                cache: false,
                async: false,
                success: function (data) {
                    if (data.success) {
                        configs = data.bos;
                    } else {
                        errors.push(data.error);
                    }
                }
            });
            this.AddNewDetailModal.Configs = configs;
            if (configs.length == 0) {
                $('#addNewDetailError').append(arrayToUnorderedList(errors));
                $('#addNewDetailError').show();
            } 
        },
        selectConfig: function () {
            this.resetAddNewDetailError();

            var configIds = $('[name="SelectedConfigs"]:checked').map(function () {
                return this.value;
            }).get().join(',');  

            if (configIds == '' || configIds == null) {
                $('#addNewDetailError').append(arrayToUnorderedList(["No Configurations Selected"]));
                $('#addNewDetailError').show();

                return;
            }

            var details = [];
            $.ajax({
                url: CreateDetailsUrl,
                type: "POST",
                data: { retroTreatyId: Model.Id, configIds: configIds },
                cache: false,
                async: false,
                success: function (data) {
                    details = data.bos;
                }
            });
                
            this.RetroTreatyDetails.push(...details);
            $('#addNewDetailModal').modal('hide');
        },
        editDetailLink: function (index) {
            var retroTreatyDetail = this.RetroTreatyDetails[index];
            return EditDetailUrl + '/' + retroTreatyDetail.Id + '?retroTreatyId=' + this.RetroTreaty.Id;
        },
        deleteDetailLink: function (index) {
            var retroTreatyDetail = this.RetroTreatyDetails[index];
            return DeleteDetailUrl + '/' + retroTreatyDetail.Id + '?retroTreatyId=' + this.RetroTreaty.Id;
        },
    }
});

function selectAll(checked) {
    $('[name="SelectedConfigs"]').prop('checked', checked);
}

function resetSelectAll() {
    var allSelected = $('[name="SelectedConfigs"]:not(:checked)').length == 0;
    $('#selectAllConfigChk').prop('checked', allSelected);

}