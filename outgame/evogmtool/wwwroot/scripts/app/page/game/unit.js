
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
                dispItem: [],
                isStartUpLoading: true,
                isLoading: false,
            }
        },
        mounted: function () {
            this.getMyData();
            this.getData();
        },

        computed: {
            dispData: function () {
                if (this._data.dispItem.units) {
                    return this._data.dispItem.units;
                } else {
                    return [];
                }
            },
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
                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/unit', location.origin);
                var url = targetApi.pathname;
                //alert(url.pathname);
                axios.get(url, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.dispItem = response.data;
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            changeEnable: function (item) {
                var postData = {
                    isEnabledOnGmTool: item.isEnabledOnGmTool
                };
                var self = this;
                var targetApi = new URL('api/game/unit/' + item.mobileSuitId + '/temporaryavailability', location.origin);
                var url = targetApi.pathname;
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self.getData();
                        toastr.success('Succeeded.');
                    }).catch(function (error) {
                        toastr.error('error.');
                        //this.data.fail = true;

                    });
            }

        },
        watch: {

        },
    });
});