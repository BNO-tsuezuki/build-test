
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
                showMessage: false,
                message: '',
                passwordOld: '',
                passwordNew: '',

            }
        },
        mounted:function(){
            
        },

        computed:{
            myAccount: function () {
                return store.getters.account;
            }
        },
        //async created(){
        created(){

        },
        methods:{
            changePassword: function () {
                
                //if (this._data.password1 != this._data.password2) {
                //    this._data.showMessage = true;
                //    this._data.message = 'Different password entered';
                //}

                var postData = {
                    password: this._data.passwordOld,
                    NewPassword: this._data.passwordNew
                }
                var self = this;
                axios.put('/api/auth/ChangePassword', postData)
                    .then(response => {
                        self._data.message = 'success!';
                        self._data.showMessage = true;
                    }).catch(error => {
                        self._data.message = 'error';
                        self._data.showMessage = true;
                    });
            }

        },
        watch: {
            
        },
    });
});