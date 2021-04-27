
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
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
            toast1: function () {
                Toast.fire({
                    icon: 'success',
                    title: 'Toast Test 1.'
                });
                return false;
            },
            toast2: function () {
                Toast.fire({
                    icon: 'info',
                    title: 'Toast Test 2.'
                });
                return false;
            },
            toast3: function () {
                Toast.fire({
                    icon: 'error',
                    title: 'Toast Test 3.'
                });
                return false;
            },
            toast4: function () {
                Toast.fire({
                    icon: 'warning',
                    title: 'Toast Test 4.'
                });
                return false;
            },
            toast5: function () {
                Toast.fire({
                    icon: 'question',
                    title: 'Toast Test 5.'
                });
                return false;
            },
            toast6: function () {
                toastr.success('Lorem ipsum dolor sit amet, consetetur sadipscing elitr.');
                return false;
            },
            toast7: function () {
                toastr.error('Lorem ipsum dolor sit amet, consetetur sadipscing elitr.');
                return false;
            }

        },
        watch: {

        },
    });
});