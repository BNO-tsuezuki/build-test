
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
                selectedUserId: '',
                startDate: moment().subtract('days', 29).format('YYYY/MM/DD HH:mm'),
                endDate: moment().format('YYYY/MM/DD HH:mm'),
                ipAddress: '',
                List: []
            }
        },
        mounted: function () {
            var self = this;
            $('#reservationtime').daterangepicker({
                startDate: moment().subtract('days', 29),
                endDate: moment(),
                maxDate: moment().add('days', 1).format('MM/DD/YYYY'),
                timePicker: true,
                timePicker24Hour: true,
                timePickerIncrement: 30,
                locale: {
                    format: 'MM/DD/YYYY HH:mm'
                }
            },
                function (start, end) {
                    console.log("Callback has been called!");
                    self._data.startDate = start.format('YYYY/MM/DD HH:mm');
                    self._data.endDate = end.format('YYYY/MM/DD HH:mm');

                }
            );

        },

        computed: {

        },
        //async created(){
        created() {

        },
        methods: {
            getLogs: function () {
                var self = this;
                var queryArray = [];
                var timezone = 'Asia/Tokyo'; // todo: タイムゾーンを変更できるようにする
                if (this.startDate) queryArray.push('From=' + toISOString(this.startDate, timezone));
                if (this.endDate) queryArray.push('To=' + toISOString(this.endDate, timezone));
                if (this.selectedUserId) queryArray.push('UserId=' + this.selectedUserId);
                //if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);

                var targetApi = new URL('/api/log/operation', location.origin);

                targetApi.search = queryArray.join('&')

                var res = axios.get(targetApi.href)
                    .then(response => {
                        self._data.List = response.data.operationLogList;
                    }).catch(error => {
                        console.log(error);
                    });
            }
        },
        watch: {

        },
    });
    function toISOString(dateTime, timezone) {
        return moment.tz(moment(dateTime).format('YYYY-MM-DDTHH:mm'), timezone).utc().format(); // todo: 共通関数にする？
    };
});