Vue.component('main-manu', {
    data: function () {
      return {
          count: 0,
          paramDomain: ''
      }
    },
    store,
    template: `
        <aside class="main-sidebar sidebar-dark-primary elevation-4">
            <!-- Brand Logo -->
            <a v-bind:href="myUrl('/index.html')" class="brand-link">
                <img src="/images/logo.png" alt="AdminLTE Logo" class="brand-image img-circle elevation-3"
                     style="opacity: .8">
                <span class="brand-text font-weight-light">EVO GMTool</span>
            </a>
            <!-- Sidebar -->
            <div class="sidebar">
                <!-- Sidebar user panel (optional) -->
                <div class="user-panel mt-3 pb-3 mb-3 d-flex">
                    <div class="image">
                        <img src="/dist/img/user2-160x160.jpg" class="img-circle elevation-2" alt="User Image">
                    </div>
                    <div class="info">
                        <a href="#" class="d-block">{{myName}}</a>
                        <p class="d-block" style="color: #c2c7d0;margin-bottom: 0px;">{{myPublisherName}}</p>
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
                            <a href="#" class="nav-link " v-bind:class="{ active: isPlayerPages }">
                                <i class="nav-icon fas fa-address-card"></i>
                                <p>
                                    Player
                                    <i class="right fas fa-angle-left"></i>
                                </p>
                            </a>
                            <ul class="nav nav-treeview">
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/player/selectPlayer.html')" class="nav-link" v-bind:class="{ active: isPlayerPages }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>selectPlayer</p>
                                    </a>
                                </li>
                            </ul>
                        </li>
                        <li class="nav-item has-treeview menu-open">
                            <a href="#" class="nav-link " v-bind:class="{ active: isGamePages }">
                                <i class="nav-icon fas fa-archway"></i>
                                <p>
                                    Game Management
                                    <i class="right fas fa-angle-left"></i>
                                </p>
                            </a>
                            <ul class="nav nav-treeview">
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/game/announcement.html')" class="nav-link" v-bind:class="{ active: isAnnouncement }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>announcement(Chat)</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/game/announcementPopup.html')" class="nav-link" v-bind:class="{ active: isAnnouncementPopup }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>announcement(Popup)</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/game/topic.html')" class="nav-link" v-bind:class="{ active: isTopic }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Topic</p>
                                    </a>
                                </li>
                                <li class="nav-item" >
                                    <a v-bind:href="myUrl('/game/unit.html')" class="nav-link" v-bind:class="{ active: isUnit }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>unit</p>
                                        
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/game/Gasha.html')" class="nav-link" v-bind:class="{ active: isGasha }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Gasha</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/game/Map.html')" class="nav-link" v-bind:class="{ active: isMap }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Map</p>
                                    </a>
                                </li>
                                <li class="nav-item" v-if="myRole() <= 2">
                                    <a v-bind:href="myUrl('/game/Version.html')" class="nav-link" v-bind:class="{ active: isVersion }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Version</p>
                                        
                                    </a>
                                </li>
                            </ul>
                        </li>
                        <li class="nav-item has-treeview menu-open">
                            <a href="#" class="nav-link " v-bind:class="{ active: isGameLogPages }">
                                <i class="nav-icon fas fa-archive"></i>
                                <p>
                                    GameLog Pages
                                    <i class="right fas fa-angle-left"></i>
                                </p>
                            </a>
                            <ul class="nav nav-treeview">
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/log/GameLog.html')" class="nav-link" v-bind:class="{ active: isGameLog }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>GameLog</p>
                                    </a>
                                </li>
                            </ul>
                        </li>
                        <li class="nav-item has-treeview menu-open">
                            <a href="#" class="nav-link " v-bind:class="{ active: isLogPages }">
                                <i class="nav-icon fas fa-tachometer-alt"></i>
                                <p>
                                    Log Pages
                                    <i class="right fas fa-angle-left"></i>
                                </p>
                            </a>
                            <ul class="nav nav-treeview">
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/log/authLogView.html')" class="nav-link" v-bind:class="{ active: isAuthLog }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>AuthLog</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/log/operationLogView.html')" class="nav-link" v-bind:class="{ active: isOperationLog }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>OperationLog</p>
                                        
                                    </a>
                                </li>
                            </ul>
                        </li>
                        <li class="nav-item has-treeview menu-open">
                            <a href="#" class="nav-link " v-bind:class="{ active: isManagementPages }">
                                <i class="nav-icon fas fa-chalkboard-teacher"></i>
                                <p>
                                    Management Pages
                                    <i class="right fas fa-angle-left"></i>
                                </p>
                            </a>
                            <ul class="nav nav-treeview">
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/management/memberList.html')" class="nav-link" v-bind:class="{ active: isMemberList }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Member List</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/management/memberAdd.html')" class="nav-link" v-bind:class="{ active: isMemberAdd }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Member Add</p>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a v-bind:href="myUrl('/management/setting.html')" class="nav-link" v-bind:class="{ active: isSetting }">
                                        <i class="far fa-circle nav-icon"></i>
                                        <p>Setting</p>
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
        },
        myName: function () {
            return this.$store.getters['name'];
        },
        myPublisherName: function () {
            return this.$store.getters['publisherName'];
        }, isDummy: function () {
            return false;
        }, isCMSPages: function () {
            if (this.isSample) return true;
            if (this.isStarter) return true;
            if (this.isPost) return true;
            if (this.isListOfPosts) return true;
            return false;
        }, isPlayerPages: function () {
            if (this.isSelectPlayer) return true;
            if (this.isPlayer) return true;
            if (this.isItem) return true;
            if (this.isUserOption) return true;
            if (this.isAchievement) return true;
            if (this.isChalenge) return true;
            if (this.isRate) return true;
            if (this.isBattlePass) return true;
            if (this.isPlayerCustomize) return true;
            return false;
        }, isGamePages: function () {
            if (this.isAnnouncement) return true;
            if (this.isAnnouncementPopup) return true;
            if (this.isTopic) return true;
            if (this.isUnit) return true;
            if (this.isVersion) return true;
            if (this.isGasha) return true;
            if (this.isMap) return true;
            if (this.isMasterData) return true;
            return false;
        }, isLogPages: function () {
            if (this.isAuthLog) return true;
            if (this.isOperationLog) return true;
            return false;
        }, isGameLogPages: function () {
            if (this.isGameLog) return true;
            return false;
        }, isManagementPages: function () {
            if (this.isMemberList) return true;
            if (this.isMemberAdd) return true;
            if (this.isEditRole) return true;
            if (this.isSetting) return true;
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
        }, isAddMember: function () {
            if (location.pathname == '/management/addMember.html') {
                return true;
            }
            return false;
        }, isMemberAdd: function () {
            if (location.pathname == '/management/memberAdd.html') {
                return true;
            }
            return false;
        }, isSetting: function () {
            if (location.pathname == '/management/setting.html') {
                return true;
            }
            return false;
        }, isEditRole: function () {
            if (location.pathname == '/management/editRole.html') {
                return true;
            }
            return false;
        }, isAuthLog: function () {
            if (location.pathname == '/log/authLogView.html') {
                return true;
            }
            return false;
        }, isOperationLog: function () {
            if (location.pathname == '/log/operationLogView.html') {
                return true;
            }
            return false;
        }, isSelectPlayer: function () {
            if (location.pathname == '/player/selectPlayer.html') {
                return true;
            }
            return false;
        }, isPlayer: function () {
            if (location.pathname == '/player/player.html') {
                return true;
            }
            return false;
        }, isItem: function () {
            if (location.pathname == '/player/item.html') {
                return true;
            }
            return false;
        }, isUserOption: function () {
            if (location.pathname == '/player/userOption.html') {
                return true;
            }
            return false;
        }, isAchievement: function () {
            if (location.pathname == '/player/Achievement.html') {
                return true;
            }
            return false;
        }, isChalenge: function () {
            if (location.pathname == '/player/Chalenge.html') {
                return true;
            }
            return false;
        }, isRate: function () {
            if (location.pathname == '/player/Rate.html') {
                return true;
            }
            return false;
        }, isAnnouncement: function () {
            if (location.pathname == '/game/announcement.html' || location.pathname == '/game/announcementPost.html' ) {
                return true;
            }
            return false;
        }, isAnnouncementPopup: function () {
            if (location.pathname == '/game/announcementPopup.html' || location.pathname == '/game/announcementPopupPost.html' ) {
                return true;
            }
            return false;
        }, isTopic: function () {
            if (location.pathname == '/game/topic.html') {
                return true;
            }
            return false;
        }, isBattlePass: function () {
            if (location.pathname == '/player/battlePass.html') {
                return true;
            }
            return false;
        }, isUnit: function () {
            if (location.pathname == '/game/unit.html') {
                return true;
            }
            return false;
        }, isVersion: function () {
            if (location.pathname == '/game/Version.html') {
                return true;
            }
            return false;
        }, isGasha: function () {
            if (location.pathname == '/game/Gasha.html') {
                return true;
            }
            return false;
        }, isMap: function () {
            if (location.pathname == '/game/Map.html') {
                return true;
            }
            return false;
        }, isMasterData: function () {
            if (location.pathname == '/game/MasterData.html') {
                return true;
            }
            return false;
        }, isPlayerCustomize: function () {
            if (location.pathname == '/player/PlayerCustomize.html') {
                return true;
            }
            return false;
        }, isGameLog: function () {
            if (location.pathname == '/log/GameLog.html') {
                return true;
            }
            return false;
        }, searchVal: function () {
            if (getParameter("search")) {
                return getParameter("search");
            }
            else{
                return '';
            }
        }

    },
    mounted:function() {

    },
    methods: {
        myUrl: function (_url) {
            var url = new URL(_url, location.origin);
            if (getParameter("search")) {
                url.search = "search=" + getParameter("search");
            }
            if (this.searchVal.searchValue) {
                url.search = "search=" + this.searchVal.searchValue;
            }
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