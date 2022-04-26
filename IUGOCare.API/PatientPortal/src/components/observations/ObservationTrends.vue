<template>
  <div>
    <div align="center" v-if="observationType === 'blood-pressure'">
      <fa-icon v-if="change.split(',')[0] === 'Rising'" :icon="['fas', 'arrow-alt-up']" class="fa-fw fa-button" />
      <fa-icon v-if="change.split(',')[0] === 'Falling'" :icon="['fas', 'arrow-alt-down']" class="fa-fw fa-button" />
      <fa-icon v-if="change.split(',')[0] === 'Steady'" :icon="['fal', 'minus']" class="fa-fw fa-button" />/
      <fa-icon v-if="change.split(',')[1] === 'Rising'" :icon="['fas', 'arrow-alt-up']" class="fa-fw fa-button" />
      <fa-icon v-if="change.split(',')[1] === 'Falling'" :icon="['fas', 'arrow-alt-down']" class="fa-fw fa-button" />
      <fa-icon v-if="change.split(',')[1] === 'Steady'" :icon="['fal', 'minus']" class="fa-fw fa-button" />
    </div>
    <div align="center" v-else-if="observationType === 'heart-rate' && change.split(',').length >= 3">
      <fa-icon v-if="change.split(',')[2] === 'Rising'" :icon="['fas', 'arrow-alt-up']" class="fa-fw fa-button" />
      <fa-icon v-if="change.split(',')[2] === 'Falling'" :icon="['fas', 'arrow-alt-down']" class="fa-fw fa-button" />
      <fa-icon v-if="change.split(',')[2] === 'Steady'" :icon="['fal', 'minus']" class="fa-fw fa-button" />
    </div>
    <div align="center" v-else>
      <fa-icon v-if="change === 'Rising'" :icon="['fas', 'arrow-alt-up']" class="fa-fw fa-button" />
      <fa-icon v-if="change === 'Falling'" :icon="['fas', 'arrow-alt-down']" class="fa-fw fa-button" />
      <fa-icon v-if="change === 'Steady'" :icon="['fal', 'minus']" class="fa-fw fa-button" />
    </div>
  </div>
</template>

<script lang="ts">
export default {
  props: ['observationType', 'change'],
};
</script>
