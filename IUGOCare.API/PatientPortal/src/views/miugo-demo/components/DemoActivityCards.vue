<template>
  <v-item-group>
    <v-row>
      <v-item v-slot:default="{ active, toggle }">
        <v-col cols="12" :md="active ? '6' : ''" class="d-flex">
          <BaseCard class="flex">
            <BaseLinkedImage
              :src="activityImg"
              :lazy-src="activityImg"
              cover
              width="100%"
              height="0"
              :class="{'aspect-ratio': !active}"
              class="mb-4 reset-responsive"
            />
            <v-row>
              <v-col cols="auto" class="text-h6 flex-grow-1"> {{ $t("activity") }} </v-col>
              <v-col cols="auto" class="d-flex justify-end">
                <BaseButton @click="toggle">
                  <fa-icon
                    :icon="['far', active ? 'times' : 'arrows-alt-h']"
                    class="fa-fw fa-button"
                  />
                </BaseButton>
              </v-col>
            </v-row>
            <div class="text-subtitle-1" v-show="!active">
              {{$t("activityCardShortText")}}
            </div>
            <div class="text-subtitle-1" v-show="active">
          {{$t("activityCardLongText")}}
            </div>
          </BaseCard>
        </v-col>
      </v-item>
      <v-item v-slot:default="{ active, toggle }">
        <v-col cols="12" :md="active ? '6' : ''" class="d-flex">
          <BaseCard class="flex">
            <BaseLinkedImage
              :src="sleepImg"
              :lazy-src="sleepImg"
              cover
              width="100%"
              height="0"
              :class="{'aspect-ratio': !active}"
              class="mb-4 reset-responsive"
            />
            <v-row>
              <v-col cols="auto" class="text-h6 flex-grow-1"> {{$t("sleep")}}</v-col>
              <v-col cols="auto" class="d-flex justify-end">
                <BaseButton @click="toggle">
                  <fa-icon
                    :icon="['far', active ? 'times' : 'arrows-alt-h']"
                    class="fa-fw fa-button"
                  />
                </BaseButton>
              </v-col>
            </v-row>
            <div class="text-subtitle-1" v-show="!active">
              {{$t("sleepCardShortText")}}
            </div>
            <div class="text-subtitle-1" v-show="active">
              {{$t("sleepCardLongText")}}
            </div>
          </BaseCard>
        </v-col>
      </v-item>
      <v-item v-slot:default="{ active, toggle }">
        <v-col cols="12" :md="active ? '6' : ''" class="d-flex">
          <BaseCard class="flex">
            <BaseLinkedImage
              :src="vitalsignsImg"
              :lazy-src="vitalsignsImg"
              cover
              width="100%"
              height="0"
              :class="{'aspect-ratio': !active}"
              class="mb-4 reset-responsive"
            />
            <v-row>
              <v-col cols="auto" class="text-h6 flex-grow-1">
                {{$t("vitalSigns")}}
              </v-col>
              <v-col cols="auto" class="d-flex justify-end">
                <BaseButton @click="toggle">
                  <fa-icon
                    :icon="['far', active ? 'times' : 'arrows-alt-h']"
                    class="fa-fw fa-button"
                  />
                </BaseButton>
              </v-col>
            </v-row>
            <div class="text-subtitle-1" v-show="!active">
              {{$t("vitalsCardShortText")}}
            </div>
            <div class="text-subtitle-1" v-show="active">
              {{$t("vitalsCardLongText")}}
            </div>
          </BaseCard>
        </v-col>
      </v-item>
    </v-row>
  </v-item-group>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
  name: 'DemoActivityCards',
  data() {
      return {
      activityImg: '/miugo-demo/dashboard_activity.jpg?1',
      sleepImg: '/miugo-demo/dashboard_sleep.jpg?1',
      vitalsignsImg: '/miugo-demo/dashboard_vitalsigns.jpg?1',
      };
  },
  methods: {},
  components: {},
});
</script>
<style lang="scss">
.reset-responsive .v-responsive__content{
  width: auto !important;
}
.aspect-ratio {
   position: relative;
   padding-top: 56.25%; /* 16:9 Aspect Ratio */
}
</style>
