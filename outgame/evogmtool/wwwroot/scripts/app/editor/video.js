//プラグインを構成するクラス
class EasyVideo {

    constructor({ data, api }) {
        this.data = data;
        this.api = api;
        this.wrapper = undefined;
        this.settings = [
            {
                name: 'changeSize'
            }
        ];
      }

    //メニューバーにアイコンを表示
    static get toolbox() {
        return {

            title: 'Video',
            icon: '<i class="fas fa-file-video"></i>'
    
        };
    }

    renderSettings() {
        var self = this;
        const div = document.createElement('div');

        this.settings.forEach(tune => {

            if (tune.name == 'changeSize') {
                let form = document.createElement('form');
                let input = document.createElement('input');
                input.id = 'txtSize';
                input.type = 'number';

                input.value = self.data.width;

                form.appendChild(input);
                let button = document.createElement('input');
                button.type = 'button';
                button.value = 'Resize';
                button.addEventListener('click', (event) => {
                    var size = document.getElementById('txtSize').value;
                    self.data.width = size;

                    var index = self.api.blocks.getCurrentBlockIndex();

                    self.api.blocks.delete(index);

                    self.api.blocks.insert('video', self.data, null, index, true);

                });
                form.appendChild(button);

                let label = document.createElement('label');
                label.textContent = 'Thumbnail:';
                form.appendChild(document.createElement("br"));
                form.appendChild(label);

                let img = document.createElement('img');
                img.src = self.data.poster;
                img.width = 150;

                form.appendChild(img);

                button.innerHTML = tune.icon;
                div.appendChild(form);
            }
        });

        return div;
    }
  
    //プラグインのUIを作成
    render(){
        var self = this;
        const div = document.createElement('div');
        const input = document.createElement('input');
        input.placeholder = 'Paste an video URL...';

        input.addEventListener('paste', (event) => {

            const video = document.createElement('video');
      
            video.src = event.clipboardData.getData('text');
            video.width = 400;
            video.setAttribute('controls','');
            
            div.innerHTML = '';
            div.appendChild(video);

        });
        
        if(self.data && self.data.url){
            const video = document.createElement('video');
            video.src = self.data.url;
            video.setAttribute('controls','');
            video.width = self.data.width;

            if (self.data.poster != "") {
                video.poster = self.data.poster;
            }

            div.innerHTML = '';
            div.appendChild(video);
        }else{
            $('#modal-video').modal('show');

            _fncVideo = function (videoSrc, imageSrc, width) {
                self.data.url = videoSrc;
                self.data.width = width;
                self.data.poster = imageSrc;

                div.innerHTML = '';

                const video = document.createElement('video');
                video.src = videoSrc;
                video.width = width;
                video.poster = imageSrc;
                video.setAttribute('controls', '');

                div.appendChild(video);
            };
        }

        return div;
     }
  
    //保存時のデータを抽出
    save(data){ 
        return {
            url: data.querySelector('video').src,
            width: data.querySelector('video').width,
            poster: data.querySelector('video').poster,
          }
    }
  
  }