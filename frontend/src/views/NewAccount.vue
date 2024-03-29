<script>

import axios from "axios";

export default {
  data() {
    return {
      model: {
        currencyType: this.currencyType,
        Sum: 0,
        Name: ""
      }
    }
  },
  methods: {
    async createAccount() {
      axios.post("http://107.173.25.219:81/account/create", this.model)
        .then(response => {
        }, error => {
          console.log(error);
        });
    },
  },
  name: "new-account",
  props: {
    currencyType: {
      type: Number,
      default: 0,
      description: 'Breadcrumb background type'
    }
  },
  mounted() {
    this.model.currencyType = this.currencyType;
  }
};

</script>

<template>
  <div>
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
