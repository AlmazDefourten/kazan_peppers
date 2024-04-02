<template>
  <footer class="footer px-4" style="background-color: white">
    <b-row align-v="center" class="justify-content-lg-between">
      <b-col lg="6">
        <div class="copyright text-center text-lg-left text-muted">
          © {{year}} <a class="font-weight-bold ml-1" target="_blank">Казанские Перчики, Лига Приключений</a>
        </div>
      </b-col>
    </b-row>
  </footer>
</template>
<script>
export default {
  data() {
    return {
      year: new Date().getFullYear()
    };
  }
};
</script>
<style></style>
