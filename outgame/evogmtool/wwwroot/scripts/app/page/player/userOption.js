
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: '',
                lang: '',
                dispItem: [],
                dispType: '',
                targetMS: 'Common',
                searchData:
                {
                    options: [],
                    player: {
                        playnerName: '',
                        playerNo: ''
                    }
                },
                mobileSuitList:[]
            }
        },
        mounted: function () {
            this.dispType = 'Game options';
            this.getMyData();
            this.getData();
            this.getPlayerData();
            this.getMobileSuitList();
        },

        computed: {
            isSearch: function () {
                if (getParameter('playerId')) return true;
                return false;
            },
            dispNameAndNo: function () {
                if (this.searchData.player.playerName != undefined && this.searchData.player.playerId != undefined) {
                    return 'playerName: ' + this._data.searchData.player.playerName;
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
            getData: function () {
                if (getParameter('playerId') == '') return;
                var opt = '0';
                if (this.dispType == 'Game options') opt = '0';
                if (this.dispType == 'Graphic options') opt = '1';
                if (this.dispType == 'Sound options') opt = '3';

                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId') + '/appoption/' + opt, location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData.options = response.data.options;
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            getMSData: function () {
                if (getParameter('playerId') == '') return;
                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId') + '/mobilesuitoption/' + this.targetMS, location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        _.forEach(response.data.options, function (value) {
                            if (value.valueType == 'range-ms' && value.rangeMsSetting.isCommonSettingAvailable) {
                                if (value.value == -1) {
                                    value.useCommon = true;
                                } else {
                                    value.useCommon = false;
                                }
                            } 
                            if (value.valueType == 'switch-ms' && value.switchMsSetting.isCommonSettingAvailable) {
                                if (value.value == -1) {
                                    value.useCommon = true;
                                } else {
                                    value.useCommon = false;
                                }
                            } 
                        });
                        self._data.searchData.options = response.data.options;
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            getMobileSuitList: function () {
                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/misc/MobileSuitList', location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        self._data.mobileSuitList = response.data.mobileSuits;
                    }).catch(function (error) {
                    });
            },
            getPlayerData: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId') , location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData.player = response.data.player;
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            save: function (item) {
                if (this.dispType == 'Control options') {
                    this.saveMsOption(item);
                } else {
                    this.saveAppOption(item);
                }
            },
            saveAppOption: function (item) {
                if (getParameter('playerId') == '') return;
                var opt = '0';
                if (this.dispType == 'Game options') opt = '0';
                if (this.dispType == 'Graphic options') opt = '1';
                if (this.dispType == 'Sound options') opt = '3';
                var postData = {
                    optionNo: item.optionNo,
                    value: item.value
                };
                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId') + '/appoption/' + opt, location.origin);
                //var url = targetApi.pathname + '/' + store.getters.searchValue.searchValue;
                //alert(url.pathname);
                axios.put(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        toastr.success(item.enText + ': Changes have been saved.');
                        self.getData();
                    }).catch(function (error) {
                        toastr.error('error.');
                    });
            },
            saveMsOption: function (item) {
                if (getParameter('playerId') == '') return;
                var postData = {
                    optionNo: item.optionNo,
                    value: item.useCommon ? -1 : item.value
                };
                if (item.useCommon == false && item.value == -1) {
                    if (item.valueType == 'range-ms') {
                        postData.value = item.rangeMsSetting.min;
                        item.value = item.rangeMsSetting.min;
                    }
                    if (item.valueType == 'switch-ms') {
                        postData.value = item.switchMsSetting.items[0].index;
                        item.value = item.switchMsSetting.items[0].index;
                    }
                }

                var self = this;
                var targetApi = new URL('/api/game/player/' + getParameter('playerId') + '/mobilesuitoption/' + this.targetMS, location.origin);
                //var url = targetApi.pathname + '/' + store.getters.searchValue.searchValue;
                //alert(url.pathname);
                axios.put(targetApi.href, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        toastr.success(item.enText + ': Changes have been saved.');
                        self.getMSData();
                    }).catch(function (error) {
                        toastr.error('error.');
                    });
            },
            rangeString: function (item) {
                if (item.valueType == 'range') {
                    return 'min: ' + item.rangeSetting.min + '<br />' + 'max: ' + item.rangeSetting.max;
                } else {
                    return 'min: ' + item.rangeMsSetting.min + '<br />' + 'max: ' + item.rangeMsSetting.max;
                }
            },
            validationRange: function (item) {
                if (item.valueType == 'range') {
                    if (item.value > item.rangeSetting.max || item.value < item.rangeSetting.min) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    if (item.value > item.rangeMsSetting.max || item.value < item.rangeMsSetting.min) {
                        return true;
                    } else {
                        return false;
                    }
                }

            }
        },
        watch: {

        },
    });
});