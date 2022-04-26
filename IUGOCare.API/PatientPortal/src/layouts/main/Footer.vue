<template>
  <v-footer id="patient-portal-footer" inset app flat>
    <v-row align="center" no-gutters>
      <v-col class="col-12 text-center py-2">
        <span class="text-subtitle-1">&copy; {{ new Date().getFullYear() }} {{ $t('footerText')}}</span>
      </v-col>
    </v-row>
  </v-footer>
</template>

<script>
export default {
  name: 'PatientPortalFooter',
};
</script>
