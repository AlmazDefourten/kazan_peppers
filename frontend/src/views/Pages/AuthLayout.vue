<template>
  <div class="main-content bg-white">
    <base-nav
      v-model="showMenu"
      :transparent="true"
      menu-classes="justify-content-end"
      class="navbar-horizontal navbar-main navbar-top navbar-dark"
      expand="lg"
    >
      <div slot="brand" class="navbar-wrapper">
        <b-navbar-brand to="/">
          <img src="img/brand/logo.png">
        </b-navbar-brand>
      </div>

     <template>
         <b-navbar-nav  class="align-items-lg-center ml-lg-auto">
           <b-nav-item href="https://sovcombank.ru/?utm_referrer=https%3A%2F%2Fonline.sovcombank.ru%2F">
               <i class="ni"></i>
               <span class="nav-link-inner--text">Частным лицам</span>
           </b-nav-item>
           <b-nav-item href="https://sovcombank.ru/business">
               <i class="ni"></i>
               <span class="nav-link-inner--text">Бизнесу</span>
           </b-nav-item>
           <b-nav-item href="https://sovcombank.ru/corporate?type=kib">
               <i class="ni"></i>
               <span class="nav-link-inner--text">Корпорациям</span>
           </b-nav-item>
           <b-nav-item href="https://sovcombank.ru/about/info">
               <i class="ni"></i>
               <span class="nav-link-inner--text">О банке</span>
           </b-nav-item>
           <b-nav-item href="https://sovcombank.ru/help">
             <i class="ni"></i>
             <span class="nav-link-inner--text">Помощь</span>
           </b-nav-item>
           <b-nav-item href="https://sovcombank.ru/office">
             <i class="ni"></i>
             <span class="nav-link-inner--text">Офисы и банкоматы</span>
           </b-nav-item>
       </b-navbar-nav>
     </template>
    </base-nav>

    <div class="main-content">
      <zoom-center-transition
        :duration="pageTransitionDuration"
        mode="out-in"
      >
        <router-view></router-view>
      </zoom-center-transition>
    </div>

    <footer class="py-5" id="footer-main">
      <b-container >
        <b-row align-v="center" class="justify-content-xl-between">
          <b-col xl="6">
            <div class="copyright text-center text-xl-left text-muted">
              © {{year}} <a href="https://www.creative-tim.com" class="font-weight-bold ml-1" target="_blank">Creative Tim</a>
            </div>
          </b-col>
          <b-col xl="6" class="col-xl-6">
            <b-nav  class="nav-footer justify-content-center justify-content-xl-end">
              <b-nav-item href="https://www.creative-tim.com" target="_blank" >
                Creative Tim
              </b-nav-item>
              <b-nav-item href="https://www.creative-tim.com/presentation" target="_blank" >
                About Us
              </b-nav-item>
              <b-nav-item href="http://blog.creative-tim.com"  target="_blank">
                Blog
              </b-nav-item>
              <b-nav-item href="https://www.creative-tim.com/license" target="_blank">
                License
              </b-nav-item>
            </b-nav>
          </b-col>
        </b-row>
      </b-container>
    </footer>
  </div>
</template>
<script>
  import { BaseNav } from '@/components';
  import { ZoomCenterTransition } from 'vue2-transitions';

  export default {
    components: {
      BaseNav,
      ZoomCenterTransition
    },
    props: {
      backgroundColor: {
        type: String,
        default: 'black'
      }
    },
    data() {
      return {
        showMenu: false,
        menuTransitionDuration: 250,
        pageTransitionDuration: 200,
        year: new Date().getFullYear(),
        pageClass: 'login-page'
      };
    },
    computed: {
      title() {
        return `${this.$route.name} Page`;
      }
    },
    methods: {
      toggleNavbar() {
        document.body.classList.toggle('nav-open');
        this.showMenu = !this.showMenu;
      },
      closeMenu() {
        document.body.classList.remove('nav-open');
        this.showMenu = false;
      },
      setBackgroundColor() {
        document.body.classList.add('bg-default');
      },
      removeBackgroundColor() {
        document.body.classList.remove('bg-default');
      },
      updateBackground() {
        if (!this.$route.meta.noBodyBackground) {
          this.setBackgroundColor();
        } else {
          this.removeBackgroundColor()
        }
      }
    },
    beforeDestroy() {
      this.removeBackgroundColor();
    },
    beforeRouteUpdate(to, from, next) {
      // Close the mobile menu first then transition to next page
      if (this.showMenu) {
        this.closeMenu();
        setTimeout(() => {
          next();
        }, this.menuTransitionDuration);
      } else {
        next();
      }
    },
    watch: {
      $route: {
        immediate: true,
        handler: function () {
          this.updateBackground()
        }
      }
    }
  };
</script>
<style lang="scss">
  $scaleSize: 0.8;
  @keyframes zoomIn8 {
    from {
      opacity: 0;
      transform: scale3d($scaleSize, $scaleSize, $scaleSize);
    }
    100% {
      opacity: 1;
    }
  }

  .main-content .zoomIn {
    animation-name: zoomIn8;
  }

  @keyframes zoomOut8 {
    from {
      opacity: 1;
    }
    to {
      opacity: 0;
      transform: scale3d($scaleSize, $scaleSize, $scaleSize);
    }
  }

  .main-content .zoomOut {
    animation-name: zoomOut8;
  }

</style>
