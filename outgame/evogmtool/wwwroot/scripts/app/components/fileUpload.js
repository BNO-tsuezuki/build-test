Vue.component('file-upload', {
    data: function () {
        return {
            isEnter: false,
            files: []
        }
    },
    mounted: function () {

    },
    template: `
            <div class="file_uploader_module">
                <input type="file" ref="file_upload" style="display: none" @change="onFileChange" multiple>
                <div class="drop_area" @dragenter="dragEnter" @dragleave="dragLeave" @dragover.prevent @drop.prevent="dropFile"
                    :class="{enter: isEnter}" type="button" @click="$refs.file_upload.click()">
                    <i class="fas fa-arrow-circle-up text-6xl mb-3 fa-5x upload-icon"></i>
                    <b>Drop files or Browse</b>
                </div>
                <ul class="flex-fileUpload">
                    <li class="flex-col" v-for="(file,index) in files" :key="index" @click="deleteFile(index)">
                        <div style="position: relative;"></div>
                        <div class="file-box" style="display: block;">
                            <span>
                                {{ file.name }}
                                <i class="fa fa-times delete-mark"></i>
                            </span>
                        </div>
                    </li>
                </ul>
                <div v-show="files.length">
                    <button class="button" @click="sendFile">Upload</button>
                </div>
            </div>
    `,
    computed: {

    },
    methods: {
        dragEnter() {
            this.isEnter = true;
        },
        dragLeave() {
            this.isEnter = false;
        },
        dragOver() {
        },
        dropFile() {
            this.files.push(...event.dataTransfer.files)
            this.isEnter = false;
        },
        deleteFile(index) {
            this.files.splice(index, 1)
        },
        sendFile() {
            var self = this;
            let formData = new FormData();
            for (var i = 0; i < this.files.length; i++) {
                formData.append('files', this.files[i]);
            }
            axios.post('/api/file', formData)
                .then(response => {
                    toastr.success('Upload Succeeded.');
                    self.files = [];
                    console.log(response.data);
                    self.$emit('get_list');
                })
                .catch(error => {
                    toastr.error('Error.');
                    console.log(error);
                });
        },
        btnclick() {
            this.$refs.input.click();
        },
        onFileChange(e) {
            let files = e.target.files || e.dataTransfer.files;
            this.files.push(...files)
            console.log(files)
        }
    }
})