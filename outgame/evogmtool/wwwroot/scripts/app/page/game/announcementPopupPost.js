var editor_jp,
    editor_en,
    editor_fr,
    editor_ger;
var vm_select;
window.addEventListener('DOMContentLoaded', function () {
    //if(getParameter("debug") == "true") 
    const Toast = Swal.mixin({
        toast: true,
        position: 'top',
        showConfirmButton: false,
        timer: 1000,
        timerProgressBar: false,
        onOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    })

    const VueCtkDateTimePicker = window['vue-ctk-date-time-picker'];

    Vue.component('vue-ctk-date-time-picker', VueCtkDateTimePicker);
    Vue.component('paginate', VuejsPaginate);
    Vue.config.devtools = true;
    vm_select = new Vue({
        el: '#content',
        store,
        data: function () {
            return {
                loginInfo: {},
                myDomain: {},
                domain: [],
                postData: {
                    release: false,
                    target: '',
                    memo: '',
                    beginDate: '',
                    endDate: '',
                    enabledEnglish: false,
                    msgEnglish: '',
                    enabledFrench: false,
                    msgFrench: '',
                    enabledGerman: false,
                    msgGerman: '',
                    enabledJapanese: false,
                    msgJapanese: '',
                    titleEnglish: '',
                    titleFrench: '',
                    titleGerman: '',
                    titleJapanese: '',
                },
                start: {
                    datetime: ''
                },
                end: {
                    datetime: ''
                },
                editorData: {
                    en: '',
                    ja: '',
                    fr: '',
                    ger: ''
                },
                pickerShow: false

                /*
                editorData: {
                    en: `
                {
                    "time": 1594715952369,
                    "blocks": [

                        ],
                    "version": "2.18.0"
                }
                `,
                    ja: `
                {
                    "time": 1594715952369,
                    "blocks": [
                        {
                            "type":"paragraph",
                            "data":
                            {
                                    "text":"asdf<red>asd</red>fasdf<blue>asdf</blue>asdf<black>asd</black>fasdf",
                                    "level":4
                            }
                        }
                    ],
                    "version": "2.18.0"
                }
                `,
                    fr: `
                {
                    "time": 1594715952369,
                    "blocks": [
	                        
                        ],
                    "version": "2.18.0"
                }
                `,
                    ger: `
                {
                    "time": 1594715952369,
                    "blocks": [
	                        
                        ],
                    "version": "2.18.0"
                }
                `
                },
                */
            }
        },
        mounted: function () {
            this.getMyData();
            if (getParameter("id")) {
                this.getData(getParameter("id"));
            } else {
                this.pickerShow = true;
            }
        },

        computed: {
            mode: function () {
                var self = this;
                if (getParameter("id") && this.myDomain.publisher && this.postData.target) {
                    var targetDomain = _.find(self.domain, function (o) { return o.target == self.postData.target; });
                    if (this.myDomain.publisher.publisherId == targetDomain.publisher.publisherId) {
                        return 'Edit';
                    } else {
                        return 'View';
                    }
                } else {
                    return 'New Post';
                }
            }
        },
        created() {

        },
        methods: {
            getMyData: function () {
                var self = this;
                var res = axios.get('/api/Auth')
                    .then(response => {
                        self.loginInfo = response.data;
                        self.getDomain();
                    }).catch(error => {
                        console.log(error);
                    });
            },
            getDomain: function () {
                var self = this;
                var res = axios.get('/api/domainregion')
                    .then(response => {
                        self.domain = response.data;

                        var myDomain = _.find(self.domain, function (o) { return o.publisher.publisherId == self.loginInfo.publisher.publisherId; });
                        self.myDomain = myDomain;

                    }).catch(error => {
                        console.log(error);
                    });
            },
            editorSetting: function () {
                editor_jp = new EditorJS(
                    {
                        holder: 'editor_ja',
                        tools: {
                            paragraph: {
                                class: Text,
                                inlineToolbar: ['bold', 'RedText', 'BlueText', 'BlackText'],
                            },
                            BlackText: {
                                class: BlackText,
                                inlineToolbar: true
                            },
                            BlueText: {
                                class: BlueText,
                                inlineToolbar: true
                            },
                            RedText: {
                                class: RedText,
                                inlineToolbar: true
                            }

                        },
                        data: JSON.parse(this.editorData.ja)
                    }
                );
                editor_en = new EditorJS(
                    {
                        holder: 'editor_en',
                        tools: {
                            paragraph: {
                                class: Text,
                                inlineToolbar: ['bold']
                            }

                        },
                        data: JSON.parse(this.editorData.en)
                    }
                );
                editor_fr = new EditorJS(
                    {
                        holder: 'editor_fr',
                        tools: {
                            paragraph: {
                                class: Text,
                                inlineToolbar: ['bold']
                            }

                        },
                        data: JSON.parse(this.editorData.fr)
                    }
                );
                editor_ger = new EditorJS(
                    {
                        holder: 'editor_ger',
                        tools: {
                            paragraph: {
                                class: Text,
                                inlineToolbar: ['bold']

                            }

                        },
                        data: JSON.parse(this.editorData.ger)
                    }
                );
            },
            getData: function (id) {
                var self = this;

                var targetApi = new URL('/api/game/opsnotice/popup/' + id , location.origin);
                var res = axios.get(targetApi.href)
                    .then(response => {
                        self._data.postData = response.data;
                        self._data.editorData.en = self._data.postData.msgEnglish;
                        self._data.editorData.ja = self._data.postData.msgJapanese;
                        self._data.editorData.fr = self._data.postData.msgFrench;
                        self._data.editorData.ger = self._data.postData.msgGerman;

                        self.pickerShow = true;
                    }).catch(error => {
                        console.log(error);
                    });
            },
            saveConfirm: function () {
                Swal.fire({
                    title: 'Can I save the announcement in a valid state ?',
                    showCancelButton: true,
                    confirmButtonText: `Yes`,
                    confirmButtonColor: 'blue',
                }).then((result) => {
                    /* Read more about isConfirmed, isDenied below */
                    if (result.isConfirmed) {
                        this.save();
                    }
                })
            },
            save: function () {
                if (getParameter("id")) {
                    this.put();
                } else {
                    this.post();
                }
            
            },
            post: function () {
                var self = this;
                if (this.mode == 'New Post') {
                    this.postData.target = this.myDomain.target;
                }
                var postData = _.cloneDeep(this.postData);
                postData.msgEnglish = this.editorData.en;
                postData.msgJapanese = this.editorData.ja;
                postData.msgFrench = this.editorData.fr;
                postData.msgGerman = this.editorData.ger;

                postData.beginDate = moment.tz(postData.beginDate, self.loginInfo.timezone.timezoneCode).utc().format('YYYY/MM/DD HH:mm');
                postData.endDate = moment.tz(postData.endDate, self.loginInfo.timezone.timezoneCode).utc().format('YYYY/MM/DD HH:mm');


                var targetApi = new URL('/api/game/opsnotice/popup', location.origin);
                axios.post(targetApi.href, postData)
                    .then(response => {
                        toastr.success('Save Succeeded.');
                        location.href = '/game/announcementPopupPost.html?id=' + response.data.id;
                    }).catch(error => {
                        toastr.error('error.');
                    });
            },
            put: function () {
                var self = this;
                var postData = _.cloneDeep(this.postData);
                postData.msgEnglish = this.editorData.en;
                postData.msgJapanese = this.editorData.ja;
                postData.msgFrench = this.editorData.fr;
                postData.msgGerman = this.editorData.ger;

                postData.beginDate = moment.tz(postData.beginDate, self.loginInfo.timezone.timezoneCode).utc().format('YYYY/MM/DD HH:mm');
                postData.endDate = moment.tz(postData.endDate, self.loginInfo.timezone.timezoneCode).utc().format('YYYY/MM/DD HH:mm');

                delete postData.updateDate;
                delete postData.version;


                var targetApi = new URL('/api/game/opsnotice/popup/' + getParameter("id"), location.origin);
                axios.put(targetApi.href, postData)
                    .then(response => {
                        toastr.success('Save Succeeded.');
                    }).catch(error => {
                        toastr.error('error.');
                    });
            },
            deleteConfirm: function () {
                Swal.fire({
                    title: 'Do you really want to delete this ?',
                    showCancelButton: true,
                    confirmButtonText: `Delete`,
                    confirmButtonColor: 'red',
                }).then((result) => {
                    /* Read more about isConfirmed, isDenied below */
                    if (result.isConfirmed) {
                        this.del();
                    }
                })
            },
            del: function () {
                var self = this;
                var postData = {

                }

                var targetApi = new URL('/api/game/opsnotice/popup/' + getParameter("id") , location.origin);
                axios.delete(targetApi.href, postData)
                    .then(response => {
                        location.href = '/game/announcementPopup.html';
                    }).catch(error => {
                        self._data.fail = true;
                    });
            
            }

        },
        watch: {

        },
    });
});