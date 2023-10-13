
$(document).ready(function () {
    initial();

    getAdditionalCheckBox().each(function () {
        $("#" + this.id).change(function () {
            checkboxAdditionalOnChange();
        });
    });

    getAllCheckBox('C').each(function () {
        $("#" + this.id).change(function () {
            checkboxOnChange('createAll', 'C');
        });
    });

    getAllCheckBox('R').each(function () {
        $("#" + this.id).change(function () {
            checkboxOnChange('readAll', 'R');
        });
    });

    getAllCheckBox('U').each(function () {
        $("#" + this.id).change(function () {
            checkboxOnChange('updateAll', 'U');
        });
    });

    getAllCheckBox('D').each(function () {
        $("#" + this.id).change(function () {
            checkboxOnChange('deleteAll', 'D');
        });
    });

    $('#createAll').change(function () {
        check('C', this.checked);
    });
    $('#readAll').change(function () {
        check('R', this.checked);
    });
    $('#updateAll').change(function () {
        check('U', this.checked);
    });
    $('#deleteAll').change(function () {
        check('D', this.checked);
    });
    $('#additionalAll').change(function () {
        checkAdditional(this.checked);
    });
});

var app = new Vue({
    el: '#app',
    data: {
        AccessGroup: AccessGroup,
        //Documents: (typeof DocumentBos === 'undefined') ? null : DocumentBos,
        Documents: DocumentBos,
        DocumentTypeItems: (typeof DocumentTypeItems === 'undefined') ? null : DocumentTypeItems,
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
            this.DocumentModal.ModuleId = this.AccessGroup.ModuleId;
            this.DocumentModal.ObjectId = this.AccessGroup.Id;
            this.DocumentModal.File = null;
            this.DocumentModal.Filename = null;
            this.DocumentModal.Type = 0;
            this.DocumentModal.Description = "";

            clearSelectedFiles();
        },
        saveDocument() {
            var file = getFiles()[0];

            var save = true;
            if (!this.AccessGroup.Id)
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