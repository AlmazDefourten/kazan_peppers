<template>
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
<!--    <a href="#" aria-current="page" class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block active router-link-active"> {{$route.name}} </a>-->
    <!-- Navbar links -->
<!--    <b-navbar-nav class="align-items-center ml-md-auto">-->
      <!-- This item dont have <b-nav-item> because item have data-action/data-target on tag <a>, wich we cant add -->
<!--      <li class="nav-item d-sm-none">-->
<!--        <a class="nav-link" href="#" data-action="search-show" data-target="#navbar-search-main">-->
<!--          <i class="ni ni-zoom-split-in"></i>-->
<!--        </a>-->
<!--      </li>-->
<!--    </b-navbar-nav>-->

    <b-navbar-nav class="align-content-lg-center ml-lg-1" >
      <b-nav-item href="https://sovcombank.ru/?utm_referrer=https%3A%2F%2Fonline.sovcombank.ru%2F">
        <i class="ni"></i>
        <span class="nav-link-inner--text custom-class">Частным лицам</span>
      </b-nav-item>
      <b-nav-item href="https://sovcombank.ru/business">
        <i class="ni"></i>
        <span class="nav-link-inner--text custom-class">Бизнесу</span>
      </b-nav-item>
      <b-nav-item href="https://sovcombank.ru/corporate?type=kib">
        <i class="ni"></i>
        <span class="nav-link-inner--text custom-class">Корпорациям</span>
      </b-nav-item>
      <b-nav-item href="https://sovcombank.ru/about/info">
        <i class="ni"></i>
        <span class="nav-link-inner--text custom-class">О банке</span>
      </b-nav-item>
      <b-nav-item href="https://sovcombank.ru/help">
        <i class="ni"></i>
        <span class="nav-link-inner--text custom-class">Помощь</span>
      </b-nav-item>
      <b-nav-item href="https://sovcombank.ru/office">
        <i class="ni"></i>
        <span class="nav-link-inner--text custom-class">Офисы и банкоматы</span>
      </b-nav-item>
    </b-navbar-nav>
    <b-navbar-nav class="align-items-center ml-4 ml-md--4">
        <b-form class="navbar-search form-inline mr-sm-3"
            :class="{'navbar-search-dark': type === 'default', 'navbar-search-light': type === 'light'}"
            id="navbar-search-main">
<!--        <b-form-group class="mb-0">-->
<!--          <b-input-group class="input-group-alternative input-group-merge">-->
<!--&lt;!&ndash;            <b-form-input placeholder="Search" type="text"> </b-form-input>&ndash;&gt;-->

<!--            <div class="input-group-append">-->
<!--              <span class="input-group-text"><i class="fas fa-search"></i></span>-->
<!--            </div>-->
<!--          </b-input-group>-->
<!--        </b-form-group>-->
      </b-form>
      <base-dropdown menu-on-right
                     class="nav-item"
                     tag="li"
                     title-tag="a"
                     title-classes="nav-link pr-0">
        <a href="#" class="nav-link pr-0" style="color: black;" @click.prevent slot="title-container">
          <b-media no-body class="align-items-center">
                  <span class="avatar avatar-sm rounded-circle">
                    <img alt="Image placeholder" src="img/theme/team-4.jpg">
                  </span>
            <b-media-body class="ml-2 d-none d-lg-block">
              <span class="mb-0 text-sm  font-weight-bold">ИМЯ</span>
            </b-media-body>
          </b-media>
        </a>

        <template>
          <b-dropdown-header class="noti-title">
            <h6 class="text-overflow m-0">Welcome!</h6>
          </b-dropdown-header>
          <b-dropdown-item href="#!">
            <i class="ni ni-single-02"></i>
            <span>My profile</span>
          </b-dropdown-item>
          <b-dropdown-item href="#!">
            <i class="ni ni-settings-gear-65"></i>
            <span>Settings</span>
          </b-dropdown-item>
          <b-dropdown-item href="#!">
            <i class="ni ni-calendar-grid-58"></i>
            <span>Activity</span>
          </b-dropdown-item>
          <b-dropdown-item href="#!">
            <i class="ni ni-support-16"></i>
            <span>Support</span>
          </b-dropdown-item>
          <div class="dropdown-divider"></div>
          <b-dropdown-item href="#!">
            <i class="ni ni-user-run"></i>
            <span>Logout</span>
          </b-dropdown-item>

        </template>
      </base-dropdown>
    </b-navbar-nav>
  </base-nav>
</template>
<script>
import { CollapseTransition } from 'vue2-transitions';
import { BaseNav, Modal } from '@/components';

export default {
  components: {
    CollapseTransition,
    BaseNav,
    Modal
  },
  props: {
    type: {
      type: String,
      default: 'default', // default|light
      description: 'Look of the dashboard navbar. Default (Green) or light (gray)'
    }
  },
  computed: {
    routeName() {
      const { name } = this.$route;
      return this.capitalizeFirstLetter(name);
    }
  },
  data() {
    return {
      activeNotifications: false,
      showMenu: false,
      searchModalVisible: false,
      searchQuery: ''
    };
  },
  methods: {
    capitalizeFirstLetter(string) {
      return string.charAt(0).toUpperCase() + string.slice(1);
    },
    toggleNotificationDropDown() {
      this.activeNotifications = !this.activeNotifications;
    },
    closeDropDown() {
      this.activeNotifications = false;
    }
  }
};
</script>
<style>
.custom-class {
  color: black;
  font-size: 14px; /* Установите желаемый размер шрифта */
  line-height: 1;
}
.custom-class:hover{
  text-decoration: underline;
}
.navbar-wrapper img {
  float: left; /* Это выровняет изображение по левому краю */
  margin-right: 1px; /* Это добавит отступ справа от изображения */
  margin-left: 1px;
}


</style>
