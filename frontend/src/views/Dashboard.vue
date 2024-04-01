<template>
<div>
  <link href="/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
  <b-modal ref="listRequest" :hide-footer="true" id="modal-5" title="Список заявок">
    <div role="tablist" class="accordion">
      <b-card v-for="(data, index) in requestList" :key="index" no-body class="mb-1">
        <b-card-header header-tag="header" role="tab" :v-b-toggle="'accordion-' + index" class="d-flex justify-content-between">
          <h5 class="mb-0">Заявка №{{ data.id }}</h5>
          <base-button id="inter-account-transfer" v-b-modal.modal-5 style="background-color: gray; width: 40px; display: flex; justify-content: center; align-items: center;">
            <i class="fas fa-times"></i>
          </base-button>
        </b-card-header>
        <b-collapse :id="'accordion-' + index" visible accordion="my-accordion" role="tabpanel">
          <b-card-body>
            <b-card-text>
              На сумму: {{ data.amountToBuy }}<br>
              Цена: {{ data.costToBy }}<br>
              Счета для оплаты {{ data.accountFrom }}, Счет пополнения {{ data.accountTo }}
            </b-card-text>
          </b-card-body>
        </b-collapse>
      </b-card>
    </div>
  </b-modal>

  <div class="h-100 w-100 d-flex">
    <b-modal ref="transferModal" :hide-footer="true" id="modal-4" title="Перевод между счетами" >
      <template slot="default">
        <label for="accountFrom"class="col-md-12 col-form-label form-control-label">С какого счета:</label>
        <b-button v-b-toggle.collapse-2-modal class="m-1 my-button" v-if="accounts.length > 0" v-model="transferModel.selectedFrom" style="width: 295px"> Счёт:
          {{ transferModel.selectedFrom.name }}, {{ getCurrencyTypeString(transferModel.selectedFrom.currencyType) }}, {{ transferModel.selectedFrom.sum }}
        </b-button>
        <b-collapse v-for="(data, index) in accounts" :key="index" id="collapse-2-modal">
          <stats-card :title="data.name"
                      type="gradient-info"
                      :sub-title="data.sum + ' ' + getCurrencyTypeString(data.currencyType)"
                      class="mb-4 w-25"
                      style="min-width: 300px"
          >
            <template slot="icon">
              <base-button size="sm" style="background-color: #003791" icon class="w-100 h-100" @click="() => {transferModel.selectedFrom=data;}">
                <i class="mdi mdi-check"></i>
              </base-button>
            </template>
          </stats-card>
        </b-collapse>

        <label for="accountFrom"class="col-md-12 col-form-label form-control-label">На какой счет:</label>
        <b-button v-b-toggle.collapse-3-modal class="m-1 my-button" v-if="accounts.length > 0" v-model="transferModel.selectedTo" style="width: 295px"> Счёт:
          {{ transferModel.selectedTo.name }}, {{ getCurrencyTypeString(transferModel.selectedTo.currencyType) }}, {{ transferModel.selectedTo.sum }}
        </b-button>

        <b-collapse v-for="(data, index) in accounts" :key="index" id="collapse-3-modal">
          <stats-card :title="data.name"
                      type="gradient-info"
                      :sub-title="data.sum + ' ' + getCurrencyTypeString(data.currencyType)"
                      class="mb-4 w-25"
                      style="min-width: 300px"
          >
            <template slot="icon">
              <base-button size="sm" style="background-color: darkblue" icon class="w-100 h-100" @click="transferModel.selectedTo=data">
                <i class="mdi mdi-check"></i>
              </base-button>
            </template>
          </stats-card>
        </b-collapse>

        <label for="amount" class="col-md-12 col-form-label form-control-label">Введите сумму, которую хотите перевести:</label>
        <base-input alternative v-model="transferModel.sum"></base-input>
        <label for="amount" class="col-md-12 col-form-label form-control-label pb-1">Актуальный курс валют:</label>
        <div class="d-flex pt-2 justify-content-between pb-3">
          <stats-card title="Юань"
                      type="mdi"
                      :sub-title="yuanCurrency"
          >

          </stats-card>

          <stats-card title="Доллар"
                      type="mdi"
                      sub-title="1000"
          >
          </stats-card>


          <stats-card title="Дирхам"
                      type="gradient-orange"
                      :sub-title="dirhamCurrency"
          >


          </stats-card>

        </div>


        <base-button @click="interAccountTransfer" size="xl" type="neutral">Перевести деньги</base-button>
      </template>
    </b-modal>
    <!--    КУРС ВАЛЮТ-->
    <div class="pb-2 pt-5 pt-md-8 w-75" ref="tradingviewContainer">
      <div class="d-flex pt-2 justify-content-around">
        <stats-card title="Юань"
                    type="mdi"
                    :sub-title="yuanCurrency"
                    >
          <template slot="footer">
            <span class="font-weight-bold">Курс обновляется 1 раз в сутки!</span>
          </template>
          <template slot="icon" >
            <img width="35" src="../../public/img/currencies/dirham.png" alt="">
          </template>
        </stats-card>

        <stats-card title="Доллар США"
                    type="mdi"
                    sub-title="1000"
        >
          <template slot="footer">
            <span class="font-weight-bold">Курс обновляется 1 раз в сутки!</span>
          </template>
          <template slot="icon" >
            <img width="30" src="../../public/img/currencies/DOLLAR.png" alt="">
          </template>
        </stats-card>

        <stats-card title="Дирхам"
                    type="gradient-orange"
                    :sub-title="dirhamCurrency"
                    icon="ni ni-chart-pie-35">
          <template slot="footer">
            <span class="font-weight-bold">Курс обновляется 1 раз в сутки!</span>
          </template>
          <template slot="icon" >
            <img width="30" src="../../public/img/currencies/YUAN.png" alt="">
          </template>
        </stats-card>

      </div>
    </div>
    <!--      ЗАЯВКА-->
    <div class="pb-2 pt-5 pt-md-8 pr-0">
      <b-button id="create-order-button" class="m-md-2" style="width: 150px; border-color: #1fa2ff" v-b-modal.modal-1> Заявка </b-button>
      <b-button @click="loadRequestList" id="listRequest" class="m-md-2" v-b-modal.modal-5  style="width: 150px; border-color: #1fa2ff">Список заявок</b-button>
      <b-modal :hide-footer="true" id="modal-1" title="Создание заявки">
        <template slot="default">
          <label for="accountFrom"class="col-md-12 col-form-label form-control-label">На:</label>
          <b-button v-b-toggle.collapse-2-modal class="m-1 my-button" v-if="accounts.length > 0" v-model="model.selectedAccountFrom" style="width: 295px"> Счёт:
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
          <b-button v-b-toggle.collapse-3-modal class="m-1 my-button" v-if="accounts.length > 0" v-model="model.selectedAccountTo" style="width: 295px"> Счёт:
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
          <base-input alternative v-model="model.amountToBuy"></base-input>


          <label for="amount" class="col-md-12 col-form-label form-control-label">Введите сумму, за которую хотите получить 1 еденицу валюты:</label>
          <base-input alternative v-model="model.costToBy"></base-input>

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
        <b-button v-b-toggle.collapse-1 class="m-2 my-button" v-if="accounts.length > 0" style="width: 250px">
          {{ accounts[0].name }}, {{ getCurrencyTypeString(accounts[0].currencyType) }}, {{ accounts[0].sum }}
        </b-button>

        <!-- Если у пользователя нет счетов, отображаем "Toggle Collapse" -->
        <b-button v-b-toggle.collapse-2 class="m-1 my-button" v-else>
          Toggle Collapse
        </b-button>
        <b-button id="create-account-bottom" class="m-2 my-button" v-b-modal.modal-2 > + </b-button>

        <!-- Element to collapse -->
        <b-collapse v-for="(data, index) in accounts" :key="index" id="collapse-1" style="padding-left: 8px; padding-bottom: -1px">
          <stats-card :title="data.name"
                      type="gradient-info"
                      :sub-title="data.sum + ' ' + getCurrencyTypeString(data.currencyType)"
                      class="mb-4 w-25"
                      style="min-width: 250px;"
          >
            <template slot="icon">
              <base-button id="inter-account-transfer" v-b-modal.modal-4 size="sm" style="background-color: #003791" icon class="w-100 h-100">
                <i class="mdi mdi-swap-horizontal"></i>
              </base-button>
            </template>
          </stats-card>
        </b-collapse>


        <b-modal ref="accountModal" :hide-footer="true" id="modal-2" title="Открытие нового счета" >
          <template slot="default">
            <new-account @close="() => { this.$refs.accountModal.hide();  loadAccounts()}"> </new-account>
          </template>
        </b-modal>
      </div>
    </div>
<!--    Список заявок-->
<!--    <div class="pb-2 pt-5 pt-md-8">-->
<!--&lt;!&ndash;      <b-button id="create-order-button" class="m-md-2" style="width: 259px; border-color: #1fa2ff" v-b-modal.modal-1> Заявка </b-button>&ndash;&gt;-->
<!--      <b-button @click="loadRequestList" id="listRequest" class="m-md-2" v-b-modal.modal-5  style="width: 150px; border-color: #1fa2ff">Список заявок</b-button>-->
<!--      <div class="d-flex pt-2 justify-content-start">-->
<!--      </div>-->
<!--    </div>-->
  </div>
  </div>
</template>

<script>
  import axios from "axios";
  import NewAccount from "./NewAccount.vue";
  import '@mdi/font/css/materialdesignicons.css';
  import {ApiAddress} from "@/common.ts";

  export default {
    name: "my-cool-component",
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
          selectedTime: null,
        },
        transferModel:{
          selectedFrom: 0,
          selectedTo: 0,
          sum: 0,
        },
        accounts: [0, 1],
        yuanCurrency: 0,
        dirhamCurrency: 0,
        requestList: []
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

      async interAccountTransfer(){
        const modelToTransfer ={
          idTo: this.transferModel.selectedTo.id,
          idFrom: this.transferModel.selectedFrom.id,
          sum: this.transferModel.sum
        }
        axios.post(ApiAddress + "/account/transfer", {idTo: modelToTransfer.idTo, idFrom: modelToTransfer.idFrom, sum: modelToTransfer.sum })
          .then(response => {

          }, error => {

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
      async loadRequestList() {
        await axios.get(ApiAddress + "/request/list")
          .then(response => {
            this.requestList = response.data;
            console.log(this.requestList[0]);
          }, error => {
            console.log(error);
          });
      },
      getCurrencyTypeString(currencyTypeInput) {
        switch (currencyTypeInput) {
          case(1):
            return "₽";
          case(2):
            return "¥";
          case(3):
            return "د.إ"
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
      this.$refs.tradingviewContainer.insertBefore(script, this.$refs.tradingviewContainer.firstChild);
    },
    async beforeMount() {
      await axios.get(ApiAddress + "/account/list")
        .then(response => {
          this.accounts = response.data;
          console.log(response.data);
        }, error => {
          console.log(error);
        });
      await axios.get(ApiAddress + "/currency/get")
        .then(response => {
          const arr = response.data.split(" ");
          this.yuanCurrency = parseFloat(arr[0]).toFixed(2);
          this.dirhamCurrency = parseFloat(arr[1]).toFixed(2);
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
  color: white;
  background-color: #003791;
  border-color: black;
  max-width: 350px;
  height: 50px; /* Высота кнопки */
  transition: background-color 0.4s, color 0.4s;
}
.my-button:hover {
  color: black;
  background-color: white;
  box-shadow: inset 0 0 0 3px #3a7999;
}

</style>
