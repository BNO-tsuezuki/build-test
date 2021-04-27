
var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
                dispItem: [],
                dispType: '',
                searchData:
                {
                    options: [],
                    player: {
                        playnerName: '',
                        playerNo: ''
                    }
                },
            }
        },
        mounted: function () {
            this._data.dispType = 'category1';
            this.getData();
        },

        computed:{
            dispData: function () {
                var self = this;
                if (this._data.dispType == '' || this._data.searchData.options.length == 0) {
                    return [];
                } else {
                    if (this._data.dispType == 'category1') return this._data.searchData.options[0].values;
                    if (this._data.dispType == 'category2') return this._data.searchData.options[1].values;
                    if (this._data.dispType == 'category3') return this._data.searchData.options[2].values;
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
        methods:{
            getData: function () {
                if (store.getters.searchValue.searchValue == "") return;

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
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            putData: function () {
                if (store.getters.searchValue.searchValue == "") return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + store.getters.searchValue.searchValue;
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        //location.href = '/index.html';
                        self._data.searchData = response.data;
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
        },
        watch: {
            
        },
    });
});