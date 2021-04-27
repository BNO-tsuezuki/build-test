
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    $(window).on('load', function () {
        $('input[name="serchText"]').val('');
        $('#customRadio1').prop('checked', true);
        $('#customRadio2').prop('checked', false);
    });
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                searchMode: 'playerId',
                serchNameText: '',
                serchIdText: '',
                serchInkyID: '',
                notFound: false,
                players: null,
            }
        },
        mounted: function () {

        },

        computed: {

        },
        //async created(){
        created() {

        },
        methods: {
            searchClick: function () {
                if (this.serchText == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/player', location.origin);
                if (this.searchMode == 'playerId') targetApi.search = 'playerId=' + this.serchIdText.trim();
                if (this.searchMode == 'playerName') targetApi.search = 'playerName=' + this.serchNameText.trim();
                if (this.searchMode == 'InkyID') targetApi.search = 'account=' + this.serchInkyID.trim();
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        //self._data.searchData = response.data;
                        //self._data.editTarget = response.data;

                        self.players = response.data.players;
                        //location.href = '/player/player.html?playerId=' + response.data.player.playerId;
                    }).catch(function (error) {
                        //this.data.fail = true;
                        self.notFound = true;
                    });
            },
            openUser: function (data) {
                location.href = '/player/player.html?playerId=' + data.playerId;
            }

        },
        watch: {

        },
    });
});