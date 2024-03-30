<template>
  <div>
    <!-- Header -->
    <div class="header py-5 py-lg-2 pt-lg-6" style="background-color: #f6f8fb">
<!--      <b-container class="container">-->
<!--        <div class="header-body text-center mb-7">-->
<!--          <b-row class="justify-content-center">-->
<!--            <b-col xl="5" lg="6" md="8" class="px-5">-->
<!--              <h1 class="text-white">Create an account</h1>-->
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
      <!-- Table -->
      <b-row class="justify-content-center">
        <b-col lg="6" md="8" >
          <b-card no-body class="bg-secondary border-0">
            <b-card-body class="px-lg-5 py-lg-5">
              <div class="text-center text-muted mb-4">
                <small>Введите все обязательные поля</small>
              </div>
              <validation-observer v-slot="{handleSubmit}" ref="formValidator">
                <b-form role="form">
                  <base-input alternative
                              class="mb-3"
                              prepend-icon="ni ni-email-83"
                              placeholder="Email"
                              name="Email"
                              :rules="{required: true, email: true}"
                              v-model="model.email">
                  </base-input>
                  <base-input alternative
                              class="mb-3"
                              prepend-icon="ni ni-mobile-button"
                              placeholder="Телефон"
                              name="Phone"
                              :rules="{required: true, phone: true}"
                              v-model="model.phone">
                  </base-input>
                  <base-input alternative
                              class="mb-3"
                              prepend-icon="ni ni-lock-circle-open"
                              placeholder="Пароль"
                              type="password"
                              name="Password"
                              :rules="{required: true, min: 6 }"
                              v-model="model.password">
                  </base-input>
                  <base-input alternative
                              class="mb-3"
                              prepend-icon="ni ni-check-bold"
                              placeholder="Подтверждение пароля"
                              type="password"
                              name="Confirm Password"
                              :rules="{required: true, min: 6, passconfirmed: model.password !== model.confirmPassword }"
                              v-model="model.confirmPassword">
                  </base-input>
                  <b-row class=" my-4">
                    <b-col cols="12">
                      <base-input :rules="{ required: { allowFalse: false } }" name=Privacy Policy>
                        <b-form-checkbox v-model="model.agree">
                          <span class="text-muted">Я согласен с <a href="#!">Политика конфиденциальности</a></span>
                        </b-form-checkbox>
                      </base-input>
                    </b-col>
                  </b-row>
                    <base-button type="submit" @click="handleSubmit(onSubmit)" variant="primary" class="mt-4">Создать аккаунт</base-button>
                </b-form>
              </validation-observer>
            </b-card-body>
          </b-card>
        </b-col>
      </b-row>
    </b-container>
  </div>
</template>
<script>
  import axios from "axios";
  import {ApiAddress} from "@/common.ts";

  export default {
    name: 'register',
    data() {
      return {
        model: {
          name: '',
          email: '',
          password: '',
          confirmPassword: '',
          agree: false
        }
      }
    },
    methods: {
      onSubmit() {
        axios.post(ApiAddress + "/register", this.model)
          .then(response => {
            localStorage.removeItem("accessToken");
            window.location = window.location.protocol + "" + window.location.host + "/#/login"
            this.$notify({type: "success", icon: "mdi mdi-check-bold", verticalAlign: 'top', horizontalAlign: 'right', message: 'Вы успешно зарегестрировлись'});
            console.log(response.data);
          }, error => {
            this.$notify({type: "danger", icon: "mdi mdi-remove", verticalAlign: 'top', horizontalAlign: 'right', message: 'Не удалось создать аккаунт'});
            console.log(error);
          });
      }
    }
  };
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
