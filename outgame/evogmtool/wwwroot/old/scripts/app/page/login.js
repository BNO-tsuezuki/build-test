var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    if(getParameter("debug") == "true") Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        mounted:function(){

        },
        data:function(){
            return {
                id:'',
                password:'',
                fail: false
            }
        },
        computed:{

        },
        //async created(){
        created(){

        },
        methods:{
            login:function(){
                //this._data.fail = true;
                var self = this;
                var postData = {
                    account : this._data.id,
                    password: this._data.password,
                    returnUrl: ''
                }

                axios.post('/api/Auth/Login', postData)
                    .then(response => {
                        location.href = '/index.html';
                    }).catch(error => {
                        self._data.fail = true;
                    });
            }

        },
        watch: {
            
        },
    });
});