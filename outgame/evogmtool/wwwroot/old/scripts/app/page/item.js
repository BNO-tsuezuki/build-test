
var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    //if(getParameter("debug") == "true") 
    var options = {
        //position: 'top-right',
        duration: 2000,
        //fullWidth: true,
        type: 'success',
        closeOnSwipe: true,
    }
    Vue.use(Toasted, options);
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: '',
                searchData:
                {
                    player: {
                        playnerName: '',
                        playerNo: ''
                    }
                },
                dispItem: [],
                dispType: '',
                isStartUpLoading: true,
                isLoading: false,

            }
        },
        mounted: function () {
            if (getParameter("dispType")) {
                this._data.dispType = getParameter("dispType");
            }
            this.getData();
        },

        computed:{
            dispData: function () {
                var self = this;
                if (this._data.dispType == '' || !this._data.searchData.items) {
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
                if (this._data.searchData.player.playerName != undefined && this._data.searchData.player.playerNo != undefined) {
                    return this._data.searchData.player.playerName + '#' + this._data.searchData.player.playerNo
                }
                return '#';
            }
        },
        //async created(){
        created(){

        },
        methods: {
            getData: function (item) {
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
                        self._data.searchData = response.data;
                        self._data.isStartUpLoading = false;
                        self._data.isLoading = false;
                        if (item) item.isLoading = false;
                    }).catch(function (error) {
                        //this.data.fail = true;
                        self._data.isStartUpLoading = false;
                        self._data.isLoading = false;
                        if (item) item.isLoading = false;
                    });
            },
            getAll: function () {
                if (this.chkLoading()) return;
                this._data.isLoading = true;
                var tmpData = _.cloneDeep(this._data.dispItem);
                tmpData.forEach(function (item) {
                    if (item.isDefault == false) {
                        item.owned = true;
                    }
                });
                this.putItems(tmpData);
            },
            reset: function () {
                if (this.chkLoading()) return;
                this._data.isLoading = true;
                var tmpData = _.cloneDeep(this._data.dispItem);
                tmpData.forEach(function (item) {
                    if (item.isDefault == false) {
                        item.owned = false;
                    }
                });
                this.putItems(tmpData);
            }, putItems: function (targetData) {
                if (!store.getters.searchValue.searchValue) return;

                var array = targetData.filter(function (item) {
                    return item.isDefault == false;
                });
                var postData = {
                    items: array,
                };

                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + this._data.searchData.player.playerId + '/item';
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        self.getData();
                    }).catch(function (error) {
                        self.getData();
                    });

            },
            ownedClick: function (item) {
                if (this.chkLoading()) return false;
                item.isLoading = true;
                var array = [];
                var tmp = _.cloneDeep(item);
                tmp.owned = !tmp.owned;
                array.push(tmp);
                var postData = {
                    items: array,
                };

                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + this._data.searchData.player.playerId + '/item';
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        self.$toasted.show(item.displayName + ' Owned: ' + item.owned);
                        self.getData(item);
                    }).catch(function (error) {
                        self.getData();
                    });
            },
            chkLoading: function () {
                return this._data.searchData.items.some(function (element, index, array) {
                    return element.isLoading == true;
                });
            }

        },
        watch: {
            
        },
    });
});