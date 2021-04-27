
var vm_select;
var Tabs = window['VueTabs'].Tabs,
    Tab = window['VueTabs'].Tab,
    drag = window['vuedraggable'],
    rangePicker = window["vue2-daterange-picker"].default;

window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    Vue.component('paginate', VuejsPaginate);
    Vue.component('date-range-picker', rangePicker);
    Vue.use(VTooltip);
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
                index: 1,
                loginInfo: {},
                dateFormat: '',
                windowCount: 1,
                selectedAccount: '',
                selectedMatchId: '',
                selectedIpAddress: '',
                selectedGroupId: '',
                selectedSide: '',
                selectedSearchType: '0',
                startDate: moment().subtract(7, 'days').format('MM/DD/YYYY HH:mm'),
                endDate: moment().format('MM/DD/YYYY HH:mm'),

                searchAccount: '',
                searchMatchId: '',
                searchIpAddress: '',
                searchGroupId: '',
                searchSide: '',
                searchSearchType: '',
                searchStartDate: moment().subtract(7, 'days').format('MM/DD/YYYY HH:mm'),
                searchEndDate: moment().format('MM/DD/YYYY HH:mm'),

                selected: 'PlayerAccountCreateHistory',

                dateRange: {
                    startDate: moment().subtract(7, 'days').format('MM/DD/YYYY HH:mm'),
                    endDate: moment().format('MM/DD/YYYY HH:mm'),
                },
                locale: null,

                searchArray: [
                    {
                        enable: false,
                        name: 'PlayerAccountCreate',
                        apiName: 'PlayerAccountCreateHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: false,
                        Side: false,
                        SearchType :false
                    },
                    {
                        enable: false,
                        name: 'Login',
                        apiName: 'LoginHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: true,
                        GroupId: false,
                        MatchId: false,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'Logout',
                        apiName: 'LogoutHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: true,
                        GroupId: false,
                        MatchId: false,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'PlayerExp',
                        apiName: 'PlayerExpHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: false,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'ChatSay',
                        apiName: 'ChatSayHistory',
                        PlayerId: false,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: false,
                        Side: false,
                        SearchType: true
                    },
                    {
                        enable: false,
                        name: 'ChatDirect',
                        apiName: 'ChatDirectHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: false,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'Party',
                        apiName: 'PartyHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: true,
                        GroupId: true,
                        MatchId: false,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'MatchCue',
                        apiName: 'MatchCueHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: false,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'MatchStartPlayer',
                        apiName: 'MatchStartPlayerHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: true,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'MatchExecution',
                        apiName: 'MatchExecutionHistory',
                        PlayerId: false,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: true,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'MatchExitPlayer',
                        apiName: 'MatchExitPlayerHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: true,
                        Side: false,
                        SearchType: false
                    },
                    {
                        enable: false,
                        name: 'MatchEntryPlayer',
                        apiName: 'MatchEntryPlayerHistory',
                        PlayerId: true,
                        From: true,
                        To: true,
                        RemoteIp: false,
                        GroupId: false,
                        MatchId: true,
                        Side: false,
                        SearchType: false
                    },

                ],

                searchDataList: [],

            }
        },
        components: {
            Tabs,
            Tab,
            drag
        },
        mounted: function () {
            var self = this;
            this.getMyData();

            if (getParameter("playerId")) {
                this.selectedAccount = getParameter("playerId");
                this.searchAccount = getParameter("playerId");
            }
            if (getParameter("matchId")) {
                searchAccount
                this.selectedMatchId = getParameter("matchId");
                this.searchMatchId = getParameter("matchId");
            }

            //$('#reservationtime').daterangepicker({
            //    startDate: moment().utc().subtract(7, 'days'),
            //    endDate: moment().utc(),
            //    maxDate: moment().utc().add(1, 'days').format('MM/DD/YYYY'),
            //    timePicker: true,
            //    timePicker24Hour: true,
            //    timePickerIncrement: 30,
            //    locale: {
            //        format: 'MM/DD/YYYY HH:mm'
            //    }
            //},
            //    function (start, end) {
            //        console.log("Callback has been called!");
            //        self._data.startDate = start.format('MM/DD/YYYY HH:mm');
            //        self._data.endDate = end.format('MM/DD/YYYY HH:mm');

            //    }
            //);
            $('#ipInput').inputmask();
            $('#ipInput').change(function () {
                self._data.ipAddress = $(this).val();
            });

            //this.getLogs();
        },

        computed: {
            showPlayerId: function () {
                var self = this;
                var tmp = _.filter(this.searchArray, (obj) => {
                    //return obj.enable == true;
                    return obj.apiName == self.selected;
                });
                if (tmp.length > 0) {
                    return tmp[0].PlayerId;
                } else {
                    return false;
                }
            },
            showMatchId: function () {
                var self = this;
                var tmp = _.filter(this.searchArray, (obj) => {
                    //return obj.enable == true;
                    return obj.apiName == self.selected;
                });
                if (tmp.length > 0) {
                    return tmp[0].MatchId;
                } else {
                    return false;
                }
            },
            showIpAddress: function () {
                var self = this;
                var tmp = _.filter(this.searchArray, (obj) => {
                    //return obj.enable == true;
                    return obj.apiName == self.selected;
                });
                if (tmp.length > 0) {
                    return tmp[0].RemoteIp;
                } else {
                    return false;
                }
            },
            showGroupId: function () {
                var self = this;
                var tmp = _.filter(this.searchArray, (obj) => {
                    //return obj.enable == true;
                    return obj.apiName == self.selected;
                });
                if (tmp.length > 0) {
                    return tmp[0].GroupId;
                } else {
                    return false;
                }
            },
            showSide: function () {
                var self = this;
                var tmp = _.filter(this.searchArray, (obj) => {
                    //return obj.enable == true;
                    return obj.apiName == self.selected;
                });
                if (tmp.length > 0) {
                    return tmp[0].Side;
                } else {
                    return false;
                }
            },
            showSearchType: function () {
                var self = this;
                var tmp = _.filter(this.searchArray, (obj) => {
                    //return obj.enable == true;
                    return obj.apiName == self.selected;
                });
                if (tmp.length > 0) {
                    return tmp[0].SearchType;
                } else {
                    return false;
                }
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
                        if (this.value != '') {
                            var m = moment.utc(self.value, "YYYY-MM-DDTHH:mm:ss");
                            self.date = m.tz(self.loginInfo.timezone.timezoneCode).format('YYYY-MM-DD');
                            self.time = m.tz(self.loginInfo.timezone.timezoneCode).format('HH:mm');
                        }
                        this.settingOK = true;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            search: function () {
                var self = this;
                var tmp = _.filter(_.cloneDeep(this.searchArray), (obj) => {
                    //return obj.enable == true;
                    return obj.apiName == self.selected;
                });
                if (tmp.length == 0) return;

                this.selectedIpAddress = $('#ipInput').val();

                this.searchDataList.push(this.makeSearchData());

                var tmp = this.windowCount;
                this.windowCount = 0;
                this.windowCount = tmp;
            },
            makeSearchData: function () {
                var self = this;

                this.searchAccount = this.selectedAccount;
                this.searchMatchId = this.selectedMatchId;
                this.searchStartDate = this.startDate;
                this.searchEndDate = this.endDate;
                this.searchGroupId = this.selectedGroupId;
                this.searchIpAddress = this.selectedIpAddress;
                this.searchSide = this.selectedSide;
                this.searchSearchType = this.selectedSearchType;
                //this.getLogs();

                var tmp = _.filter(_.cloneDeep(this.searchArray), (obj) => {
                    //return obj.enable == true;
                    return obj.apiName == self.selected;
                });

                if (tmp.length == 0) {
                    return null;
                }
                //_.cloneDeep(tmp),
                var tmpData = {
                    searchItems: tmp[0],
                    account: _.cloneDeep(this.searchAccount),
                    matchId: _.cloneDeep(this.searchMatchId),
                    ipAddress: _.cloneDeep(this.searchIpAddress),
                    groupId: _.cloneDeep(this.searchGroupId),
                    side: _.cloneDeep(this.searchSide),
                    SearchType: _.cloneDeep(this.searchSearchType),
                    startDate: _.cloneDeep(this.searchStartDate),
                    endDate: _.cloneDeep(this.searchEndDate),
                    queryArray: self.getQuery(),
                    isActive: false,
                    timeStamp: moment().unix(), 
                    isRemove: false
                };

                //if (self.searchDataList.length == 0) {
                //    tmpData.isActive = true;
                //} else {
                //    tmpData.isActive = false;
                //}
                
                _.forEach(self.searchDataList, function (item, key) {
                    item.isActive = false;
                });

                tmpData.isActive = true;

                if (tmp.length > 0) {

                    _.forEach(tmp, function (item, index) {

                        var tmpItem = {
                            
                            parPage: 10,
                            currentPage: 1,
                            pageCount: 0,
                            totalCount: 0,
                            headerItems: [],
                            bodyItems: [],
                            isDone: false
                        }


                        item = Object.assign(item, tmpItem);

                        var queryArray = self.getQuery();
                        item.queryArray = queryArray;

                        item.getList = function (page) {
                            item.currentPage = page;

                            var query = [];
                            query.push('CountPerPage=' + item.parPage);
                            query.push('PageNumber=' + item.currentPage);
                            query = query.concat(tmpData.queryArray);

                            var targetApi = new URL('/api/log/game/' + item.apiName, location.origin);
                            //targetApi.search = tmpData.queryArray.join('&')
                            targetApi.search = query.join('&')
                            var res = axios.get(targetApi.href)
                                .then(response => {
                                    _.forEach(response.data.logList, function (log) {
                                        log.datetime = self.getDateTime(log.datetime);
                                    });
                                    //
                                    item.items = response.data.logList;

                                    item.headerItems = self.getHeaderArray(response.data.logList);
                                    item.bodyItems = self.getBodyArray(response.data.logList);

                                    item.pageCount = Math.ceil(response.data.totalCount / item.parPage);
                                    item.totalCount = response.data.totalCount;

                                    item.isDone = true;

                                    var tmp = self.windowCount;
                                    self.windowCount = 0;
                                    self.windowCount = tmp;
                                }).catch(error => {
                                    console.log(error);
                                });
                        };
                        item.getList(1);

                    });

                    tmpData.searchItems = tmp[0];

                    if (this.searchDataList.length == 0) {
                        tmpData.name = 'result_' + parseInt(this.index + 1);
                    } else {
                        tmpData.name = 'result_' + parseInt(this.index + 1);
                    }
                    this.index++;

                }
                return tmpData;
            },
            removeItem: function (list, target) {

                target.isRemove = true;

            },
            tabClick: function (list, target) {

                var removeItems = _.filter(list, (obj) => {
                    return obj.isRemove == true;
                });

                if (removeItems.length > 0) {
                    var tmp = [];
                    if (removeItems[0].isActive == true) {
                        var isNext = 0;

                        _.forEach(list, function (item, key) {
                            item.isActive = false;
                        });

                        _.forEach(list, function (item, key) {
                            if (isNext == 1) {
                                item.isActive = true;
                                isNext = 2;
                            }
                            if (item.isRemove == true) isNext = 1;
                        });

                        if (isNext == 1) {
                            tmp = _.filter(list, (obj) => {
                                return obj.isRemove == false;
                            });

                            if (tmp.length > 0) tmp[tmp.length - 1].isActive = true;

                        } else {
                            tmp = _.filter(list, (obj) => {
                                return obj.isRemove == false;
                            });
                        }

                    } else {
                        tmp = _.filter(list, (obj) => {
                            return obj.isRemove == false;
                        });
                    }

                    this.searchDataList = tmp;
                } else {
                    _.forEach(list, function (item, key) {
                        item.isActive = false;
                    });
                    target.isActive = true;
                }

            },
            getQuery: function () {
                var queryArray = [];
                queryArray.push('From=' + moment.tz(this.searchStartDate, this.loginInfo.timezone.timezoneCode).utc().format('YYYY-MM-DDTHH:mm').toString());
                queryArray.push('To=' + moment.tz(this.searchEndDate, this.loginInfo.timezone.timezoneCode).utc().format('YYYY-MM-DDTHH:mm').toString());
                if (this.searchAccount && this.showPlayerId) queryArray.push('PlayerId=' + this.searchAccount.trim().toString());
                if (this.searchMatchId && this.showMatchId) queryArray.push('MatchId=' + this.searchMatchId.trim().toString());
                if (this.searchGroupId && this.showGroupId) queryArray.push('GroupId=' + this.searchGroupId.trim().toString());
                if (this.searchIpAddress && this.showIpAddress) queryArray.push('RemoteIp=' + this.searchIpAddress.trim().toString());
                if (this.searchSide && this.showSide) queryArray.push('Side=' + this.searchSide.trim().toString());
                if (this.searchSearchType && this.showSearchType) {
                    queryArray.push('SearchType=' + this.searchSearchType.trim().toString());
                    if (this.searchSearchType == '0') {
                        queryArray.push('PlayerId=' + this.searchAccount.trim().toString());
                    }
                    if (this.searchSearchType == '1') {
                        queryArray.push('GroupId=' + this.searchGroupId.trim().toString());
                    }
                    if (this.searchSearchType == '2') {
                        queryArray.push('MatchId=' + this.searchMatchId.trim().toString());
                        queryArray.push('Side=' + this.searchSide.trim().toString());
                    }
                    if (this.searchSearchType == '3') {
                        queryArray.push('MatchId=' + this.searchMatchId.trim().toString());
                    }
                }
                return _.cloneDeep(queryArray);
            },
            locate: function () {
                var tz = jstz.determine();

                return tz.name();
            },
            selectViewTarget: function (list, target) {
                _.each(list, function (li) {
                    li.isActive = false;
                });
                target.isActive = true;
                //target.getList(1);

                var tmp = this.windowCount;
                this.windowCount = 0;
                this.windowCount = tmp;
            },
            getHeaderArray: function (data) {
                if (data.length == 0) return;
                return Object.keys(data[0]);
            },
            getBodyArray: function (data) {
                if (data.length == 0) return;
                var keys = Object.keys(data[0]);
                var retArray = [];

                _.each(data, function (el) {
                    var tmp = [];
                    _.each(keys, function (key) {
                        tmp.push(el[key]);
                    });
                    retArray.push(tmp);
                });
                return retArray;
            },
            getTooltipValue: function (data) {

                var retArray = [];
                if (data.searchItems.PlayerId) retArray.push( 'PlayerId: ' + data.account);
                if (data.searchItems.RemoteIp) retArray.push(  'RemoteIp: ' + data.ipAddress);
                if (data.searchItems.MatchId) retArray.push(  'MatchId: ' + data.matchId);
                if (data.searchItems.GroupId) retArray.push('GroupId: ' + data.groupId);
                if (data.searchItems.Side) retArray.push('Side: ' + data.side);
                if (data.searchItems.SearchType) {
                    retArray.push('SearchType: ' + data.SearchType);
                    if (data.SearchType == '0') {
                        retArray.push('PlayerId: ' + data.account);
                    }
                    if (data.SearchType == '1') {
                        retArray.push('GroupId: ' + data.groupId);
                    }
                    if (data.SearchType == '2') {
                        retArray.push('MatchId: ' + data.matchId);
                        retArray.push('Side: ' + data.side);
                    }
                    if (data.SearchType == '3') {
                        retArray.push('MatchId: ' + data.matchId);
                    }
                }
                if (data.searchItems.From) retArray.push(  'From: ' + data.startDate);
                if (data.searchItems.To) retArray.push(  'To: ' + data.endDate);

                return retArray.join('<br />');
            },
            getResultName: function (data) {
                if (data.searchItems.isDone) {
                    return data.searchItems.name + '(' + data.searchItems.totalCount + ')';
                } else {
                    return data.searchItems.name + '(-)';
                }
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
                var m = moment.utc(val, "YYYY-MM-DDTHH:mm:ss");
                return m.tz(this.loginInfo.timezone.timezoneCode).format(this.dateFormat);
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
            }
        },
        watch: {

        },
    });
});