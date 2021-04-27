
//プラグインを構成するクラス
Vue.component('b-img');
class EasyImage {

    constructor({data, api}){
        this.api = api;
        this.data = {
            url:data.url || '',
            width:data.width || '',
            // withBorder: data.withBorder !== undefined ? data.withBorder : false,
            // withBackground: data.withBackground !== undefined ? data.withBackground : false,
            // stretched: data.stretched !== undefined ? data.stretched : false,
        };

        this.wrapper = undefined;
        this.settings = [
          {
            name: 'changeSize'
          }
        ];
    }

    renderSettings(){
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

                    self.api.blocks.insert('image', self.data, null , index , true);

                });
                form.appendChild(button);
                button.innerHTML = tune.icon;
                div.appendChild(form);
            }
        });
     
        return div;
      }

    //メニューバーにアイコンを表示
    static get toolbox() {
        return {

            title: 'Image',
            icon: '<i class="fas fa-images"></i>'
    
        };
     }
  
    //プラグインのUIを作成
    render(){
        var self = this;
        const div = document.createElement('div');

        if (self.data && self.data.url) {
            const img = document.createElement('img');
            img.src = self.data.url;
            if (self.data.width) img.width = self.data.width;
            div.innerHTML = '';
            div.appendChild(img);
        } else {
            //div.appendChild(input);
            $('#modal-default').modal('show');

            _fncImg = function (src, width) {
                self.data.url = src;
                self.data.width = width;

                div.innerHTML = '';

                const img = document.createElement('img');
                img.src = src;
                img.width = width;

                div.appendChild(img);
            };
        }

        return div;
     }
  
    //保存時のデータを抽出
    save(data){ 
        return {
            url: data.querySelector('img').src,
            width: data.querySelector('img').width,
          }
    }
  
  }