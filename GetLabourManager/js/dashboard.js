$(function () {
    setTimeout(function () {
        loadDefault();
       
    }, 3000);
    var model = new dashboardModel();
    ko.applyBindings(model, document.getElementById('koserver'));
    model.LoadHeader();
   
});



function loadDefault() {
    $("#chart").dxChart({
        dataSource:'/home/getMonthlyGangRequest/',
        tooltip: {
            enabled: true,
             customizeTooltip: function (arg) {
                return {
                    text: arg.argumentText + "<br/>" + arg.valueText
                };
            }
        },
        "export": {
            enabled: true
        },
        series: {
            label: {
                visible: true,
                backgroundColor: "#c18e92"
            },
            argumentField: "MonthName",
            valueField: "value",
            name: "Gang Advice",
            type: "bar",
            color: '#4FC1E9'
        }
    });

}

var dashboardModel = function () {

    var self = this;
    self.Casuals = ko.observable(0);
    self.Users = ko.observable(0);
    self.Gangs = ko.observable(0);
    self.Foremen = ko.observable(0);
    $.fn.increment = function (options) {
        var $this = $(this);
        var coef = options.coef;
        var speed = options.speed;
        var value = 0.0;
        var stp = options.stp;
        debugger;
        setInterval(function () {
            if (value < stp) {
                value = value + coef;
                $this.html(value);
            }
        }, speed);
    };

    self.LoadHeader = function () {
        $.getJSON('/Home/HeaderDetails/', {}, function (data) {
            self.Casuals(data.data.Casuals);
            self.Users(data.data.Users);
            self.Gangs(data.data.Gangs);
            self.Foremen(data.data.Foremen);
            $("#odo1").increment({ coef: 1, speed: 5, stp: self.Casuals() });
            $("#odo2").increment({ coef: 1, speed: 5, stp: self.Gangs() });
            $("#odo3").increment({ coef: 0.50, speed: 5, stp: self.Users() });
            $("#odo4").increment({ coef: 0.50, speed: 5, stp: self.Foremen() });


        })

    }


}
