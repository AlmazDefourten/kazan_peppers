<template>
  <div class="h-100 w-100">


    <div class="pb-5 pt-5 pt-md-8 h-100" ref="tradingviewContainer">
    </div>

    <div>
      <b-dropdown id="dropdown-1" text="Заявка на покупку по определенной цене" class="m-md-2">
        <b-dropdown-item>Рубли</b-dropdown-item>
        <b-dropdown-item>Юани</b-dropdown-item>
        <b-dropdown-item>Дирхамы</b-dropdown-item>
      </b-dropdown>

      <div type="hbox">
        <!-- Using modifiers -->
        <b-button v-b-toggle.collapse-2 class="m-1">Toggle Collapse</b-button>

        <!-- Element to collapse -->
        <b-collapse v-for="(data, index) in accounts" :key="index" id="collapse-2">
          <b-card v-text="data.name"></b-card>
        </b-collapse>
        <b-dropdown id="dropdown-1" text="+" class="m-md-2">
          <b-dropdown-item v-b-modal.modal-1>Рубли</b-dropdown-item>
          <b-dropdown-item v-b-modal.modal-2>Юани</b-dropdown-item>
          <b-dropdown-item v-b-modal.modal-3>Дирхамы</b-dropdown-item>
        </b-dropdown>
        <b-modal :hide-footer="true" id="modal-1" title="Открытие нового счета в рублях">
          <template slot="default">
            <new-account :currency-type="1"> </new-account>
          </template>
        </b-modal>
        <b-modal :hide-footer="true" id="modal-2" title="Открытие нового счета в юанях">
          <template slot="default">
            <new-account :currency-type="2"> </new-account>
          </template>
        </b-modal>
        <b-modal :hide-footer="true" id="modal-3" title="Открытие нового счета в дирхамах">
          <template slot="default">
            <new-account :currency-type="3"> </new-account>
          </template>
        </b-modal>
      </div>
    </div>
  </div>
</template>
<script>
  // Charts
  import * as chartConfigs from '@/components/Charts/config';

  // Components
  import BaseProgress from '@/components/BaseProgress';
  import StatsCard from '@/components/Cards/StatsCard';

  // Tables
  import SocialTrafficTable from './Dashboard/SocialTrafficTable';
  import PageVisitsTable from './Dashboard/PageVisitsTable';
  import axios from "axios";
  import NewAccount from "./NewAccount.vue";
  import {reactive} from "vue";

  export default {
    components: {
      NewAccount
    },
    data() {
      return {
        accounts: [0, 1]
      };
    },
    methods: {
      initBigChart(index) {
        let chartData = {
          datasets: [
            {
              label: 'Performance',
              data: this.bigLineChart.allData[index]
            }
          ],
          labels: ['May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        };
        this.bigLineChart.chartData = chartData;
        this.bigLineChart.activeIndex = index;
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
</style>
