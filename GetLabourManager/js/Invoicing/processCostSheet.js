var invoiceHub = $.connection.invoiceHub;
$.connection.hub.logging = true;
$.connection.hub.start();
$.connection.hub.disconnected(function () {
    //  $('#frmSession').addClass('loading');
    setTimeout(function () {
        $.connection.hub.start();
        //    $('#frmSession').removeClass('loading');

    }, 2000); // Restart connection after 5 seconds.
});
//  debugger;
invoiceHub.client.newMessages = function (message) {
    model.ReceiveMessages(message);
};

invoiceHub.client.IsWorking = function (value) {
    model.Reset(value);
}

invoiceHub.client.IsComplete = function (value) {
    if (value == true) {
      //  var lookup = $("#lookupContainer").dxLookup('instance');
        //lookup.reset();
        $('.ui.cg.dropdown').dropdown('restore default text');
        model.ClearGrid();
    }
}

var invoicingModel = function () {
    var self = this;
    self.message = ko.observable();
    self.messages = ko.observableArray([]);
    self.processing = ko.observable(true);
    self.sessionDate = ko.observable('');
    self.IsDone = ko.observable(false);
    self.SelectedItems = ko.observableArray([]);
    self.CostSheetEntries = ko.observableArray([]);

    self.loadCostSheetByClient = function (Id) {
        $.getJSON('/BackOffice/getCostSheetByClient/', { Id: Id }, function (data) {
            self.CostSheetEntries([]);
            self.SelectedItems([]);
            self.CostSheetEntries(data.data);
        })
    }

    self.AddItem = function (item_value) {
        self.SelectedItems.push(item_value);
    }


    self.RemoveItems = function (item_value) {

        var records = ko.utils.arrayForEach(self.SelectedItems(), function (item) {
            if (item == item_value) {
                self.SelectedItems.remove(item);
            }
        })
     }

    self.setSelected = function () {
        $('#tbItems').on('click', 'input[data-bind]', function (e) {
            var value = $(this).data('vid');
            var records = ko.utils.arrayForEach(self.CostSheetEntries(), function (item) {
                if (item.CCode == value) {
                    if (item.Selected == true)
                        self.SelectedItems.push(item.CCode);
                    else {
                        self.SelectedItems.remove(item.CCode);
                    }
                }
            });
        });
    };
            

    self.doLookup = function () {
        //$("#lookupContainer").dxLookup({
        //    searchMode: 'startswith',
        //    searchPlaceholder: 'Type client name',
        //    searchExpr: 'Name',
        //    valueExpr: "Name",
        //    displayExpr: "Name",
        //    dataSource: new DevExpress.data.DataSource({
        //        loadMode: "json",
        //        load: function () {
        //            return $.getJSON('/BackOffice/getFieldClients/');
        //        }
        //    }),
        //    onItemClick: function (info) {
        //        model.loadCostSheetByClient(info.itemData.Id);
        //    }
            
        //});
        //$("#container").dxTreeView({
        //    dataSource: '/Operation/getAppliedCostSheet/',
        //    dataStructure: 'plain',
        //    width: 250,
        //    showCheckBoxesMode: 'normal',
        //    //onItemClick: function (e) {
        //    //    var item = e.itemData;
        //    //    debugger;
        //    //    //var selectedItem = $("#simple-treeview").dxTreeView('instance').selectedItem
        //    //    if (item.text == "APPLIED COST SHEET") {
        //    //        return;
        //    //    }
        //    //    else {
        //    //        // $('#frmRequest').addClass('loading');

        //    //    }
        //    //},
        //    onItemSelected: function (e) {
        //        debugger;
        //        //unselectAll()
        //       // if (e.itemData.selected == true&&)


        //        if (e.itemData.selected == true) {
        //            self.AddItem(e.itemData.tag);
        //        }
        //        else {
        //            self.RemoveItems(e.itemData.tag);
        //        }
        //    }
        //});
    }

}

invoicingModel.prototype = {
    ClearGrid: function () {
        var self = this;
        self.CostSheetEntries([]);
        self.SelectedItems([]);
    },
    ReceiveMessages: function (message) {
        var self = this;
        self.message(message);
        self.messages.push(self.message());
        if (self.messages().length >= 5) {
            $('#myMessageContainer').stop().animate({
                scrollTop: $('#myMessageContainer')[0].scrollHeight
            });
        }
    },
    Reset: function (value) {
        var self = this;
        self.processing(value);
    },
    StartProcessJob: function () {
        var self = this;
        var code = self.SelectedItems();
        var userId = $('#UserId').val();
        if (code.length == 0) {
            swal('COST SHEET', 'PLEASE SELECT A COST SHEET N0.', 'error');
            return;
        }

        if (userId == '' || userId == 0) {
            swal('COST SHEET', 'USER NOT FOUND', 'error');
            return;
        }
        invoiceHub.server.processInvoice(code, userId);
        self.processing(false);
    }
};

var model = new invoicingModel();
$(function () {
    $('.ui.cg.dropdown').dropdown({
        fullTextSearch: true,
        onChange: function (value, text, $element)
        {
            model.loadCostSheetByClient(value);
        }
    })

    ko.applyBindings(model, document.getElementById('koserver'));
    model.doLookup();
    model.setSelected();
    invoiceHub.client.newMessages("WAITING ON YOUR COMMAND...");
    //$("#lookupContainer").dxLookup({
    //    searchMode: 'startswith',
    //    searchPlaceholder: 'Type client name',
    //    searchExpr: 'Name',
    //    key: ' Id',
    //    valueExpr: "Name",
    //    displayExpr: "Name",
    //    dataSource: new DevExpress.data.DataSource({
    //        loadMode: "json",
    //        load: function () {
    //            return $.getJSON('/BackOffice/getFieldClients/');
    //        }
    //    }),
    //    onItemClick: function (info) {
    //        model.loadCostSheetByClient(info.itemData.Id);
    //    }

    //});
});
