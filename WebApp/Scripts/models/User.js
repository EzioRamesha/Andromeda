function getAccessGroups(departmentId) {
    var selectedAccessGroupId = AccessGroupId;

    $.ajax({
        url: GetAccessGroupsUrl,
        type: "POST",
        data: { departmentId: departmentId },
        cache: false,
        async: false,
        success: function (data) {
            var AccessGroups = data.AccessGroupBos;
            refreshDropDownItems('accessGroupId', AccessGroups, selectedAccessGroupId, 'Name')

            $('#accessGroupId').selectpicker('refresh');
        }
    });
}

function changeLoginMethod(loginMethod) {
    var emailInput = $("#emailInput");
    var nameInput = $("#nameInput");
    var userName = $("#usernameInput").val();

    $('#findAdDiv').hide();
    $('#passwordDiv').hide();

    if (loginMethod == LoginMethodAD) {
        emailInput.attr('readonly', true);
        emailInput.attr('placeholder', '');
        emailInput.val('');

        nameInput.attr('readonly', true);
        nameInput.attr('placeholder', '');
        nameInput.val('');

        $('#findAdDiv').show();

        if (OriginalLoginMethod == LoginMethodAD && OriginalUserName == userName) {
            emailInput.val(OriginalEmail || '');
            nameInput.val(OriginalFullName || '');
        }

    } else {
        emailInput.prop('readonly', false);
        emailInput.attr('placeholder', 'Type Here');

        nameInput.prop('readonly', false);
        nameInput.attr('placeholder', 'Type Here');

        if (loginMethod == LoginMethodPassword) {
            $('#passwordDiv').show();
        }

        initTooltip();
    }
}

function initTooltip() {
    //$('[data-toggle="tooltip"]').tooltip();
    $('body').tooltip({ selector: '[data-toggle="tooltip"]' });
}

$(document).ready(function () {
    changeLoginMethod(UserModel.LoginMethod)
});

var app = new Vue({
    el: '#app',
    data: {
        User: UserModel,
        Documents: DocumentBos,
        StatusHistories: StatusHistoryBos,
        StatusClassList: (typeof StatusClassList === 'undefined') ? null : StatusClassList,
        LoginMethodItems: LoginMethodItems,
        DocumentTypeItems: DocumentTypeItems,
        LoginMethodAD: LoginMethodAD,
        DocumentMaxIndex: 0,
        DocumentModal: {
            CreatedByName: AuthUserName ? AuthUserName : null,
            File: null,
            Type: 0,
            Description: '',
        },
    },
    methods: {
        resetDocumentModal() {
            this.DocumentModal.CreatedAtStr = moment().format(DateTimeFormat);
            this.DocumentModal.ModuleId = this.User.ModuleId;
            this.DocumentModal.ObjectId = this.User.Id;
            this.DocumentModal.File = null;
            this.DocumentModal.Filename = null;
            this.DocumentModal.Type = 0;
            this.DocumentModal.Description = "";

            clearSelectedFiles();
        },
        saveDocument() {
            var file = getFiles()[0];

            var save = true;
            if (!this.User.Id)
                save = false;
            var document = saveDocument(this.DocumentModal, file, save);

            if (document != null) {
                this.Documents.push(Object.assign({}, document));
                this.DocumentMaxIndex++;
            }
        },
        removeDocument(index) {
            var document = this.Documents[index];

            if (deleteDocument(document)) {
                this.Documents.splice(index, 1);
                this.DocumentMaxIndex--;
            }
        },
        generatePassword() {
            var passwordInput = $("#passwordInput");
            var confirmPasswordInput = $("#confirmPasswordInput");
            $("#generatePassword").prop('disabled', true);
            $.ajax({
                url: GeneratePasswordUrl,
                type: "POST",
                cache: false,
                async: false,
                success: function (data) {
                    $("#generatePassword").prop('disabled', false);
                    passwordInput.val(data.password || '');
                    confirmPasswordInput.val(data.password || '');
                }
            });
        },
        findInAD() {
            var userName = $("#usernameInput").val();
            var emailInput = $("#emailInput");
            var nameInput = $("#nameInput");
            var user = this.User;
            $("#findInAD").prop('disabled', true);
            $.ajax({
                url: ActiveDirectoryUrl,
                type: "POST",
                data: { userName: userName },
                cache: false,
                async: false,
                success: function (data) {
                    $("#findInAD").prop('disabled', false);
                    user.Email = '';
                    user.Name = '';
                    emailInput.val(data.email || '');
                    nameInput.val(data.name || '');

                    if (data.error) {
                        alert(data.error);
                    }
                },
            });

            this.User = user;
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
        if (this.Documents)
            this.DocumentMaxIndex = this.Documents.length - 1;
    },
    updated() {
        $(this.$refs.select).selectpicker('refresh');
    }
});