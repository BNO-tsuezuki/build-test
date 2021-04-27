
var vm_select;
var Tabs = window['VueTabs'].Tabs,
    Tab = window['VueTabs'].Tab;
const VueProgress = window['vue-progress-path'].default;

window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    Vue.use(VueProgress);
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "9",
                progress: 0.5,
                indeterminate: false,// ©“®
                counterClockwise: false,// ƒvƒƒOƒŒƒX‚Ì”½“]
                hideBackground: false, //”wŒi‚Ì”ñ•\¦
            }
        },
        components: {
            Tabs,
            Tab
        },
        mounted: function () {

        },

        computed: {

        },
        //async created(){
        created() {

        },
        methods: {


        },
        watch: {

        },
    });
});