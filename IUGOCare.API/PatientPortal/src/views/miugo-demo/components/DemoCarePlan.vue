<template>
  <BaseCard flat>
    <section v-for="section in sections"
             :key="section.title"
             class="mb-4">
      <div class="text-h6 mb-4">
        {{ section.title }}
      </div>
      <v-expansion-panels :readonly="section.readOnly"
                          :multiple="!section.readOnly"
                          accordion
                          flat
                          tile>
        <template v-for="item in section.items">
          <v-expansion-panel
                            :key="item.label"
                            v-if="showPanel(section, item, selectedSymptom)">
            <v-expansion-panel-header :hide-actions="!item.expand">
              <v-row>
                <v-col cols="12" sm="11" class="mb-2">
                  <div>{{ item.label }}</div>
                </v-col>
                <v-col cols="1" align="end">
                  <fa-icon @click.stop="goToPage(item)"
                          :icon="['fal', item.icon]"
                          class="fa-fw mr-2"
                          v-if="item.icon"/>
                </v-col>
                <v-col cols="12">
                  <ul>
                    <li v-for="content in item.content"
                        :key="content.label"
                        class="mb-2">
                      <v-row align="center"
                            class="pl-2">
                        <div class="mr-2">{{ content.label }}</div>
                        <template v-for="chip in content.chipLabel">
                        <v-chip color="primary"
                                :key="chip"
                                text-color="white"
                                small
                                class="mr-2"
                                v-if="content.chipLabel"
                                >
                          {{ chip }}
                        </v-chip>
                        </template>
                      </v-row>
                    </li>
                  </ul>
                </v-col>
              </v-row>
            </v-expansion-panel-header>
            <v-expansion-panel-content>
              {{ defaultLabel }}
              <v-chip color="primary"
                      text-color="white"
                      small
                      class="mr-2"
                      v-for="chip in item.chipLabel"
                      :key="chip">
                {{ chip }}
              </v-chip>
            </v-expansion-panel-content>
          </v-expansion-panel>
        </template>
      </v-expansion-panels>
      <v-divider></v-divider>
    </section>
  </BaseCard>
</template>
<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';

export default Vue.extend({
  name: 'DemoCarePlan',
  components: {},
  computed: {
    ...mapGetters('demo', ['selectedSymptom']),
  },
  methods: {
    goToPage(item: any): void {
      this.$router.push(item.to);
    },
    showPanel(section: any, item: any, selectedSymptom: string): boolean {
      if (section.readOnly && (selectedSymptom !== 'All')) {
        return (item.label === selectedSymptom);
      } else if (item.chipLabel && item.chipLabel.length === 1 && selectedSymptom !== 'All') {
        return (item.chipLabel[0] === selectedSymptom);
      }
      return true;
    },
  },
  data(): any {
    return {
      sections: [
        {
          title: 'Problem:',
          readOnly: true,
          items: [
            {
              expand: false,
              label: 'High Blood Sugar',
            },
            {
              expand: false,
              label: 'Overweight',
            },
          ],
        },
        {
          title: 'What are my treatment goals?',
          readOnly: true,
          items: [
            {
              expand: false,
              label: 'Weight reduction to 195 lbs over 6 months.',
            },
            {
              expand: false,
              label: 'Maintain stable blood sugar levels with a resulting decreased A1c result',
            },
            {
              expand: false,
              label: 'Create consistent habits with',
              content: [
                {
                  label: 'Dietary choices',
                },
                {
                  label: 'Daily exercise',
                },
              ],
            },
          ],
        },
        {
          title: 'What are my expected outcomes?',
          readOnly: true,
          items: [
            {
              expand: false,
              label: 'Understanding of diet choices and daily exercise.',
            },
            {
              expand: false,
              label: 'Self management of diabetes care.',
            },
          ],
        },
        {
          title: 'What has my doctor planned for me?',
          readOnly: true,
          items: [
            {
              expand: false,
              label: 'Daily monitoring of blood glucose levels using a connected glucometer device.',

            },
            {
              expand: false,
              label: 'Oral Medication',
              icon: 'capsules',
            },
            {
              expand: false,
              label: 'Weight control',
              content: [
                {
                  label: 'Target weight of 195 lbs',
                },
                {
                  label: 'Target weight loss of 3 - 5 lbs per month',
                },
              ],
            },
            {
              expand: false,
              label: 'Increase diet knowledge',
              content: [
                {
                  label: 'Understand how certain foods impact your hypertension and diabetes',
                },
                {
                  label: 'Apply new knowledge to your daily diet choices',
                },
              ],
            },
            {
              expand: false,
              label: 'Exercise',
              content: [
                {
                  label: 'Gradually increase your exercise tolerance to assist in your weight control goals',
                },
              ],
            },
          ],
        },
      ],
    };
  },
});
</script>
