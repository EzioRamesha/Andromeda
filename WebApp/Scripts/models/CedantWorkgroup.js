var app = new Vue({
    el: '#app',
    data: {
        CedantWorkgroup: CedantWorkgroupModel,
        CedantWorkgroupCedants: CedantWorkgroupCedants,
        CedantWorkgroupUsers: CedantWorkgroupUsers,
        Cedants: Cedants,
        Users: Users,
        CedantMaxIndex: -1,
        UserMaxIndex: -1,
    },
    methods: {
        openCedantSelection: function () {
            this.Cedants.map(function (cedant) {
                cedant.selected = false;
                return cedant;
            });

            var toggleCedantCheck = this.toggleCedantCheck;
            this.CedantWorkgroupCedants.forEach(
                function (cedantWorkgroupCedant, index) {
                    var cedantId = cedantWorkgroupCedant.CedantId;
                    toggleCedantCheck(cedantId, true);
                }
            );
        },
        saveCedantSelection: function () {
            var cedants = [];
            this.Cedants.filter(c => c.selected).forEach(
                function (cedant, index) {
                    cedants.push({
                        CedantId: cedant.Id,
                        CedantName: cedant.Code + ' - ' + cedant.Name,
                    })
                }
            );
            this.CedantWorkgroupCedants = cedants;
            this.CedantMaxIndex = cedants.length - 1;
        },
        toggleCedantCheck: function (cedantId, initial = false) {
            var selectedCedant = this.Cedants.find(c => c.Id == cedantId);

            var index = this.Cedants.indexOf(selectedCedant);
            if (initial || !selectedCedant.selected)
                this.Cedants[index].selected = true;
            else
                this.Cedants[index].selected = false;

            this.$forceUpdate();
        },
        removeCedant: function (index) {
            this.CedantWorkgroupCedants.splice(index, 1);
            this.CedantMaxIndex--;
        },
        openUserSelection: function () {
            this.Users.map(function (user) {
                user.selected = false;
                return user;
            });

            var toggleUserCheck = this.toggleUserCheck;
            this.CedantWorkgroupUsers.forEach(
                function (cedantWorkgroupUser, index) {
                    var userId = cedantWorkgroupUser.UserId;
                    toggleUserCheck(userId, true);
                }
            );
        },
        saveUserSelection: function () {
            var users = [];
            this.Users.filter(c => c.selected).forEach(
                function (user, index) {
                    users.push({
                        UserId: user.Id,
                        UserName: user.UserName,
                    })
                }
            );
            this.CedantWorkgroupUsers = users;
            this.UserMaxIndex = users.length - 1;
        },
        toggleUserCheck: function (userId, initial = false) {
            var selectedUser = this.Users.find(c => c.Id == userId);

            if (selectedUser) {
                var index = this.Users.indexOf(selectedUser);
                if (initial || !selectedUser.selected)
                    this.Users[index].selected = true;
                else
                    this.Users[index].selected = false;
            }

            this.$forceUpdate();
        },
        removeUser: function (index) {
            this.CedantWorkgroupUsers.splice(index, 1);
            this.UserMaxIndex--;
        },
    },
    created: function () {
        this.openCedantSelection();
        this.openUserSelection();

        this.CedantMaxIndex = this.CedantWorkgroupCedants.length - 1;
        this.UserMaxIndex = this.CedantWorkgroupUsers.length - 1;
    }
});

function searchCedant(cedant) {
    // Declare variables
    var input, filter, table, tr, code, name, i, codeTxt, nameTxt;
    input = document.getElementById("cedantSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("cedantListTable");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        code = tr[i].getElementsByTagName("td")[0];
        name = tr[i].getElementsByTagName("td")[1];
        if (code || name) {
            codeTxt = code.textContent || code.innerText;
            nameTxt = name.textContent || name.innerText;
            if (codeTxt.toUpperCase().indexOf(filter) > -1 || nameTxt.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

function searchUser(user) {
    // Declare variables
    var input, filter, table, tr, userName, fullName, i, userNameTxt, fullNameTxt;
    input = document.getElementById("userSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("userListTable");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        userName = tr[i].getElementsByTagName("td")[0];
        fullName = tr[i].getElementsByTagName("td")[1];
        if (userName || fullName) {
            userNameTxt = userName.textContent || userName.innerText;
            fullNameTxt = fullName.textContent || fullName.innerText;
            if (userNameTxt.toUpperCase().indexOf(filter) > -1 || fullNameTxt.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}