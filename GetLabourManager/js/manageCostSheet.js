$(function () {
    var model = new sheetEntry();
    ko.applyBindings(model, document.getElementById('koserver'));
    model.getDetails();
    $('#tblList').on('click', 'button[data-act]', function (e) {
        var code = $('#RequestCodeId').val();
        var value_id = $(this).data('vid');
        if (value_id == 0) {
       
            $("#myModal").modal('show');
            $('#casuals_mod').load('/Operation/CasualMembers/', { term: code });
        }
        else {
            $("#myModal").modal('show');
            $('#casuals_mod').load('/Operation/ListCasualContainers/', { term: code });
        }
    });

})


var sheetEntry = function () {
    var self = this;
    self.Entries = ko.observableArray([]);

    self.getDetails = function () {
        var code = $('#RequestCodeId').val();

        $.getJSON('/Operation/getCostSheetRequest/', { term: code }, function (data) {
            self.Entries(data);
        });
    }

    //self.showCasuals = function () {

    //}

    self.navigateBack = function () {
        window.location = '/Operation/CostSheetIndex/';
    }
}

//$(function () {

//    //var model = new sheetEntry();
//    //ko.applyBindings(model, document.getElementById('koserver'));
//    //model.doSearchTask();
//    //model.Navigate();
//    //$("#tablex td:first input").focus();
//    //$('input[name="txtCostSheetDate"]').daterangepicker(
//    //    {
//    //        singleDatePicker: true,
//    //        showDropdowns: true,
//    //        startDate: new Date().getDate(),
//    //        locale: {
//    //            format: 'DD/MM/YYYY'
//    //        }
//    //    }, function (start, end, label) {
//    //        var years = moment().diff(start, 'years');
//    //        // $('#AgeGroup').val('CHILD');
//    //    });

// //   setTimeout(function () {
//        //$("#progressBarContainer").dxProgressBar({
//        //    min: 0,
//        //    max: 100,
//        //    value: 75,
//        //    onComplete: function () {
//        //        DevExpress.ui.dialog.alert("Completed");
//        //    }
//        //});
//   // }, 5000);


//})

