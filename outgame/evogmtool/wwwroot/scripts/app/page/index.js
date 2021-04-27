
var vm_select;
var visitorsChart;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    const vueAwesomeCountdown = window['vue-awesome-countdown'].default;
    const rangePicker = window["vue2-daterange-picker"].default;
    Vue.use(vueAwesomeCountdown);
    Vue.component('date-range-picker', rangePicker);
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                locale: null,
                datePickerLang: '',
                lang: '',
                dateFormat: '',
                startDate: moment().subtract(1, 'hours').format('MM/DD/YYYY HH:mm'),
                endDate: moment().format('MM/DD/YYYY HH:mm'),
                searchStartDate: moment().subtract(1, 'hours').format('MM/DD/YYYY HH:mm'),
                searchEndDate: moment().format('MM/DD/YYYY HH:mm'),
                dateRange: {
                    startDate: moment().subtract(1, 'hours').format('MM/DD/YYYY HH:mm'),
                    endDate: moment().format('MM/DD/YYYY HH:mm'),
                },
                loginInfo: {},
                chartDataOrg: {},
                chartLabel: [],
                chartData: [],
                selectedArea: 'hoge',
                areaName:['hoge','fuga']
            }
        },
        mounted: function () {
            this.getMyData();
            
            
        },

        computed: {
            nowCount: function () {
                if (this.chartData.length == 0) return 0;
                return this.chartData[this.chartData.length-1];
            }
        },
        //async created(){
        created() {

        },
        methods: {
            getMyData: function () {
                var self = this;
                var res = axios.get('/api/Auth')
                    .then(response => {
                        self.loginInfo = response.data;
                        self.lang = self.loginInfo.language.languageCode;
                        self.datePickerLang = self.loginInfo.language.languageCode;
                        if (self.loginInfo.language.languageCode == 'ja') {
                            self.dateFormat = 'YYYY/MM/DD HH:mm:DD';
                        } else {
                            self.dateFormat = 'MM/DD/YYYY HH:mm:DD';
                        }
                        self.locale = self.makeLocaleData(self.loginInfo.language.languageCode);
                        self.getAreaNameData();
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getAreaNameData: function () {
                var self = this;

                var queryArray = [];

                var targetApi = new URL('/api/log/game/SessionCountHistory/AreaName', location.origin);
                targetApi.search = queryArray.join('&');
                var res = axios.get(targetApi.href)
                    .then(response => {
                        self.areaName = response.data.areaNameList;
                        if (self.areaName.length > 0) self.selectedArea = self.areaName[0];
                        self.getChartData();
                    }).catch(error => {
                        console.log(error);
                    });
            },
            rangeUpdate: function (range) {
                this.startDate = moment(range.startDate).format('YYYY-MM-DD HH:mm');
                this.endDate = moment(range.endDate).format('YYYY-MM-DD HH:mm');
                this.getChartData();
            },
            makeLocaleData: function (lang) {
                if (lang == 'ja') {
                    var data =
                    {
                        direction: 'ltr',
                        format: 'yyyy/mm/dd HH:MM',
                        separator: ' - ',
                        applyLabel: 'Apply',
                        cancelLabel: 'Cancel',
                        weekLabel: 'W',
                        daysOfWeek: ['日', '月', '火', '水', '木', '金', '土'],
                        monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
                        firstDay: 0
                    };
                    return data;
                }
                if (lang == 'en') {
                    var data =
                    {
                        direction: 'ltr',
                        format: 'mm/dd/yyyy HH:MM',
                        separator: ' - ',
                        applyLabel: 'Apply',
                        cancelLabel: 'Cancel',
                        weekLabel: 'W',
                        daysOfWeek: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
                        monthNames: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                        firstDay: 0
                    };
                    return data;
                }
                if (lang == 'de') {
                    var data =
                    {
                        direction: 'ltr',
                        format: 'mm/dd/yyyy HH:MM',
                        separator: ' - ',
                        applyLabel: 'Apply',
                        cancelLabel: 'Cancel',
                        weekLabel: 'W',
                        daysOfWeek: ['So.', 'Mo.', 'Di.', 'Mi.', 'Do.', 'Fr.', 'Sa.'],
                        monthNames: ['Jan', 'Feb', 'Mär', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dez'],
                        firstDay: 0
                    };
                    return data;
                }
                if (lang == 'fr') {
                    var data =
                    {
                        direction: 'ltr',
                        format: 'mm/dd/yyyy HH:MM',
                        separator: ' - ',
                        applyLabel: 'Apply',
                        cancelLabel: 'Cancel',
                        weekLabel: 'W',
                        daysOfWeek: ['Dim', 'Lun', 'Mar', 'Mer', 'Jeu', 'Ven', 'Sam'],
                        monthNames: ['Jan', 'Fév', 'Mär', 'Avr', 'Mai', 'Juin', 'Juil', 'Août', 'Sep', 'Okt', 'Nov', 'Déc'],
                        firstDay: 0
                    };
                    return data;
                }
            },
            makeLabels: function (sec) {
                var arrLabel = [];
                var arrData = [];
                //arr.push(moment().format('MM/DD/YYYY HH:mm'));

                _.forEach(this.chartDataOrg.logList, function (value) {
                    arrLabel.push(moment(value.datetime).format('YYYY/MM/DD HH:mm:ss').toString());
                    arrData.push(value.count);
                });

                this.chartLabel = arrLabel;
                this.chartData = arrData;

            },
            getChartData: function () {
                var self = this;
                if (visitorsChart) visitorsChart.destroy();

                var queryArray = [];
                queryArray.push('From=' + moment.tz(this.startDate, this.loginInfo.timezone.timezoneCode).utc().format('YYYY-MM-DDTHH:mm').toString());
                queryArray.push('To=' + moment.tz(this.endDate, this.loginInfo.timezone.timezoneCode).utc().format('YYYY-MM-DDTHH:mm').toString());

                queryArray.push('AreaName=' + this.selectedArea)
                var targetApi = new URL('/api/log/game/SessionCountHistory', location.origin);
                targetApi.search = queryArray.join('&');
                var res = axios.get(targetApi.href)
                    .then(response => {
                        self.chartDataOrg = response.data;
                        self.makeLabels();
                        self.chartSetting();
                    }).catch(error => {
                        console.log(error);
                    });
            },
            chartSetting: function () {
                var ticksStyle = {
                    fontColor: '#495057',
                    fontStyle: 'bold'
                }
                var labelsData = this.makeLabels(60);

                var mode = 'index'
                var intersect = true

                var $visitorsChart = $('#visitors-chart');
                visitorsChart = new Chart($visitorsChart, {
                    data: {
                        labels: this.chartLabel,
                        datasets: [{
                            type: 'line',
                            data: this.chartData,
                            backgroundColor: 'transparent',
                            borderColor: '#007bff',
                            pointBorderColor: '#007bff',
                            pointBackgroundColor: '#007bff',
                            fill: false
                            // pointHoverBackgroundColor: '#007bff',
                            // pointHoverBorderColor    : '#007bff'
                        }
                        //    ,
                        //{
                        //    type: 'line',
                        //    data: [60, 80, 70, 67, 80, 77, 100],
                        //    backgroundColor: 'tansparent',
                        //    borderColor: '#ced4da',
                        //    pointBorderColor: '#ced4da',
                        //    pointBackgroundColor: '#ced4da',
                        //    fill: false
                        //    // pointHoverBackgroundColor: '#ced4da',
                        //    // pointHoverBorderColor    : '#ced4da'
                        //    }
                        ]
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            mode: mode,
                            intersect: intersect
                        },
                        hover: {
                            mode: mode,
                            intersect: intersect
                        },
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                // display: false,
                                gridLines: {
                                    display: true,
                                    lineWidth: '4px',
                                    color: 'rgba(0, 0, 0, .2)',
                                    zeroLineColor: 'transparent'
                                },
                                ticks: $.extend({
                                    beginAtZero: true,
                                    suggestedMax: 200
                                }, ticksStyle)
                            }],
                            xAxes: [{
                                display: true,
                                gridLines: {
                                    display: false
                                },
                                ticks: ticksStyle
                            }]
                        }
                    }
                });
            }
        },
        watch: {

        },
    });
});