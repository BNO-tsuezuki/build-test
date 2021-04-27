
Vue.component('date-time-picker', {
    props: ['value'],
    data: function () {
        return {
            count: 0,
            date: '',
            time: '',
            settingOK: false,
            loginInfo: {},
            myLanguage: null,
            dateFormat: '',
            placeholder: ''
        }
    },
    components: {
        'vuejs-datepicker': vuejsDatepicker,
        'vue-timepicker': VueTimepicker.default
    },
    mounted: function () {
        this.getMyData();
    },
    template: `
            <div v-if="settingOK">
                <vuejs-datepicker clear-button bootstrap-styling
                                    :format="dateFormat"
                                    :language="myLanguage"
                                    name="calText"
                                    style="background-color: white;display: inline-block;"
                                    v-model="date"
                                    :placeholder="placeholder"
                                    @closed="pickerClosedChange">
                </vuejs-datepicker>
                <div style="display: inline-block;">
                    <vue-timepicker v-model="time"></vue-timepicker>
                </div>
                <input type="button" class="btn btn-outline-secondary" style="padding-top: 4px;height: 36px;" value="Now" v-on:click="setNow()" />
            <p style="display:none;">{{dateTime}}</p>
            </div>
    `,
    computed: {

        dateTime: function () {
            if (!this.date) {
                this.$emit('input', '');
                return '';
            }
            if (this.date && (!this.time || this.time.indexOf('HH') != -1 || this.time.indexOf('mm') != -1)) {
                this.$emit('input', this.date);
                return '';
            }

            this.$emit('input', this.date + ' ' + this.time);
            return this.date + ' ' + this.time;
        }
    },
    methods: {
        getMyData: function () {
            var self = this;
            var res = axios.get('/api/Auth')
                .then(response => {
                    self.loginInfo = response.data;
                    self.lang = self.loginInfo.language.languageCode;
                    self.datePickerLang = self.loginInfo.language.languageCode;

                    if (self.loginInfo.language.languageCode == 'fr') this.myLanguage = vdp_translation_fr.js;
                    if (self.loginInfo.language.languageCode == 'en') this.myLanguage = vdp_translation_en.js;
                    if (self.loginInfo.language.languageCode == 'ja') this.myLanguage = vdp_translation_ja.js;
                    if (self.loginInfo.language.languageCode == 'de') this.myLanguage = vdp_translation_de.js;

                    if (self.loginInfo.language.languageCode == 'ja') {
                        self.dateFormat = 'yyyy/MM/dd';
                        self.placeholder = 'YYYY/MM/DD';
                    } else {
                        self.dateFormat = 'MM/dd/yyyy';
                        self.placeholder = 'MM/DD/YYYY';
                    }
                    if (this.value != '') {
                        var m = moment.utc(self.value, "YYYY-MM-DDTHH:mm:ss");
                        self.date = m.tz(self.loginInfo.timezone.timezoneCode).format('YYYY-MM-DD');
                        self.time = m.tz(self.loginInfo.timezone.timezoneCode).format('HH:mm');
                    }
                    this.settingOK = true;
                }).catch(error => {
                    console.log(error);
                });
        },
        pickerClosedChange: function () {
            if (this.date) this.date = moment(this.date).format('YYYY-MM-DD');
        },
        setNow: function () {
            this.date = moment().tz(this.loginInfo.timezone.timezoneCode).format('YYYY-MM-DD');
            this.time = moment().tz(this.loginInfo.timezone.timezoneCode).format('HH:mm');
        }

    }
})