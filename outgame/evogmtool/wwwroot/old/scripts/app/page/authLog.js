
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
                searchData: []
            }
        },
        mounted:function(){
            this.getLog();
        },

        computed:{

        },
        //async created(){
        created(){

        },
        methods:{
            getLog: function () {
                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/log/auth', location.origin);
                var url = targetApi.pathname + '/';
                //alert(url.pathname);
                axios.get(url, postData)
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