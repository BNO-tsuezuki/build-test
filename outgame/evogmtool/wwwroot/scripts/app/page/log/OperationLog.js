
var vm_select;
var rangePicker = window["vue2-daterange-picker"].default;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    Vue.component('date-range-picker', rangePicker);
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
                startDate: moment().subtract('days', 29).format('YYYY-MM-DD HH:mm'),
                endDate: moment().format('YYYY-MM-DD HH:mm'),
                dateRange: {
                    startDate: moment().subtract(7, 'days').format('MM/DD/YYYY HH:mm'),
                    endDate: moment().format('MM/DD/YYYY HH:mm'),
                },
                loginInfo: {},
                locale: null,
                ipAddress: '',
                List: [],
                selectedLog: {},
                addSearchItem: {},
                searchItems: [
                    {
                        name: 'UserId',
                        enable: true,
                        value: '',
                        type: 'integer'
                    },
                    {
                        name: 'StatusCode',
                        enable: false,
                        value: '',
                        type: 'integer'
                    },
                    {
                        name: 'Method',
                        enable: false,
                        value: '',
                        type: 'string'
                    },
                    {
                        name: 'Url',
                        enable: false,
                        value: '',
                        type: 'string'
                    },
                    {
                        name: 'QueryString',
                        enable: false,
                        value: '',
                        type: 'string'
                    },
                    {
                        name: 'RequestBody',
                        enable: false,
                        value: '',
                        type: 'string'
                    },
                    {
                        name: 'ResponseBody',
                        enable: false,
                        value: '',
                        type: 'string'
                    },
                    {
                        name: 'Exception',
                        enable: false,
                        value: '',
                        type: 'string'
                    },
                    {
                        name: 'IpAddress',
                        enable: false,
                        value: '',
                        type: 'string'
                    }
                ]
            }
        },
        mounted: function () {
            var self = this;
            this.getMyData();

        },

        computed: {

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
                            self.dateFormat = 'YYYY/MM/DD HH:mm:SS';
                        } else {
                            self.dateFormat = 'MM/DD/YYYY HH:mm:SS';
                        }
                        self.locale = self.makeLocaleData(self.loginInfo.language.languageCode);
                        if (this.value != '') {
                            var m = moment.utc(self.value, "YYYY-MM-DDTHH:mm:ss");
                            self.date = m.tz(self.loginInfo.timezone.timezoneCode).format('YYYY-MM-DD');
                            self.time = m.tz(self.loginInfo.timezone.timezoneCode).format('HH:mm');
                        }
                        this.settingOK = true;
                    }).catch(error => {
                        console.log(error);
                    });
            },getLogs: function () {
                var self = this;
                var queryArray = [];

                if (this.startDate) queryArray.push('From=' + moment.tz(this.startDate, this.loginInfo.timezone.timezoneCode).utc().format('YYYY-MM-DDTHH:mm').toString());
                if (this.endDate) queryArray.push('To=' + moment.tz(this.endDate, this.loginInfo.timezone.timezoneCode).utc().format('YYYY-MM-DDTHH:mm').toString());
                queryArray = queryArray.concat(this.getQuery());

                var targetApi = new URL('/api/log/operation', location.origin);

                targetApi.search = queryArray.join('&')

                var res = axios.get(targetApi.href)
                    .then(response => {
                        self._data.List = response.data.operationLogList;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getQuery: function () {
                var queryArray = [];
                _.each(this.searchItems, function (item) {
                    if (item.enable) queryArray.push(item.name + '=' + item.value);
                });
                return _.cloneDeep(queryArray);
            },
            selectRow: function (log) {
                this.selectedLog = log;
            },
            getRequestJSONString: function () {
                if (this.selectedLog.requestBody) {
                    //return 'hoge\nhoge';
                    return '\n' + JSON.stringify(JSON.parse(this.selectedLog.requestBody), null, 4);
                } else {
                    return '';
                }
                
            },
            getResponseJSONString: function () {
                if (this.selectedLog.responseBody) {
                    //return 'hoge\nhoge';
                    return '\n' + JSON.stringify(JSON.parse(this.selectedLog.responseBody), null, 4);
                } else {
                    return '';
                }

            },
            isSelect: function (row) {
                if (this.selectedLog) {
                    if (row.id == this.selectedLog.id) {
                        return true;
                    } else {
                        return false;
                    }

                } else {
                    return false;
                }
            },
            removeItem: function (item) {
                item.enable = false;
            },
            addItem: function () {
                this.addSearchItem.enable = true;
                this.addSearchItem = null;
            },
            rageUpdate: function (range) {
                this.startDate = moment(range.startDate).format('YYYY-MM-DD HH:mm');
                this.endDate = moment(range.endDate).format('YYYY-MM-DD HH:mm');
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
            }, getDateTime: function (val) {
                if (val) {
                    var m = moment.utc(val, "YYYY-MM-DDTHH:mm:SS");
                    return m.tz(this.loginInfo.timezone.timezoneCode).format(this.dateFormat);
                } else {
                    return '';
                }
            }
        },
        filters: {
            truncate: function (value) {
                var length = 20;
                var ommision = "...";
                if (value.toString().length <= length) {
                    return value.toString();
                }
                return value.toString().substring(0, length) + ommision;
            },
            truncateURL: function (value) {
                var url = new URL(window.location.href);
                return value.toString().replace(url.protocol + '//' + url.hostname + '/api/', '');
            },
            dateFormat: function (value) {
                var m = moment(value, "YYYY-MM-DDTHH:mm:ss");
                return m.format('MM/DD/YYYY HH:mm');
            },
            dateFormat2: function (value) {
                var m = moment(value, "YYYY-MM-DDTHH:mm:ss");
                return m.format('MM/DD/YYYY HH:mm:ss');
            }
        },
        watch: {

        },
    });
    function toISOString(dateTime, timezone) {
        return moment.tz(moment(dateTime).format('YYYY-MM-DDTHH:mm'), timezone).utc().format(); // todo: 共通関数にする？
    };
});