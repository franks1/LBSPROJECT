var TimeAuditing = function () {
    var self = this;
    self.CostSheetEntries = ko.observableArray([]);
    self.SelectedCostSheet = ko.observableArray([]);
    self.processing = ko.observable(true);
    self.IsDone = ko.observable(false);
    self.message = ko.observable();
    self.messages = ko.observableArray([]);

    self.loadRequisitions = function (Id) {
        $.getJSON('/BackOffice/getRequestByClient/', { Id: Id }, function (data) {
            self.CostSheetEntries([]);
            self.SelectedCostSheet([]);
            self.CostSheetEntries(data.data);
        })
    }


    self.setSelected = function () {
        $('#tbItems').on('click', 'input[data-bind]', function (e) {
            var value = $(this).data('vid');
            var records = ko.utils.arrayForEach(self.CostSheetEntries(), function (item) {
                if (item.Code == value) {
                    if (item.Selected == true)
                        self.SelectedCostSheet.push(item.Code);
                    else {
                        self.SelectedCostSheet.remove(item.Code);
                    }
                }
            });
        });
    };

}

TimeAuditing.prototype = {

    ClearGrid: function () {
        var self = this;
        self.CostSheetEntries([]);
        self.SelectedCostSheet([]);
        $('#txtWorked').val(0);
        $('#txtOverTime').val(0);
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
        var requestCode = self.SelectedCostSheet();
        var userId = $('#UserId').val();
        var overtime_hrs = $('#txtOverTime').val();
        if (overtime_hrs < 0) {
            swal('TIME AUDIT', 'PLEASE SPECIFY OVERTIME HOURS', 'error');
            return;
        }

        if (requestCode.length == 0) {
            swal('TIME AUDIT', 'PLEASE SELECT AN ITEM FROM THE LIST', 'error');
            return;
        }

        if (userId == '' || userId == 0) {
            swal('TIME AUDIT', 'USER NOT FOUND', 'error');
            return;
        }
        timeAudit.server.applyTimeSlots(requestCode, userId, overtime_hrs);
        self.processing(false);
    }
};
var model = new TimeAuditing();

var timeAudit = $.connection.timeAuditHub;
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
timeAudit.client.newMessages = function (message) {
    model.ReceiveMessages(message);
};

timeAudit.client.IsWorking = function (value) {
    model.Reset(value);
}

timeAudit.client.IsComplete = function (value) {
    if (value == true) {
        //   swal('TIME AUDIT', 'Task Completed', 'success');
       // var lookup = $("#lookupContainer").dxLookup('instance');
        //lookup.reset();
        $('.ui.cg.dropdown').dropdown('restore default text');
        model.ClearGrid();
    }
}


$(function () {

    ko.applyBindings(model, document.getElementById('koserver'));
    model.setSelected();
    $('.ui.cg.dropdown').dropdown({
        fullTextSearch:true,
        onChange: function (value, text, $element) {
          
            model.loadRequisitions(value);

        }
    })

   

    $('#btnReset').click(function () {
      ///  var lookup = $("#lookupContainer").dxLookup('instance');

       // lookup.reset();
        $('.ui.cg.dropdown').dropdown('restore default text');
    })
    timeAudit.client.newMessages("Ready To Apply Time Slots...");
});