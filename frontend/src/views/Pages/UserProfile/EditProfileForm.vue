<template>
  <card style="margin-top: 150px" >
<!--    <b-row align-v="center" slot="header"  >-->
<!--      <b-col cols="8" >-->
<!--        <h3 class="mb-0">Редактировать профиль </h3>-->
<!--      </b-col>-->
<!--      <b-col cols="4" class="text-right">-->
<!--        <a href="#!" class="btn btn-sm btn-primary">Settings</a>-->
<!--      </b-col>-->
<!--    </b-row>-->

    <b-form @submit.prevent="updateProfile" >
      <h6 class="heading-small text-muted mb-4">Информация о пользователе</h6>

      <div class="pl-lg-4" >
        <b-row>
          <b-col md="12">
            <base-input
              type="text"
              label="Фамилия"
              placeholder="Иванов"
              v-model="user.lastName"
            >
            </base-input>
          </b-col>
          <b-col md="12">
            <base-input
              type="text"
              label="Имя"
              placeholder="Иван"
              v-model="user.firstName"
            >
            </base-input>
          </b-col>
        </b-row>
        <b-row >
          <b-col md="12">
            <base-input
              type="text"
              label="Отчество"
              placeholder="Иванович"
              v-model="user.surName"
            >
            </base-input>
          </b-col>
          <b-col md="12">
            <base-input
              type="email"
              label="Email"
              placeholder="bank@bank.ru"
              v-model="user.email"
            >
            </base-input>
          </b-col>
        </b-row>
      </div>
      <hr class="my-4">

      <!-- Address -->
      <h6 class="heading-small text-muted mb-4">Контактная информация</h6>

      <div class="pl-lg-4">
        <b-row>
          <b-col md="6">
            <base-input
              type="text"
              label="Номер телефона"
              placeholder="89999999999"
              v-model="user.numberPhone"
            >
            </base-input>
          </b-col>
          <b-col lg="6">
            <base-input
              type="text"
              label="Дата рождения"
              placeholder="01.01.1999"
              v-model="user.dateOfBirth"
            >
            </base-input>
          </b-col>
        </b-row>
        <b-row>
          <b-col lg="6">
            <base-input
              type="text"
              label="Город"
              placeholder="Казань"
              v-model="user.city"
            >
            </base-input>
          </b-col>
          <b-col lg="6">
            <base-input
              type="text"
              label="Почтовый индекс"
              placeholder="Индекс"
              v-model="user.postalCode"
            >
            </base-input>
          </b-col>
        </b-row>
      </div>

      <hr class="my-4">
      <b-col cols="7" class="text-right">
        <a href="#" class="btn btn-sm btn-primary">Сохранить</a>
      </b-col>
      <!-- Description -->
<!--      <h6 class="heading-small text-muted mb-4">About me</h6>-->
<!--      <div class="pl-lg-4">-->
<!--        <b-form-group label="About Me" label-class="form-control-label" class="mb-0" label-for="about-form-textaria">-->
<!--         &lt;!&ndash;  <label class="form-control-label">About Me</label> &ndash;&gt;-->
<!--          <b-form-textarea rows="4" value="A beautiful premium dashboard for BootstrapVue." id="about-form-textaria" placeholder="A few words about you ..."></b-form-textarea>-->
<!--        </b-form-group>-->
<!--      </div>-->

    </b-form>
  </card>
</template>
<script>
export default {
  data() {
    return {
      user: {
        company: '',
        username: '',
        email: '',
        firstName: '',
        lastName: '',
        surName: '',
        dateOfBirth: '',
        numberPhone: '',
        address: '',
        city: '',
        postalCode: ''

      }
    };
  },
  methods: {
    updateProfile() {
      alert('Your data: ' + JSON.stringify(this.user));
    }
  }
};
</script>
<style scoped>
.card {
  background-color: #f6f8fb
}
</style>
