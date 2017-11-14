window.onload = function () {
   var makeAsyncDataSource = function (file) {
        return new DevExpress.data.CustomStore({
            loadMode: "raw",
            key: "RequestCode",
            load: function () {
                var deferred = $.Deferred(),
                    args = {};
                $.ajax({
                    url: "/backoffice/getGangRequest/",
                    data: args,
                    success: function (result) {
                        deferred.resolve(result.data);
                    },
                    error: function () {
                        deferred.reject("Data Loading Error");
                    }
                    //,
                    //timeout: 5000
                });

                return deferred.promise();
            }
        });
    };

   var MAIN_DS_ =   new DevExpress.data.DataSource({
       store: makeAsyncDataSource('')
   });

   $('.ui.f.checkbox').checkbox({
        onChecked: function () {
            var details = $("#gridBox").dxDropDownBox('instance').option('value');

            if (details == null || details == '') {
                toastr.info('SELECT GANG REQUEST', 'EXEMPT CASUAL');
                $('.ui.f.checkbox').checkbox('set unchecked');
                return;
            }

            $('.ui.e.modal').modal('setting', {
                allowMultiple: false,
                onHidden: function () {
                    $('.ui.f.checkbox').checkbox('set unchecked');
                },
                transition: 'horizontal flip'
            }).modal('show');;

        },
        onUnchecked: function () {


        }
    });

    $('.ui.a.checkbox').checkbox({
        onChecked: function ()
        {
            var details = $("#gridBox").dxDropDownBox('instance').option('value');

            if (details == null || details == '')
            {
                toastr.info('SELECT GANG REQUEST', 'GANG REQUEST APPROVAL');
                $('.ui.a.checkbox').checkbox('set unchecked');
                return;
            }
            $('.ui.a.modal').modal('setting', {
                allowMultiple: false,
                onHidden: function () {
                    $('.ui.a.checkbox').checkbox('set unchecked');
                },
                transition: 'horizontal flip'
            }).modal('show');;

        },
        onUnchecked: function () {


        }
    });
    var model = new approvalWorkList(MAIN_DS_);
    ko.applyBindings(model, document.getElementById('koserver'));


    loadGangMembers(model, '');
    loadGangContainers('');
    $('.menu .item').tab();
  
    justAccount();

    function justAccount() {
        var dataGrid;

        $("#gridBox").dxDropDownBox({
            // value: 3,
            // valueExpr: "Account",
          
            placeholder: "Select Code...",
            displayExpr: function (item) {
                LoadDetails(item.RequestCode);
                loadGangMembers(model, item.RequestCode);
                loadGangContainers(item.RequestCode);
                return item.RequestCode + '-' + item.Name;
            },
            onValueChanged: function (e) {
                try {
                    var option = e.value;
                    if (option == null || option == '') {
                        $('#txtGangType').val('');
                        $('#txtGang').val('');
                        $('#txtPrepared').val('');
                        $('#txtSchedule').val(''); loadGangMembers(model, '');
                        loadGangContainers('');
                    }

                } catch (e) {

                }
            },
            showClearButton: true,
            dataSource: MAIN_DS_,
            contentTemplate: function (e) {
                var value = e.component.option("value"),
                    $dataGrid = $("<div>").dxDataGrid({
                       
                        dataSource: e.component.option("dataSource"),
                        columns: ["RequestCode", "Name", "RequestDate", "Status"],
                        hoverStateEnabled: true,
                        paging: { enabled: true, pageSize: 10 },
                        filterRow: { visible: true },
                        allowColumnResizing: true,
                        showColumnLines: false,
                        showRowLines: true,
                        rowAlternationEnabled: true,
                        showBorders: true,
                        columnAutoWidth: true,
                        scrolling: { mode: "infinite" },
                        height: 400,
                        selection: { mode: "single" },
                        selectedRowKeys: [value],
                        onSelectionChanged: function (selectedItems) {
                            var keys = selectedItems.selectedRowKeys,
                                hasSelection = keys.length;

                            e.component.option("value", hasSelection ? keys[0] : null);
                        }
                    });

                dataGrid = $dataGrid.dxDataGrid("instance");

                e.component.on("valueChanged", function (args) {
                    dataGrid.selectRows(args.value, false);
                    $("#gridBox").dxDropDownBox('close');
                });
                return $dataGrid;
            }
        });
    }
}



function loadGangMembers(model, code) {
    var $table = $('#tablecasuals').bootstrapTable({
        showColumns: false,
        sortStable: false,
        silentSort: false,
        showToggle: false,
        url: '/BackOffice/getGangMembers/?code=' + code,
        columns: [
            {
                field: 'StaffCode', align: 'left',
                title: 'CODE'
            },
            {
                field: 'FullName', align: 'left',
                title: 'NAME'
            },
            {
                field: 'Description', align: 'left',
                title: 'GANG'
            },
            {
                field: 'GroupName', align: 'left',
                title: 'GROUP'
            }]
    });


    var $table_flagged = $('#tableflagcasuals').bootstrapTable({
        showColumns: false,
        sortStable: false,
        silentSort: false,
        showToggle: false,
        url: '/BackOffice/getGangMembers/?code=' + code,
        onCheck: function (row, $element) {

            model.addFlaggedCasual(row.StaffCode);
            toastr.info(row.StaffCode + ' selected', 'EXEMPT CASUAL MEMBER');
        },
        onUncheck: function (row, $element) {
            model.removeFlaggedCasual(row.StaffCode);
            toastr.success(row.StaffCode + ' deselected', 'EXEMPT CASUAL MEMBER');
        },
        onCheckAll: function (row) {

        },
        onUncheckAll: function (row) {
            model.resetAll();
        },
        columns: [
            {
                checkbox: true,
                align: 'center',
            },
            {
                field: 'StaffCode', align: 'left',
                title: 'CODE'
            },
            {
                field: 'FullName', align: 'left',
                title: 'NAME'
            },
            {
                field: 'Description', align: 'left',
                title: 'GANG'
            },
            {
                field: 'GroupName', align: 'left',
                title: 'GROUP'
            }]
    });

    if (code == '' || code == null) {
        $table.bootstrapTable('load', []);
    }
    else {
        $.getJSON('/BackOffice/getGangMembers/', { code: code }, function (data) {
            $table.bootstrapTable('showLoading');
            $table.bootstrapTable('load', data.data);
            $table.bootstrapTable('hideLoading');
            var options = $table.bootstrapTable('getOptions');
            if (options.totalPages > 0) {
                $table.bootstrapTable('selectPage', options.totalPages);
            }
        });
    }

    if (code == '' || code == null) {
        $table_flagged.bootstrapTable('load', []);
    }
    else {
        $.getJSON('/BackOffice/getGangMembers/', { code: code }, function (data) {
            $table_flagged.bootstrapTable('showLoading');
            $table_flagged.bootstrapTable('load', data.data);
            $table_flagged.bootstrapTable('hideLoading');
            var options = $table.bootstrapTable('getOptions');
            if (options.totalPages > 0) {
                $table_flagged.bootstrapTable('selectPage', options.totalPages);
            }
        });
    }
}


function loadGangContainers(code) {
    var $table = $('#tablecontainers').bootstrapTable({
        showColumns: false,
        sortStable: false,
        silentSort: false,
        showToggle: false,
        url: '/BackOffice/getGangContainers/?code=' + code,
        columns: [
            {
                field: 'Container', align: 'left',
                title: 'CONTAINER'
            },
            {
                field: 'ContainerNumber', align: 'left',
                title: 'NUMBER'
            },
            {
                field: 'Category', align: 'left',
                title: 'GANG TYPE'
            }]
    });

    if (code == '' || code == null) {
        $table.bootstrapTable('load', []);
    }
    else {
        $.getJSON('/BackOffice/getGangContainers/', { code: code }, function (data) {
            $table.bootstrapTable('showLoading');
            $table.bootstrapTable('load', data.data);
            $table.bootstrapTable('hideLoading');
            var options = $table.bootstrapTable('getOptions');
            if (options.totalPages > 0) {
                $table.bootstrapTable('selectPage', options.totalPages);
            }
        });
    }


}

function LoadDetails(code) {

    if (code != '') {
        $('#frmLoader').addClass('loading');
        $.getJSON('/Backoffice/getGangDetails/', { code: code }, function (data) {
            try {
                $('#frmLoader').removeClass('loading');
                $('#txtGangType').val(data.data.GangType);
                $('#txtGang').val(data.data.Gang);
                $('#txtPrepared').val(data.data.PreparedBy);
                $('#txtSchedule').val(data.data.Shift);
            } catch (e) {
                $('#frmLoader').removeClass('loading');
            }
        })
    }
}

var approvalWorkList = function (MAIN_DS_) {

    var self = this;
    self.DS_STORE = MAIN_DS_;
    self.FlaggedCasuals = ko.observableArray([]);

    self.removeFlaggedCasual = function (item) {
        ko.utils.arrayForEach(self.FlaggedCasuals(), function (data) {
            if (data == item) {
                self.FlaggedCasuals.remove(item);
            }
        })
    }

    self.addFlaggedCasual = function (item) {
        self.FlaggedCasuals.push(item);
    }

    self.resetAll = function () {
        self.FlaggedCasuals([]);
    }

    self.ApplyFlag = function () {
        if (self.FlaggedCasuals().length > 0) {
            var details = $("#gridBox").dxDropDownBox('instance').option('value');
            var code = details.split('-')[0];
            var reason = $('#txtReason').val();

            $.post('/BackOffice/ExemptCasuals/', {
                staff: self.FlaggedCasuals(), request_code:
                    code, reason: reason
            }, function (data) {
                if (data.message == 'success')
                {
                    $('.ui.basic.e.modal').modal('hide');
                }
                else
                {
                    toastr.error(data.message, 'CASUAL EXEMPTION');
                }
            })
        }
        else {
            toastr.info('NO CASUAL SELECTED', 'EXEMPT CASUAL MEMBER');
        }
    }

    self.ApproveRequest = function () {
        var details = $("#gridBox").dxDropDownBox('instance').option('value');
        var reason = $('#txtApprovalNote').val();
        $.post('/BackOffice/ApproveGangAdvice/', { code: details, reason: reason }, function (data) {
            if (data.message == 'success')
            {
                self.DS_STORE.reload();
                toastr.success('APPROVAL COMPLETED', 'APPROVAL');
                $("#gridBox").dxDropDownBox('reset');
                $('.ui.basic.a.modal').modal('hide');
            }
            else
            {
                toastr.error(data.message, 'APPROVAL');
            }
        })
    }
}
