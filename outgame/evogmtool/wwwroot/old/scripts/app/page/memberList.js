
var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: '',
                searchData: '',
                dispItem: [],
                dispType: '',
                showReset: false,
                resetInfo: {
                    member: {},
                    password: '',

                },
                deleteInfo: {
                    member: {},
                },
                showDelete: false,
                showError: false,
                errorMessage: '',
                showEditName: false,
                showEditRole: false,
                editTarget: {},

            }
        },
        mounted:function(){
            if (store.getters.searchValue.searchValue == "") return;

            this.getMember();
        },

        computed:{
            dispData: function () {
                return this._data.searchData;
            }
        },
        //async created(){
        created(){

        },
        methods: {
            getMember: function () {
                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/user', location.origin);
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
            addMember: function () {
                var url = new URL('addMember.html', location.origin);
                if (store.getters.searchValue.searchValue) {
                    url.search = "search=" + store.getters.searchValue.searchValue;
                }

                location.href = url.href;
            },
            passwordReset: function (item) {
                var self = this;
                this._data.resetInfo.member = item;

                var postData = {
                    password: ''
                }

                axios.put('/api/user/' + item.userId + '/ResetPassword', postData)
                    .then(response => {
                        self._data.resetInfo.password = response.data.password;
                        self._data.showReset = true;
                    }).catch(error => {
                        self._data.errorMessage = 'Lack of authority';
                        self._data.showError = true;

                    });
            },
            deleteMember: function (item) {
                var self = this;
                this._data.deleteInfo.member = item;

                var postData = {};

                axios.delete('/api/user/' + item.userId , postData)
                    .then(response => {
                        toastr.success('Delete Succeeded.');
                        self.getMember();
                    }).catch(error => {
                        toastr.error('error.');

                    });
            },
            editName: function (item) {
                this._data.showEditName = true;
                this._data.editTarget = _.cloneDeep(item);
            },
            saveEditName: function () {
                this.putUserName();
            },
            editRole: function (item) {
                this._data.showEditRole = true;
                this._data.editTarget = _.cloneDeep(item);
            },
            saveEditRole: function () {
                this.putUserRole();
            },
            putUserName: function () {
                var self = this;
                var postData = this._data.editTarget;

                axios.put('/api/user/' + this._data.editTarget.userId + '/name', postData)
                    .then(response => {
                        self._data.showEditName = false;
                        self._data.showEditRole = false;
                        self.getMember();
                    }).catch(error => {
                        self._data.showEditName = false;
                        self._data.showEditRole = false;
                        self._data.errorMessage = 'Edit failed';
                        self._data.showError = true;

                    });
            },
            putUserRole: function () {
                var self = this;
                var postData = this._data.editTarget;

                axios.put('/api/user/' + this._data.editTarget.userId + '/role', postData)
                    .then(response => {
                        self._data.showEditName = false;
                        self._data.showEditRole = false;
                        self.getMember();
                    }).catch(error => {
                        self._data.showEditName = false;
                        self._data.showEditRole = false;
                        self._data.errorMessage = 'Edit failed';
                        self._data.showError = true;

                    });
            }

        },
        watch: {
            
        },
    });
});