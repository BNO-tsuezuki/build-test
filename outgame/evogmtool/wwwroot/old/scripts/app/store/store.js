// ストアオブジェクトの作成
const store = new Vuex.Store({
    state: {
        loginInfo: {
            //id : 0 ,
            //name: '' ,
            //Authority: '' ,
        },
        searchValue: ''
    },
    mutations: {
        getInfo: function (state,val) {
            state.loginInfo = val;
        },
        searchValue: function (state, val) {
            state.searchValue = val;
        }
    },
    getters: {
        account: function (state) {
            if (state.loginInfo.account) {
                return state.loginInfo.account;
            } else {
                return '';
            }
        },
        name: function (state) {
            if (state.loginInfo.name) {
                return state.loginInfo.name;
            } else {
                return '';
            }
        },
        role: function (state) {
            if (state.loginInfo.role) {
                return state.loginInfo.role;
            } else {
                return '';
            }
        },
        userId: function (state) {
            if (state.loginInfo.userId) {
                return state.loginInfo.userId;
            } else {
                return '';
            }
        },
        roleSet: function (state) {
            var set = [];
            set.push({ id: 1, role: 'super' });
            set.push({ id: 2, role: 'administrator' });
            set.push({ id: 3, role: 'publisher' });
            set.push({ id: 4, role: 'operator' });
            set.push({ id: 5, role: 'watcher' });

            if (state.loginInfo.role) {
                return set;
            } else {
                return set;
            }
        },
        searchValue: function (state) {
            if (state.searchValue) {
                return state.searchValue;
            } else {
                return '';
            }
        }
    }
});

