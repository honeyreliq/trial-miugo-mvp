<template>
  <v-row justify="center">
    <v-snackbar
      app
      class="pb-4"
      timeout="2000"
      v-model="showSuccessMessage"
      color="primary"
      center
      bottom
      dark>
      <div justify="center" align="center" class="text-subtitle-1">Your appointment request was successfully submitted!</div>
    </v-snackbar>
    <v-dialog
      max-width="600px"
      :value="value"
      @input="$emit('input')"
      scrollable
      persistent>
      <v-card>
        <BaseToolbar title="Request An Appointment"></BaseToolbar>
        <v-divider></v-divider>
        <v-card-text>
          <v-select hide-details
                    :items="providers"
                    v-model="model.provider"
                    :label="'Please select the provider you wish to see'"></v-select>
          <v-text-field hide-details
                        v-model="model.reasonOfVisit"
                        label="What is the reason for your visit?"></v-text-field>
          <v-menu
            v-model="menu"
            :close-on-content-click="false"
            transition="scale-transition"
            min-width="290px">
            <template v-slot:activator="{ on, attrs }">
              <v-text-field
                v-model="model.date"
                label="Preferred Date"
                readonly
                v-bind="attrs"
                v-on="on"
              >
              <fa-icon :icon="['fal', 'calendar']" class="fa-fw" slot="prepend" />
              </v-text-field>
            </template>
            <v-date-picker
              v-model="model.date"
              :allowed-dates="allowedDates"
              @input="menu = false"
            ></v-date-picker>
          </v-menu>
          <BaseTimePicker :label="'Preferred Time'"
                          hide-details="auto"
                          format="24"
                          dense
                          class="mb-4"
                          v-model="model.time"></BaseTimePicker>
          <div class="text-subtitle-2">
            Please select your preferred contact method for your appointment confirmation and details.
          </div>
          <v-radio-group v-model="model.contactMethod" hide-details column>
            <v-radio v-for="option in contactMethods"
                     :key="option.label"
                     :label="option.label"
                     :value="option.value"
                     hide-details/>
          </v-radio-group>
          <v-text-field :label="'Please specify'"
                        hide-details
                        maxlength="50"
                        v-model="model.contactMethodOther"
                        v-if="model.contactMethod === 'Other'"
                        type="text"></v-text-field>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <BaseButton @click.native="cancel">Cancel</BaseButton>
          <BaseButton color="primary" @click.native="submit">Submit</BaseButton>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script lang="ts">
    import Vue from 'vue';

    export default Vue.extend({
        name: 'DemoRequestAppointmentModal',
        components: {},
        props: ['value'],
        methods: {
            // Only even dates are allowed
            allowedDates(val: any): boolean {
                return parseInt(val.split('-')[2], 10) % 2 === 0;
            },
            cancel() {
                this.$emit('input');
            },
            submit() {
                this.showSuccessMessage = true;
                this.$emit('input');
            },
        },
        data(): any {
            return {
                menu: false,
                showSuccessMessage: false,
                providers: [
                    'Dr. Michael Vanier',
                    'Dr. Andrea Rodriguez',
                    'Anita Gomez',
                ],
                contactMethods: [
                    {label: 'Home Phone', value: 'Home Phone'},
                    {label: 'Mobile Phone', value: 'Mobile Phone'},
                    {label: 'Email', value: 'Email'},
                    {label: 'Other', value: 'Other'},
                ],
                model: {
                    provider: '',
                    reasonOfVisit: '',
                    date: '',
                    time: '',
                    contactMethod: '',
                    contactMethodOther: '',
                },
            };
        },
    });
</script>
