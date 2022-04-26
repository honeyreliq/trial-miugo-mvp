<template>
  <v-container>
    <div class="pa-4">
      <v-row justify="center">
        <v-col cols="12" sm="10" xl="8">
          <h1
            class="font-weight-regular mt-4"
          >{{ $t('completeRegistrationTitle') }}</h1>
          <h4>{{ $t('completeRegistrationMsg') }}</h4>

          <v-form ref="form" v-model="valid" action="/views/boxedlogin">
            <v-text-field v-model="userMail" :label="$t('email')" readonly></v-text-field>
            <v-text-field
              v-model="password"
              :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
              :rules="passwordRules"
              hide-details="auto"
              :type="showPassword ? 'text' : 'password'"
              counter
              required
              name="input-10-1"
              :label="$t('passwordPlaceholder')"
              @click:append="showPassword = !showPassword"
              :hint="$t('required')"
              persistent-hint
              ref="password"
            ></v-text-field>
            <v-checkbox v-model="optIn" hide-details>
              <div slot="label">{{ $t('iAgreeToLbl') + $t('optInMsg') }}</div>
            </v-checkbox>
            <v-checkbox
              v-model="termsOfService"
              :rules="[v => !!v || $t('tosErrorMsg')]"
              required
              hide-details="auto"
            >
              <v-dialog v-model="tosDialog" scrollable width="600px" slot="label">
                <template v-slot:activator="{ on }" class="text-caption">
                  <div slot="label">
                    {{ $t('iAgreeToLbl') }}
                    <a @click.stop v-on="on">{{ $t('tosMsg') }}</a>
                  </div>
                </template>
                <v-card>
                  <v-toolbar dark color="primary" elevation="0">
                    <v-toolbar-title>{{ $t('tosMsg') }}</v-toolbar-title>
                    <v-spacer></v-spacer>
                    <v-btn icon dark @click="tosDialog = false">
                      <fa-icon :icon="['fal', 'times']" class="fa-fw" />
                    </v-btn>
                  </v-toolbar>
                  <v-card-text v-html="terms" />
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="primary" elevation="0" block @click="tosDialog = false">Close</v-btn>
                  </v-card-actions>
                </v-card>
              </v-dialog>
            </v-checkbox>
            <v-btn
              class="outlined_button primary my-4"
              :loading="loading"
              :disabled="!valid || patientId === ''"
              width="100%"
              @click="patientAcceptsToS()"
            >{{ $t('registerAccountBtn')}}</v-btn>
            <v-alert type="success" dense outlined dismissible v-model="showAlert">{{ alertMessage }}</v-alert>
            <v-alert type="error" dense outlined dismissible v-model="showErrorAlert">{{ alertMessage }}</v-alert>
          </v-form>

        </v-col>
      </v-row>
    </div>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue';
  import {PatientsClient, PatientAcceptsToSCommand, PatientQueryFilter,
           EnableMarketingEmailsCommand } from '../../IUGOCare-api';

  export default Vue.extend({
    name: 'ActivateAccount',
    data() {
      return {
        patientId: '',
        userMail: '',
        loader: null,
        loading: false,
        valid: false,
        showPassword: false,
        showConfirmPwd: false,
        password: '',
        termsOfService: false,
        optIn: false,
        tosDialog: false,
        showAlert: false,
        showErrorAlert: false,
        alertMessage: '',
        terms: '<h2>Quicquid porro animo cernimus, id omne oritur a sensibus;</h2><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quod si ita sit, cur opera philosophiae sit danda nescio. Superiores tres erant, quae esse possent, quarum est una sola defensa, eaque vehementer. Inde sermone vario sex illa a Dipylo stadia confecimus. Bonum patria: miserum exilium. <i>Nos commodius agimus.</i> Duo Reges: constructio interrete. </p><p>Velut ego nunc moveor. At ille non pertimuit saneque fidenter: Istis quidem ipsis verbis, inquit; Nec lapathi suavitatem acupenseri Galloni Laelius anteponebat, sed suavitatem ipsam neglegebat; Profectus in exilium Tubulus statim nec respondere ausus; <b>Duo enim genera quae erant, fecit tria.</b> Eaedem enim utilitates poterunt eas labefactare atque pervertere. Qua tu etiam inprudens utebare non numquam. Si enim ad populum me vocas, eum. Hoc ille tuus non vult omnibusque ex rebus voluptatem quasi mercedem exigit. </p><h2>Quo plebiscito decreta a senatu est consuli quaestio Cn.</h2><p>Praeclare enim Plato: Beatum, cui etiam in senectute contigerit, ut sapientiam verasque opiniones assequi possit. Quod maxime efficit Theophrasti de beata vita liber, in quo multum admodum fortunae datur. Et harum quidem rerum facilis est et expedita distinctio. Aliter autem vobis placet. <strong>Quid est igitur, inquit, quod requiras?</strong> Quas enim kakaw Graeci appellant, vitia malo quam malitias nominare. </p><p>Quicquid porro animo cernimus, id omne oritur a sensibus; Mihi, inquam, qui te id ipsum rogavi? Conclusum est enim contra Cyrenaicos satis acute, nihil ad Epicurum. <strong>Verum hoc idem saepe faciamus.</strong> Nulla profecto est, quin suam vim retineat a primo ad extremum. Videsne quam sit magna dissensio? </p><dl><dt><dfn>Sed fortuna fortis;</dfn></dt><dd>Mihi enim satis est, ipsis non satis.</dd><dt><dfn>Recte, inquit, intellegis.</dfn></dt><dd>At multis se probavit.</dd><dt><dfn>Sed haec omittamus;</dfn></dt><dd>Quid affers, cur Thorius, cur Caius Postumius, cur omnium horum magister, Orata, non iucundissime vixerit?</dd></dl><ul><li>Illa argumenta propria videamus, cur omnia sint paria peccata.</li><li>Quae similitudo in genere etiam humano apparet.</li></ul><h3>Eaedem enim utilitates poterunt eas labefactare atque pervertere.</h3><p>Cum audissem Antiochum, Brute, ut solebam, cum M. Nam quibus rebus efficiuntur voluptates, eae non sunt in potestate sapientis. Magna laus. At quicum ioca seria, ut dicitur, quicum arcana, quicum occulta omnia? Facillimum id quidem est, inquam. Peccata paria. Tu autem negas fortem esse quemquam posse, qui dolorem malum putet. <b>Sint ista Graecorum;</b> Isto modo ne improbos quidem, si essent boni viri. Quod non faceret, si in voluptate summum bonum poneret. </p>',
      };
    },
    props: ['code'],
    watch: {
      loader() {
        this.loader = !this.loader;

        setTimeout(() => (this.loader = false), 3000);
        this.loader = null;
      },
    },
    methods: {
      async patientAcceptsToS() {
        this.showAlert = false;
        this.showErrorAlert = false;

        if (this.patientId !== '') {
          const command = new PatientAcceptsToSCommand();
          command.patientId = this.patientId;
          command.password = this.password;

          const ctc = new PatientsClient(process.env.VUE_APP_API_URL);
          await ctc.activatePatient(command).then(
            () => {
              this.enableMarketingEmails();
              this.clearForm();
              this.alertMessage = this.$t('patientActivatedMsg').toString();
              this.showAlert = true;
              setTimeout(() => {
                this.showAlert = false;
                this.$router.push({ name: 'authentication'});
              }, 2000);
            })
            .catch((ex) => {
              this.alertMessage = 'Error: ' + ex.status + ' ' + ex.message;

              if (ex.response.errors) {
                const errors = ex.response.errors;
                errors.forEach((e: any) => this.alertMessage += e);
              }
              this.showErrorAlert = true;
            });
        }
      },
      enableMarketingEmails(): void {
        if (this.patientId !== '' && this.optIn) {
          const command = new EnableMarketingEmailsCommand();
          command.patientId = this.patientId;

          const ctc = new PatientsClient(process.env.VUE_APP_API_URL);
          ctc.enableMarketingEmails(command);
        }
      },
      clearForm(): void {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        this.$refs.form.resetValidation();
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        this.$refs.form.reset();
        this.userMail = '';
        this.password = '';
        this.termsOfService = false;
        this.optIn = false;
      },
    },
    computed: {
      passwordRules(): any {
        return [(v: string) => !!v || this.$t('passwordRequiredMsg'),
          (v: string) => v.length >= 8 || this.$t('minCharErrorMsg')];
      },
    },
    async created() {
      this.patientId = '';
      this.$vuetify.theme.dark = false;
      const ctc = new PatientsClient(process.env.VUE_APP_API_URL);
      const vm = await ctc.getPatients(PatientQueryFilter.InactiveOnly);
      const patient = vm.patients
        .find((p) => p.activationCode === (this.code || this.$route.params.code));

      if (patient) {
        this.patientId = patient.id;
        this.userMail = patient.emailAddress;
        this.$i18n.locale = patient.patientLanguage.toLowerCase();

        if (patient.emailAddress) {
          // @ts-ignore
          this.$refs.password.focus();
        }
      } else {
        this.alertMessage = this.$t('activationCodeInvalid').toString();
        this.showErrorAlert = true;
      }
    },
    mounted() {
      this.$root.$on('change_locale', () => {
        // @ts-ignore - Support for Vuetify form validation for TS set for v3
        this.$refs.form.resetValidation();
      });
    },
  });
</script>
