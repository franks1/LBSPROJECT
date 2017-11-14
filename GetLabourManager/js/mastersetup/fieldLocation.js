$(function () {
    window.operateEvents = {
        'click .btn-primary': function (e, value, row, index)
        {
            $('#myModal').modal('show');
            $('#casuals_mod').load('/MasterSetup/LocationEditView/', {Id:row.Id});
        },
        'click .btn-danger': function (e, value, row, index)
        {
            $.post('/MasterSetup/DeleteLocation/', { Id: row.Id }, function (data)
            {
                if (data.message == 'success')
                {
                    swal('FIELD LOCATION', 'LOCATION DELETED', 'success');
                    LoadFieldLocation();
                }
                else
                {
                    swal('FIELD LOCATION', data.message, 'info');
                }
            })
        }
    };

    LoadFieldLocation();
    var model = new LocationModel();
    ko.applyBindings(model, document.getElementById('koserver'));
})

function operateFormatters(value, row, index) {
    return [
        '<a class="btn btn-primary btn-xs" href="javascript:void(0)" title="Edit">',
        '<i class="fa fa-edit"></i>',
        '</a>&nbsp;',
        '<a class="btn btn-danger btn-xs" href="javascript:void(0)" title="Delete">',
        '<i class="fa fa-remove"></i>',
        '</a>&nbsp;'
    ].join('');
}

function LoadFieldLocation() {
    var $table = $('#tablex').bootstrapTable({
        showColumns: true,
        url: '/MasterSetup/getFieldLocation/',
        sortStable: true,
        silentSort: false,
        showToggle: true,
        columns: [
            {
                field: 'Location', align: 'left',
                title: 'LOCATION'
            },
            {
                field: 'Lat', align: 'left',
                title: 'LATITUDE'
            },
            {
                field: 'Long', align: 'left',
                title: 'LONGITUDE'
            },
            {
                title: 'ACTION',
                align: 'center',
                events: operateEvents,
                formatter: operateFormatters
            }]
    });

    $.getJSON('/MasterSetup/getFieldLocation/', {}, function (data) {
        $table.bootstrapTable('showLoading');
        $table.bootstrapTable('load', data.data);
        $table.bootstrapTable('hideLoading');
        var options = $table.bootstrapTable('getOptions');

        if (options.totalPages > 0) {
            $table.bootstrapTable('selectPage', options.totalPages);
        }
    });
}

var LocationModel = function () {
    var self = this;

    self.AddLocation = function () {
        var VM =
            {
                Id: 0,
                Location: $('#txtArea').val(),
                LocationLat: $('#txtLat').val(),
                LocationLong: $('#txtLng').val()
            };

        if (VM.Location == '' | VM.Location == undefined || VM.Location == null) {
            swal('FIELD LOCATION', 'PLEASE SPECIFY LOCATION', 'error');
            return;
        }

        if (VM.LocationLat == '' | VM.LocationLat == undefined || VM.LocationLat == null) {
            swal('FIELD LOCATION', 'PLEASE SPECIFY LATITUDE', 'error');
            return;
        }


        if (VM.LocationLong == '' | VM.LocationLong == undefined || VM.LocationLong == null) {
            swal('FIELD LOCATION', 'PLEASE SPECIFY LATITUDE', 'error');
            return;
        }
        $('#fmLoc').addClass('loading');
        $.post('/MasterSetup/SaveLocation', { model: VM }, function (data) {
            $('#fmLoc').removeClass('loading');
            try {
                if (data.message = 'success') {
                    LoadFieldLocation();
                    $('#txtArea').val('');
                    $('#txtLat').val('');
                    $('#txtLng').val('');

                }
                else {
                    swal('FIELD LOCATION', data.message, 'error');
                }
            } catch (e) {
                $('#fmLoc').removeClass('loading');
            }
        })


    }


}