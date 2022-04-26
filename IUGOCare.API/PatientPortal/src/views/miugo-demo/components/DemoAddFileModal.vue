<template>
  <section>
    <v-dialog :value="value" persistent fixed max-width="600px" :eager="true">
      <v-card class="DemoAddFileModal">
        <BaseToolbar :title="$t('addRecord')" class="colorFiles" light></BaseToolbar>
        <v-card-text>
          <v-container>
            <!-- File upload input -->
            <v-row>
              <v-col cols="12" sm="8" class="d-flex align-center">
                <v-file-input
                    accept="*"
                    label="File"
                    v-model="fileData"
                    hide-details
                    class="mb-8"
                    @change="imageUpdateHandler"
                ></v-file-input>
              </v-col>
              <v-col cols="12" sm="4" class="d-flex justify-center justify-sm-end">
                <v-fade-transition :hide-on-leave="true">
                  <div class="file-preview d-flex justify-center align-center naturalGrey lighten-1"
                       :class="(file.type && file.type.includes('pdf')?'pdf': '')"
                  >
                    <embed v-if="file.type"
                           width="100%"
                           :type="file.type"
                           :src="file.url"/>
                    <div v-if="!file.type">
                      Preview
                    </div>
                  </div>
                </v-fade-transition>
              </v-col>
            </v-row>
            <!-- Category selector -->
            <v-row>
              <v-col cols="12 mt-8 mt-sm-4">
                <v-select
                    :items="categories"
                    v-model="selectedCategory"
                    label="Category"
                    dense
                ></v-select>
              </v-col>
            </v-row>
            <!-- Date picker -->
            <v-row>
              <v-col cols="12">
                <v-menu
                    v-model="menu"
                    :close-on-content-click="false"
                    max-width="290"
                >
                  <template v-slot:activator="{ on, attrs }">
                    <v-text-field
                        :value="computedDate"
                        label="Date"
                        readonly
                        v-bind="attrs"
                        v-on="on"
                        @click:clear="date = null"
                    >
                      <fa-icon :icon="['fal', 'calendar']" class="fa-fw" slot="prepend"/>
                    </v-text-field>
                  </template>
                  <v-date-picker
                      v-model="date"
                      @change="menu = false"
                  ></v-date-picker>
                </v-menu>
              </v-col>
            </v-row>
            <!-- Description -->
            <v-row>
              <v-col cols="12">
                <v-textarea
                    v-model="description"
                    rows="1"
                    label="Description"
                    auto-grow
                    hide-details
                    value=""
                ></v-textarea>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <BaseButton text @click="closeHandler" :disabled="isSaving">Cancel</BaseButton>
          <BaseButton color="primary" depressed @click="submitHandler" :disabled="!file.type || isSaving">Submit</BaseButton>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- save success -->
    <v-snackbar
        app
        class="pb-4"
        timeout="2000"
        v-model="saveSuccess"
        color="primary"
        center
        bottom
        dark>
        <div
          justify="center"
          align="center"
          class="text-subtitle-1">
           Your file was successfully submitted!
           <v-btn
              text
              to="/miugo/files">
                View File
          </v-btn>
        </div>


    </v-snackbar>
  </section>
</template>

<style lang="scss">
.DemoAddFileModal {
  .file-preview {
    height: 125px;
    width: 100%;
    overflow-y: auto;
  }

  .pdf {
    overflow-y: hidden !important;
  }
}
</style>

<script lang="ts">
import Vue from 'vue';
import moment from 'moment';
import { format, parseISO } from 'date-fns';


export default Vue.extend({
  name: 'DemoAddFileModal',
  components: {},
  computed: {
    computedDate(): string {
      return moment(this.date).format('MM/DD/YYYY');
    },
  },
  watch: {},
  props: ['value'],
  data(): any {
    return {
      isSaving: false,
      fileData: null,
      selectedCategory: null,
      saveSuccess: false,
      description: '',
      menu: false,
      date: format(parseISO(new Date().toISOString()), 'yyyy-MM-dd'),
      file: {
        url: null,
        type: null,
      },
      categories: ['Education', 'Record', 'Report', 'Wound Image', 'Other'],
    };
  },
  methods: {
    closeHandler(): void {
      this.$emit('input');
      this.cleanComponent();
    },
    submitHandler(): void {
      this.isSaving = true;
      setTimeout(() => {
        this.cleanComponent();
        this.saveSuccess = true;
        this.$emit('input');
        this.$root.$emit('file-submitted', true); // Fake file submit for Demo
      }, 1000);
    },
    imageUpdateHandler(): void {
      if (this.fileData) {
        this.file.type = this.fileData.type;
        this.file.url = `${URL.createObjectURL(this.fileData)}#toolbar=0&navpanes=0&scrollbar=0"`;
      } else {
        this.cleanComponent(0);
      }
    },
    cleanComponent(delay = 1000): void {
      setTimeout(() => {
        this.fileData = null;
        this.selectedCategory = null;
        this.description = '';
        this.isSaving = false;
        this.file = {
          url: null,
          type: null,
        };
      }, delay);
    },
  },
});
</script>
