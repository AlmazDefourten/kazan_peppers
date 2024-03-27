<template>
  <b-card no-body class="card-profile" alt="Image placeholder" img-top style="margin-top: 150px">
    <b-row class="justify-content-center">
      <b-col lg="3" class="order-lg-2">
        <div class="card-profile-image">
          <a href="#">
            <b-img src="img/theme/team-4.jpg" rounded="circle" />
          </a>
        </div>
      </b-col>
    </b-row>
    <b-card-body class="pt-9" style="background-color: #f6f8fb">
      <div class="text-center">
<!--        калькулятор для подсчета возраста-->
        <h5 class="h3"> Ваше имя <span class="font-weight-light">, Возраст</span>
        </h5>
        <div class="h5 font-weight-300">
          <i class="ni location_pin mr-2"></i>ГОРОД
        </div>
        <div class="h5 mt-4">
          <i class="ni business_briefcase-24 mr-2"></i>Почтовый адрес
        </div>
        <div>
          <i class="ni education_hat mr-2"></i>Номер телефона
        </div>
      </div>
    </b-card-body>
  </b-card>
</template>
<script>
export default {};
</script>
<style></style>
