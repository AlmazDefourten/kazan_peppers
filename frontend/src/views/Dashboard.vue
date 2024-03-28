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
      <div>
        <!-- Using modifiers -->
        <b-button v-b-toggle.collapse-2 class="m-1">Toggle Collapse</b-button>

        <!-- Element to collapse -->
        <b-collapse id="collapse-2">
          <b-card>  Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident.</b-card>
        </b-collapse>
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

  export default {
    components: {
    },
    data() {
      return {
        accounts: [],
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

      this.accounts = await axios.post("http://107.173.25.219:81/account/list", this.model)
        .then(response => {
          localStorage.setItem("accessToken", response.data.accessToken);
          window.location = window.location.protocol + "//" + window.location.host;
        }, error => {
          console.log(error);
        });
    },
  };
</script>
<style>
.el-table .cell{
  padding-left: 0px;
  padding-right: 0px;
}
</style>
