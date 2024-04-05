/*!

=========================================================
* BootstrapVue Argon Dashboard - v1.0.0
=========================================================

* Product Page: https://www.creative-tim.com/product/bootstrap-vue-argon-dashboard
* Copyright 2020 Creative Tim (https://www.creative-tim.com)

* Coded by www.creative-tim.com

=========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

*/
import Vue from 'vue';
import DashboardPlugin from './plugins/dashboard-plugin';
import App from './App.vue';

// router setup
import router from './routes/router';
import { extend } from 'vee-validate';
import {required, min, confirmed, regex, email} from "vee-validate/dist/rules";
// plugin setup
Vue.use(DashboardPlugin);

extend('verify_password', {
  message: field => `Пароль должен содержать: 1 большую букву, 1 маленькую букву, 1 цифру, и один специальной символ (.)`,
  validate: value => {
    var strongRegex = new RegExp("^(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\|,.<>\\/?])(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$");
    return strongRegex.test(value);
  }
});

extend('required', {
  ...required,
  message: 'Это поле обязательно для заполнения'
});

extend('min', {
  ...min,
  message: 'Минимальная длина поля должна быть не менее {length} символов'
});

extend('passconfirmed', {
  ...confirmed,
  message: 'Пароли не совпадают'
});

extend('email', {
  ...email,
  message: 'Неверный формат почты'
});

extend('phone', {
  ...regex,
  validate: value => {
    return /^(\+7|7|8)?[\s]?\(?[489][0-9]{2}\)?[\s]?[0-9]{3}[\s]?[0-9]{2}[\s]?[0-9]{2}$/.test(value);
  },
  message: 'Неверный формат телефонного номера'
});

/* eslint-disable no-new */
new Vue({
  el: '#app',
  render: h => h(App),
  router
});
