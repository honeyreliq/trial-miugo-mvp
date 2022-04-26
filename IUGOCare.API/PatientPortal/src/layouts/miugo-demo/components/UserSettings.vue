<template>
  <v-dialog :value="value"
            @input="$emit('input')"
            fullscreen
            hide-overlay
            transition="dialog-bottom-transition"
            id="user-settings">
    <v-card fixed color="var(--v-background-base)">
      <BaseToolbar :title="$t('accountAndProfile')" tile>
        <v-spacer></v-spacer>
        <BaseButton icon dark @click.native="$emit('input')">
          <fa-icon :icon="['fal', 'times']" class="fa-fw" />
        </BaseButton>
      </BaseToolbar>

      <v-row no-gutters>
        <v-col cols="12" sm="12" md="3" lg="3">
          <v-list flat nav color="transparent">
            <v-list-item-group color="darkerGrey" mandatory v-model="selectedOptionLocal">
              <v-list-item key="0">
                <v-list-item-icon class="mr-4">
                  <fa-icon :icon="['fad', 'user-circle']" class="fa-fw" />
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title>{{$t('account')}}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
              <v-list-item key="1">
                <v-list-item-icon class="mr-4">
                  <fa-icon :icon="['fad', 'address-card']" class="fa-fw" />
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title>{{$t('profile')}}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </v-list-item-group>
          </v-list>
        </v-col>
        <v-col cols="12" sm="12" md="9" lg="6">
          <div id="account-section">
            <v-card-title class="justify-left">
              <span class="mr-4 light"><fa-icon :icon="['fad', 'user-circle']" class="fa-fw" /></span>
              <span class="text-h5" id="account">{{$t('account')}}</span>
            </v-card-title>
            <v-card-text>
              <BaseCard>
                <v-row no-gutters class="mb-4">
                  <v-col class="text-h6" cols="auto">{{$t('email')}}</v-col>
                  <v-spacer />
                  <v-col cols="auto">
                    <UpdateEmail></UpdateEmail>
                  </v-col>
                  <v-col class="text-subtitle-1" cols="12">
                    mcaron@gmail.com
                    <!-- {{(currentPatient)?currentPatient.emailAddress:null}} -->
                  </v-col>
                </v-row>
                <v-row no-gutters class="mb-4">
                  <v-col cols="12">
                    <v-divider />
                  </v-col>
                </v-row>
                <v-row no-gutters class="mb-4">
                  <v-col class="text-h6" cols="auto">{{$t('password')}}</v-col>
                  <v-spacer />
                  <v-col cols="auto">
                    <UpdatePassword></UpdatePassword>
                  </v-col>
                </v-row>
              </BaseCard>
            </v-card-text>
          </div>
          <div id="profile-section">
            <v-card-title class="justify-left">
              <span class="mr-4 light"><fa-icon :icon="['fad', 'address-card']" class="fa-fw" /></span>
              <span class="text-h5" id="profile">{{$t('profile')}}</span>
            </v-card-title>
            <v-card-subtitle class="pb-0">
              <span class="text-h6">{{$t('personalInformation')}}</span>
            </v-card-subtitle>
            <v-card-text>
              <BaseCard>
                <v-row no-gutters class="mb-4">
                  <!-- <v-text-field name="fullName"
                                :label="$t('patientNameTitle')"
                                :value="fullName"
                                readonly
                                class="custom"></v-text-field> -->
                    <v-text-field name="fullName"
                      :label="$t('patientNameTitle')"
                      value="Monique Alicia Caron"
                      readonly
                      class="custom"></v-text-field>
                </v-row>
                <v-row no-gutters class="mb-4">
                  <!-- <v-text-field :label="$t('phone')" :value="(currentPatient)?currentPatient.phone:null" readonly></v-text-field> -->
                  <v-text-field :label="$t('phone')" value="+1 (345) 888-7728" readonly></v-text-field>
                </v-row>
                <v-row no-gutters class="mb-4">
                  <!-- <v-text-field name="birthDate" :label="$t('birthDate')" :value="birthDate" readonly></v-text-field> -->
                  <v-text-field name="birthDate" :label="$t('birthDate')" value="04/28/1953" readonly></v-text-field>
                </v-row>

                <v-row no-gutters class="mb-4">
                  <v-col cols="12">
                    <v-divider />
                  </v-col>
                </v-row>

                <v-row no-gutters class="mb-4">
                  <v-col cols="12">
                    <span class="text-h6">{{$t('emergencyContact')}}</span>
                  </v-col>
                </v-row>

                <v-row no-gutters class="mb-4">
                  <v-text-field :label="$t('name')" :value="contactName" readonly></v-text-field>
                </v-row>
                <v-row no-gutters class="mb-4">
                  <v-text-field :label="$t('relationship')" :value="relationship" readonly></v-text-field>
                </v-row>
                <v-row no-gutters class="mb-4">
                  <!-- <v-text-field :label="$t('phone')" :value="contactPhone" readonly></v-text-field> -->
                  <v-text-field :label="$t('phone')" value="+1 (555) 357-9512" readonly></v-text-field>
                </v-row>
              </BaseCard>
            </v-card-text>

            <v-card-subtitle class="pb-0">
              <span class="text-h6">{{$t('medicalInformation')}}</span>
            </v-card-subtitle>
            <v-card-text>
              <BaseCard>
                <v-row v-if="profile != null && profile.patientPrograms != null">
                  <v-row v-for="(program, i) in profile.patientPrograms" :key="i">
                    <v-col cols="12">
                      <v-row no-gutters class="mb-4">
                        <v-col cols="12">
                          <span class="sustitle-1">{{$t('careManagementPrograms')}}</span>
                        </v-col>
                        <v-col cols="12" v-for="(item, i) in program.careManagementPrograms" :key="i">
                          <span class="title">{{item.name}}</span>
                        </v-col>
                      </v-row>
                    </v-col>
                  </v-row>
                </v-row>
              </BaseCard>
            </v-card-text>
          </div>
          <v-card-actions>
            <v-spacer></v-spacer>
            <BaseButton color="primary" @click.native="$emit('input')">{{$t('close')}}</BaseButton>
          </v-card-actions>
        </v-col>
        <v-col cols="3"></v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapState } from 'vuex';
import { format } from 'date-fns';
import { IPatientVm, IPatientProfileVm, IEmergencyContactDto, IPatientCareManagementProgramDto } from '../../../IUGOCare-api';
import UpdatePassword from '../../main/components/UpdatePassword.vue';
import UpdateEmail from '../../main/components/UpdateEmail.vue';

import VueScrollTo, {ScrollOptions} from 'vue-scrollto';
Vue.use(VueScrollTo);

export default Vue.extend({
  name: 'UserSettings',
  props: ['value', 'selectedOption'],
  data(): any {
    return {
      component: {
        scrollTransitionTime: 1, // always 1, 0 is the default time
      },
    };
  },
  methods: {
    getProgramOrganizationAddressCityAndState(program: IPatientCareManagementProgramDto) {
      if (!program) {
        return '';
      }
      const address = [program?.billingProvider?.organization?.addressCity,
        program?.billingProvider?.organization?.addressState];
      return address.filter(Boolean).join(', ');
    },
    scrollToSection() {
      setTimeout(() => {
        const element: NodeListOf<HTMLElement> = document.querySelectorAll(`#${this.selectedOption}-section`);
        if (element.item(0)) {
          this.$scrollTo(`#${this.selectedOption}-section`, {
            container: '.v-dialog.v-dialog--fullscreen',
            easing: 'ease-in-out',
            offset: -60,
            force: true,
            cancelable: true,
            duration: this.component.scrollTransitionTime,
            onDone: (): void => {
              // as soon as the first animation happens (which is when the account/profile sections opens),
              // set the new animation time
              this.component.scrollTransitionTime = 500;
            },
          } as ScrollOptions);
        }
      }, 0);
    },
  },
  computed: {
    ...mapState('patients', ['currentPatient', 'currentPatientProfile']),
    birthDate(): string {
      const patient = this.currentPatient as IPatientVm;
      if (! patient?.birthDate) {
        return '';
      }

      return format(patient.birthDate, 'MMMM d, yyyy');
    },
    fullName(): string {
      const patient = this.currentPatient as IPatientVm;
      if (!patient) {
        return '';
      }
      const nameParts = [patient.givenName, patient.middleName, patient.familyName];
      return nameParts.filter((n) => Boolean(n)).join(' ');
    },
    profile(): IPatientProfileVm {
      return this.currentPatientProfile as IPatientProfileVm;
    },
    emergencyContact(): IEmergencyContactDto {
      const profile = this.currentPatientProfile as IPatientProfileVm;
      return profile?.emergencyContact;
    },
    contactName(): string {
      return !this.emergencyContact ? '' : this.emergencyContact.contactName;
    },
    relationship(): string {
      return !this.emergencyContact ? '' : this.emergencyContact.relationship;
    },
    contactPhone(): string {
      return !this.emergencyContact ? '' : this.emergencyContact.phone;
    },
    selectedOptionLocal: {
      get(): number {
        this.scrollToSection();
        return this.selectedOption === 'account' || this.selectedOption === '' ? 0 : 1;
      },
      set(newValue: number) {
        this.$root.$emit('updateSelectedMenuOption', newValue === 0 ? 'account' : 'profile');
      },
    },
  },
  watch: {
    value() {
      if (this.value) {
        this.$store.dispatch('patients/fetchCurrentPatientProfile');
      }
    },
  },
  components: {
    UpdatePassword,
    UpdateEmail,
  },
  created(): void {
    this.$store.dispatch('patients/fetchCurrentPatientProfile');
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

