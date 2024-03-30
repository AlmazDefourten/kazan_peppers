<template>
  <div class="h-100 w-100">
    <div class="pb-5 pt-5 pt-md-8 h-100" ref="tradingviewContainer"></div>
    <div>
<!--      ЗАЯВКА-->
      <b-button id="create-order-button" class="m-md-2" v-b-modal.modal-1> Заявка </b-button>
      <b-modal :hide-footer="true" id="modal-1" title="Создание заявки">
        <template slot="default">
          <label for="accountTo">Выберите счет для покупки валюты:</label>
          <b-button v-b-toggle.collapse-2 class="m-2 my-button" v-if="accounts.length > 0">
            {{ accounts[0].name }}, {{ getCurrencyTypeString(accounts[0].currencyType) }}, {{ accounts[0].sum }}
          </b-button>

          <label for="accountFrom">Выберите счет для оплаты:</label>
          <b-button v-b-toggle.collapse-2 class="m-2 my-button" v-if="accounts.length > 0">
            {{ accounts[0].name }}, {{ getCurrencyTypeString(accounts[0].currencyType) }}, {{ accounts[0].sum }}
          </b-button>

          <label for="currency">Выберите валюту для покупки:</label>
          <b-dropdown id="dropdown-1" class="m-md-2">
            <template #button-content> {{ selectedCurrency || '-----' }} </template>
            <b-dropdown-item @click="selectedCurrency = 'Рубли'">Рубли (₽)</b-dropdown-item>
            <b-dropdown-item @click="selectedCurrency = 'Юани'">Юани (¥)</b-dropdown-item>
            <b-dropdown-item @click="selectedCurrency = 'Дирхам'">Дирхам (د.إ)</b-dropdown-item>
          </b-dropdown>


          <label for="amount">Введите сумму, которую хотите получить:</label>
          <b-form-group>
            <b-input-group prepend="¤">
              <b-form-input></b-form-input>
            </b-input-group>
          </b-form-group>

          <label for="date">Выберите дату окончания действия заявки:</label>
          <input type="date" id="date" v-model="selectedDate">
          <br>
          <b-button>Отправить</b-button>
        </template>
      </b-modal>


<!--      ОТОБРАЖЕНИЕ СЧЕТОВ-->
      <div type="hbox">
        <!-- Если у пользователя есть счета, отображаем первый счет -->
        <b-button v-b-toggle.collapse-2 class="m-2 my-button" v-if="accounts.length > 0">
          {{ accounts[0].name }}, {{ getCurrencyTypeString(accounts[0].currencyType) }}, {{ accounts[0].sum }}
        </b-button>

        <!-- Если у пользователя нет счетов, отображаем "Toggle Collapse" -->
        <b-button v-b-toggle.collapse-2 class="m-1 my-button" v-else>
          Toggle Collapse
        </b-button>

        <!-- Element to collapse -->
        <b-collapse v-for="(data, index) in accounts.sort((a, b) => a.id - b.id)" :key="index" id="collapse-2">
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
        <b-dropdown id="dropdown-1" text="+" class="m-md-2">
          <b-dropdown-item v-b-modal.modal-1>Рубли</b-dropdown-item>
          <b-dropdown-item v-b-modal.modal-2>Юани</b-dropdown-item>
          <b-dropdown-item v-b-modal.modal-3>Дирхамы</b-dropdown-item>
        </b-dropdown>
        <b-modal ref="modal1" :hide-footer="true" id="modal-1" title="Открытие нового счета в рублях">

<!--        Add-->
        <b-button id="create-account-bottom" class="m-md-2" v-b-modal.modal-2> + </b-button>
        <b-modal :hide-footer="true" id="modal-2" title="Открытие нового счета">
          <template slot="default">
            <new-account @close="() => { this.$refs.modal1.hide(); }" :currency-type="1"> </new-account>
            <label for="currency">Выберите валюту:</label>
            <select id="currency" v-model="selectedCurrency">
              <option value="1">Рубли</option>
              <option value="2">Юани</option>
              <option value="3">Дирхамы</option>
            </select>
            <new-account :currency-type="selectedCurrency"> </new-account>
          </template>
        </b-modal>
        <b-modal ref="modal2" :hide-footer="true" id="modal-2" title="Открытие нового счета в юанях">
          <template slot="default">
            <new-account @close="() => { this.$refs.modal2.hide(); }" :currency-type="2"> </new-account>
          </template>
        </b-modal>
        <b-modal ref="modal3" :hide-footer="true" id="modal-3" title="Открытие нового счета в дирхамах">
          <template slot="default">
            <new-account @close="() => { this.$refs.modal3.hide(); }" :currency-type="3"> </new-account>
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

  export default {
    components: {
      NewAccount
    },
    data() {
      return {
        accounts: [0, 1],
      };
    },
    methods: {
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
      await axios.get("http://107.173.25.219:81/account/list")
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
