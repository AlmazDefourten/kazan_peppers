<template>
  <router-view></router-view>
</template>

<script>

import axios from "axios";
import Notifications from "@/components/NotificationPlugin/Notifications.vue";

axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*';

 axios.interceptors.request.use(function (config) {
   console.log(localStorage.getItem("accessToken"));
   config.headers.Authorization = "Bearer " + localStorage.getItem("accessToken");

   return config;
 });

 axios.interceptors.response.use((response) => {
   return response;
 }, (error) => {
   console.log(error);
   if (error.message == "Network Error") {
     window.location = window.location.protocol + "" + window.location.host + "/#/login"
   } else {
     return Promise.reject(error);
   }
 });
export default {
  components: {Notifications}
}
</script>
