
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    Vue.component('paginate', VuejsPaginate);
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
                periodList: [],
                unitList: [],
                loginInfo: {},
                target: null,
                displayPeriod: '',
                showUnit: '',
                searchData: [],
                parPage: 10,
                currentPage: 1,
                pageCount: 0,
                totalCount: 0,
                sort: 'asc'
            }
        },
        components: {

        },
        mounted: function () {
            this.getMyData();
            this.getPlayerData();
            this.getSeasonData();
            this.getMobileSuitData();
        },

        computed: {
            showList: function () {
                if (!this.searchData.careerRecords) return [];
                var sortedData = _.orderBy(this.searchData.careerRecords, 'recordItemId', this.sort);
                var start = (this.currentPage - 1) * this.parPage;
                var end = this.currentPage * this.parPage;
                return sortedData.slice(start, end);
            },
            playerName: function () {
                if (this.target) {
                    return this.target.player.playerName;
                } else {
                    return '';
                }
            },
            lang: function () {
                if (this.loginInfo.language) {
                    return this.loginInfo.language.languageCode;
                } else {
                    return '';
                }
            }
        },
        //async created(){
        created() {

        },
        methods: {
            getPlayerData: function () {
                if (getParameter('playerId') == '') return;

                var postData = {

                };
                var self = this;
                var targetApi = new URL('api/game/player/' + getParameter('playerId'), location.origin);
                //targetApi.search = 'playerId=' + getParameter('playerId');
                axios.get(targetApi.href, postData)
                    .then(function (response) {
                        self.target = response.data;
                    }).catch(function (error) {
                        //this.data.fail = true;
                    });
            },
            getMyData: function () {
                var self = this;
                var res = axios.get('/api/Auth')
                    .then(response => {
                        self.loginInfo = response.data;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getSeasonData: function () {
                var self = this;
                var targetApi = new URL('/api/game/misc/SeasonList', location.origin);
                var res = axios.get(targetApi.href)
                    .then(response => {
                        self.periodList = response.data;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getMobileSuitData: function () {
                var self = this;
                var targetApi = new URL('/api/game/misc/MobileSuitList', location.origin);
                var res = axios.get(targetApi.href)
                    .then(response => {
                        self.unitList = response.data;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getCareerRecord: function () {
                if (getParameter('playerId') == '') return;

                var self = this;

                var targetApi = new URL('/api/game/player/' + getParameter('playerId') + '/careerrecord', location.origin);
                var query = [];
                query.push('seasonNo=' + this.displayPeriod);
                query.push('mobileSuitId=' + this.showUnit);
                targetApi.search = query.join('&')
                var res = axios.get(targetApi.href)
                    .then(response => {
                        self.searchData = response.data;
                        self.pageCount = Math.ceil(self.searchData.careerRecords.length / self.parPage);
                        self.totalCount = self.searchData.careerRecords.length;
                        self.paging(self.currentPage);
                    }).catch(error => {
                        console.log(error);
                    });
            },
            paging: function (num) {
                this.currentPage = num;
            },
            toggleSort: function (sort) {
                this.sort = sort;
            },
            editConfirm: function (row) {
                var self = this;
                Swal.fire({
                    title: 'Are you sure ?',
                    html: this.confirmBeforeTable(row),
                    inputPlaceholder: 'Battle Record Value Input field.',
                    input: 'text',
                    width: 810,
                    showCancelButton: true,
                    confirmButtonText: 'Submit',
                    cancelButtonText: 'Cancel'
                }).then((result) => {
                    if (result.value) {
                        var postData = {
                            value: result.value,
                            numForAverage: row.numForAverage
                        }
                        var res = axios.put('/api/game/player/' + getParameter('playerId') + '/careerrecord/' + row.careerRecordId, postData)
                            .then(response => {
                                toastr.success('Succeeded.');
                                self.getCareerRecord();
                            }).catch(error => {
                                console.log(error);
                                toastr.error('Error.');
                            });
                    }
                });
            },
            confirmBeforeTable: function (row) {
                var html = '';
                html += '<table  style="border:1px solid #dee2e6;width: 750px;">';
                html += '<thead style="background:#d3d3d347;">';
                html += '<tr>';
                html += '<th style="text-align:center">Battle Record ID</th>';
                html += '<th style="text-align:center">Battle Record Name</th>';
                html += '<th style="text-align:center">Battle Record Value1</th>';
                html += '<th style="text-align:center">Battle Record Value2</th>';
                html += '</tr>';
                html += '</thead>';
                html += '<tbody>';
                html += '<tr>';
                html += '<td style="text-align:center">' + row.recordItemId + '</td>';
                if (this.lang == 'ja') {
                    html += '<td style="text-align:center">' + row.displayNameJapanese + '</td>';
                } else {
                    html += '<td style="text-align:center">' + row.displayNameEnglish + '</td>';
                }
                html += '<td style="text-align:center">' + row.value + '</td>';
                html += '<td style="text-align:center">' + row.numForAverage + '</td>';
                html += '</tr>';
                html += '</tbody>';
                html += '</table>';
                return html;
            }

        },
        watch: {

        },
    });
});