
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                lang: '',
                playerData:
                {
                    player: {
                        playnerName: '',
                        playerNo: ''
                    }
                },
                searchData:
                {

                },
                dispItem: [],
                dispType: '',
                isStartUpLoading: true,
                isLoading: false,
                showEditExp: false,
                editTarget: {
                    totalExp: 0,

                },

            }
        },
        mounted: function () {
            this.getData();
            this.getPlayer();
            this.getMyData();
        },

        computed: {
            dispData: function () {
                var self = this;
                if (this._data.dispType == '') {
                    this._data.dispItem = this._data.searchData.items;
                    return this._data.dispItem;
                } else {
                    var array = this._data.searchData.items.filter(function (value) {
                        return value.itemType == self.dispType;
                    });
                    this._data.dispItem = array;
                    return this._data.dispItem;
                }
            },
            isSearch: function () {
                if (getParameter('playerId') != '') return true;
                return false;
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
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getPlayer: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId'), location.origin);
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self.playerData = response.data.player;
                        //if (self.searchData.player.playerId) self.getLogs();
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            getData: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId') + '/pass', location.origin);
                //alert(url.pathname);
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        _.forEach(response.data.battlePasses, function (item) {
                            item.editExp = item.totalExp;
                            item.isEdit = false;
                        });
                        self._data.searchData = response.data;

                    }).catch(function (error) {

                    });
            },
            isPremiumClick: function (item) {
                var tmp = _.cloneDeep(item);
                tmp.isPremium = !tmp.isPremium;
                var postData = {
                    battlePass: {
                        totalExp: tmp.totalExp,
                        isPremium: tmp.isPremium
                    }
                };
                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + this.playerData.playerId + '/pass/' + item.id;
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        toastr.success('Succeeded.');
                        self.getData();
                    }).catch(function (error) {
                        toastr.error('error.');
                    });
            },
            editExp: function (item) {
                item.isEdit = true;
            },
            saveEdit: function (item) {
                var postData = {
                    battlePass: {
                        totalExp: item.editExp,
                        isPremium: item.isPremium
                    }
                };
                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + this.playerData.playerId + '/pass/' + item.id;
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        self.getData();
                        toastr.success('Succeeded.');
                        item.isEdit = false;
                    }).catch(function (error) {
                        toastr.error('error.');
                        self.getData();
                        item.isEdit = false;
                    });

            },
            cancelEdit: function (item) {
                item.isEdit = false;
                item.editExp = item.totalExp;
            }

        },
        watch: {

        },
    });
});