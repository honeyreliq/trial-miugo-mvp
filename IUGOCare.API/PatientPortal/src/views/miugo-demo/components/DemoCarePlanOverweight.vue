<template>
  <div>
    <section v-for="section in sections" :key="section.title" class="mb-4">
      <div class="text-h6 mb-4">
        {{ section.title }}
      </div>
      <v-simple-table class="striped">
        <template>
          <tbody>
            <tr v-for="item in section.items" v-bind:key="item.body">
              <td>
                <div class="d-flex">
                  <div v-html="item.body" class="my-4 mr-auto"></div>
                  <div
                    v-tooltip="enableTooltip(item)"
                    v-if="item.icon"
                    class="mt-4"
                  >
                    <fa-icon
                      @click.stop="goToPage(item)"
                      :icon="['fal', item.icon]"
                      class="fa-fw mr-2"
                      v-if="item.icon"
                    />
                  </div>
                </div>
              </td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
      <v-divider></v-divider>
    </section>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'DemoCarePlanOverweight',
  components: {},
  methods: {
    goToPage(item: any): void {
      this.$router.push(item.to);
    },
    enableTooltip(item: any) {
      // v-tooltip has not a prop to disable, have to be manually
      if (item.tooltip) {
        return {
          content: this.$t(item.tooltip),
          placement: 'right',
          boundariesElement: 'body',
          classes: [
            'add-tooltop',
            this.$vuetify.theme.dark ? 'theme--dark' : 'theme--light',
          ],
          offset: 17,
        };
      }
      return null;
    },
  },
  data(): any {
    return {
      sections: [
        {
          title: 'What are my treatment goals?',
          items: [
            {
              body: 'Weight reduction to 195 lbs over 6 months.',
            },
            {
              body: `
                <span>Create consistent habits with:</span>
                <ul>
                  <li>Dietary choices</li>
                  <li>Daily exercise</li>
                </ul>
              `,
            },
          ],
        },
        {
          title: 'What are my expected outcomes?',
          items: [
            {
              body: 'Understanding of diet choices and daily exercise.',
            },
            {
              body: 'Self management of diabetes care.',
            },
          ],
        },
        {
          title: 'What has my doctor planned for me?',
          items: [
            {
              body: `
                <span>Weight control:</span>
                <ul>
                  <li>Target weight of 195 lbs.</li>
                  <li>Target weight loss of 3 - 5 lbs per month.</li>
                </ul>
              `,
            },
            {
              body: `
                <span>Increase diet knowledge:</span>
                <ul>
                  <li>Understand how certain foods impact your hypertension and diabetes.</li>
                  <li>Apply new knowledge to your daily diet choices.</li>
                </ul>
              `,
            },
            {
              body: `
                <span>Exercise:</span>
                <ul>
                  <li>Gradually increase your exercise tolerance to assist in your weight control goals.</li>
                </ul>
              `,
            },
          ],
        },
      ],
    };
  },
});
</script>
