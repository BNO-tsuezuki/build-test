
var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                account: "",
                role: '',
                name: '',
                message: '',
                showModal: false,
                success: false,
                password: '',
                isAccountError: false,
                isRoleError: false,
                isNameError: false,
                isAccountValid: false,
                isRoleValid: false,
                isNameValid: false,

            }
        },
        mounted:function(){
            
        },

        computed:{

        },
        //async created(){
        created(){

        },
        methods:{
            makeMember: function () {
                if (this.validate()) return;
                
                var postData = {
                    account: this._data.account,
                    role: this._data.role,
                    name: this._data.name
                };
                var self = this;
                var targetApi = new URL('api/user', location.origin);
                var url = targetApi.pathname;
                axios.post(url, postData)
                    .then(function (response) {
                        self._data.password = response.data.password;
                        self._data.message = 'Member created!';
                        self._data.success = true;
                        self._data.showModal = true;
                        self.reset();
                    }).catch(function (error) {
                        self._data.message = 'Processing failed';
                        self._data.success = false;
                        self._data.showModal = true;
                    });
            },
            close: function () {
                this._data.success = false;
                this._data.showModal = false;
            },
            validate: function () {
                this.validate_clear();

                if (!this._data.account) this._data.isAccountError = true;
                if (!this._data.role) this._data.isRoleError = true;
                if (!this._data.name) this._data.isNameError = true;

                if ((this._data.isAccountError || this._data.isRoleError || this._data.isNameError)) {
                    return true;
                } else {
                    this.validate_clear();
                    return false;
                }
            },
            validate_clear: function () {
                this._data.isAccountError = false;
                this._data.isRoleError = false;
                this._data.isNameError = false;
                this._data.isAccountValid = false;
                this._data.isRoleValid = false;
                this._data.isNameValid = false;
            },
            onBlurAccount: function () {
                if (this._data.account) this._data.isAccountValid = true;
            },
            onBlurRole: function () {
                if (this._data.role) this._data.isRoleValid = true;
            },
            onBlurName: function () {
                if (this._data.name) this._data.isNameValid = true;
            },
            reset: function () {
                this._data.account = '';
                this._data.role = '';
                this._data.name = '';
            }

        },
        watch: {
            
        },
    });
});