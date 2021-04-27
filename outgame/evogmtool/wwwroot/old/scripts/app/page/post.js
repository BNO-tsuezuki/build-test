
var vm_select;

function getParameter(key) {
    //パラメーターを配列で取得する。
    var str = location.search.split("?");
    if (str.length < 2) {
      return "";
    }

    var params = str[1].split("&");
    for (var i = 0; i < params.length; i++) {
      var keyVal = params[i].split("=");
      if (keyVal[0] == key && keyVal.length == 2) {
        return decodeURIComponent(keyVal[1]);
      }
    }
    return "";
  }
window.addEventListener('DOMContentLoaded', ()=> {
    Vue.config.devtools = true
    vm_select = new Vue({
        el: '#content',
        mounted:function(){
            CKEDITOR.replace( 'editor1' );
            CKEDITOR.on('change',function(){alert("tt")});
            CKEDITOR.instances.editor1.on('change', function() { 
                vm_select.text = CKEDITOR.instances.editor1.getData();
                vm_select.$forceUpdate();
            });
            //CKEDITOR.instances.editor1.getData();
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
            this.getData(1);
        },
        methods:{
            getData:function(page){

            },
            getCSV:function(){

            },
            deleteTaget:function(item){

            },
            insertTarget:function(item){

            },
            updateTarget:function(item){

            },

        },
        watch: {
            
        },
    });
});