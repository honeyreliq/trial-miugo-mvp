<template>
  <v-container>
    <div class="pa-4">
      <v-row justify="center">
          <v-row v-if="$auth.loading">
            <p>{{ $t('pleaseWait')}}</p>
          </v-row>

          <v-row v-else justify="center">
            <BaseButton v-if="!$auth.isAuthenticated"
                        @click="login"
                        color="primary">{{ $t('loginRequest')}}</BaseButton>
          </v-row>
      </v-row>
    </div>
  </v-container>
</template>

<script>
  export default {
    name: 'LoginPage',

    created() {
      if (this.$auth.loading) {
        this.$auth.$watch('loading', (loading) => {
          if (loading === false) {
            this.performRedirect();
          }
        });
      } else {
        this.performRedirect();
      }
    },
    methods: {
      login() {
        setTimeout(() => {
          this.$auth.loginWithRedirect();
        }, 5000);
      },
      performRedirect() {
        if (this.$auth.isAuthenticated) {
          this.$router.push('demos');
        } else {
          this.login();
        }
      },
    },
};
</script>
