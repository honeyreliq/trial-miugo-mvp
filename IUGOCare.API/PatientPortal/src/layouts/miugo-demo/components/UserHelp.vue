<template>
  <v-dialog
    :value="value"
    @input="$emit('input')"
    fullscreen
    hide-overlay
    transition="dialog-bottom-transition"
  >
    <v-card fixed color="var(--v-background-base)">
      <BaseToolbar :title="$t('help')" tile>
      </BaseToolbar>
      <v-card-text></v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <BaseButton color="primary" @click.native="$emit('input')">Close</BaseButton>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: 'UserHelp',
  props: ['value'],
};
</script>
