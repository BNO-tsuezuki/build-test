//プラグインを構成するクラス
class EasyImage2 {

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
          name: 'changeSize',
          icon: `<i class="material-icons">photo_size_select_actual</i>`
        },
        {
          name: 'changeSize2',
          icon: `<i class="material-icons">photo_size_select_actual</i>`
        },
      //   {
      //     name: 'stretched',
      //     icon: `<svg width="17" height="10" viewBox="0 0 17 10" xmlns="http://www.w3.org/2000/svg"><path d="M13.568 5.925H4.056l1.703 1.703a1.125 1.125 0 0 1-1.59 1.591L.962 6.014A1.069 1.069 0 0 1 .588 4.26L4.38.469a1.069 1.069 0 0 1 1.512 1.511L4.084 3.787h9.606l-1.85-1.85a1.069 1.069 0 1 1 1.512-1.51l3.792 3.791a1.069 1.069 0 0 1-.475 1.788L13.514 9.16a1.125 1.125 0 0 1-1.59-1.591l1.644-1.644z"/></svg>`
      //   },
      //   {
      //     name: 'withBackground',
      //     icon: `<svg width="20" height="20" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path d="M10.043 8.265l3.183-3.183h-2.924L4.75 10.636v2.923l4.15-4.15v2.351l-2.158 2.159H8.9v2.137H4.7c-1.215 0-2.2-.936-2.2-2.09v-8.93c0-1.154.985-2.09 2.2-2.09h10.663l.033-.033.034.034c1.178.04 2.12.96 2.12 2.089v3.23H15.3V5.359l-2.906 2.906h-2.35zM7.951 5.082H4.75v3.201l3.201-3.2zm5.099 7.078v3.04h4.15v-3.04h-4.15zm-1.1-2.137h6.35c.635 0 1.15.489 1.15 1.092v5.13c0 .603-.515 1.092-1.15 1.092h-6.35c-.635 0-1.15-.489-1.15-1.092v-5.13c0-.603.515-1.092 1.15-1.092z"/></svg>`
      //   }
      ];
  }

  renderSettings(){
      var self = this;
      const div = document.createElement('div');
   
      this.settings.forEach( tune => {
        let button = document.createElement('div');
   
        button.classList.add('cdx-settings-button');
        if(tune.name == 'changeSize'){
          button.addEventListener('click',(event) => {
              //alert('asdf');
              self.data.width = 300;
  
              var index = self.api.blocks.getCurrentBlockIndex();
  
              self.api.blocks.delete(index);
  
              self.api.blocks.insert('Image2',self.data);
  
            });
        }
        if(tune.name == 'changeSize2'){
          button.addEventListener('click',(event) => {
              //alert('asdf');
              self.data.width = 600;
  
              var index = self.api.blocks.getCurrentBlockIndex();
  
              self.api.blocks.delete(index);
  
              self.api.blocks.insert('Image2',self.data);
  
            });
        }
        button.innerHTML = tune.icon;
        div.appendChild(button);
      });
   
      return div;
    }

  //メニューバーにアイコンを表示
  static get toolbox() {
      return {

          title: 'Image2',
          icon: '<i class="fas fa-images"></i>'
  
      };
   }

  //プラグインのUIを作成
  render(){
      var self = this;
      const div = document.createElement('div');

      //const input = document.createElement('input');

      //input.placeholder = 'Paste an image URL...';


      $('#modal-default').modal('show');

      _fncImg = function (src,width) {
          self.data.url = src;
          self.data.width = width;

          div.innerHTML = '';

          const img = document.createElement('img');
          img.src = src;
          img.width = width;

          div.appendChild(img);
      };

      //input.addEventListener('paste', (event) => {

      //    const img = document.createElement('img');
      //    img.src = event.clipboardData.getData('text');
      //    //img.width = 400;
      //    self.data.url = img.src;

      //    div.innerHTML = '';
      //    div.appendChild(img);

      //});

      if (self.data && self.data.url) {
          const img = document.createElement('img');
          img.src = self.data.url;
          if (self.data.width) img.width = self.data.width;
          div.innerHTML = '';
          div.appendChild(img);
      } else {
          //div.appendChild(input);
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