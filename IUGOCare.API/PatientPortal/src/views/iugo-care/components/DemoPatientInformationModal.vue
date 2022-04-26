<template>
  <v-dialog
    :value="value"
    @input="$emit('input')"
    fullscreen
    hide-overlay
    transition="dialog-bottom-transition"
    id="user-settings"
  >
    <v-card fixed color="var(--v-background-base)">
      <BaseToolbar :title="patientName" tile>
        <v-spacer></v-spacer>
        <BaseButton icon dark @click.native="$emit('input')">
          <fa-icon :icon="['fal', 'times']" class="fa-fw" />
        </BaseButton>
      </BaseToolbar>

      <v-row no-guttersx>
        <v-col cols="12" sm="12" md="3" lg="3">
          <v-list flat color="transparent">
            <v-list-item-group color="darkerGrey" mandatory v-model="selectedOptionLocal">
              <div v-for="section in sectionsData" :key="section.key">
                <v-list-item v-if="section.subSections.length === 1">
                  <v-list-item-icon v-if="section.icon" class="mr-4">
                    <fa-icon :icon="['fad', section.icon]" class="fa-fw" />
                  </v-list-item-icon>
                  <v-list-item-title>{{ section.title }}</v-list-item-title>
                </v-list-item>

                <v-list-group v-else color="darkerGrey">
                  <template v-slot:activator>
                    <v-list-item-icon v-if="section.icon" class="mr-4">
                      <fa-icon :icon="['fad', section.icon]" class="fa-fw" />
                    </v-list-item-icon>
                    <v-list-item-title>{{ section.title }}</v-list-item-title>
                  </template>

                  <v-list-item
                    class="pl-12"
                    v-for="subSection in section.subSections"
                    :key="subSection.key"
                    link
                  >
                    <v-spacer></v-spacer>
                    <v-list-item-title
                      v-text="subSection.title"
                    ></v-list-item-title>
                  </v-list-item>
                </v-list-group>
              </div>
            </v-list-item-group>
          </v-list>
        </v-col>
        <v-col cols="12" sm="12" md="9" lg="6">
          <div
            v-for="section in sectionsData"
            :key="section.key"
            :id="section.key"
          >
            <v-card-title class="justify-left">
              <span v-if="section.icon" class="mr-4 light">
                <fa-icon :icon="['fad', section.icon]" class="fa-fw" />
              </span>
              <span class="text-h5" id="account">{{ section.title }}</span>
            </v-card-title>
            <template v-for="(subSection, i) in section.subSections">
              <v-card-subtitle class="pb-0" :key="subSection.title" :id="subSection.key">
                <span class="text-h6">{{ subSection.title }}</span>
                <v-spacer />
                <span class="caption">{{ subSection.date }}</span>
              </v-card-subtitle>
              <v-card-text class="”subtitle-1”" :key="`card-text-${i}`">
                <BaseCard>
                  <v-row class="mb-4">
                    <v-col
                      cols="12"
                      sm="6"
                      v-for="field in subSection.fields"
                      :key="field.label"
                    >
                      <v-select
                        :label="field.label"
                        v-model="field.value"
                        :items="statusLis"
                        class="disable-events"
                        v-if="field.type === 'select'"
                      ></v-select>
                      <template v-else-if="field.type === 'multiline'">
                        <span class="caption">{{ field.label }}</span>
                        <div class="mt-1" v-html="field.value"></div>
                      </template>
                      <v-text-field
                        :label="field.label"
                        :value="field.value"
                        v-else
                        readonly
                      ></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row justify="end">
                    <BaseButton color="primary" class="pa-4">Edit</BaseButton>
                  </v-row>
                </BaseCard>
              </v-card-text>
            </template>
          </div>
          <v-card-actions>
            <v-spacer></v-spacer>
            <BaseButton color="primary" @click.native="$emit('input')">{{
              $t("close")
            }}</BaseButton>
          </v-card-actions>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import Vue from 'vue';
import VueScrollTo, { ScrollOptions } from 'vue-scrollto';
import moment from 'moment';

Vue.use(VueScrollTo);

function setCreatedDate(months: number, time: string) {
  const date = moment().subtract(months, 'months').format('MM/DD/YYYY');
  return date + ' @ ' + time;
}

export default Vue.extend({
  name: 'DemoPatientInformationModal',
  props: ['value', 'selectedOption'],
  data(): any {
    return {
      defaultSelection: 'personal',
      selectedSection: '',
      patientName: 'Caron, Monique Alicia (Alicia) - 67 F',
      statusLis: ['Active', 'Inactive'],
      sectionsData: [
        {
          title: 'Personal Information',
          key: 'personal-section',
          icon: 'user-circle',
          subSections: [
            {
              title: 'Basic Information',
              key: 'basicInfo-section',
              date:
                'Last updated by Jackie Jones  ' +
                setCreatedDate(1, '11:23 AM'),
              fields: [
                {
                  label: 'Patient Name',
                  value: 'Caron, Monique Alicia',
                },
                {
                  label: 'Preferred Name',
                  value: 'Alicia',
                },
                {
                  label: 'Date of Birth',
                  value: '04/28/1953',
                },
                {
                  label: 'Gender',
                  value: 'Female',
                },
                {
                  label: 'Preferred Language',
                  value: 'English',
                },
                {
                  label: 'Race',
                  value: 'White',
                },
                {
                  label: 'Employment Status',
                  value: 'Employed',
                },
                {
                  label: 'Patient Status',
                  value: 'Active',
                  type: 'select',
                },
              ],
            },
            {
              title: 'Contact Information',
              key: 'contactInfo-section',
              date:
                'Last updated by Jackie Jones  ' +
                setCreatedDate(1, '11:23 AM'),
              fields: [
                {
                  label: 'Primary Phone',
                  value: '+1 (345) 888-7728',
                },
                {
                  label: 'Secondary Phone',
                  value: '+1 (345) 888-7727',
                },
                {
                  label: 'Email',
                  value: 'mcaron@gmail.com',
                },
                {
                  label: 'Preferred Contact Method',
                  value: 'Email',
                },
                {
                  label: 'Home Address',
                  value: '123 Main St W. Dallas, TX. United States. 12345'.replace(
                    /\./gi,
                    '</br>'
                  ),
                  type: 'multiline',
                },
              ],
            },
            {
              title: 'Emergency Contact',
              key: 'energencyInfo-section',
              date:
                'Last updated by Jackie Jones  ' +
                setCreatedDate(1, '11:23 AM'),
              fields: [
                {
                  label: 'Name',
                  value: 'Josh Caron',
                },
                {
                  label: 'Relationship',
                  value: 'Brother',
                },
                {
                  label: 'Phone',
                  value: '+1 (555) 888-5555',
                },
              ],
            },
          ],
        },
        {
          title: 'Medical Information',
          key: 'medical-section',
          icon: 'notes-medical',
          subSections: [
            {
              date:
                'Last updated by Jackie Jones  ' +
                setCreatedDate(1, '11:23 AM'),
              fields: [
                {
                  label: 'Primary Care Provider',
                  value: 'Dr. Michael Vanier',
                },
                {
                  label: 'Allergies',
                  value: 'Penicillin, Peanuts',
                },
                {
                  label: 'Pharmacy',
                  value: 'Good Health Pharmacy',
                },
              ],
            },
          ],
        },
        {
          title: 'Program Information',
          key: 'program-section',
          icon: 'laptop-medical',
          subSections: [
            {
              date:
                'Last updated by Jackie Jones  ' +
                setCreatedDate(1, '11:23 AM'),
              fields: [
                {
                  label: 'Organization',
                  value: 'Sunny Side Long Term Care Facility',
                },
                {
                  label: 'Active Programs',
                  value: 'RPM, CCM',
                },
                {
                  label: 'Billing Provider',
                  value: 'Dr. Michael Vanier',
                },
                {
                  label: 'Insurance Type',
                  value: 'Medicare',
                },
                {
                  label: 'Insurance ID',
                  value: '1EG4-TE5-MK72',
                },
              ],
            },
          ],
        },
      ],
      component: {
        scrollTransitionTime: 1, // always 1, 0 is the default time
      },
    };
  },
  methods: {
    scrollToSection() {
      setTimeout(() => {
        const element: NodeListOf<HTMLElement> = document.querySelectorAll(
          `#${this.selectedSection}-section`
        );
        if (element.item(0)) {
          this.$scrollTo(`#${this.selectedSection}-section`, {
            container: '.v-dialog.v-dialog--fullscreen',
            easing: 'ease-in-out',
            offset: -60,
            force: true,
            cancelable: true,
            duration: this.component.scrollTransitionTime,
            onDone: (): void => {
              // as soon as the first animation happens
              // (which is when the account/profile sections opens),
              // set the new animation time
              this.component.scrollTransitionTime = 500;
            },
          } as ScrollOptions);
        }
      }, 0);
    },
  },
  computed: {
    selectedOptionLocal: {
      get(): number {
        this.scrollToSection();
        switch (this.selectedSection) {
          case 'personal':
            return 0;
          case 'medical':
            return 1;
          case 'program':
            return 2;
          case 'basicInfo':
            return 3;
          case 'contactInfo':
            return 4;
          case 'energencyInfo':
            return 5;
          default:
            return null;
        }
      },
      set(newValue: number) {
        if (newValue) {
          switch (newValue) {
            case 0:
              this.selectedSection = 'personal';
              break;
            case 1:
              this.selectedSection = 'medical';
              break;
            case 2:
              this.selectedSection = 'program';
              break;
            case 3:
              this.selectedSection = 'basicInfo';
              break;
            case 4:
              this.selectedSection = 'contactInfo';
              break;
            case 5:
              this.selectedSection = 'energencyInfo';
              break;
          }
        } else {
          this.selectedSection = this.defaultSelection;
        }
      },
    },
  },
  watch: {},
  components: {},
  created(): void {
    // overrides the scroll animation when the account section is closed
    this.$on('input', () => {
      this.component.scrollTransitionTime = 1;
    });
  },
});
</script>
<style>
.v-dialog > .v-card > .v-toolbar {
  position: sticky;
  top: 0;
  z-index: 999;
}

.v-dialog > .v-card .v-list {
  position: sticky;
  top: 65px;
  z-index: 998;
}
</style>
