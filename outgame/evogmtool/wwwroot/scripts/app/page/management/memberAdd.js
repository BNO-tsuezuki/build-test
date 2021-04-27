
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
                formData: {
                    account: '',
                    name: '',
                    publisherId: 0,
                    role: '',
                    timezoneCode: '',
                    languageCode: ''
                },
                domain: [],
                publisher: [],
                timezone: [],
                language: [],
                showModal: false,
                showErrorModal: false,
            }
        },
        mounted: function () {
            this.getPublisher();
            this.getTimezone();
            this.getLanguage();
        },

        computed: {
            myRole: function () {
                return store.getters.role;
            },
            myPublisher: function () {
                return store.getters.publisher;
            }
        },
        //async created(){
        created() {

        },
        methods: {
            postData: function () {
                //this._data.fail = true;
                var self = this;
                var postData = this._data.formData;

                axios.post('/api/user', postData)
                    .then(response => {
                        toastr.success('Succeeded.');
                        self._data.formData = {
                            account: '',
                            name: '',
                            publisherId: 0,
                            role: '',
                            timezoneCode: '',
                            languageCode: ''
                        }
                    }).catch(error => {
                        toastr.error('error');
                        console.log(error);
                    });
            },
            getDomain: function () {
                var self = this;
                var res = axios.get('/api/domain')
                    .then(response => {
                        self._data.domain = response.data;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getTimezone: function () {
                var self = this;
                var res = axios.get('/api/timezone')
                    .then(response => {
                        self._data.timezone = response.data;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getLanguage: function () {
                var self = this;
                var res = axios.get('/api/language')
                    .then(response => {
                        self._data.language = response.data;
                    }).catch(error => {
                        console.log(error);
                    });
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
            getPublisherList: function () {
                if (!this.myPublisher) return [];
                if (!this.myRole) return [];
                if (this.myRole != 'super') {
                    var arr = [];
                    arr.push(this.myPublisher);
                    return arr;
                } else {
                    return this.publisher;
                }
            },
            getRole: function () {
                var tmpMyRole = this.myRole;
                if (tmpMyRole == '') return [];
                var roleSet = store.getters.roleSet;

                var myRole = _.find(roleSet, { role: store.getters.role });
                var retRole = _.filter(roleSet, (obj) => {
                    return obj.id > myRole.id;
                });
                return retRole;
            },
            close: function () {
                this._data.success = false;
                this._data.showModal = false;
            },
            closeError: function () {
                this._data.success = false;
                this._data.showErrorModal = false;
            }
        },
        watch: {

        },
    });
});