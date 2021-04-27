
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: '',
                loginInfo: {
                    timezone: {
                        timezoneCode: ''
                    },
                    language: {
                        languageCode: '',
                        languageName: ''
                    },
                },
                timezone: [],
                language: [],
            }
        },
        mounted: function () {
            this.getData();
            this.getTimezone();
            this.getLanguage();
        },

        computed: {

        },
        //async created(){
        created() {

        },
        methods: {
            getData: function () {
                var self = this;
                var res = axios.get('/api/Auth')
                    .then(response => {
                        self._data.loginInfo = response.data;
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
            saveTimezone: function () {
                var postData = {
                    timezoneCode: this.loginInfo.timezone.timezoneCode
                };
                axios.put('/api/user/' + this.loginInfo.userId + '/timezone', postData)
                    .then(response => {
                        toastr.success('Timezone is change.');
                    }).catch(error => {
                        toastr.error('error');
                        console.log(error);
                    });
            },
            saveLanguage: function () {
                var postData = {
                    languageCode: this.loginInfo.language.languageCode
                };
                axios.put('/api/user/' + this.loginInfo.userId + '/language', postData)
                    .then(response => {
                        toastr.success('Language is change.');
                    }).catch(error => {
                        toastr.error('error');
                        console.log(error);
                    });
            }
        },
        watch: {

        },
    });
});