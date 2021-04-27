Vue.component('loading', {
    data: function () {
      return {
        count: 0
      }
    },
    mounted: function () {

    },
    template: `
    <transition name="loading">
        <div class="loading-mask">
            <div class="loading-wrapper" @click.self="$emit('close')">
                <div class="loading-container">

                    <div class="loading-body">
                        <p style="margin:0;"><img :src="icon" /></p>
                    </div>

                </div>
            </div>
        </div>
    </transition>
    `,
    computed: {
        icon: function () {
            var icons = [
                'images/loading/Preloader_1.gif',
                'images/loading/Preloader_2.gif',
                'images/loading/Preloader_3.gif',
                'images/loading/Preloader_4.gif',
                'images/loading/Preloader_5.gif',
                'images/loading/Preloader_6.gif',
                'images/loading/Preloader_7.gif',
                'images/loading/Preloader_8.gif',
                'images/loading/Preloader_9.gif',
                'images/loading/Preloader_10.gif',
            ];
            var random = Math.floor(Math.random() * icons.length);
            return icons[random];
        }
    },
    methods: {

    }
  })