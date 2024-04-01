<template>
  <div class="h-100 w-100 d-flex">
    <div class="pb-5 pt-5 pt-md-8 w-75" ref="tradingviewContainer"></div>
    <div class="pb-5 pt-5 pt-md-8">
<!--      ЗАЯВКА-->
      <b-button id="create-order-button" class="m-md-2" v-b-modal.modal-1> Заявка </b-button>
      <b-modal :hide-footer="true" id="modal-1" title="Создание заявки">
        <template slot="default">
          <label for="accountFrom"class="col-md-12 col-form-label form-control-label">Выберите счет для покупки валюты:</label>
          <b-button v-b-toggle.collapse-2-modal class="m-1 w-75 my-button" v-if="accounts.length > 0" v-model="model.selectedAccountFrom"> Счёт:
          {{ model.selectedAccountTo.name }}, {{ getCurrencyTypeString(model.selectedAccountTo.currencyType) }}, {{ model.selectedAccountTo.sum }}
          </b-button>
          <b-collapse v-for="(data, index) in accounts" :key="index" id="collapse-2-modal">
            <stats-card :title="data.name"
                        type="gradient-info"
                        :sub-title="data.sum + ' ' + getCurrencyTypeString(data.currencyType)"
                        class="mb-4 w-25"
                        style="min-width: 300px"
            >
              <template slot="icon">
                <base-button size="sm" style="background-color: darkblue" icon class="w-100 h-100" @click="() => {model.selectedAccountTo=data;}">
                  <i class="mdi mdi-check"></i>
                </base-button>
              </template>
            </stats-card>
          </b-collapse>

          <label for="accountFrom"class="col-md-12 col-form-label form-control-label">Выберите счет для оплаты:</label>
          <b-button v-b-toggle.collapse-3-modal class="m-1 w-75 my-button" v-if="accounts.length > 0" v-model="model.selectedAccountTo"> Счёт:
            {{ model.selectedAccountFrom.name }}, {{ getCurrencyTypeString(model.selectedAccountFrom.currencyType) }}, {{ model.selectedAccountFrom.sum }}
          </b-button>

          <b-collapse v-for="(data, index) in accounts" :key="index" id="collapse-3-modal">
            <stats-card :title="data.name"
                        type="gradient-info"
                        :sub-title="data.sum + ' ' + getCurrencyTypeString(data.currencyType)"
                        class="mb-4 w-25"
                        style="min-width: 300px"
            >
              <template slot="icon">
                <base-button size="sm" style="background-color: darkblue" icon class="w-100 h-100" @click="model.selectedAccountFrom=data">
                  <i class="mdi mdi-check"></i>
                </base-button>
              </template>
            </stats-card>
          </b-collapse>

          <label for="amount" class="col-md-12 col-form-label form-control-label">Введите сумму, которую хотите получить:</label>
          <b-form-group>
            <b-input-group prepend="¤">
              <b-form-input v-model="model.amountToBuy"></b-form-input>
            </b-input-group>
          </b-form-group>

          <label for="amount" class="col-md-12 col-form-label form-control-label">Введите сумму, за которую хотите получить 1 еденицу валюты:</label>
          <b-form-group>
            <b-input-group>
              <b-form-input v-model="model.costToBy"></b-form-input>
            </b-input-group>
          </b-form-group>

<!--          <label for="date">Выберите дату и вроемя окончания действия заявки:</label>-->
          <label for="date" class="col-md-12 col-form-label form-control-label">Выберите дату и время окончания действия заявки:</label>
          <b-row class="form-group">
            <b-col md="6">
              <base-input type="date" id="date" v-model="model.selectedDate"/>
            </b-col>
            <b-col md="6" class="text-right">
              <base-input type="time" id="time" v-model="model.selectedTime"/>
            </b-col>
          </b-row>

          <base-button @click="createRequest" size="xl" type="neutral">Создать заявку</base-button>

        </template>
      </b-modal>


<!--      ОТОБРАЖЕНИЕ СЧЕТОВ-->
      <div type="hbox">
        <!-- Если у пользователя есть счета, отображаем первый счет -->
        <b-button v-b-toggle.collapse-1 class="m-2 my-button" v-if="accounts.length > 0">
          {{ accounts[0].name }}, {{ getCurrencyTypeString(accounts[0].currencyType) }}, {{ accounts[0].sum }}
        </b-button>

        <!-- Если у пользователя нет счетов, отображаем "Toggle Collapse" -->
        <b-button v-b-toggle.collapse-2 class="m-1 my-button" v-else>
          Toggle Collapse
        </b-button>

        <!-- Element to collapse -->
        <b-collapse v-for="(data, index) in accounts" :key="index" id="collapse-1">
          <stats-card :title="data.name"
                      type="gradient-info"
                      :sub-title="data.sum + ' ' + getCurrencyTypeString(data.currencyType)"
                      class="mb-4 w-25"
                      style="min-width: 300px"
          >
            <template slot="icon">
              <base-button size="sm" style="background-color: darkblue" icon class="w-100 h-100">
                <i class="mdi mdi-swap-horizontal"></i>
              </base-button>
            </template>
          </stats-card>
        </b-collapse>

        <b-button id="create-account-bottom" class="m-md-2" v-b-modal.modal-2> + </b-button>
        <b-modal ref="accountModal" :hide-footer="true" id="modal-2" title="Открытие нового счета">
          <template slot="default">
            <new-account @close="() => { this.$refs.accountModal.hide();  loadAccounts()}"> </new-account>
          </template>
        </b-modal>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from "axios";
  import NewAccount from "./NewAccount.vue";
  import '@mdi/font/css/materialdesignicons.css';
  import {ApiAddress} from "@/common.ts";

  export default {
    components: {
      NewAccount
    },
    data() {
      return {
        model:{
          selectedAccountTo: 0,
          selectedAccountFrom: 0,
          amountToBuy: 0,
          costToBy: 0,
          selectedDate: null,
          selectedTime: null
        },
        accounts: [0, 1],
      };
    },
    methods: {
      async createRequest() {
        const modelToSend = {
          accountTo: { id: this.model.selectedAccountTo.id },
          accountFrom: { id: this.model.selectedAccountFrom.id },
          amountToBuy: this.model.amountToBuy,
          costToBy: this.model.costToBy,
          expirationTime: this.model.selectedDate + "T" + this.model.selectedTime + "Z",
        }
        axios.post(ApiAddress + "/request/create", modelToSend)
          .then(response => {
            this.$notify({type: "success", icon: "mdi mdi-check-bold", verticalAlign: 'top', horizontalAlign: 'right', message: 'Счет успешно создан'});
            this.loadAccounts();
          }, error => {
            this.$notify({type: "danger", icon: "mdi mdi-remove", verticalAlign: 'top', horizontalAlign: 'right', message: 'Не удалось создать акаунт'});
            console.log(error);
          });
      },
      async loadAccounts() {
        await axios.get(ApiAddress + "/account/list")
          .then(response => {
            this.accounts = response.data;
            console.log(response.data);
          }, error => {
            console.log(error);
          });
      },
      getCurrencyTypeString(currencyTypeInput) {
        switch (currencyTypeInput) {
          case(1):
            return "RUB ₽";
          case(2):
            return "CNY ¥";
          case(3):
            return "AED د.إ"
        }
      }
    },
    async mounted() {
      const script = document.createElement('script');
      script.src = 'https://s3.tradingview.com/external-embedding/embed-widget-advanced-chart.js';
      script.async = true;
      script.text = JSON.stringify({
        "autosize": true,
        "symbol": "FX_IDC:CNYRUB",
        "interval": "D",
        "timezone": "Europe/Moscow",
        "theme": "light",
        "style": "1",
        "locale": "ru",
        "height": 500,
        "hide_side_toolbar": false,
        "enable_publishing": false,
        "allow_symbol_change": true,
        "calendar": false,
        "support_host": "https://www.tradingview.com"
      });
      this.$refs.tradingviewContainer.appendChild(script);
    },
    async beforeMount() {
      await axios.get(ApiAddress + "/account/list")
        .then(response => {
          this.accounts = response.data;
          console.log(response.data);
        }, error => {
          console.log(error);
        });
      this.$forceUpdate();
    }
  };
</script>
<style>
.el-table .cell{
  padding-left: 0px;
  padding-right: 0px;
}
.my-button {
  color: black;
  background-color: #87CEEB;
  border-color: #87CEEB;
  max-width: 350px;
  height: 50px; /* Высота кнопки */
}

</style>
