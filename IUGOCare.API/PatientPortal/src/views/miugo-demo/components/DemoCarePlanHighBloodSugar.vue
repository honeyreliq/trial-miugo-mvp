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

<script lang='ts'>
import Vue from 'vue';

export default Vue.extend({
  name: 'DemoCarePlanHighBloodSugar',
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
              body:
                'Maintain stable blood sugar levels with a resulting decreased A1c result.',
            },
          ],
        },
        {
          title: 'What are my expected outcomes?',
          items: [
            {
              body: 'Self management of diabetes care.',
            },
          ],
        },
        {
          title: 'What has my doctor planned for me?',
          items: [
            {
              body:
                'Daily monitoring of blood glucose levels using a connected glucometer device.',
              icon: 'books',
              to: '/miugo/education',
              tooltip: 'Education',
            },
            {
              body: 'Oral Medication',
              icon: 'capsules',
              to: '/miugo/medications',
              tooltip: 'Medication',
            },
          ],
        },
      ],
    };
  },
});
</script>
