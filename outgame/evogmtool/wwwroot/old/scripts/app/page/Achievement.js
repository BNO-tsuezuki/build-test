
var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                text: "",
            }
        },
        mounted:function(){
            
        },

        computed:{

        },
        //async created(){
        created(){

        },
        methods:{


        },
        watch: {
            
        },
    });
});