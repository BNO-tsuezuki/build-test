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
                showModal: false,
                showEditName: false,
                searchData: {
                    item: null,
                    player: {
                        playerName: '',
                        playerNo:''
                    }
                },
                editTarget: {}
            }
        },
        mounted: function () {
            this.getData();
        },
        computed:{
            searchVal: function () {
                return store.getters.searchValue;
            }
        },
        //async created(){
        created(){

        },
        methods: {
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
            myUrl: function (_url) {
                var url = new URL(_url, location.origin);
                if (this.searchVal.searchValue) {
                    url.search = 'search=' + this.searchVal.searchValue;
                }
                return url.href;
            },
            editName: function () {
                this._data.editTarget = _.cloneDeep(this.searchData.player);
                this.showEditName = true;
            },
            saveEdit: function () {
                var postData = {
                    player: {
                        playerName: this._data.editTarget.playerName
                    }
                };
                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                var url = targetApi.pathname + '/' + this.searchData.player.playerId + '/name';
                //alert(url.pathname);
                axios.put(url, postData)
                    .then(function (response) {
                        self.getData();
                        self.showEditName = false;
                    }).catch(function (error) {
                        self.showEditName = false;
                    });

            }

        },
        watch: {
            
        },
    });
});