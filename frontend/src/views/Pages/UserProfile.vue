<template>
  <div>
    <div class="header pb-4 pt-5 pt-lg-6 d-flex align-items-center profile-header">
      <b-container fluid ><span class="mask" style="background-color: #f6f8fb"></span>
        <b-navbar-nav class="align-content-lg-center ml-lg-auto">
        </b-navbar-nav>
        <b-container fluid class="d-flex align-items-center"></b-container>
      </b-container>
    </div>
    <b-container fluid class="mt--6" style="background-color: white">
      <b-row>
        <b-col xl="4" class="order-xl-2 mb-5">
          <user-card></user-card>
        </b-col>
        <b-col xl="8" class="order-xl-1">
          <edit-profile-form></edit-profile-form>
        </b-col>
      </b-row>
    </b-container>
  </div>
</template>






<script>
  import EditProfileForm from './UserProfile/EditProfileForm.vue';
  import UserCard from './UserProfile/UserCard.vue';

  export default {
    components: {
      EditProfileForm,
      UserCard
    }
  };
</script>
<style>

</style>
