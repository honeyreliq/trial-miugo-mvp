<template>
  <v-container fluid pa-0>
    <v-row align="end" no-gutters>
      <div class="text-subtitle-2">{{ label }}</div>
    </v-row>
    <v-row align="center" class="mb-0" no-gutters>
      <fa-icon :icon="['fal', 'clock']" class="fa-fw mr-2 mt-4"/>
      <v-col cols="auto">
        <v-menu
            bottom
            :disabled="!showList"
        >
          <template v-slot:activator="{ attrs, on }">
            <v-text-field
                v-bind="attrs"
                v-on="on"
                hide-details
                ref="hourField"
                color="primary"
                type="number"
                class="time-inputs"
                placeholder="––"
                v-model="hour"
                :min="(format === '12') ? 1 : 0"
                :max="(format === '12') ? 12 : 23"
                :rules="getHourRules()"
                :dense="dense"
                :filled="filled"
                :class="{'hide-spin-arrows': (hideArrows || showList)}"
                @keyup="hourKeyHandler($event, 'hour')"
                oninput="if((this.value.length > 2 && this.value[0] !== '0') || (this.value === '000')) this.value = this.value.slice(0,2);"
            >
            </v-text-field>
          </template>
          <v-list class="ma-0 pa-0" height="250">
            <v-list-item class="ma-0 pa-0 white" v-for="(item, i) in getHourList()" :key="`hour-list-${i}`">
              <BaseButton :class="{'primary': (item === hour)}"
                          @click="selectedTimeHandler('hour', item)">
                {{ item }}
              </BaseButton>
            </v-list-item>
          </v-list>
        </v-menu>


      </v-col>
      <span class="mx-1">:</span>
      <v-col cols="auto">
        <v-menu
            bottom
            :disabled="!showList"
        >
          <template v-slot:activator="{ attrs, on }">
            <v-text-field
                v-bind="attrs"
                v-on="on"
                hide-details
                ref="minuteField"
                color="primary"
                type="number"
                min="0"
                max="59"
                maxlength="2"
                class="time-inputs"
                placeholder="––"
                v-model="minute"
                :rules="(!hideDetails || hideDetails === 'auto') ? minuteRules : []"
                :dense="dense"
                :filled="filled"
                :class="{'hide-spin-arrows': (hideArrows || showList)}"
                @keyup="hourKeyHandler($event, 'minute')"
                oninput="if((this.value.length > 2 && this.value[0] !== '0') || (this.value === '000')) this.value = this.value.slice(0,2);"
            >
            </v-text-field>
          </template>
          <v-list class="ma-0 pa-0" height="250">
            <v-list-item class="ma-0 pa-0 white" v-for="(item, i) in getMinuteList()" :key="`minute-list-${i}`">
              <BaseButton :class="{'primary': (item === minute)}"
                          @click="selectedTimeHandler('minute', item)">
                {{ item }}
              </BaseButton>
            </v-list-item>
          </v-list>
        </v-menu>
        <v-spacer></v-spacer>
      </v-col>

      <v-col cols="auto" class="mt-2 ml-1" v-if="(format === '12')">
        <BaseButtonGroup
            :value="time.amPm"
            class="ml-2"
            :dense="dense"
        >
          <BaseButton v-for="(item, i) in ['AM', 'PM']"
            :key="`am-pm${i}`"
            :value="item"
            :class="{'button-dense': dense}"
            @click="selectAmPmHandler(item)"
          >{{ item }}
          </BaseButton>
        </BaseButtonGroup>
      </v-col>
    </v-row>
    <v-messages v-if="(hideDetails === 'auto') ? (errors.length > 0) : !hideDetails"
                :value="errors"
                color="error"
                class="mt-3">
    </v-messages>
  </v-container>
</template>

<script lang="ts">
import { isInteger, isBetween } from '../../validation';
import Vue, { PropType } from 'vue';

export default Vue.extend({
  name: 'BaseTimePicker',
  data() {
    return {
      time: {
        hour: '',
        minute: '',
        amPm: 'AM',
      },
      minuteRules: (!this.hideDetails || this.hideDetails === 'auto') ? [isInteger, isBetween(0, 59)] : [],
      errors: [],
    };
  },
  computed: {
    minute: {
      get(): string {
        return this.getTimeDigit('minute');
      },
      set(val: string): void {
        this.time.minute = val;
      },
    },
    hour: {
      get(): string {
        return this.getTimeDigit('hour');
      },
      set(val: string): void {
        this.time.hour = val;
      },
    },
  },
  props: {
    label: {
      type: String,
      default: '',
    },
    hideDetails: {
      type: [Boolean, String] as PropType<boolean | 'auto'>,
      default: 'auto',
    },
    hideArrows: Boolean,
    showList: Boolean,
    dense: Boolean,
    filled: Boolean,
    format: {
      type: String,
      default: '12',
      validator(value: string): boolean {
        return (value === '12' || value === '24');
      },
    },
    initialValue: {
      type: String,
      default: null,
    },
  },
  watch: {
    hour(): void {
      this.emitInput();
    },
    minute(): void {
      this.emitInput();
    },
    format(): void {
      this.errors = [];
      this.time.hour = null;
      this.time.minute = null;
      // @ts-ignore
      this.$refs.hourField.reset();
      // @ts-ignore
      this.$refs.minuteField.reset();
    },
    hideDetails(): void {
      this.validateFields(null);
    },
  },
  methods: {
    getTimeDigit(key: 'minute' | 'hour'): string {
      const digit: number = parseInt(this.time[key], 10);
      if (isNaN(digit)) {
        return '';
      }

      return digit < 10 ? `0${digit}` : digit.toString();
    },
    emitInput(): void {
      this.validateFields(() => {
        if (this.hour && this.minute) {
          const hour: string = this.hour ? this.hour : '00';
          const minute: string = this.minute ? this.minute : '00';
          const amPm: string = (this.format === '12') ? ` ${this.time.amPm}` : '';

          this.$emit('input', `${hour}:${minute}${amPm}`);
        } else {
          this.$emit('input', null);
        }
      });
    },
    getHourList(): string[] {
      const list: string[] = [];
      const length: number = (this.format === '12') ? 12 : 23;
      let i: number = (this.format === '12') ? 1 : 0;

      for (i; i <= length; i++) {
        list.push((i < 10) ? `0${i}` : i.toString());
      }
      return list;
    },
    getMinuteList(): string[] {
      const list: string[] = [];
      for (let i = 0; i < 60; i++) {
        list.push((i < 10) ? `0${i}` : i.toString());
      }
      return list;
    },
    getHourRules(): any[] {
      const hourRules: any = (this.format === '24') ? isBetween(0, 23) : isBetween(1, 12);
      return (!this.hideDetails || this.hideDetails === 'auto') ? [isInteger, hourRules] : [];
    },
    validateFields(cb: () => any): void {
      // async updates are made when this is fired, need to take control over event loop
      setTimeout(() => {
        this.errors = [];
        // @ts-ignore
        this.$refs.hourField.validate();
        // @ts-ignore
        this.$refs.minuteField.validate();
        // @ts-ignore
        if (this.$refs.hourField.valid && this.$refs.minuteField.valid) {
          if (cb) {
            cb();
          }
        } else {
          this.errors = [
            // @ts-ignore
            ...this.$refs.hourField.messagesToDisplay,
            // @ts-ignore
            ...this.$refs.minuteField.messagesToDisplay,
          ];
        }
      }, 0);
    },

    // event handlers
    selectedTimeHandler(key: string, value: string): void {
      // @ts-ignore
      this.time[key] = value;
    },
    selectAmPmHandler(val: string): void {
      this.time.amPm = val;
      this.emitInput();
    },
    // focus next text-field item
    hourKeyHandler(event: KeyboardEvent, key: string): void {
      // @ts-ignore
      const value: string = this[key];
      if (value.length === 2 && value[0] !== '0') {
        const ref: any = (key === 'hour') ? this.$refs.minuteField : this.$refs.hourField;
        if (!ref.value || key === 'hour') {
          ref.focus();
        }
      }
    },
  },
  mounted() {
    if (this.initialValue) {
      const values: string[] = this.initialValue.split(':');
      this.hour = values[0];
      this.minute = values[1];
    }
  },
});
</script>
<style lang="scss">
// avoid issues with showList or hideArrows are set
.time-inputs {
  min-width: 42px;
}

.hide-spin-arrows {
  input[type=number]::-webkit-inner-spin-button,
  input[type=number]::-webkit-outer-spin-button {
    -webkit-appearance: none;
  }

  input[type=number] {
    -moz-appearance: textfield;
  }
}

.time-list {
  border: 1px solid red;
  z-index: 100;
}

.button-dense {
  // TODO: dense styles for AM/PM buttons goes here
}
</style>
