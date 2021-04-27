
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


    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
                List: {
                    totalCount: 0,
                    newsList: []
                },
                countPerPage: 2,
                pageNumber: 1,
                paginationArray: [],
                lang: ''
            }
        },
        mounted: function () {
            this.getList();
        },

        computed: {

        },
        //async created(){
        created() {

        },
        methods: {
            getList: function () {
                if (getParameter('CountPerPage')) this.countPerPage = getParameter('CountPerPage');
                if (getParameter('PageNumber')) this.pageNumber = getParameter('PageNumber');
                if (getParameter('lang')) this.lang = getParameter('lang');

                var self = this;

                var queryArray = [];
                if (this.countPerPage) queryArray.push('CountPerPage=' + this.countPerPage);
                if (this.pageNumber) queryArray.push('PageNumber=' + this.pageNumber);
                if (getParameter("domain")) queryArray.push('DomainId=' + getParameter('domain'));

                var targetApi = new URL('/api/news', location.origin);

                targetApi.search = queryArray.join('&')

                Toast.fire({
                    title: 'loading...'
                })
                var res = axios.get(targetApi.href)
                    .then(response => {
                        self._data.List = response.data;

                        var totalView = 6;
                        self.paginationArray = [];
                        if (self.List.totalCount == 0) {
                            self.paginationArray = [];
                        } else {
                            for (var i = -3; i < totalView; i++) {
                                if (self.paginationArray.length > totalView || (Math.floor(self.List.totalCount / self.countPerPage) + 1) == (Number(self.pageNumber) + i)) break;
                                if (self.paginationArray.length <= totalView && Number(self.pageNumber) + i > 0) self.paginationArray.push(Number(self.pageNumber) + i);

                            }
                        }

                    }).catch(error => {
                        console.log(error);
                    });
            },
            resetCountPerPage: function () {
                this.pageNumber = 1;
                var queryArray = [];
                if (this.countPerPage) queryArray.push('CountPerPage=' + this.countPerPage);
                if (this.pageNumber) queryArray.push('PageNumber=' + Number(this.pageNumber));
                if (this.lang) queryArray.push('lang=' + this.lang);
                if (getParameter("domain")) queryArray.push('domain=' + getParameter("domain"));

                var url = new URL(location.pathname, location.origin);
                url.search = queryArray.join('&')

                location.href = url.href;
            },
            getShowingTo: function () {
                if (this.List.length == 0) return '';
                var showingTo = this.pageNumber * this.countPerPage;
                if (showingTo > this.List.totalCount) return this.List.totalCount;
                return showingTo;
            },
            setPagination: function (num) {
                this.pageNumber = num;
            },
            isPrevious: function () {
                if (this.pageNumber == 1) return true;
                var arr = this.paginationArray;
                if (arr.length == 1) return true;
                return false;
            },
            clickPrevious: function () {
                this.checkLogin();
                if (this.isPrevious()) return;

                var queryArray = [];
                if (this.countPerPage) queryArray.push('CountPerPage=' + Number(this.countPerPage));
                if (this.pageNumber) queryArray.push('PageNumber=' + (Number(this.pageNumber) - 1));
                if (this.lang) queryArray.push('lang=' + this.lang);
                if (getParameter("domain")) queryArray.push('domain=' + getParameter("domain"));

                var url = new URL(location.pathname, location.origin);
                url.search = queryArray.join('&')

                history.pushState('', '', url.href);
                this.getList();
            },
            previousURL: function () {
                if (this.isPrevious()) return;
                var queryArray = [];
                if (this.countPerPage) queryArray.push('CountPerPage=' + Number(this.countPerPage));
                if (this.pageNumber) queryArray.push('PageNumber=' + (Number(this.pageNumber) - 1));
                if (this.lang) queryArray.push('lang=' + this.lang);
                if (getParameter("domain")) queryArray.push('domain=' + getParameter("domain"));

                var url = new URL(location.pathname, location.origin);
                url.search = queryArray.join('&')

                return url.href;
            },
            isNext: function () {
                var arr = this.paginationArray;
                if (arr.length <= 1) return true;
                if (this.pageNumber == (Math.ceil(this.List.totalCount / this.countPerPage))) return true;
                return false;
            },
            clickNext: function () {
                this.checkLogin();
                if (this.isNext()) return;
                var queryArray = [];
                if (this.countPerPage) queryArray.push('CountPerPage=' + Number(this.countPerPage));
                if (this.pageNumber) queryArray.push('PageNumber=' + (Number(this.pageNumber) + 1));
                if (this.lang) queryArray.push('lang=' + this.lang);
                if (getParameter("domain")) queryArray.push('domain=' + getParameter("domain"));

                var url = new URL(location.pathname, location.origin);
                url.search = queryArray.join('&')

                //location.href = url.href;
                history.pushState('', '', url.href);
                this.getList();
            },
            nextURL: function () {
                if (this.isNext()) return;
                var queryArray = [];
                if (this.countPerPage) queryArray.push('CountPerPage=' + Number(this.countPerPage));
                if (this.pageNumber) queryArray.push('PageNumber=' + (Number(this.pageNumber) + 1));
                if (this.lang) queryArray.push('lang=' + this.lang);
                if (getParameter("domain")) queryArray.push('domain=' + getParameter("domain"));

                var url = new URL(location.pathname, location.origin);
                url.search = queryArray.join('&')

                return url.href;
            },
            isSelfNumber: function (num) {
                if (this.pageNumber == num) return true;
                return false;
            },
            pagingURL: function (pageNum) {
                var queryArray = [];
                if (this.countPerPage) queryArray.push('CountPerPage=' + Number(this.countPerPage));
                if (this.pageNumber) queryArray.push('PageNumber=' + pageNum);
                if (this.lang) queryArray.push('lang=' + this.lang);
                if (getParameter("domain")) queryArray.push('domain=' + getParameter("domain"));

                var url = new URL(location.pathname, location.origin);
                url.search = queryArray.join('&')

                return url.href;
            },
            jumpURL: function (pageNum) {
                this.checkLogin();
                var queryArray = [];
                if (this.countPerPage) queryArray.push('CountPerPage=' + Number(this.countPerPage));
                if (this.pageNumber) queryArray.push('PageNumber=' + pageNum);
                if (this.lang) queryArray.push('lang=' + this.lang);
                if (getParameter("domain")) queryArray.push('domain=' + getParameter("domain"));

                var url = new URL(location.pathname, location.origin);
                url.search = queryArray.join('&')

                history.pushState('', '', url.href);
                this.getList();
            },
            checkLogin: function () {
                var res = axios.get('/api/Auth')
                    .then(response => {

                    }).catch(error => {
                        if (error.response.status == 401) {
                            location.href = "/login.html";
                        } else {
                            console.log(error);
                        }
                    });
            },
            getNewsData: function (news) {
                if (news.newsHistories.length == 0) {
                    return {
                        category: '',
                        createdAt: '',
                        createdBy: '',
                        displayStartTime: '',
                        imageFileId: '',
                        isDisplayed: false,
                        newsHistoryId: '',
                        newsTexts: [],
                        version: ''
                    };
                } else {
                    return news.newsHistories[0];
                }
            },
            editPage: function (news) {
                this.checkLogin();
                var queryArray = [];
                queryArray.push('newsId=' + news.newsId);
                if (this.lang) queryArray.push('lang=' + this.lang);
                if (getParameter("domain")) queryArray.push('domain=' + getParameter("domain"));

                var url = new URL('/cms/post.html', location.origin);
                url.search = queryArray.join('&')

                location.href = url.href;
            },
            newPost: function () {
                var myInfo = store.getters.loginInfo;
                var url = new URL('/cms/post.html', location.origin);
                var queryArray = [];
                queryArray.push('domain=' + myInfo.domain.domainId);
                queryArray.push('lang=' + this.lang);
                url.search = queryArray.join('&')
                location.href = url.href;
            },
            getLang: function () {
                var domainList = store.getters.domainList;
                var targetDomain = _.filter(domainList, function (dom) {
                    return dom.domainId == getParameter("domain");
                });
                if (targetDomain.length == 0) return [];
                var hasEnglish = _.filter(targetDomain[0].languages, function (la) {
                    return la.languageCode == 'en';
                });
                if (hasEnglish.length == 0) {
                    this.lang = targetDomain[0].languages[0].languageCode;
                } else {
                    this.lang = 'en';
                }
                return targetDomain[0].languages;
            }

        },
        watch: {

        },
    });
});