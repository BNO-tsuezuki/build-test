
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
                userList: [],
                userListOrg: [],
                domain: {},
                publisher: [],
                passwordOld: '',
                passwordNew: ''
            }
        },
        mounted: function () {
            //this.getDomain();
            this.getData();
            this.getPublisher();
        },

        computed: {

        },
        //async created(){
        created() {

        },
        methods: {
            getData: function () {
                //this._data.fail = true;
                var self = this;
                var postData = {
                }

                axios.get('/api/user', postData)
                    .then(response => {
                        _.forEach(response.data, function (user, key) {
                            user.roleEdit = false;
                            user.nameEdit = false;
                            user.publisherEdit = false;
                        });
                        self._data.userList = response.data;
                        self.userListOrg = _.cloneDeep(response.data);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            //getDomain: function () {
            //    var self = this;
            //    var res = axios.get('/api/domain')
            //        .then(response => {
            //            self._data.domain = response.data;
            //        }).catch(error => {
            //            console.log(error);
            //        });
            //},
            getRole: function (user) {
                var roleSet = store.getters.roleSet;

                var myRole = _.find(roleSet, { role: store.getters.role });
                var targetRole = _.find(roleSet, { role: user.role });

                if (store.getters.userId == user.userId) {
                    var retRole = _.filter(roleSet, (obj) => {
                        return obj.id >= myRole.id;
                    });
                    return retRole;
                } else {
                    
                    if (myRole.id > targetRole.id) {
                        var retRole = _.filter(roleSet, (obj) => {
                            return obj.id == targetRole.id;
                        });
                        return retRole;
                    } else {
                        var retRole = _.filter(roleSet, (obj) => {
                            return obj.id > myRole.id;
                        });
                        return retRole;
                    }
                }
            },
            getPublisher: function () {
                var self = this;
                axios.get('/api/publisher')
                    .then(response => {
                        self._data.publisher = response.data;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            cancelAll: function () {
                var self = this;
                _.forEach(this.userList, function (user, key) {
                    self.cancelEditRole(user);
                    self.cancelEditName(user);
                    self.cancelEditPublisher(user);
                });
            },
            cancelEditRole: function (user) {
                user.roleEdit = false;
                var tmp = this.userListOrg.find(v => v.userId == user.userId);
                user.role = tmp.role;
            },
            saveEditRole: function (user) {
                var self = this;
                var postData = {
                    role : user.role
                }
                var res = axios.put('/api/user/' + user.userId + '/role', postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getData();
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            cancelEditName: function (user) {
                user.nameEdit = false;
                var tmp = this.userListOrg.find(v => v.userId == user.userId);
                user.name = tmp.name;
            },
            saveEditName: function (user) {
                var self = this;
                var postData = {
                    name: user.name
                }
                var res = axios.put('/api/user/' + user.userId + '/name', postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getData();
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            nameEdit: function (user,index) {
                this.cancelAll();
                user.nameEdit = true;
                this.$nextTick(() => {
                    this.$refs.focusName[0].focus();
                })
                
            },
            cancelEditPublisher: function (user) {
                user.publisherEdit = false;
                var tmp = this.userListOrg.find(v => v.userId == user.userId);
                user.publisher.publisherId = tmp.publisher.publisherId;
            },
            saveEditPublisher: function (user) {
                var self = this;
                var postData = {
                    publisherId: user.publisher.publisherId
                }
                var res = axios.put('/api/user/' + user.userId + '/publisher', postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getData();
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            publisherEdit: function (user, index) {
                this.cancelAll();
                user.publisherEdit = true;
                this.$nextTick(() => {
                    this.$refs.focusName[0].focus();
                })

            },
            resetPassword: function (user) {
                var self = this;
                this.cancelAll();
                var postData = {
                    
                }
                var res = axios.put('/api/user/' + user.userId + '/ResetPassword', postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getData();
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            isMyAccount: function (user) {
                if (user.userId == store.getters.userId) {
                    return true;
                } else {
                    return false;
                }
            },
            isAvailable: function (user) {
                if (user.isAvailable == true) {
                    return true;
                } else {
                    return false;
                }
            },
            toggleAvailableConfirm: function (user) {
                var text = !user.isAvailable ? 'available' : 'unavailable';    
                var color = !user.isAvailable ? 'blue' : 'red';    
                Swal.fire({
                    title: 'Do you really want to change this ?',
                    showCancelButton: true,
                    confirmButtonText: text,
                    confirmButtonColor: color,
                }).then((result) => {
                    /* Read more about isConfirmed, isDenied below */
                    if (result.isConfirmed) {
                        this.toggleAvailable(user);
                    }
                })
            },
            toggleAvailable: function (user) {
                var self = this;
                var postData = {
                    isAvailable: !user.isAvailable
                }
                var res = axios.put('/api/user/' + user.userId + '/IsAvailable', postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getData();
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
            },
            showModal: function () {
                this.passwordNew = '';
                this.passwordOld = '';
                $('#modal-default').modal('show');
            },
            changePassword: function () {
                var self = this;
                this.cancelAll();
                var postData = {
                    password: this.passwordOld,
                    newPassword: this.passwordNew
                }
                var res = axios.put('/api/auth/ChangePassword', postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self.getData();
                    }).catch(error => {
                        console.log(error);
                        toastr.error('Error.');
                    });
                $('#modal-default').modal('hide');
            },
            showEdit: function (user) {
                if (store.getters.userId == '') return false;
                var myData = this.userListOrg.find(v => v.userId == store.getters.userId);
                var roleSet = store.getters.roleSet;
                
                if (user.userId == myData.userId) return true;

                var myRole = roleSet.find(v => v.role == myData.role);

                if (myRole.id >= 2 && myData.publisher.publisherId != user.publisher.publisherId) {
                    return false;
                }

                var targetRole = roleSet.find(v => v.role == user.role);
                if (targetRole == undefined) return true;
                if (targetRole.id > myRole.id) return true;
                return false;
            }

        },
        watch: {

        },
    });
});

