
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
                selectedAccount: '',
                startDate: moment().subtract('days', 29).format('YYYY/MM/DD HH:mm'),
                endDate: moment().format('YYYY/MM/DD HH:mm'),
                ipAddress: '',
                List:[]
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
            $('#ipInput').inputmask();
            $('#ipInput').change(function () {
                self._data.ipAddress = $(this).val();
            });
        },

        computed: {

        },
        //async created(){
        created() {

        },
        methods: {
            getLogs: function () {
                var self = this;

                this.ipAddress = $('#ipInput').val();

                var queryArray = [];
                if (this.startDate) queryArray.push('From=' + this.startDate);
                if (this.endDate) queryArray.push('To=' + this.endDate);
                if (this.selectedAccount) queryArray.push('Account=' + this.selectedAccount);
                if (this.ipAddress) queryArray.push('IpAddress=' + this.ipAddress);

                var targetApi = new URL('/api/log/auth', location.origin);

                targetApi.search = queryArray.join('&')
                
                var res = axios.get(targetApi.href)
                    .then(response => {
                        self._data.List = response.data;
                    }).catch(error => {
                        console.log(error);
                    });
            }

        },
        watch: {

        },
    });
});