
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
                isStartUpLoading: true,
                isLoading: false,
            }
        },
        mounted:function(){
            this.getData();
        },

        computed:{
            dispData: function () {
                if (this._data.dispItem.units) {
                    return this._data.dispItem.units;
                } else {
                    return [];
                }
            },
        },
        //async created(){
        created(){

        },
        methods:{
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
                        self._data.isStartUpLoading = false;
                        self._data.isLoading = false;
                    }).catch(function (error) {
                        //this.data.fail = true;
                        self._data.isStartUpLoading = false;
                        self._data.isLoading = false;
                    });
            },

        },
        watch: {
            
        },
    });
});