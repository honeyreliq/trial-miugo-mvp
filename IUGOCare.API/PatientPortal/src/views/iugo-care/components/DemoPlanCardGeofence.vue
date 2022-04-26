<template>
  <BaseCard :title="model.cardTitle">
    <section v-for="section in model.sections" :key="section.subTitle" class="mb-4">
      <div class="text-subtitle-1 font-weight-bold">{{section.subTitle}}</div>
      <div class="text-body-1" v-html="section.body"></div>
    </section>
  </BaseCard>
</template>

<script lang="ts">
    import Vue from 'vue';
    export default Vue.extend({
      name: 'DemoPlanCardGeofence',
      components: {},
      data: () => ({
          model : {
              cardTitle: 'Plan',
              sections: [
                {
                  subTitle: 'Relevant Information',
                  body: 'Independent with BADLs. <br /> Is known to have increased anxiety in the evening or when left alone.',
                },
                {
                  subTitle: 'Instructions',
                  body: 'Attempt contact with Monique to determine her intentions (for example, walking with company is permitted). If unable to make contact with Monique, contact the facility and inform them of her location.',
                },
              ],
          },
      }),
    });
</script>
