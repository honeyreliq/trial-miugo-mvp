<template>
  <v-row class="text-center" justify="center">
    <v-col cols="12"  align="center" >
        <v-img
          max-width="600"
          class="mb-16"
          contain
          :src="require(`@/assets/${iugoImg}`)"
        ></v-img>
      <h1 class="font-weight-black extra-large white--text mb-8">DEMOS</h1>
    </v-col>
    <v-col cols="12" class="text-center theme--light">
      <BaseButton class="white text-center mr-4 mb-4 light" height="200" width="200" to="/iugo-care">
        <fa-icon :icon="['fad', 'user-md']" class="fa-fw" />
        <div class="text-h5 ma-4 black--text">iUGO Care</div>
      </BaseButton>
      <BaseButton class="white text-center mr-4 mb-4 light" height="200" width="200" to="/miugo">
        <fa-icon :icon="['fad', 'user']" class="fa-fw" />
        <div class="text-h5 ma-4 black--text">MiUGO</div>
      </BaseButton>
    </v-col>
  </v-row>
</template>

<script>
export default {
  name: 'IugoDemos',
  data: () => ({
    iugoImg: 'iUGO_Health_W.svg',
  }),
};
</script>

<style lang="scss" scoped>
.extra-large {
  font-size: 3rem;
}
.svg-inline--fa.fa-fw {
  width: 80px;
  height: 80px;
}
</style>
