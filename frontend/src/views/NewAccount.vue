<script>

import axios from "axios";

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
      axios.post("http://107.173.25.219:81/account/create", this.model)
        .then(response => {
          this.$emit('close');
        }, error => {
          console.log(error);
        });
    },
  },
  name: "new-account",

};

</script>

<template>
  <div>
    <b-dropdown id="dropdown-1" class="m-md-2">
      <template #button-content> {{ selectedCurrency || '-----' }} </template>
      <b-dropdown-item @click="() =>{model.currencyType= 1; selectedCurrency='Рубли';}">Рубли (₽)</b-dropdown-item>
      <b-dropdown-item @click="() =>{model.currencyType= 1; selectedCurrency='Юани';}">Юани (¥)</b-dropdown-item>
      <b-dropdown-item @click="() =>{model.currencyType= 1; selectedCurrency='Дирхам';}">>Дирхам (د.إ)</b-dropdown-item>
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
