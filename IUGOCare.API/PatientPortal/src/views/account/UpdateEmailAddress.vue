<template>
  <v-container>
    <div class="pa-4">
      <v-row justify="center">
        <v-col cols="12">
          <h2 v-if="component.processing" class="mb-4">{{$t('pleaseWait')}}</h2>
          <v-container v-else>
            <h2 v-if="component.success" class="mb-4">{{$t('emailVerificationSuccess')}}</h2>
            <v-alert type="error" v-else>{{$t('emailVerificationFail')}}</v-alert>
          </v-container>
          <v-row v-if="!component.processing">
            <v-col cols="12">
              <BaseButton @click="goToPortalHandler" color="primary">{{$t('goToPatientsPortal')}}</BaseButton>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </div>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue';
  import {PatientsClient} from '@/IUGOCare-api';
  import axiosClient from '@/auth/axiosInstance';

  export default Vue.extend({
    name: 'UpdateEmailAddress',
    computed: {},
    data(): any {
      return {
        component: {
          processing: true,
          success: false,
        },
      };
    },
    methods: {
      checkToken(token: string): void {
        const ctc = new PatientsClient(process.env.VUE_APP_API_URL, axiosClient);
        ctc.getEmailToken(token)
          .then(() => {
            this.makeEmailUpdate(token);
          })
          .catch(() => {
            this.renderResults(false);
          });
      },
      makeEmailUpdate(token: string): void {
        const ctc = new PatientsClient(process.env.VUE_APP_API_URL, axiosClient);
        ctc.useEmailToken(token)
          .then(() => {
            this.renderResults();
          })
          .catch(() => {
            this.renderResults(false);
          });
      },
      renderResults(isSuccess = true): void {
        this.component.success = isSuccess;
        this.component.processing = false;
      },
      goToPortalHandler(): void {
        this.$router.push('/');
      },
    },
    mounted(): void {
      const token: string = this.$route.params.token;
      // if there is no token, will be redirected to the root page
      if (!token) {
        // this avoids potential unhandled router issues
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        this.$router.push('/').catch(() => {});
      } else {
        this.checkToken(token);
      }
    },
  });
</script>
