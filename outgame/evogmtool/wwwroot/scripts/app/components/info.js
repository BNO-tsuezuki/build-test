Vue.component('info-form', {
    data: function () {
      return {
          count: 0,
          searchText: ''
      }
    },
    mounted: function () {
        var res = axios.get('/api/Auth')
            .then(response => {
                //self.state.loginInfo = response.data;
                store.commit('getInfo', response.data);
                if (store.state.loginInfo) {
                    //
                } else {
                    location.href = "/login.html";
                }
            }).catch(error => {
                console.log(error);
                location.href = "/login.html";
            });
        //if (getParameter("search")) {
        //    this._data.searchText = getParameter("search");
        //    store.commit('searchValue', { searchValue: getParameter("search") });
        //}
    },
    template: `
        <nav class="main-header navbar navbar-expand navbar-white navbar-light">
            <!-- Left navbar links -->
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
                </li>
                <li class="nav-item d-none d-sm-inline-block">
                    <a href="/index.html" class="nav-link">Home</a>
                </li>
                <!--<li class="nav-item d-none d-sm-inline-block">
                  <a href="#" class="nav-link">Contact</a>
                </li>-->
            </ul>
            <!-- domain -->

            <!-- Right navbar links -->
            <ul class="navbar-nav ml-auto">
                <a href="#" v-on:click="logout">Logout</a>
            </ul>
        </nav>
    `,
    computed: {

    },
    methods: {
        logout: function () {
            var res = axios.post('/api/Auth/Logout')
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
                //if (store.getters.searchValue) {
                //    url.search = "search=" + store.getters.searchValue.searchValue;
                //}
                url.search = "search=" + this.searchText;
                location.href = url.href;

                e.preventDefault();

            }
        },
        searchClick: function () {
            var url = new URL(location.origin + location.pathname);
            url.search = "search=" + this.searchText;
            location.href = url.href;
        }
    }
  })