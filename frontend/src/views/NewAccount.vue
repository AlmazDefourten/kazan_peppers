<script>

import axios from "axios";
import {ApiAddress} from "@/common.ts";

export default {
  data() {
    return {
      model: {
        currencyType: 0,
        Sum: 0,
        Name: ""
      }
    }
  },
  methods: {
    async createAccount() {
      axios.post(ApiAddress + "/account/create", this.model)
        .then(response => {
          this.$emit('close');
          this.$notify({type: "success", icon: "mdi mdi-check-bold", verticalAlign: 'top', horizontalAlign: 'right', message: 'Счет успешно создан'});
        }, error => {
          this.$notify({type: "danger", icon: "mdi mdi-remove", verticalAlign: 'top', horizontalAlign: 'right', message: 'Не удалось создать счет'});
          console.log(error);
        });
    },
  },
  name: "new-account",

};

</script>

<template>
  <div>
    <label for="currency">Выберите валюту для покупки:</label>
    <b-dropdown id="dropdown-1" class="m-md-2">
      <template #button-content> {{ selectedCurrency || '-----' }} </template>
      <b-dropdown-item @click="() =>{model.currencyType = 1; selectedCurrency='Рубли';}">Рубли (₽)</b-dropdown-item>
      <b-dropdown-item @click="() =>{model.currencyType = 2; selectedCurrency='Юани';}">Юани (¥)</b-dropdown-item>
      <b-dropdown-item @click="() =>{model.currencyType = 3; selectedCurrency='Дирхам';}">>Дирхам (د.إ)</b-dropdown-item>
    </b-dropdown>
  <base-input
    type="text"
    label="Наименование счета"
    v-model="model.Name"
  >
  </base-input>
  <base-button @click="createAccount" size="sm" type="neutral">Создать новый счет</base-button>
  </div>
</template>

<style scoped>

</style>
