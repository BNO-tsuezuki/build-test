Vue.component('footer-menu', {
    data: function () {
      return {
        count: 0
      }
    },
    template: `
        <footer class="main-footer">
            <!-- To the right -->
            <!--<div class="float-right d-none d-sm-inline">
                Anything you want
            </div>-->
            <!-- Default to the left -->
            <strong>Copyright &copy;  <a href="https://www.bandainamco-ol.co.jp/">BANDAI NAMCO Online inc.</a>.</strong> All rights reserved.
        </footer>
    `
  })