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
      <div justify="center" align="center" class="text-subtitle-1">Your survey was successfully submitted!</div>
    </v-snackbar>
    <v-dialog
      max-width="600px"
      :value="value"
      @input="$emit('input')"
      scrollable
      persistent>
      <v-card>
        <BaseToolbar title="Survey" class="colorSurveys"></BaseToolbar>
        <v-divider></v-divider>
        <v-card-text>
          <section class="mb-4">
            <div class="text-h6">
              Generalized Anxiety Disorder-7
            </div>
            <div class="text-subtitle-2">
              {{setSurveyDate()}}
            </div>
          </section>
          <section>
            <div class="my-4">
              Over the last two weeks, how often have you been bothered by the following problems?
            </div>
            <v-form ref="form" lazy-validation>
              <div v-for="question in questions" :key="question.label" class="my-4">
                <div class="text-subtitle-2">
                  {{question.label}}
                </div>
                <v-radio-group column  class="ml-2 mt-0"
                    :rules="[v => !!v || 'A response is required.']" required>
                  <v-radio v-for="(option, i) in radioOptions"
                    :key="`radio-option-${i}`"
                    :label="option.label"
                    :value="option.value"
                    hide-details/>
                </v-radio-group>
              </div>
            </v-form>
          </section>
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
  import moment from 'moment';

  export default Vue.extend({
    name: 'DemoSurveyModal',
    components: {},
    props: ['value'],
    methods: {
      setSurveyDate(): string {
        const date = moment().subtract(7, 'd').format('MM/DD/YYYY');
        const time = moment().format('hh:mm A');
        return date + ' @ ' + time;
      },
      cancel() {
        this.$emit('input');
        this.$refs.form.reset();
      },
      submit() {
        if (this.$refs.form.validate()) {
          this.showSuccessMessage = true;
          this.$emit('input');
          this.$root.$emit('submitForm', true);
          this.$refs.form.reset();
        }
      },
    },
    data(): any {
      return {
        showSuccessMessage: false,
        radioOptions: [
          {label: 'Not at all', value: 'Not at all'},
          {label: 'Several days', value: 'Several days'},
          {label: 'More than half the days', value: 'More than half the days'},
          {label: 'Nearly every day', value: 'Nearly every day'},
        ],
        questions: [
          {
            label: '1. Feeling nervous, anxious, or on edge.',
            model: '',
          },
          {
            label: '2. Not being able to stop or control worrying.',
            model: '',
          },
          {
            label: '3. Worrying too much about different things.',
            model: '',
          },
          {
            label: '4. Trouble relaxing.',
            model: '',
          },
          {
            label: '5. Being so restless that it is hard to sit still.',
            model: '',
          },
          {
            label: '6. Becoming easily annoyed or irritable.',
            model: '',
          },
          {
            label: '7. Feeling afraid, as if something awful might happen.',
            model: '',
          },
        ],
      };
    },
  });
</script>
