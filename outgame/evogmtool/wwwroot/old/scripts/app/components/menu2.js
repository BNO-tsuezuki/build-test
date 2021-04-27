Vue.component('main-manu2', {
    data: function () {
      return {
        count: 0
      }
    },
    store,
    template: `
       <aside class="main-sidebar sidebar-dark-primary elevation-4">
            <!-- Brand Logo -->
            <a v-bind:href="myUrl('/index.html')" class="brand-link">
                <img src="/dist/img/bno.png" alt="AdminLTE Logo" class="brand-image img-circle elevation-3"
                     style="opacity: .8">
                <span class="brand-text font-weight-light">EVO CMS</span>
            </a>
            <!-- Sidebar -->
            <div class="sidebar">
                <!-- Sidebar user panel (optional) -->
                <div class="user-panel mt-3 pb-3 mb-3 d-flex">
                    <div class="image">
                        <img src="/dist/img/user2-160x160.jpg" class="img-circle elevation-2" alt="User Image">
                    </div>
                    <div class="info">
                        <a href="#" class="d-block">UserName</a>
                    </div>
                </div>
                <!-- Sidebar Menu -->
                <nav class="mt-2">
                    <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
                        <!-- Add icons to the links using the .nav-icon class
                             with font-awesome or any other icon font library -->

                        <li class="nav-item">
                            <a v-bind:href="myUrl('/index.html')" class="nav-link" v-bind:class="{ active: isIndex }">
                                <i class="nav-icon fas fa-th"></i>
                                <p>
                                    Top
                                </p>
                            </a>
                        </li>
                        <li class="nav-item has-treeview menu-open">
                            <a href="#" class="nav-link " v-bind:class="{ active: isCMSPages }">
                                <i class="nav-icon fas fa-tachometer-alt"></i>
                                <p>
                                    CMS Pages
                                    <i class="right fas fa-angle-left"></i>
                                </p>
                            </a>
                            <ul class="nav nav-treeview">
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/starter.html')" class="nav-link" v-bind:class="{ active: isStarter }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Starter Page</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/sample.html')" class="nav-link" v-bind:class="{ active: isSample }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Sample Page</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/cms/newsList.html')" class="nav-link" v-bind:class="{ active: isNewsList }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>News List</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/cms/post.html')" class="nav-link" v-bind:class="{ active: isPost }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Post Page</p>
                                    </a>
                                </li>
                            </ul>
                        </li>
                        <li class="nav-item has-treeview menu-open">
                            <a href="#" class="nav-link " v-bind:class="{ active: isManagementPages }">
                                <i class="nav-icon fas fa-tachometer-alt"></i>
                                <p>
                                    Management Pages
                                    <i class="right fas fa-angle-left"></i>
                                </p>
                            </a>
                            <ul class="nav nav-treeview">
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/management/memberList.html')" class="nav-link" v-bind:class="{ active: isMemberList }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>MemberList</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/management/memberAdd.html')" class="nav-link" v-bind:class="{ active: isMemberAdd }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>MemberAdd</p>
                                    </a>
                                </li>
                                <li class="nav-item" v-if="myRole() <= 2">
                                    <a v-bind:href="myUrl('/management/authLog.html')" class="nav-link" v-bind:class="{ active: isAuthLog }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>authLog</p>
                                        <span class="right badge badge-danger">New</span>
                                    </a>
                                </li>
                                <li class="nav-item" v-if="myRole() <= 2">
                                    <a v-bind:href="myUrl('/management/operationLog.html')" class="nav-link" v-bind:class="{ active: isOperationLog }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>operationLog</p>
                                        <span class="right badge badge-danger">New</span>
                                    </a>
                                </li>
                            </ul>
                        </li>

                    </ul>
                </nav>
                <!-- /.sidebar-menu -->
            </div>
            <!-- /.sidebar -->
        </aside>
    `,

    //メニューのicon
    //https://material.io/resources/icons/?icon=not_interested&style=baseline
    computed: {
        // 算出 getter 関数
        domain: function () {
            return store.getters.selectedDomain;
        }, isDummy: function () {
            return false;
        }, isCMSPages: function () {
            if (this.isSample) return true;
            if (this.isStarter) return true;
            if (this.isPost) return true;
            if (this.isListOfPosts) return true;
            return false;
        }, isManagementPages: function () {
            if (this.isMemberList) return true;
            if (this.isMemberAdd) return true;
            if (this.isEditRole) return true;
            if (this.isOperationLog) return true;
            if (this.isAuthLog) return true;
            return false;
        },
        isIndex: function () {
            if (location.pathname == '/index.html') {
                return true;
            }
            return false;
        }, isFormComponents: function () {
            if (location.pathname == '/form-components.html') {
                return true;
            }
            return false;
        }, isStarter: function () {
            if (location.pathname == '/starter.html') {
                return true;
            }
            return false;
        }, isSample: function () {
            if (location.pathname == '/sample.html') {
                return true;
            }
            return false;
        }, isPost: function () {
            if (location.pathname == '/cms/post.html') {
                return true;
            }
            return false;
        }, isNewsList: function () {
            if (location.pathname == '/cms/newsList.html') {
                return true;
            }
            return false;
        }, isMemberList: function () {
            if (location.pathname == '/management/memberList.html') {
                return true;
            }
            return false;
        }, isMemberAdd: function () {
            if (location.pathname == '/management/memberAdd.html') {
                return true;
            }
            return false;
        }, isEditRole: function () {
            if (location.pathname == '/management/editRole.html') {
                return true;
            }
            return false;
        }, isAuthLog: function () {
            if (location.pathname == '/management/authLog.html') {
                return true;
            }
            return false;
        }, isOperationLog: function () {
            if (location.pathname == '/management/operationLog.html') {
                return true;
            }
            return false;
        }

    },
    mounted: function () {

    },
    methods: {
        myUrl: function (_url) {
            var url = new URL(_url, location.origin);
            url.search = "domain=" + store.getters.selectedDomain;
            return url.href;
        },
        myRole: function () {
            var myData = store.getters.myRole;
            if (myData) {
                return myData.id;
            } else {
                return 5;
            }
        },
        myRole: function (state) {
            var set = store.getters.roleSet;
            if (store.getters.userId) {
                var myData = set.find(v => v.role == store.getters.role);
                return myData.id;
            } else {
                return 5;
            }
        },
    },
    watch: {
        domain(values) {
            if (values) {

            }
        }
    }
})