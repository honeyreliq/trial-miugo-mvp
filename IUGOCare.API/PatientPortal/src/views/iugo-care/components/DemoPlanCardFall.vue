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
      name: 'DemoPlanCardFall',
      components: {},
      data: () => ({
          model : {
              cardTitle: 'Plan',
              sections: [
                {
                  subTitle: 'Relevant Information',
                  body: 'Known mobility issues with gait aids (wheeled walker). <br /> History of falls (last fall April 2020). <br /> Recent functional decline.',

                },
                {
                  subTitle: 'Instructions',
                  body: 'If a fall occurs, ensure patient safety. Contact the patient immediately. If unable to reach the patient, call the emergency contact and inform them of the incident. If no contact is able to be established, call emergency services.',
                },
                {
                  subTitle: 'Injury',
                  body: 'If an injury is incurred, please notify the primary care provider at the next availability.',

                },
              ],
          },
      }),
    });
</script>
