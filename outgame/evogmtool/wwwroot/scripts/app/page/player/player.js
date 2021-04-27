var vm_select;
var Tabs = window['VueTabs'].Tabs,
    Tab = window['VueTabs'].Tab;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    Vue.component('paginate', VuejsPaginate);
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                lang: "",
                showModal: false,
                showEditName: false,
                nameEdit: false,
                expEdit: false,
                tutorialEdit: false,
                privilegelevelEdit: false,
                dateFormat: '',
                loginInfo: {},
                lang: '',
                searchData: {
                    item: null,
                    player: {
                        playerName: '',
                        playerId: ''
                    },
                    exp: {
                        totalExp: 0,
                        level: 0,
                        rewardLevel: 0,
                        levelExp: 0,
                        nextExp: 0,

                    },
                    tutorial: {
                        isFirstTutorialEnd: false,
                        tutorialProgress: 0
                    },
                    account: {
                        isCheatCommandAvailable: false
                    }
                },
                editTarget: {
                    item: null,
                    player: {
                        playerName: '',
                        playerId: ''
                    },
                    exp: {
                        totalExp: 0,
                        level: 0,
                        rewardLevel: 0,
                        levelExp: 0,
                        nextExp: 0,
                    },
                    tutorial: {
                        isFirstTutorialEnd: false,
                        tutorialProgress: 0
                    }
                },
                startDate: '',
                endDate: '',
                PlayerAccountCreateHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                LoginHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                LogoutHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                PlayerExpHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                ChatSayHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                ChatDirectHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                MatchStartPlayerHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                MatchExecutionHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                MatchExitPlayerHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                },
                MatchEntryPlayerHistory:
                {
                    items: [],
                    parPage: 10,
                    currentPage: 1,
                    pageCount: 0
                }
            }
        },
        components: {
            Tabs,
            Tab
        },
        mounted: function () {
            //this.startDate = moment().utc().subtract('days', 7);
            //this.endDate = moment().utc();
            this.getMyData();
            this.getPlayerData();
            this.getPlayerExpData();
            this.getPlayerTutorialProgress();
            this.getPlayerPrivilegelevel();
        },
        computed: {
            searchVal: function () {
                return store.getters.searchValue;
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
                        if (self.loginInfo.language.languageCode == 'ja') {
                            self.dateFormat = 'YYYY/MM/DD HH:mm';
                        } else {
                            self.dateFormat = 'MM/DD/YYYY, HH:mm';
                        }
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getPlayerData: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId'), location.origin);
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData.player = response.data.player;
                        self._data.editTarget.player = _.cloneDeep(response.data.player);
                        //if (self.searchData.player.playerId) self.getLogs();
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            getPlayerExpData: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/player/' + getParameter('playerId') + '/exp', location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData.exp = response.data.exp;
                        self._data.editTarget.exp = _.cloneDeep(response.data.exp);
                        //if (self.searchData.player.playerId) self.getLogs();
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            getPlayerTutorialProgress: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/player/' + getParameter('playerId') + '/tutorial', location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData.tutorial = response.data.tutorial;
                        self._data.editTarget.tutorial = _.cloneDeep(response.data.tutorial);
                        //if (self.searchData.player.playerId) self.getLogs();
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            getPlayerPrivilegelevel: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/account/' + getParameter('playerId') + '/privilegelevel', location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData.account = response.data.account;
                        self._data.editTarget.account = _.cloneDeep(response.data.account);
                        self.privilegelevelEdit = false;
                        //if (self.searchData.player.playerId) self.getLogs();
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            deleteConfirm: function () {
                Swal.fire({
                    title: 'Do you really want to delete this ?',
                    showCancelButton: true,
                    confirmButtonText: `Delete`,
                    confirmButtonColor: 'red',
                }).then((result) => {
                    /* Read more about isConfirmed, isDenied below */
                    if (result.isConfirmed) {
                        this.deletePlayer();
                    }
                })
            },
            deletePlayer: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/account/' + getParameter('playerId') + '/reset' , location.origin);
                axios.put(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/selectPlayer.html';
                        toastr.success('Player Delete Succeeded.');
                    }).catch(function (error) {
                        toastr.error(error.response.data.message);
                    });
            },
            cancelEditName: function () {
                this.nameEdit = false;
                this.editTarget.player = this.searchData.player;
            },
            saveEditName: function () {
                var self = this;
                var postData = {
                    player: {
                        playerName: this.editTarget.player.playerName
                    }
                }
                var targetApi = new URL('api/game/player/' + this.searchData.player.playerId + '/name', location.origin);
                var res = axios.put(targetApi.href, postData)
                    .then(response => {
                        if (response.data.containsNgWord) {
                            toastr.warning('Succeeded. ( Contains NG words. )');
                            self.getPlayerData();
                            self.nameEdit = false;
                        } else {
                            toastr.success('Succeeded.');
                            self.getPlayerData();
                            self.nameEdit = false;
                        }

                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            cancelEditExp: function () {
                this.expEdit = false;
                this.editTarget.exp = this.searchData.exp;
            },
            saveEditExp: function () {
                var self = this;
                var postData = {
                    exp: {
                        totalExp: this.editTarget.exp.totalExp
                    }
                }
                var targetApi = new URL('api/game/player/' + this.searchData.player.playerId + '/exp', location.origin);
                var res = axios.put(targetApi.href, postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getPlayerExpData();
                        self.expEdit = false;
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            cancelEditPrivilegelevel: function () {
                this.privilegelevelEdit = false;
                this.editTarget.account = this.searchData.account;
            },
            saveEditPrivilegelevel: function () {
                var self = this;
                var postData = {
                    account: {
                        isCheatCommandAvailable: this.editTarget.account.isCheatCommandAvailable
                    }
                }
                var targetApi = new URL('api/game/account/' + this.searchData.player.playerId + '/privilegelevel', location.origin);
                var res = axios.put(targetApi.href, postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getPlayerPrivilegelevel();
                        
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            saveEditTutorial: function () {
                var self = this;
                var postData = {
                    player: {
                        isFirstTutorialEnd: this.editTarget.tutorial.isFirstTutorialEnd
                    }
                }
                var targetApi = new URL('api/game/player/' + this.searchData.player.playerId + '/tutorial', location.origin);
                var res = axios.put(targetApi.href, postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getPlayerTutorialProgress();
                        self.tutorialEdit = false;
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            tutorialResetConfirm: function () {
                Swal.fire({
                    title: 'Are you sure you want to reset the tutorial progress?',
                    showCancelButton: true,
                    confirmButtonText: `Reset`,
                    confirmButtonColor: 'red',
                }).then((result) => {
                    /* Read more about isConfirmed, isDenied below */
                    if (result.isConfirmed) {
                        this.tutorialReset();
                    }
                })
            },
            tutorialReset: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/player/' + getParameter('playerId') + '/tutorial/reset', location.origin);
                axios.put(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/selectPlayer.html';
                        toastr.success('Tutorial Reset Succeeded.');
                    }).catch(function (error) {
                        toastr.error(error.response.data.message);
                    });
            },
            getLogs: function () {
                this.getPlayerAccountCreateHistory(1);
                this.getLoginHistory(1);
                this.getLogoutHistory(1);
                this.getPlayerExpHistory(1);
                this.getChatSayHistory(1);
                this.getChatDirectHistory(1);

                this.getMatchStartPlayerHistory(1);
                this.getMatchExecutionHistory(1);
                this.getMatchExitPlayerHistory(1);
                this.getMatchEntryPlayerHistory(1);
            },
            getQuery: function () {
                var queryArray = [];
                queryArray.push('From=' + moment(this.startDate).utc().format('YYYY-MM-DDTHH:mm'));
                queryArray.push('To=' + moment(this.endDate).utc().format('YYYY-MM-DDTHH:mm'));
                queryArray.push('PlayerId=' + this.searchData.player.playerId);
                return queryArray;
            },
            getPlayerAccountCreateHistory: function (page) {
                var self = this;

                with (this.PlayerAccountCreateHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/PlayerAccountCreateHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.PlayerAccountCreateHistory.items = response.data.logList;
                        self._data.PlayerAccountCreateHistory.pageCount = Math.ceil(response.data.totalCount / self.PlayerAccountCreateHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getLoginHistory: function (page) {
                var self = this;

                with (this.LoginHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/LoginHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.LoginHistory.items = response.data.logList;
                        self._data.LoginHistory.pageCount = Math.ceil(response.data.totalCount / self.LoginHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getLogoutHistory: function (page) {
                var self = this;

                with (this.LogoutHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/LogoutHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.LogoutHistory.items = response.data.logList;
                        self._data.LogoutHistory.pageCount = Math.ceil(response.data.totalCount / self.LogoutHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getPlayerExpHistory: function (page) {
                var self = this;

                with (this.PlayerExpHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/PlayerExpHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.PlayerExpHistory.items = response.data.logList;
                        self._data.PlayerExpHistory.pageCount = Math.ceil(response.data.totalCount / self.PlayerExpHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getChatSayHistory: function (page) {
                var self = this;

                with (this.ChatSayHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/ChatSayHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.ChatSayHistory.items = response.data.logList;
                        self._data.ChatSayHistory.pageCount = Math.ceil(response.data.totalCount / self.ChatSayHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getChatDirectHistory: function (page) {
                var self = this;

                with (this.ChatDirectHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/ChatDirectHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.ChatDirectHistory.items = response.data.logList;
                        self._data.ChatDirectHistory.pageCount = Math.ceil(response.data.totalCount / self.ChatDirectHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getMatchStartPlayerHistory: function (page) {
                var self = this;

                with (this.MatchStartPlayerHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/MatchStartPlayerHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.MatchStartPlayerHistory.items = response.data.logList;
                        self._data.MatchStartPlayerHistory.pageCount = Math.ceil(response.data.totalCount / self.MatchStartPlayerHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getMatchExecutionHistory: function (page) {
                var self = this;

                with (this.MatchExecutionHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/MatchExecutionHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.MatchExecutionHistory.items = response.data.logList;
                        self._data.MatchExecutionHistory.pageCount = Math.ceil(response.data.totalCount / self.MatchExecutionHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getMatchExitPlayerHistory: function (page) {
                var self = this;

                with (this.MatchExitPlayerHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/MatchExitPlayerHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.MatchExitPlayerHistory.items = response.data.logList;
                        self._data.MatchExitPlayerHistory.pageCount = Math.ceil(response.data.totalCount / self.MatchExitPlayerHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getMatchEntryPlayerHistory: function (page) {
                var self = this;

                with (this.ChatDirectHistory) {
                    currentPage = page;

                    var queryArray = this.getQuery();
                    queryArray.push('CountPerPage=' + parPage);
                    queryArray.push('PageNumber=' + currentPage);
                }

                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);


                var targetApi = new URL('/api/log/game/MatchEntryPlayerHistory', location.origin);
                targetApi.search = queryArray.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.MatchEntryPlayerHistory.items = response.data.logList;
                        self._data.MatchEntryPlayerHistory.pageCount = Math.ceil(response.data.totalCount / self.MatchEntryPlayerHistory.parPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            logPageUrl: function () {
                var queryArray = [];
                if (getParameter("search") != '') queryArray.push('search=' + getParameter("search"));
                if (this.searchData.player.playerId != '') queryArray.push('playerId=' + this.searchData.player.playerId);

                var target = new URL('/log/GameLog.html', location.origin);
                target.search = queryArray.join('&')
                return target.href;
            },
            myUrl: function (_url) {
                var url = new URL(_url, location.origin);
                if (getParameter("playerId")) {
                    url.search = "playerId=" + getParameter("playerId");
                }
                return url.href;
            }, getDateTime: function (val) {
                if (!val) return '';
                var m = moment.utc(val, "YYYY-MM-DDTHH:mm:ss");
                return m.tz(this.loginInfo.timezone.timezoneCode).format(this.dateFormat);
            }

        },
        watch: {

        },
    });
});