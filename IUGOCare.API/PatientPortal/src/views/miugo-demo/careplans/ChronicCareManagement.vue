<template>
  <v-container id="chronic-care-management" fluid>
    <v-row class="fill-height flex-column">
      <v-col cols="12">
        <v-row>
          <v-col cols="12">
            <DemoProgramInformation></DemoProgramInformation>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="9">
            <BaseCard :expanded="expanded" margin="ma-0 mb-4">
              <section class="d-flex justify-space-between mb-2">
                <div class="text-h6">What are my health conditions?</div>
                <BaseButton color="primary" dark @click="expandCards">
                  {{ expanded ? "Collapse" : "Expand" }}
                </BaseButton>
              </section>
              <v-row>
                <v-col cols="12" class="d-flex flex-wrap">
                  <v-chip-group
                    column
                    mandatory
                    active-class="primary"
                    :value="selectedOption"
                  >
                    <v-chip
                      v-for="item in options"
                      :key="item.value"
                      :value="item.value"
                      class="disable-events"
                      >{{ item.name }}
                    </v-chip>
                  </v-chip-group>
                </v-col>
              </v-row>
            </BaseCard>
            <v-expansion-panels v-model="panel" multiple tile flat class="mb-4">
              <v-expansion-panel :expanded="expanded" class="rounded">
                <v-expansion-panel-header class="text-h6 font-weight-bold">
                  <div class="text-h6 mb-4">Problem: High Blood Pressure</div>
                </v-expansion-panel-header>
                <v-expansion-panel-content>
                  <DemoCarePlanHighBloodPressure></DemoCarePlanHighBloodPressure>
                </v-expansion-panel-content>
              </v-expansion-panel>

              <v-expansion-panel :expanded="expanded" class="rounded">
                <v-expansion-panel-header class="text-h6 font-weight-bold">
                  <div class="text-h6 mb-4">Problem: High Blood Sugar</div>
                </v-expansion-panel-header>
                <v-expansion-panel-content>
                  <DemoCarePlanHighBloodSugar></DemoCarePlanHighBloodSugar>
                </v-expansion-panel-content>
              </v-expansion-panel>

              <v-expansion-panel :expanded="expanded" class="rounded">
                <v-expansion-panel-header class="text-h6 font-weight-bold">
                  <div class="text-h6 mb-4">Problem: Overweight</div>
                </v-expansion-panel-header>
                <v-expansion-panel-content>
                  <DemoCarePlanOverweight></DemoCarePlanOverweight>
                </v-expansion-panel-content>
              </v-expansion-panel>
            </v-expansion-panels>
          </v-col>
          <v-col cols="3" data-v-sticky-container>
            <div v-sticky="planServicesStickyOptions">
              <DemoCarePlanServices data-v-sticky-inner></DemoCarePlanServices>
            </div>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue';
import DemoProgramInformation from '@/views/miugo-demo/components/DemoProgramInformation.vue';
import DemoCarePlanHighBloodPressure  from '@/views/miugo-demo/components/DemoCarePlanHighBloodPressure.vue';
import DemoCarePlanHighBloodSugar from '@/views/miugo-demo/components/DemoCarePlanHighBloodSugar.vue';
import DemoCarePlanOverweight from '@/views/miugo-demo/components/DemoCarePlanOverweight.vue';
import DemoCarePlanServices from '@/views/miugo-demo/components/DemoCarePlanServices.vue';

interface ICondition {
name: string;
value: string;
}

const options: ICondition[] = [
  {name: 'Dementia', value: 'Dementia'},
  {name: 'Diabetes', value: 'Diabetes'},
  {name: 'Hypertension', value: 'Hypertension'},
  {name: 'Repeated Falls', value: 'Repeated Falls'},
];
export default Vue.extend({
  name: 'ChronicCareManagement',
  components: {
    DemoProgramInformation,
    DemoCarePlanHighBloodPressure,
    DemoCarePlanHighBloodSugar,
    DemoCarePlanOverweight,
    DemoCarePlanServices,
  },
  data() {
    return {
      expanded: true,
      panel: [0,1],
      selectedOption: options[1].value,
      options: [...options],
      appBarColor: {
        class: 'colorCareplans',
        dark: true,
      },
      planServicesStickyOptions: {
      topSpacing: 16,
      bottomSpacing: 0,
      resizeSensor: true,
      },
    };
  },
  mounted() {
    this.$root.$emit('setAppBarColor', this.appBarColor);
  },
  destroyed(): void {
    this.appBarColor.class = '';
    this.appBarColor.dark = false;
    this.$root.$emit('setAppBarColor', this.appBarColor);
  },
  methods: {
    // Add some logic here to check if the cards are -all- collapsed or expanded
    expandCards () {
      if (this.expanded) {
        this.panel=[];
      } else {
        this.panel=[0,1];
      }
      this.expanded = !this.expanded;
    },
  },
  watch: {
    panel: function(){
      if (this.panel.length === 0) {
        this.expanded = false;
      } else {
        this.expanded = true;
      }
    }
  }
});
</script>
<style lang="scss">
  .v-expansion-panel:not(:first-child) {
    margin-top: 16px;
  }
</style>
