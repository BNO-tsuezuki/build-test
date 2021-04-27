
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
                originalData: {
                    loginVersion: {
                        packageVersion: {
                            build: 0,
                            major: 0,
                            minor: 0,
                            patch: 0,
                        }
                    },
                    matchmakeVersion: {
                        packageVersion: {
                            build: 0,
                            major: 0,
                            minor: 0,
                            patch: 0,
                        }
                    },
                    replayVersion: {
                        masterDataVersion: {
                            major: 0,
                            minor: 0,
                            patch: 0,
                        },
                        packageVersion: {
                            build: 0,
                            major: 0,
                            minor: 0,
                            patch: 0,
                        }
                    }
                },
                dispItem: {
                    loginVersion: {
                        packageVersion: {
                            build: 0,
                            major: 0,
                            minor: 0,
                            patch: 0,
                        }
                    },
                    matchmakeVersion: {
                        packageVersion: {
                            build: 0,
                            major: 0,
                            minor: 0,
                            patch: 0,
                        }
                    },
                    replayVersion: {
                        masterDataVersion: {
                            major: 0,
                            minor: 0,
                            patch: 0,
                        },
                        packageVersion: {
                            build: 0,
                            major: 0,
                            minor: 0,
                            patch: 0,
                        }
                    }
                },
                isLoading: false,
            }
        },
        mounted: function () {
            this.getData();
        },

        computed: {
            LoginVersion: function () {
                return this.toPackageVersion(this._data.dispItem.loginVersion.packageVersion);
            },
            MatchmakeVersion: function () {
                return this.toPackageVersion(this._data.dispItem.matchmakeVersion.packageVersion);
            },
            ReplayPackageVersion: function () {
                return this.toPackageVersion(this._data.dispItem.replayVersion.packageVersion);
            },
            ReplayMasterDataVersion: function () {
                return this.toMasterDataVersion(this._data.dispItem.replayVersion.masterDataVersion);
            },
            orgLoginVersion: function () {
                return this.toPackageVersion(this._data.originalData.loginVersion.packageVersion);
            },
            orgMatchmakeVersion: function () {
                return this.toPackageVersion(this._data.originalData.matchmakeVersion.packageVersion);
            },
            orgReplayPackageVersion: function () {
                return this.toPackageVersion(this._data.originalData.replayVersion.packageVersion);
            },
            orgReplayMasterDataVersion: function () {
                return this.toMasterDataVersion(this._data.originalData.replayVersion.masterDataVersion);
            }
        },
        //async created(){
        created() {

        },
        methods: {
            getData: function () {
                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/version', location.origin);
                var url = targetApi.pathname;
                //alert(url.pathname);
                axios.get(url, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.dispItem = response.data;
                        self._data.originalData = _.cloneDeep(response.data);
                        self._data.isStartUpLoading = false;
                        self._data.isLoading = false;
                    }).catch(function (error) {
                        //this.data.fail = true;
                        self._data.isStartUpLoading = false;
                        self._data.isLoading = false;
                    });
            },
            putData: function () {
                this._data.isLoading = true;
                var postData = this._data.dispItem;
                var self = this;
                var targetApi = new URL('api/game/version', location.origin);
                var url = targetApi.pathname;
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        self._data.isLoading = false;
                        self.getData();
                    }).catch(function (error) {
                        self._data.isLoading = false;
                    });
            },
            toPackageVersion: function (data) {
                return _.padStart(data.major, 2, '0') + '.' + _.padStart(data.minor, 2, '0') + '.' + _.padStart(data.patch, 2, '0') + '.' + _.padStart(data.build, 5, '0');
            },
            toMasterDataVersion: function (data) {
                return _.padStart(data.major, 3, '0') + '_' + _.padStart(data.minor, 3, '0') + '_' + _.padStart(data.patch, 3, '0');
            },
            validate2: function (data) {
                if (String(data).length <= 2) {
                    return true;
                } else {
                    return false;
                }
            },
            validate3: function (data) {
                if (String(data).length <= 3) {
                    return true;
                } else {
                    return false;
                }
            },
            validate5: function (data) {
                if (String(data).length <= 5) {
                    return true;
                } else {
                    return false;
                }
            }

        },
        watch: {

        },
    });
});