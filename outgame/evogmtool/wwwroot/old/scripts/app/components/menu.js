Vue.component('main-manu', {
    data: function () {
        return {
            count: 0
        }
    },
    store,
    template: `
<aside class="main-sidebar col-12 col-md-3 col-lg-2 px-0">
    <div class="main-navbar">
      <nav class="navbar align-items-stretch navbar-light bg-white flex-md-nowrap border-bottom p-0">
        <a class="navbar-brand w-100 mr-0" href="#" style="line-height: 25px;">
          <div class="d-table m-auto" onClick="location.href='/index.html'">
            <img id="main-logo" class="d-inline-block align-top mr-1" style="max-width: 25px;" src="images/shards-dashboards-logo.svg" alt="Shards Dashboard">
            <span class="d-none d-md-inline ml-1">EVO GM Tool</span>
          </div>
        </a>
        <a class="toggle-sidebar d-sm-inline d-md-none d-lg-none">
          <i class="material-icons">&#xE5C4;</i>
        </a>
      </nav>
    </div>
    <form action="#" class="main-sidebar__search w-100 border-right d-sm-flex d-md-none d-lg-none">
      <div class="input-group input-group-seamless ml-3">
        <div class="input-group-prepend">
          <div class="input-group-text">
            <i class="fas fa-search"></i>
          </div>
        </div>
        <input class="navbar-search form-control" type="text" placeholder="[playerName#playerNo] or [account]" aria-label="Search"> </div>
    </form>
    <div class="nav-wrapper">
        <span>プラットフォーム表示</span>

        <ul class="nav flex-column">
            <li class="nav-item">
                <a class="nav-link " v-bind:class="{ active: isIndex }" v-bind:href="myUrl('index.html')" >
                    <i class="material-icons">home</i>
                    <span>Top</span>
                </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isPlayer }" v-bind:href="myUrl('player.html')" >
                <i class="material-icons">account_box</i>
                <span>player</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isItem }" v-bind:href="myUrl('item.html')" >
                <i class="material-icons">shopping_basket</i>
                <span>item</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isMemberList }" v-bind:href="myUrl('memberList.html')">
                <i class="material-icons">list</i>
                <span>memberList</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isAddMember }" v-bind:href="myUrl('addMember.html')">
                <i class="material-icons">person_add</i>
                <span>addMember</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isAnouncement }" v-bind:href="myUrl('announcement.html')">
                <i class="material-icons">contactless</i>
                <span>announcement</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isBattlePass }" v-bind:href="myUrl('battlePass.html')">
                <i class="material-icons">trending_up</i>
                <span>battlePass</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isUnit }" v-bind:href="myUrl('unit.html')">
                <i class="material-icons">settings_input_composite</i>
                <span>unit</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isUserOption }" v-bind:href="myUrl('userOption.html')">
                <i class="material-icons">settings_input_composite</i>
                <span>userOption</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isAuthLog }" v-bind:href="myUrl('authLog.html')">
                <i class="material-icons">settings_input_composite</i>
                <span>Auth Log</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isOperationLog }" v-bind:href="myUrl('operationLog.html')">
                <i class="material-icons">settings_input_composite</i>
                <span>Operation Log</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isVersion }" v-bind:href="myUrl('version.html')">
                <i class="material-icons">settings_input_composite</i>
                <span>Version</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isGasha }" v-bind:href="myUrl('Gasha.html')" >
                <i class="material-icons">analytics</i>
                <span>Gasha</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isPlayerCustomize }" v-bind:href="myUrl('PlayerCustomize.html')" >
                <i class="material-icons">analytics</i>
                <span>PlayerCustomize</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isAchievement }" v-bind:href="myUrl('Achievement.html')" >
                <i class="material-icons">analytics</i>
                <span>Achievement</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isChallenge }" v-bind:href="myUrl('Challenge.html')" >
                <i class="material-icons">analytics</i>
                <span>Challenge</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isRate }" v-bind:href="myUrl('Rate.html')" >
                <i class="material-icons">analytics</i>
                <span>Rate</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isGameLog }" v-bind:href="myUrl('GameLog.html')" >
                <i class="material-icons">analytics</i>
                <span>GameLog</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isMap }" v-bind:href="myUrl('Map.html')" >
                <i class="material-icons">analytics</i>
                <span>Map</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isMasterData }" v-bind:href="myUrl('MasterData.html')" >
                <i class="material-icons">analytics</i>
                <span>MasterData</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isCsvSample }" v-bind:href="myUrl('csvSample.html')">
                <i class="material-icons">analytics</i>
                <span>csvSample</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isFormComponents }" v-bind:href="myUrl('form-components.html')" >
                <i class="material-icons">help_center</i>
                <span>form-components</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isBase }" v-bind:href="myUrl('base.html')" >
                <i class="material-icons">analytics</i>
                <span>base</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isBase }" v-bind:href="myUrl('base.html')" >
                <i class="material-icons">analytics</i>
                <span>base</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isBase }" v-bind:href="myUrl('base.html')" >
                <i class="material-icons">analytics</i>
                <span>base</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" v-bind:class="{ active: isBase }" v-bind:href="myUrl('base.html')" >
                <i class="material-icons">analytics</i>
                <span>base</span>
              </a>
            </li>

            <li class="nav-item" style="height: 30px;">

            </li>

      </ul>
    </div>
  </aside>
    `,

    //メニューのicon
    //https://material.io/resources/icons/?icon=not_interested&style=baseline
    computed: {
        // 算出 getter 関数
        searchVal: function () {
            return store.getters.searchValue;
        }, isDummy: function () {
            return false;
        }, isPlayerPages: function () {
            if (this.isPlayer) return true;
            if (this.isItem) return true;
            if (this.isBattlePass) return true;
            if (this.isUserOption) return true;
            if (this.isPlayerCustomize) return true;
            if (this.isAchievement) return true;
            if (this.isChallenge) return true;
            if (this.isRate) return true;
            return false;
        }, isGamePages: function () {
            if (this.isUnit) return true;
            if (this.isAnouncement) return true;
            if (this.isGasha) return true;
            if (this.isMap) return true;
            if (this.isMasterData) return true;
            if (this.isVersion) return true;
            return false;
        }, isLogPages: function () {
            if (this.isAuthLog) return true;
            if (this.isOperationLog) return true;
            if (this.isGameLog) return true;
            return false;
        }, isManagementPages: function () {
            if (this.isMemberList) return true;
            if (this.isAddMember) return true;
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
        }, isBase: function () {
            if (location.pathname == '/base.html') {
                return true;
            }
            return false;
        }, isCsvSample: function () {
            if (location.pathname == '/csvSample.html') {
                return true;
            }
            return false;
        }, isPlayer: function () {
            if (location.pathname == '/player.html') {
                return true;
            }
            return false;
        }, isItem: function () {
            if (location.pathname == '/item.html') {
                return true;
            }
            return false;
        }, isAddMember: function () {
            if (location.pathname == '/addMember.html') {
                return true;
            }
            return false;
        }, isMemberList: function () {
            if (location.pathname == '/memberList.html') {
                return true;
            }
            return false;
        }, isAnouncement: function () {
            if (location.pathname == '/announcement.html') {
                return true;
            }
            return false;
        }, isBattlePass: function () {
            if (location.pathname == '/battlePass.html') {
                return true;
            }
            return false;
        }, isUnit: function () {
            if (location.pathname == '/unit.html') {
                return true;
            }
            return false;
        }, isUserOption: function () {
            if (location.pathname == '/userOption.html') {
                return true;
            }
            return false;
        }, isAuthLog: function () {
            if (location.pathname == '/authLog.html') {
                return true;
            }
            return false;
        }, isOperationLog: function () {
            if (location.pathname == '/operationLog.html') {
                return true;
            }
            return false;
        }, isVersion: function () {
            if (location.pathname == '/version.html') {
                return true;
            }
            return false;
        }, isGasha: function () {
            if (location.pathname == '/Gasha.html') {
                return true;
            }
            return false;
        }, isPlayerCustomize: function () {
            if (location.pathname == '/PlayerCustomize.html') {
                return true;
            }
            return false;
        }, isAchievement: function () {
            if (location.pathname == '/Achievement.html') {
                return true;
            }
            return false;
        }, isChallenge: function () {
            if (location.pathname == '/Challenge.html') {
                return true;
            }
            return false;
        }, isRate: function () {
            if (location.pathname == '/Rate.html') {
                return true;
            }
            return false;
        }, isGameLog: function () {
            if (location.pathname == '/GameLog.html') {
                return true;
            }
            return false;
        }, isMap: function () {
            if (location.pathname == '/Map.html') {
                return true;
            }
            return false;
        }, isMasterData: function () {
            if (location.pathname == '/MasterData.html') {
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
            if (getParameter("search")) {
                url.search = "search=" + getParameter("search");
            }
            if (this.searchVal.searchValue) {
                url.search = "search=" + this.searchVal.searchValue;
            }
            return url.href;
        }
    }
})