
var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        mounted:function(){
            
        },
        data:function(){
            return {
                text:"",
            }
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