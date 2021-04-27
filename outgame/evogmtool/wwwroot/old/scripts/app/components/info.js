Vue.component('info-form', {
    data: function () {
      return {
          count: 0,
          searchText: ''
      }
    },
    mounted: function () {
        var res = axios.get('api/Auth')
            .then(response => {
                //self.state.loginInfo = response.data;
                store.commit('getInfo', response.data);
                if (store.state.loginInfo) {
                    //
                } else {
                    location.href = "/login.html";
                }
            }).catch(error => {
                location.href = "/login.html";
                console.log(error);
            });
        if (getParameter("search")) {
            this._data.searchText = getParameter("search");
            store.commit('searchValue', { searchValue: getParameter("search") });
        }
    },
    template: `
<!-- Main Navbar -->
<div class="main-navbar sticky-top bg-white">
    <nav class="navbar align-items-stretch navbar-light flex-md-nowrap p-0">
      <form action="#" class="main-navbar__search w-100 d-none d-md-flex d-lg-flex">
        <div class="input-group input-group-seamless ml-3">
          <div class="input-group-prepend">
            <div class="input-group-text">
              <i class="fas fa-search"></i>
            </div>
          </div>
          <input class="navbar-search form-control" type="text" placeholder="[playerName#playerNo] or [account]" aria-label="Search" v-model="searchVal" v-on:keypress="onKeyPress"> </div>
          <div class="input-group-append">
		    <button type="button" class="btn btn-outline-primary" style="margin-left: 1px;" v-on:click="searchClick">Search</button>
          </div>
      </form>

      <ul class="navbar-nav border-left flex-row ">
        <li class="nav-item border-right dropdown notifications">
          <a class="nav-link nav-link-icon text-center" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <div class="nav-link-icon__wrapper">
              <i class="material-icons">&#xE7F4;</i>
              <span class="badge badge-pill badge-danger">2</span>
            </div>
          </a>
          <div class="dropdown-menu dropdown-menu-small" aria-labelledby="dropdownMenuLink">
            <a class="dropdown-item" href="#">
              <div class="notification__icon-wrapper">
                <div class="notification__icon">
                  <i class="material-icons">&#xE6E1;</i>
                </div>
              </div>
              <div class="notification__content">
                <span class="notification__category">Analytics</span>
                <p>Your website’s active users count increased by
                  <span class="text-success text-semibold">28%</span> in the last week. Great job!</p>
              </div>
            </a>
            <a class="dropdown-item" href="#">
              <div class="notification__icon-wrapper">
                <div class="notification__icon">
                  <i class="material-icons">&#xE8D1;</i>
                </div>
              </div>
              <div class="notification__content">
                <span class="notification__category">Sales</span>
                <p>Last week your store’s sales count decreased by
                  <span class="text-danger text-semibold">5.52%</span>. It could have been worse!</p>
              </div>
            </a>
            <a class="dropdown-item notification__all text-center" href="#"> View all Notifications </a>
          </div>
        </li>
        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle text-nowrap px-3" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">

            <img class="user-avatar rounded-circle mr-2" :src="icon" alt="User Avatar">

            <span class="d-none d-md-inline-block">{{ getName }}</span>
          </a>
          <div class="dropdown-menu dropdown-menu-small">
            <p class="dropdown-item" style="margin-bottom: 0rem;">
              <i class="material-icons">&#xE7FD;</i> {{ getRole }}
            </p>
            <div class="dropdown-divider"></div>

            <a class="dropdown-item" v-on:click="searchMyAccount" style="cursor: pointer">
              <i class="material-icons">vertical_split</i> Search my Account</a>

            <a class="dropdown-item" href="changePassword.html">
              <i class="material-icons">build</i> Change Password</a>

            <div class="dropdown-divider"></div>

            <a class="dropdown-item text-danger" v-on:click="logout" href="#">
              <i class="material-icons text-danger">&#xE879;</i> Logout </a>

          </div>
        </li>
      </ul>
      <nav class="nav">
        <a href="#" class="nav-link nav-link-icon toggle-sidebar d-md-inline d-lg-none text-center border-left" data-toggle="collapse" data-target=".header-navbar" aria-expanded="false" aria-controls="header-navbar">
          <i class="material-icons">&#xE5D2;</i>
        </a>
      </nav>
    </nav>
  </div>
  <!-- / .main-navbar -->
    `,
    computed: {
        getName: function () {
            return store.getters.name;
        },
        getRole: function () {
            return store.getters.role;
        },
        searchVal: {
            get() {
                return this._data.searchText;
            },
            set(v) {
                store.commit('searchValue', {searchValue:v});
            },
        },
        icon: function () {
            var icons = [
                //'images/nana/icon_menherachan01_05.jpg',
                //'images/nana/icon_menherachan01_06.jpg',
                //'images/nana/icon_menherachan01_07.jpg',
                //'images/nana/icon_menherachan01_08.jpg',
                //'images/nana/icon_menherachan01_12.jpg',
                //'images/nana/icon_menherachan01_14.jpg',
                //'images/nana/icon_menherachan01_16.jpg',
                //'images/nana/icon_menherachan01_21.jpg',
                //'images/nana/icon_menherachan01_22.jpg',
                //'images/nana/icon_menherachan01_23.jpg',
                //'images/nana/icon_menherachan01_25.jpg',
                //'images/nana/icon_menherachan01_31.jpg',
                //'images/nana/icon_menherachan01_37.jpg',
                //'images/nana/icon_menherachan01_40.jpg',
                'images/avatars/0.jpg',
                'images/avatars/1.jpg',
                'images/avatars/2.jpg',
                'images/avatars/3.jpg'
            ];
            var random = Math.floor(Math.random() * icons.length);
            return icons[random];
        }
    },
    methods: {
        logout: function () {
            var res = axios.post('api/Auth/Logout')
                .then(response => {
                    location.href = "/login.html";
                }).catch(error => {
                    console.log(error);
                });
        },
        onKeyPress: function (e) {
            const key = e.keyCode || e.charCode || 0;
            if (key == 13) {

                var url = new URL(location.href);
                if (store.getters.searchValue) {
                    url.search = "search=" + store.getters.searchValue.searchValue;
                }

                location.href = url.href;

                e.preventDefault();

            }
        },
        searchMyAccount: function () {
            var url = new URL(location.href);
            url.search = "search=" + store.getters.account;
            location.href = url.href;
        },
        searchClick: function () {
            var url = new URL(location.origin + location.pathname);
            if (store.getters.searchValue.searchValue) {
                url.search = "search=" + store.getters.searchValue.searchValue;
            }
            location.href = url.href;
        }
    }
  })