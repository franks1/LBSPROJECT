

$('a[data-url]').attr('href', 'javascript://');
function AssignPermissions() {

    $('#PermissionList').on('click', 'a.AddPermission', function () {
        var anchor = $(this);
        var id = $('#Id').val();
        var permissionId = anchor.data('permissionid');
        var roleID = id;
        $.post("/Admin/AssignRoleFromView", { roleId: id, permissionId: permissionId }, function (data) {
            if (data.message == 'Saved') {
                $.get("/Admin/AppliedRolePermissions", { RoleId: id }, function (datas) {
                    $('#PermissionView').html(datas);
                    LoadPermissions(id);
                });
            }
            else {
                $.get("/Admin/AppliedRolePermissions", { RoleId: id }, function (datas) {
                    $('#PermissionView').html(datas);
                    LoadPermissions(id);
                });
            }
        }); 
    });


}

function DeletePermission() {
    $('#PermissionView').on('click', 'a.deletePermission', function () {
        var anchor = $(this);
        var id = $('#Id').val();
        var permissionId = anchor.data('permissionid');
        var roleID = id;
        $.post("/Admin/DeletePermissionFromRole", { roleId: id, permissionId: permissionId }, function (data) {

            $('#PermissionView').load("/Admin/AppliedRolePermissions", { RoleId: id }, function (datas) {
                $('#PermissionView').html(datas);
                LoadPermissions(id);
            });
        });
    });
}
function LoadPermissions(id) {
    $.get("/Admin/GetAllPermissions", { RoleId: id }, function (data) {
        $('#PermissionList').html(data);
        $('a[data-url]').attr('href', 'javascript://');
    });
};