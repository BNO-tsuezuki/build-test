
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    var options = {
        //position: 'top-right',
        duration: 2000,
        //fullWidth: true,
        type: 'success',
        closeOnSwipe: true,
    }
    //Vue.use(Toasted, options);
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                lang: '',
                searchData:
                {
                    player: {
                        playnerName: '',
                        playerNo: ''
                    },
                    items: null

                },
                dispItem: [],
                dispType: '',
                dispTypeCheckBox: {
                    All: false,
                    VoicePack: false,
                    Ornament: false,
                    BodySkin: false,
                    WeaponSkin: false,
                    MvpCelebration: false,
                    Stamp: false,
                    Emotion: false,
                    MobileSuit: false
                },
                isStartUpLoading: true,
                isLoading: false,

            }
        },
        mounted: function () {
            if (getParameter("dispType")) {
                this.dispType = getParameter("dispType");

                if (this.dispType == 'VoicePack') this.dispTypeCheckBox.VoicePack = true;
                if (this.dispType == 'Ornament') this.dispTypeCheckBox.Ornament = true;
                if (this.dispType == 'BodySkin') this.dispTypeCheckBox.BodySkin = true;
                if (this.dispType == 'WeaponSkin') this.dispTypeCheckBox.WeaponSkin = true;
                if (this.dispType == 'MvpCelebration') this.dispTypeCheckBox.MvpCelebration = true;
                if (this.dispType == 'Stamp') this.dispTypeCheckBox.Stamp = true;
                if (this.dispType == 'Emotion') this.dispTypeCheckBox.Emotion = true;
                if (this.dispType == 'MobileSuit') this.dispTypeCheckBox.MobileSuit = true;
            } else {
                this.dispTypeCheckBox.All = true;
            }
            this.getData();
            this.getMyData();
            this.getPlayerData();
        },

        computed: {
            dispData: function () {
                var self = this;
                if (this._data.dispType == '' || !this._data.searchData.items) {
                    this._data.dispItem = this._data.searchData.items;
                    return this._data.dispItem;
                } else {
                    if (this.searchData.items) {
                        var array = this._data.searchData.items.filter(function (value) {
                            return value.itemType == self.dispType;
                        });
                        this._data.dispItem = array;
                        return this._data.dispItem;
                    }
                    else {
                        return null;
                    }
                }
            },
            isSearch: function () {
                if (getParameter('playerId') != '') return true;
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
            getData: function (item) {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId') + '/item', location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';


                        self._data.searchData.items = response.data.items;
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
            getPlayerData: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId') + '/name', location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData.player = response.data.player;
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            getAll: function () {
                var tmpData = _.cloneDeep(this._data.dispItem);
                tmpData.forEach(function (item) {
                    if (item.isDefault == false) {
                        item.owned = true;
                    }
                });
                this.putItems(tmpData, 'GetAll');
            },
            reset: function () {
                var tmpData = _.cloneDeep(this._data.dispItem);
                tmpData.forEach(function (item) {
                    if (item.isDefault == false) {
                        item.owned = false;
                    }
                });
                this.putItems(tmpData, 'Reset');
            },
            putItems: function (targetData, type) {
                if (!getParameter('playerId')) return;

                var array = targetData.filter(function (item) {
                    return item.isDefault == false;
                });
                var postData = {
                    items: array,
                };

                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + getParameter('playerId') + '/item';
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        self.getData();
                        toastr.success(type + ' processing completed');
                    }).catch(function (error) {
                        self.getData();
                    });

            },
            ownedClick: function (item) {
                var array = [];
                var tmp = _.cloneDeep(item);
                tmp.owned = !tmp.owned;
                array.push(tmp);
                var postData = {
                    items: array,
                };

                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + getParameter('playerId') + '/item';
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        //self.$toasted.show(item.displayName + ' Owned: ' + item.owned);
                        toastr.success(item.displayName + ' Owned: ' + item.owned);
                        self.getData(item);
                    }).catch(function (error) {
                        self.getData();
                    });
            }

        },
        watch: {

        },
    });
});