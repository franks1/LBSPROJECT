var MainUserRoles = function () {
    var self = this;
    self.Roles = ko.observableArray([]);

    self.LoadDetails = function () {
        var RoleV = $('#RsId').val();
        $.get('/Admin/getRolesForSelectedUser/', { Id: RoleV }, function (data) {
            self.Roles(data.data);
        });
    }

    self.EnableRoleRemoval = function () {
        $('#idTable').on('click', 'button[data-bind]', function (e) {
            var RoleId = $(this).data('vid');
            var RoleV = $('#RsId').val();
            $('#rlist').addClass('loading');
            $.post('/Admin/RemoveRoleForUser/', { UserId: RoleV, RoleId: RoleId }, function (data) {
                self.LoadDetails();
                $('#rlist').removeClass('loading');
            })
        });
    }
}