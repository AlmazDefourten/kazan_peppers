<template>
  <router-view></router-view>
</template>

<script>

import axios from "axios";

axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
export default {
}
</script>
