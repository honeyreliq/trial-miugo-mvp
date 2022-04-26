<template>
<v-row
  class="text-center"
  justify="center"
>
  <v-col cols="auto">
    <h1 class="font-weight-black extra-large">
      404
    </h1>

    <div class="text-h2 mb-5 mt-10">
      {{$t('pageNotFound')}} :(
    </div>

    <BaseButton color="white" to="/">
      {{$t('getMeOutOfHere')}}
    </BaseButton>
  </v-col>
</v-row>
</template>

<script>
  export default {
    name: 'PagesError',
  };
</script>

<style lang="sass">
.extra-large
  font-size: 12rem
</style>
