

var editor;
var vm_select;
window.addEventListener('DOMContentLoaded', function() {
    //if(getParameter("debug") == "true") 
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        //store,
        data: function () {
            return {
                text: "",
                editorData: `
                {
                    "time": 1594715952369,
                    "blocks": [
                        {
                            "type": "header",
                            "data": {
                                "text": "ほげほげサンプル",
                                "level": 2
                            }
                        },
                        {
                            "type": "paragraph",
                            "data": {
                                "text": "ほげ<b>ほげ</b>サンプル"
                            }
                        },
                        {
                            "type": "paragraph",
                            "data": {
                                "text": "画像"
                            }
                        },
                        {
                            "type": "image",
                            "data": {
                                "url": "https://img.fortawesome.com/1ce05b4b/card-new-icons.svg"
                            }
                        },
                        {
                            "type": "paragraph",
                            "data": {
                                "text": "ビデオ"
                            }
                        },
                        {
                            "type": "video",
                            "data": {
                                "url": "https://www.home-movie.biz/mov/hts-samp005.mp4"
                            }
                        }
                    ],
                    "version": "2.18.0"
                }
                `,
                saveString: ''
            }
        },
        mounted:function(){
            const tmp = new EditorJS(
                {
                    holder: 'editor',
                    tools: {
                        header: {
                            class: Header,
                            inlineToolbar: ['bold']
                        },
                        paragraph: {
                            class: Paragraph,
                            inlineToolbar: ['bold']
                        },
                        // paragraph2: {
                        //     class: Paragraph,
                        //     inlineToolbar: ['bold']
                        // },      
                        // list: List,        
                        // checklist: Checklist,
                        // quote: Quote,
                        // code: CodeTool,
                        image: {
                            class: EasyImage,
                            inlineToolbar: true
                        },
                        image2: {
                            class: EasyImage2,
                            inlineToolbar: true
                        },
                        video: EasyVideo
                    },
                    data: JSON.parse(this._data.editorData)
                }
             );
            editor = tmp;
        },

        computed:{

        },
        //async created(){
        created(){

        },
        methods:{
            saveData:function(){
                if(editor){
                    editor.save()
                    .then((savedData) => {
                
                          console.log(savedData);
                          this._data.saveString = JSON.stringify(savedData.blocks);
                
                    });
                    
                }
            }

        },
        watch: {
            
        },
    });
});