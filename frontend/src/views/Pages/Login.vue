<template>
  <div>
    <!-- Header -->
    <div class="header py-5 py-lg-2 pt-lg-6" style="background-color: #f6f8fb">
<!--      <b-container>-->
<!--        <div class="header-body text-center mb-7">-->
<!--          <b-row class="justify-content-center">-->
<!--            <b-col xl="5" lg="6" md="8" class="px-5">-->
<!--              <h1 class="text-white">Welcome!</h1>-->
<!--              <p class="text-lead text-white">Use these awesome forms to login or create new account in your project for-->
<!--                free.</p>-->
<!--            </b-col>-->
<!--          </b-row>-->
<!--        </div>-->
<!--      </b-container>-->
<!--      <div class="separator separator-bottom separator-skew zindex-100">-->
<!--        <svg x="0" y="0" viewBox="0 0 2560 100" preserveAspectRatio="none" version="1.1"-->
<!--             xmlns="http://www.w3.org/2000/svg">-->
<!--          <polygon class="fill-default" points="2560 0 2560 100 0 100"></polygon>-->
<!--        </svg>-->
<!--      </div>-->
    </div>
    <!-- Page content -->
    <b-container class="mt--8 pb-5" style="padding-top: 15%">
      <b-row class="justify-content-center">
        <b-col lg="5" md="7">
          <b-card no-body class="bg-secondary border-0 mb-0">
            <b-card-body class="px-lg-5 py-lg-5">

              <validation-observer v-slot="{handleSubmit}" ref="formValidator">
                <div class="text-center mt-2 mb-3"><h2><strong>Добро пожаловать</strong></h2></div>
                <div class="text-center mt-2 mb-3">
                  <b-form-radio-group id="btnradios1" v-model="selected" :options="options" buttons button-variant="outline-secondary"></b-form-radio-group>
                </div>
                <b-form role="form">

                  <base-input v-if="selected === 'email'" alternative
                              class="mb-3"
                              name="Email"
                              :rules="{required: true, email: true}"
                              prepend-icon="ni ni-email-83"
                              placeholder="Email"
                              v-model="model.email">
                  </base-input>

                  <base-input v-else alternative
                              class="mb-3"
                              name="Phone"
                              :rules="{required: true, phone: true}"
                              prepend-icon="ni ni-mobile-button"
                              placeholder="Телефон"
                              v-model="model.phone">
                  </base-input>

                  <base-input alternative
                              class="mb-3"
                              name="Password"
                              :rules="{required: true, min: 6}"
                              prepend-icon="ni ni-lock-circle-open"
                              type="password"
                              placeholder="Password"
                              v-model="model.password">
                  </base-input>

                  <b-form-checkbox v-model="model.rememberMe">Запомнить меня</b-form-checkbox>
                  <div class="text-center">
                    <base-button @click="handleSubmit(tryLogin)" type="primary" class="my-4">Войти</base-button>
                  </div>
                </b-form>
              </validation-observer>
            </b-card-body>
          </b-card>
          <b-row class="mt-3">
            <b-col cols="6">
              <router-link to="/dashboard" class="text-light"><small>Забыли пароль?</small></router-link>
            </b-col>
            <b-col cols="6" class="text-right">
              <router-link to="/register" class="text-light"><small>Создать аккаунт</small></router-link>
            </b-col>
          </b-row>
        </b-col>
      </b-row>
    </b-container>
  </div>
</template>

<script>
import axios from "axios";
import {ApiAddress} from "@/common.ts";

export default {
  data() {
    return {
      model: {
        email: undefined,
        password: '',
        phone: '',
        rememberMe: false
      },
      selected: 'Email',
      options: [
        { text: 'Email', value: 'email' },
        { text: 'Телефон', value: 'phone' }
      ]
    }
  },
  methods: {
    async tryLogin() {
      axios.post(ApiAddress + "/login", this.model)
        .then(response => {
          localStorage.removeItem("accessToken");
          if (response.status === 200) {
            localStorage.setItem("accessToken", response.data.accessToken);
            window.location = window.location.protocol + "//" + window.location.host + "/#/dashboard";
          }
        }, error => {
          console.log(error);
        });
    }
  }
  // methods, computed properties, etc.
}
</script>
<style scoped>
.base-button {
  background-color: #4682B4; /* Синий */
  width: 100%; /* Размер как у поля ввода данных */
  border: none;
  color: white;
  padding: 10px 32px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin: 4px 2px;
  cursor: pointer;
}
</style>

