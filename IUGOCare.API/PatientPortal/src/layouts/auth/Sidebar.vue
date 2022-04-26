<template>
<v-col cols="12" lg="7" class="info d-lg-flex flex-grow-0 align-center justify-center">
  <v-container>
    <div class="pa-4">
      <v-row justify="center">
        <v-col cols="12" sm="10" xl="8">
          <div>
            <h1 class="white--text font-weight-medium">{{ $t('welcome') }}</h1>
            <v-img :src="require(`@/assets/${iugoImg}`)" class="logo-img mt-4" />
            <h2 class="mt-4 white--text font-weight-regular">{{ $t('welcomeMsg') }}</h2>
          </div>
        </v-col>
      </v-row>
    </div>
  </v-container>
</v-col>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'AuthSidebar',
  data: () => ({
    iugoImg: 'iUGO_Care_W.svg',
  }),
});
</script>

<style lang="scss" scoped>
.logo-img {
  max-width: 320px;
}
</style>
