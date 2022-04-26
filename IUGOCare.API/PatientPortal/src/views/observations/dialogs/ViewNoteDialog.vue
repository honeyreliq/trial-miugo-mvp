<template>
  <section>
    <v-dialog :value="value" persistent fixed max-width="600px" :eager="true">
      <v-card id="view-note-dialog">
        <BaseToolbar :title="$t('viewNote')" class="colorNote" dark />
        <v-card-text>
          <v-row>
            <v-col v-if="selectedNote.title" cols="12">
              <div class="text-h6 mb-4">
                  {{ selectedNote.title }} ({{ selectedNote.status }})
              </div>
            </v-col>
            <v-col v-if="selectedNote.timeEntry.timeSpent" cols="12" class="mb-4">
                <div class="caption" >
                    {{ selectedNote.timeEntry.program }} {{ $t('timeSpent') }}
                </div>
                <div class="text-subtitle-1">
                    {{ selectedNote.timeEntry.timeSpent }}
                </div>
            </v-col>
            <v-col cols="12">
                <div class="text-subtitle-1 mb-4">
                    {{ selectedNote.text }}
                </div>
            </v-col>
            <v-col cols="12">
              <div class="caption">{{ $t('tags') }}</div>
              <v-chip-group column class="disable-events mb-4">
              <v-chip
                  v-for="item in selectedNote.tags"
                  :key="item.id"
                  color="naturalGrey"
                  small
                  dark
                  label
              >
                  {{ item.text }}
              </v-chip>
              </v-chip-group>
            </v-col>
            <v-col cols="12" class="flex-grow-0 mb-4">
              <div>{{ $t('createdBy') }}: {{ selectedNote.author }} {{ $t('onDate') }} {{ formatDate(selectedNote.isoDateCreated, true) }}</div>
              <div>
              {{ $t('lastUpdated') }}: {{ formatDate(selectedNote.isoLastUpdated, true) }}
              </div>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions class="d-flex justify-end pt-0">
            <BaseButton text @click="cancel" :disabled="isSaving">
            {{ $t('cancel') }}
            </BaseButton>
            <BaseButton
                color="primary"
                @click="showEditNoteDialog = true"
                >{{ $t('edit') }}
            </BaseButton
            >
            <BaseButton color="primary">{{ $t('signAndSave') }}</BaseButton>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </section>
</template>

<script lang="ts">
import Vue from 'vue';
import { formatDate, formatSplicedDate } from '@/common/utils/datetime-helpers';

export default Vue.extend({
  name: 'ViewNoteDialog',
  props: {
    value: {
      type: Boolean,
      default: false,
    },
  },
  data(): any {
    return {
      showMenu: false,
      isSaving: false,
      savedSuccessfully: false,
      selectedNote: {
        "id": "2",
        "title": "Review of Medication changes",
        "status": "Draft",
        "author": "Smith, David",
        "timeEntry": {
            "program": "CCM",
            "timeSpent": "11 mins 23 secs"
        },
        "text": "Following hospital discharge, the patient's medication regime needs to be updated to eventually return to pre-hospitalization doses. Based on the provider's documentation, made necessary updates to patient's medications. Attempted to contact patient, unsuccessful as of today, will attempt again within 24 hours.",
        "tags": [
            {
                "id": "1",
                "text": "Education (Medication Adherence)"
            },
            {
                "id": "3",
                "text": "Care Coordination"
            },
            {
                "id": "4",
                "text": "Patient Care (Condition/Assessment)"
            }
        ],
        "isoDateCreated": "2021-04-12T12:38:10-04:00",
        "isoLastUpdated": "2021-04-11T16:14:10-04:00"
    },
    };
  },
  computed: {},
  methods: {
    formatDate,
    formatSplicedDate,
    cancel(): void {
      this.$emit('input');
      this.resetForm();
    },
    submitPatient(): void {
      this.isSaving = true;
      this.resetForm();
      this.savedSuccessfully = true;
      this.$emit('input');
    },
    resetForm(): void {
      //
    },
  },
});
</script>
