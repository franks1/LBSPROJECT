function operateFormatter(value, row, index) {
    return [
        '<a class="btn btn-success btn-xs" href="javascript:void(0)" title="Add">',
        '<i class="fa fa-plus"></i> Add',
        '</a>&nbsp;',

    ].join('');
}

$(function () {
    var model = new sheetModel();
    ko.bindingHandlers.visibleFade = { init: function (element, valueAccessor) { var value = valueAccessor(); $(element).toggle(value()); }, update: function (element, valueAccessor) { var value = valueAccessor(); value() ? $(element).fadeIn() : $(element).fadeOut(); } }

    ko.applyBindings(model, document.getElementById('koserver'));
    $('.menu .item').tab();
    $('.combo.dropdown')
        .dropdown({
            action: 'combo'
        });
    //  $('.ui.gr.dropdown').dropdown({ hideAdditions:false, allowAdditions:true});

    $('.ui.vs.dropdown').dropdown();
    $('.ui.vw.dropdown').dropdown();
    $('.ui.l.dropdown').dropdown({
        fullTextSearch: true
    });


    $('.ui.cl.dropdown').dropdown();

    $('.ui.g.dropdown').dropdown({ fullTextSearch: true });

    $('.ui.cg.dropdown').dropdown({
        fullTextSearch: true,
        onChange: function (value, text, $element) {
            try {
                $('#frmAdvice').addClass('loading');

                setTimeout(function () {
                    model.CategoryGroupList(value);
                    model.CategoryClientList(value);
                    $('#frmAdvice').removeClass('loading');
                }, 500);



            } catch (e) {
                $('#frmAdvice').removeClass('loading');

            }

        }
    });

    $('input[name="DateIssued"]').daterangepicker(
        {
            singleDatePicker: true,
            showDropdowns: true,
            startDate: new Date().getDate(),
            locale: {
                format: 'DD/MM/YYYY'
            }
        });

    window.operateEvents = {
        'click .btn-success': function (e, value, row, index) {
            if ($('.ui.g.dropdown').dropdown('get text') == '' || $('.ui.g.dropdown').dropdown('get text') == undefined || $('.ui.g.dropdown').dropdown('get text') == null) {
                swal('GANG REQUEST', 'PLEASE SPECIFY GANG', 'info');
                return;
            }

            if ($('.ui.cg.dropdown').dropdown('get text') == '' || $('.ui.cg.dropdown').dropdown('get text') == undefined || $('.ui.g.dropdown').dropdown('get text') == null) {
                swal('GANG REQUEST', 'PLEASE SPECIFY GANG CATEGORY', 'info');
                return;
            }

            if ($('.ui.vs.dropdown').dropdown('get text') == '' || $('.ui.vs.dropdown').dropdown('get text') == undefined
                || $('.ui.vs.dropdown').dropdown('get text') == null || $('.ui.vs.dropdown').dropdown('get text') == '--SHIFT--') {
                swal('GANG REQUEST', 'PLEASE SPECIFY WORKING SHIFT', 'info');
                return;
            }

            if ($('.ui.vw.dropdown').dropdown('get text') == '' || $('.ui.vw.dropdown').dropdown('get text') == undefined
                || $('.ui.vw.dropdown').dropdown('get text') == null || $('.ui.vw.dropdown').dropdown('get text') == '--WEEK--') {
                swal('GANG REQUEST', 'PLEASE SPECIFY WORKING WEEK', 'info');
                return;
            }

            if ($('#RequestCode').val() == '' || $('#RequestCode').val() == undefined || $('#RequestCode').val() == null) {
                swal('GANG REQUEST', 'REQUEST CODE NOT SPECIFIED', 'info');
                return;
            }

            if ($('#groupDrop option:selected').text() == '--CATEGORY GROUP--' || $('#groupDrop option:selected').val() == null) {
                swal('GANG REQUEST', 'GANG GROUP HAS NOT BEEN SPECIFIED', 'info');
                return;
            }

            if ($('#groupDrop option:selected').val() == '' || $('#groupDrop option:selected').val() == null) {
                swal('GANG REQUEST', 'GANG GROUP HAS NOT BEEN SPECIFIED', 'info');
                return;
            }

            var data = {
                RequestCode: $('#RequestCode').val(), StaffCode: row.Code,
                Name: row.Name, Gang: $('.ui.g.dropdown').dropdown('get text'),
                GroupName: $('#groupDrop option:selected').text(),
                GangCode: $('.ui.g.dropdown').dropdown('get value'),
                Group: $('#groupDrop option:selected').val(),
                Category: $('.ui.cg.dropdown').dropdown('get value')
            };
            model.addItem(data);
        },
        'click .btn-warning': function (e, value, row, index) {
            //    window.location = '/cus/DetailsCustomer/' + row.Id;
        }
    };
    LoadData();
})

function LoadData() {
    var $table = $('#tablex').bootstrapTable({
        showColumns: true,
        url: '/Employee/getData/',
        sortStable: true,
        silentSort: false,
        showToggle: true,
        columns: [
            {
                field: 'Code', align: 'left',
                title: 'STAFF ID'
            },
            {
                field: 'Name', align: 'left',
                title: 'NAME'
            },
            {
                title: 'ACTION',
                align: 'center',
                events: operateEvents,
                formatter: operateFormatter
            }]
    });

    $.getJSON('/Employee/getData/', {}, function (data) {
        $table.bootstrapTable('showLoading');
        $table.bootstrapTable('load', data.data);
        $table.bootstrapTable('hideLoading');
        var options = $table.bootstrapTable('getOptions');

        if (options.totalPages > 0) {
            $table.bootstrapTable('selectPage', options.totalPages);
        }
    });
}
var sheetModel = function () {
    var self = this;
    self.sheetItems = ko.observableArray([]);
    self.GroupList = ko.observableArray([]);
    self.ClientList = ko.observableArray([]);
    self.TotalEntries = ko.observable(0);
    self.addItem = function (data) {
        var isDuplicate = false;
        var result = ko.utils.arrayForEach(self.sheetItems(), function (item) {
            if (item.StaffCode == data.StaffCode) {
                isDuplicate = true;
                return;
            }
        });
        if (isDuplicate == true) {
            swal('GANG REQUEST', 'PERSONNEL HAS ALREADY BEEN ADDED', 'info');
        }
        else {
            self.sheetItems.push(data);
            self.CountEntries();
            $('#myMessageContainer').stop().animate({
                scrollTop: $('#myMessageContainer')[0].scrollHeight
            });
        }

    }

    self.CountEntries = function () {
        self.TotalEntries(self.sheetItems().length);
    }

    self.CategoryGroupList = function (Id) {
        $.getJSON('/GangAdviceManager/getAssignedGroups/', { category: Id }, function (data) {
            self.GroupList(data.data);
        })
    }


    self.CategoryClientList = function (Id) {
        $.getJSON('/GangAdviceManager/getAssignedClients/', { category: Id }, function (data) {
            self.ClientList(data.data);
        })
    }


    self.CancelTask = function () {
        $('.ui.g.dropdown').dropdown('restore default text');
        $('.ui.vs.dropdown').dropdown('restore default text');
        $('.ui.vw.dropdown').dropdown('restore default text');
        $('.ui.cg.dropdown').dropdown('restore default text');
        $('.ui.l.dropdown').dropdown('restore default text');
        self.GroupList([]); self.ClientList([]);
        self.sheetItems([]);

    }


    self.AdviceCode = function () {
        $.getJSON('/GangAdviceManager/getAdviceCode/', {}, function (data) {
            $('#RequestCode').val(data.data);
        })
    }

    self.IndexView = function () {
        window.location = '/GangAdviceManager/Index';
    }

    self.SaveEntries = function () {

        if ($('#clientDrop option:selected').val() == '' || $('#clientDrop option:selected').val() == undefined ||
            $('#clientDrop option:selected').val() == null) {
            swal('GANG REQUEST', 'PLEASE SPECIFY CLIENT', 'info');
            return;
        }

        if (self.sheetItems().length == 0) {
            swal('GANG REQUEST', 'NO ENTRY FOUND FOR CASUALS', 'info');
            return;
        }

        var loc = $('.ui.l.dropdown').dropdown('get value');

        if (loc == '' || loc == null || loc == undefined) {
            swal('GANG REQUEST', 'FIELD LOCATION NOT SPECIFIED', 'info');
            return;
        }

        var VM = {
            RequestCode: $('#RequestCode').val(),
            DateIssued: $('#DateIssued').val(),
            FieldClient: $('#clientDrop option:selected').val(), //$('.ui.cl.dropdown').dropdown('get value'),
            Shift: $('.ui.vs.dropdown').dropdown('get text'),
            Week: $('.ui.vw.dropdown').dropdown('get text'),
            Gang: $('.ui.g.dropdown').dropdown('get value'),
            FieldLocation: loc,
            Entries: self.sheetItems()
        };



        try {
            $('#frmAdvice').addClass('loading');

            $.post('/GangAdviceManager/RaiseGangRequest/', { model: VM }, function (data) {
                if (data.message == 'success') {
                    $('#frmAdvice').removeClass('loading');
                    swal('GANG REQUEST', 'GANG REQUEST HAS BEEN CREATED', 'success');
                    setTimeout(function () {
                        window.location = window.location;
                    }, 1000);
                }
                else {
                    $('#frmAdvice').removeClass('loading');
                    swal('GANG REQUEST', data.message, 'info');
                }
            });
        } catch (e) {
            swal('GANG REQUEST', "AN ERROR OCCURED", 'info');
            $('#frmAdvice').removeClass('loading');
        }
    }

    self.removeItem = function (data) {
        self.sheetItems.remove(data);
        self.CountEntries();
    }
    // Animation callbacks for the planets list
    self.showPlanetElement = function (elem) { if (elem.nodeType === 1) $(elem).hide().slideDown() }
    self.hidePlanetElement = function (elem) { if (elem.nodeType === 1) $(elem).slideUp(function () { $(elem).remove(); }) }

    this.showProduct = function (element) { if (element.nodeType === 1) { $(element).hide().fadeIn(); } };

    this.hideProduct = function (element) { if (element.nodeType === 1) { $(element).fadeOut(function () { $(element).remove(); }); } }

};