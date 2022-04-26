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
      name: 'DemoPlanCardSOS',
      components: {},
      data: () => ({
          model : {
              cardTitle: 'Plan',
              sections: [
                  {
                    subTitle: 'Relevant Information',
                    body: 'Lives at home with husband. <br /> Independent with BADLs. <br /> Is known to have increased anxiety in the evening or when left alone.',
                  },
                  {
                    subTitle: 'Instructions',
                    body: 'Determine the type of issue and if additional services are required. The patient\'s caregiver should be the first contact if the issue cannot be resolved by support staff.',
                  },

              ],
          },
      }),
    });
</script>
