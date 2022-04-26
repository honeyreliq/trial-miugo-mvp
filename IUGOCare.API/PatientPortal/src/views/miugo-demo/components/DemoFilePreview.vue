<template>
    <BaseCard v-if="file.name" class="flex">
      <v-row>
        <v-col cols="auto mb-4" class="flex-grow-1">
          <div class="text-h6">
            {{file.name}}
          </div>
          <div>
            {{formatStringFromTimestamp(file.date, 'L', file.time)}}
          </div>
        </v-col>
        <v-col cols="auto" class="flex-grow-0">
          <BaseButton color="primary" class="mr-4">
            <fa-icon :icon="['fal', 'download']" class="fa-fw"/>
          </BaseButton>
          <BaseButton color="primary">
            <fa-icon :icon="['fal', 'print']" class="fa-fw"/>
          </BaseButton>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12">
          <div class="mb-4">{{file.description}}</div>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12">
          <div v-if="file.type === 'application/pdf'" class="mb-4 doc-container">
            <embed
              class="doc-embed"
              :type="file.type"
              :src="file.src"
              :key="file.src"
            />
          </div>
          <div v-if="file.type === 'image/jpeg'" class="mb-4">
            <BaseLinkedImage
              :src="file.src"
              contain
              height="100%"
              position="center top"
            />
          </div>
        </v-col>
        <v-col>
          <div class="caption">{{file.upload}} {{file.date}}</div>
        </v-col>
      </v-row>
    </BaseCard>
</template>
<script lang="ts">
import Vue from 'vue';
import { mapGetters } from 'vuex';
import { IFile } from '@/store/demo';
import { timestampToString } from '@/views/miugo-demo/components/utils';

export default Vue.extend({
  name: 'DemoFilePreview',
  components: {},
  methods: {
    formatStringFromTimestamp(timestamp: number, format: string, time: string): string {
        return `${timestampToString(timestamp, format)} ` + ((time != null) ? `@ ${time}` : '');
    }
  },
  computed: {
    ...mapGetters('demo', ['selectedFile']),
  },
  watch: {
    selectedFile(file: IFile): void {
      if (file.fid >= 1 && file.fid <= 25) {
        this.file = file;
        // Records:
        if (file.fid === 1) {
          // Fake file submit for Demo
          this.file.description = 'Self-reported wound photo';
          this.file.upload = 'Uploaded by Monique Alicia Caron';
          this.file.src = '/miugo-demo/Wound Care@3x.png';
          this.file.type = 'image/jpeg';
        }

        if (file.fid === 2) {
          this.file.description =
            'Blood pressure trends from the past 3 months.';
          this.file.upload = 'Uploaded by Jackie Jones';
          this.file.src =
            '/miugo-demo/Blood.Pressure.3.Month.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
        if (file.fid === 3) {
          this.file.description =
            'Blood glucose trends from the past 3 months.';
          this.file.upload = 'Uploaded by Jackie Jones';
          this.file.src =
            '/miugo-demo/Blood.Glucose.3.Month.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
        if (file.fid === 4) {
          this.file.description = 'Image of my foot ulcer healing progress.';
          this.file.upload = 'Uploaded by Monique Alicia Caron';
          this.file.src = '/miugo-demo/left_foot_003.jpg';
          this.file.type = 'image/jpeg';
        }
        if (file.fid === 5) {
          this.file.description = 'Image of my foot ulcer healing progress.';
          this.file.upload = 'Uploaded by Monique Alicia Caron';
          this.file.src = '/miugo-demo/left_foot_002.jpg';
          this.file.type = 'image/jpeg';
        }
        if (file.fid === 6) {
          this.file.description = 'Image of my foot ulcer healing progress.';
          this.file.upload = 'Uploaded by Monique Alicia Caron';
          this.file.src = '/miugo-demo/left_foot_001.jpg';
          this.file.type = 'image/jpeg';
        }
        if (file.fid === 7) {
          this.file.description =
            'Information about Chronic Care Management and MiUGO.';
          this.file.upload = 'Uploaded by Jackie Jones';
          this.file.src =
            '/miugo-demo/Chronic.Care.Management.Agreement.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
        if (file.fid === 8) {
          this.file.description =
            'Patient consent agreement for CCM.';
          this.file.upload = 'Uploaded by Jackie Jones';
          this.file.src =
            '/miugo-demo/CCM.Consent.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
        if (file.fid === 9) {
          this.file.description =
            'Patient consent agreement for RPM.';
          this.file.upload = 'Uploaded by Jackie Jones';
          this.file.src =
            '/miugo-demo/RPM.Consent.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
        // Education:
        if (file.fid === 21) {
          this.file.description =
            'Educational material on how to measure your blood glucose levels';
          this.file.upload = 'Uploaded by Dr. Michael Vanier';
          this.file.src =
            '/miugo-demo/Generic.Blood.Glucose.One.Pager.Letter.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
        if (file.fid === 22) {
          this.file.description =
            'Educational material on how to measure your blood pressure levels';
          this.file.upload = 'Uploaded by Dr. Michael Vanier';
          this.file.src =
            '/miugo-demo/Generic.Blood.Pressure.One.Pager.Letter.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
        if (file.fid === 23) {
          this.file.description =
            'A patient educational document explaining hypertension';
          this.file.upload = 'Uploaded by Dr. Michael Vanier';
          this.file.src =
            '/miugo-demo/What.is.high.blood.pressure.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
        if (file.fid === 24) {
          this.file.description =
            'A patient educational document explaining diabetes';
          this.file.upload = 'Uploaded by Dr. Michael Vanier';
          this.file.src =
            '/miugo-demo/What.is.diabetes.pdf#toolbar=0&navpanes=0&scrollbar=0';
          this.file.type = 'application/pdf';
        }
      }
    },
  },
  data(): any {
    return {
      pdf: null,
      file: {
        name: '',
        date: null,
        time: '',
        category: '',
        description: '',
        upload: '',
        file: '',
        type: '',
      },
    };
  },
});
</script>
<style lang='scss'>
.doc-container {
  position: relative;
  width: 100%;
  height: 0;
  padding-bottom: 100%;
}
.doc-embed {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}
</style>
