<template>
  <v-dialog v-model="component.showDialog" persistent max-width="600px">
    <template v-slot:activator="{ on, attrs }">
      <BaseButton
        color="primary"
        v-bind="attrs"
        v-on="on">
        {{$t('update')}}
      </BaseButton>
    </template>
    <v-card>
      <BaseToolbar :title="$t('updateEmail')">
      </BaseToolbar>
      <v-card-text>

        <v-form ref="form" v-model="component.valid" lazy-validation>
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                              counter
                              v-model="data.pwd"
                              :append-icon="component.isPwdFieldText ? 'mdi-eye' : 'mdi-eye-off'"
                              :type="component.isPwdFieldText ? 'text' : 'password'"
                              :label="$t('password')"
                              :rules="validators.passwordRequired"
                              hide-details="auto"
                              :disabled="component.disabled"
                              @click:append="component.isPwdFieldText = !component.isPwdFieldText"></v-text-field>
                <v-text-field v-model="data.email"
                              :label="$t('newEmail')"
                              :rules="validators.requiredEmailRules"
                              hide-details="auto"
                              :disabled="component.disabled"></v-text-field>
                <v-text-field v-model="data.confirm"
                              @paste.prevent
                              autocomplete='off'
                              :label="$t('confirmNewEmail')"
                              :rules="validators.matchEmail"
                              hide-details="auto"
                              :disabled="component.disabled"></v-text-field>
              </v-col>
            </v-row>
          </v-container>
          <v-row>
            <v-col>
              <v-dialog v-model="success.show" persistent max-width="600px">
                <v-card>
                  <v-card-title class="text-h5">{{$t('emailSent')}}</v-card-title>
                  <v-card-text>{{$t('emailSentMsg', {email: data.email})}}</v-card-text>
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <BaseButton color="primary" @click="closeSuccessModalHandler">
                      {{$t('close')}}
                    </BaseButton>
                  </v-card-actions>
                </v-card>
              </v-dialog>

              <v-alert dark type="error" dismissible v-if="error.show">
                <p v-for="(msg, i) in error.messages" :key="`error-message-${i}`">{{msg}}</p>
              </v-alert>
            </v-col>
          </v-row>
        </v-form>

      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <BaseButton text @click="closeUpdateEmailModalHandler" :disabled="component.disabled">{{$t('cancel')}}
        </BaseButton>
        <BaseButton color="primary" @click="submitHandler" :disabled="component.disabled">
          {{$t('save')}}
        </BaseButton>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
  import Vue from 'vue';
  import {mapState} from 'vuex';
  import {PatientsClient, PatientRequestUpdateEmailCommand} from '../../../IUGOCare-api';
  import axiosClient from '@/auth/axiosInstance';

  export default Vue.extend({
    name: 'UpdateEmail',
    props: ['value'],
    computed: {
      ...mapState('patients', ['currentPatient']),
    },
    data(): any {
      return {
        component: {
          valid: true,
          disabled: false,
          showDialog: false,
          isPwdFieldText: false,
        },
        data: {
          email: '',
          confirm: '',
          pwd: '',
        },
        error: {
          show: false,
          messages: null,
        },
        success: {
          show: false,
        },
        validators: {
          requiredEmailRules: [
            (v: string) => !!v || this.$t('emailRequired'),
            (v: string) => /^.+@\w+([.-]?\w+)*(\.\w{2,})+$/.test(v) || this.$t('emailFormatInvalid'),
          ],
          passwordRequired: [
            (v: string) => !!v || this.$t('passwordRequiredMsg'),
          ],
          matchEmail: [
            (v: string) => !!v || this.$t('emailRequired'),
            (v: string) => /^.+@\w+([.-]?\w+)*(\.\w{2,})+$/.test(v) || this.$t('emailFormatInvalid'),
            () => (this.$data.data.email === this.$data.data.confirm) || this.$t('confirmNewEmailMatchMsg'),
          ],
        },
      };
    },
    methods: {
      closeUpdateEmailModalHandler(): void {
        this.component.showDialog = !this.component.showDialog;
        // avoid flicker animation out
        setTimeout(() => {
          this.clear();
        }, 500);
      },
      closeSuccessModalHandler(): void {
        this.clear();
      },
      submitHandler(): void {
        this.error.show = false;
        if (this.$refs.form.validate()) {
          this.component.disabled = true;
          const ctc = new PatientsClient(process.env.VUE_APP_API_URL, axiosClient);
          const command = new PatientRequestUpdateEmailCommand();
          command.emailAddress = this.data.email;
          command.password = this.data.pwd;
          ctc.updateEmail(command)
            .then(() => {
              this.component.showDialog = false;
              setTimeout(() => {
                this.success.show = true;
              }, 200);
            })
            .catch((error) => {
              const errors: string = error.response.data.errors;
              if (errors) {
                let errorList: string[] = [];

                Object.keys(error.response.data.errors).forEach((key: any) => {
                  errorList = errorList.concat(errors[key]);
                });

                this.component.disabled = false;
                this.error.messages = errorList;
                this.error.show = true;
              } else {
                this.component.disabled = false;
                this.error.messages = [error.response.data.title];
                this.error.show = true;
              }
            });
        }
      },
      clear(): void {
        this.$refs.form.reset();
        this.component.valid = true;
        this.component.disabled = false;

        this.data.email = '';
        this.data.confirm = '';
        this.data.pwd = '';

        this.error.show = false;
        this.error.messages = null;
        this.success.show = false;
      },
    },
  });
</script>
