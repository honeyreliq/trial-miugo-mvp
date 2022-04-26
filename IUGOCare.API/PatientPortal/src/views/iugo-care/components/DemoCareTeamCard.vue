<template>
  <BaseCard>
    <section class="d-flex justify-space-between mb-2">
      <div class="text-h6">
        {{ $t("careTeam") }}
      </div>
      <BaseButton color="primary">
        <fa-icon :icon="['fal', 'plus']" class="fa-fw fa-button mr-1 pt-1" />
        <span class="text-subtitle-2">{{ $t("addMember") }}</span>
      </BaseButton>
    </section>
    <section>
      <v-data-table
          :headers="table.headers"
          :items="table.rows"
          class="striped"
          hide-default-footer
      ></v-data-table>
    </section>
  </BaseCard>
</template>

<style lang="scss"></style>
<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'DemoCareTeamCard',
  components: {},
  computed: {},
  watch: {},
  props: {},
  data() {
    return {
      table: {
        headers: [
          {text: this.$t('careTeamMember'), value: 'member', sortable: false, class: 'text-subtitle-1', width: '60%'},
          {text: this.$t('role'), value: 'role', sortable: false, class: 'text-subtitle-1', width: '40%'},
        ],
        rows: [
          {member: 'Dr. Michael Vanier', role: this.$t('diabetesSpecialist')},
          {member: 'Dr. Andrea Rodriguez', role: this.$t('diabetesSpecialist')},
          {member: 'Anita Gomez', role: this.$t('diabetesEducator')},
        ],
      },
    };
  },
  methods: {},
});
</script>
