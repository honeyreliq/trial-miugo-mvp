<template>
  <v-footer id="simple-footer" inset app flat>
    <v-container fluid>
      <v-row align="center" no-gutters>
        <v-col class="col-sm-12 col-12 text-center">
          <span class="text-body-2">© {{ new Date().getFullYear() }} {{ $t('footerText')}}</span>
        </v-col>
      </v-row>
    </v-container>
  </v-footer>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'SimplePagesFooter',
});
</script>
