Vue.component('modal', {
    data: function () {
      return {
        count: 0
      }
    },
    mounted: function () {

    },
    template: `
    <transition name="modal">
        <div class="modal-mask">
            <div class="modal-wrapper" @click.self="$emit('close')">
                <div class="modal-container">
                    <div class="modal-header">
                        <slot name="header"></slot>
                    </div>
 
                    <div class="modal-body">
                        <slot name="body"></slot>
                    </div>
 
                    <div class="modal-footer">
                        <slot name="footer"></slot>
                    </div>
                </div>
            </div>
        </div>
    </transition>
    `,
    computed: {

    },
    methods: {

    }
  })