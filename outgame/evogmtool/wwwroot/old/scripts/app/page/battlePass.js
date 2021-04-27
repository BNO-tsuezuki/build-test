
var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
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
            this.getPlayer();
            
        },

        computed:{
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
                if (store.getters.searchValue.searchValue) return true;
                return false;
            },
            dispNameAndNo: function () {
                if (this._data.playerData.player.playerName != undefined && this._data.playerData.player.playerNo != undefined) {
                    return this._data.playerData.player.playerName + '#' + this._data.playerData.player.playerNo
                }
                return '#';
            }
        },
        //async created(){
        created(){

        },
        methods: {
            getPlayer: function () {
                if (!store.getters.searchValue.searchValue) {
                    this._data.isStartUpLoading = false;
                    return;
                }

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + store.getters.searchValue.searchValue;
                //alert(url.pathname);
                axios.get(url, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.playerData = response.data;
                        self.getData();
                    }).catch(function (error) {

                    });
            },
            getData: function () {
                if (!store.getters.searchValue.searchValue) {
                    this._data.isStartUpLoading = false;
                    return;
                }

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + this.playerData.player.playerId + '/pass';
                //alert(url.pathname);
                axios.get(url, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData = response.data;
                        self._data.isStartUpLoading = false;
                        self._data.isLoading = false;
                    }).catch(function (error) {
                        //this.data.fail = true;
                        self._data.isStartUpLoading = false;
                        self._data.isLoading = false;
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
                var url = targetApi.pathname + '/' + this.playerData.player.playerId + '/pass/' + item.id;
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        toastr.success('Succeeded.');
                        self.getData();
                    }).catch(function (error) {

                    });
            },
            editExp: function (item) {
                this._data.showEditExp = true;
                this._data.editTarget = _.cloneDeep(item);

            },
            saveEdit: function () {
                var postData = {
                    battlePass: {
                        totalExp: this._data.editTarget.totalExp,
                        isPremium: this._data.editTarget.isPremium
                    }
                };
                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + this.playerData.player.playerId + '/pass/' + this._data.editTarget.id;
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        self.getData();
                        toastr.success('Succeeded.');
                        self._data.showEditExp = false;
                    }).catch(function (error) {
                        self.getData();
                        self._data.showEditExp = false;
                    });

            }

        },
        watch: {
            
        },
    });
});