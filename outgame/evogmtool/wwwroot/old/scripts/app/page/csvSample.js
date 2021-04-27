
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
                message: "",
                workers: []
            }
        },
        computed:{

        },
        //async created(){
        created(){

        },
        methods:{

            loadCsvFile: function (e) {
                let vm = this;
                vm.workers = [];
                vm.message = "";
                let file = e.target.files[0];

                //if (!file.type.match("text/csv")) {
                //    vm.message = "Please select a CSV file";
                //    return;
                //}

                let reader = new FileReader();
                reader.readAsText(file);

                reader.onload = () => {
                    let lines = reader.result.split("\n");
                    lines.shift();
                    let linesArr = [];
                    for (let i = 0; i < lines.length; i++) {
                        linesArr[i] = lines[i].split(",");
                    }
                    vm.workers = linesArr;
                }
            }
        },
        watch: {
            
        },
    });
});