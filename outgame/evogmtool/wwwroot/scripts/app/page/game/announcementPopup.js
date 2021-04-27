
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    const Toast = Swal.mixin({
        toast: true,
        position: 'top',
        showConfirmButton: false,
        timer: 1000,
        timerProgressBar: false,
        onOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    })

    const VueCtkDateTimePicker = window['vue-ctk-date-time-picker'];

    Vue.component('vue-ctk-date-time-picker', VueCtkDateTimePicker);
    Vue.component('paginate', VuejsPaginate);
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                loginInfo: {
                    userId: 0,
                    account: '',
                    name: '',
                    role: '',
                    publisher: {
                        publisherId: 0,
                        publisherName: ''
                    },
                    createdAt: '',
                    updatedAt: ''
                },
                domain: [],
                language: [],
                selectedDomain: {},
                List: {
                    totalCount: 0,
                    pageCount: 0,
                    data: []
                },
                parPage: 10,
                currentPage: 1,
                lang: '',
                dateFormat: 'MM/DD/YYYY, HH:mm',
                start: {
                    datetime: ''
                },
                end: {
                    datetime: ''
                }

            }
        },
        components: {
            'vuejs-datepicker': vuejsDatepicker
        },
        mounted: function () {
            this.getMyData();
            this.getLanguage();
        },

        computed: {

        },
        //async created(){
        created() {

        },
        methods: {
            getMyData: function () {
                var self = this;
                var res = axios.get('/api/Auth')
                    .then(response => {
                        self.loginInfo = response.data;
                        self.lang = self.loginInfo.language.languageCode;
                        if (self.loginInfo.language.languageCode == 'ja') {
                            self.dateFormat = 'YYYY/MM/DD HH:mm';
                        } else {
                            self.dateFormat = 'MM/DD/YYYY, HH:mm';
                        }
                        this.getDomain();
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getDomain: function () {
                var self = this;
                var res = axios.get('/api/domainregion')
                    .then(response => {
                        self.domain = response.data;

                        var firstDomain = _.find(self.domain, function (o) { return o.publisher.publisherId == self.loginInfo.publisher.publisherId; });
                        self.selectedDomain = firstDomain;

                        self.getList(1);
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
            search: function () {
                this.List.pageCount = 0;
                this.getList(1);
            },
            getList: function (page) {
                var self = this;

                this.currentPage = page;
                var queryArray = [];
                queryArray.push('CountPerPage=' + this.parPage);
                queryArray.push('PageNumber=' + this.currentPage);
                if (this.start.datetime)queryArray.push('From=' + this.start.datetime);
                if (this.end.datetime)queryArray.push('To=' + this.end.datetime);
                if (this.selectedDomain.target) queryArray.push('Target=' + this.selectedDomain.target);

                var targetApi = new URL('/api/game/opsnotice/popup', location.origin);
                targetApi.search = queryArray.join('&');
                var res = axios.get(targetApi.href)
                    .then(response => {
                        _.forEach(response.data.logList, function (log) {
                            log.datetime = moment(log.datetime).format('MM/DD/YYYY HH:mm');
                        });
                        //
                        self._data.List.data = response.data.list;
                        self._data.List.pageCount = Math.ceil(response.data.totalCount / self._data.parPage);
                        self._data.List.totalCount = response.data.totalCount;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            isEditable: function (target) {
                var self = this;
                var myDomain = _.find(self.domain, function (o) { return o.publisher.publisherId == self.loginInfo.publisher.publisherId; });
                var targetDomain = _.find(self.domain, function (o) { return o.target == target; });

                if (myDomain && targetDomain) {
                    if (myDomain.publisher.publisherId == targetDomain.publisher.publisherId) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            },
            editPage: function (edit) {
                var queryArray = [];
                queryArray.push('id=' + edit.id);

                var tagetPage = new URL('/game/announcementPopupPost.html', location.origin);
                tagetPage.search = queryArray.join('&');

                if (this.isEditable(edit.target)) {
                    location.href = tagetPage.href;
                } else {
                    window.open(tagetPage.href);
                }
            },
            newPost: function () {
                location.href = '/game/announcementPopupPost.html';
            },
            domainSelect: function (domain) {
                this.selectedDomain = domain;
                this.search();
            },
            isSelectedDomain: function (item) {
                if (item.domain.domainId == this.selectedDomain.domain.domainId && item.region.regionId == this.selectedDomain.region.regionId) {
                    return true;
                } else {
                    return false;
                }
            }, getMemo: function (row) {
                return row.memo;
            }, getDateTime: function (val) {
                var m = moment.utc(val, "YYYY-MM-DDTHH:mm:ss");
                return m.tz(this.loginInfo.timezone.timezoneCode).format(this.dateFormat);
            }

        },
        filters: {
            truncate: function (value) {
                var length = 20;
                var ommision = "...";
                if (value.toString().length <= length) {
                    return value.toString();
                }
                return value.toString().substring(0, length) + ommision;
            }
        },
        watch: {

        },
    });
});