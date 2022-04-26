<template>
  <v-card class="DemoTelemedicineCard mb-4" flat rounded>
    <section>
      <v-overlay
        :absolute="true"
        :value="isCallFailed"
        :opacity="1"
        color="error"
        class="rounded"
        dark
      >
        <div class="text-h6">Call Failed</div>
      </v-overlay>
    </section>
    <v-row class="fill-height no-gutters">
      <v-col
        cols="8"
        class="align-self-center justify-start px-4 rounded-l fill-height primary"
        :class="
          callStarted && connectionStabilized
            ? 'green darken-2'
            : callStarted
            ? 'naturalGrey lighten-1'
            : ''
        "
      >
        <!-- default screen PRIMARY -->
        <v-sheet
          dark
          v-show="!callStarted"
          class="dialer fill-height transparent"
        >
          <v-row class="fill-height">
            <v-col cols="4" class="align-self-center">
              <v-select
                :items="contacts"
                :item-text="'name'"
                @change="selectedContactHandler"
                hide-details
                :value="selectedContact"
                label="Contact:"
              ></v-select>
            </v-col>
            <v-col cols="4" class="align-self-center">
              <v-text-field
                label="Relation:"
                :value="selectedContact ? selectedContact.relation : ''"
                hide-details
                class="disable-events"
              >
              </v-text-field>
            </v-col>
            <v-col cols="4" class="align-self-center">
              <v-text-field
                label="Number:"
                :value="selectedContact ? selectedContact.phone : ''"
                hide-details
                class="disable-events"
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-sheet>
        <!-- screen when a call is active GREY then GREEN -->
        <v-fade-transition :hide-on-leave="true">
          <v-sheet dark v-if="callStarted" class="transparent fill-height">
            <v-row class="fill-height d-flex justify-space-between">
              <v-col cols="12" md="auto" class="call-details align-self-center">
                <span class="text-h6 font-weight-bold" v-if="callStarted && !connectionStabilized">
                  Calling:
                </span>
                <span class="text-h6">
                  {{ selectedContact.name }}
                </span>
                <span class="text-h6">
                  - {{ selectedContact.relation }}
                </span>
                <span class="text-h6 hidden-lg-and-down">
                  - {{ selectedContact.phone }}
                </span>
              </v-col>
              <v-col
                cols="auto"
                class="align-self-center justify-end"
                v-if="callStarted && connectionStabilized"
              >
                <v-fade-transition :hide-on-leave="true">
                  <div
                    class="text-h4 text-right"
                  >
                    {{ callCounter }}
                  </div>
                </v-fade-transition>
              </v-col>
            </v-row>
          </v-sheet>
        </v-fade-transition>
      </v-col>
      <v-col
        cols="4"
        class="fill-height rounded-r px-4 justify-end flex-grow-1"
      >
        <!-- default call controls -->
        <section v-if="selectedContact && !callStarted" class="fill-height">
          <v-row class="fill-height justify-center pa-2">
            <v-col cols="auto" class="align-self-center px-0 my-1">
              <v-btn
                depressed
                large
                icon
                color="green"
                class="call-button mx-2"
                @click="startCallHandler"
              >
                <fa-icon :icon="['fas', 'phone']" class="fa-fw" />
              </v-btn>
              <v-btn
                depressed
                large
                icon
                class="call-button mx-2"
                @click="startCallHandler"
              >
                <fa-icon :icon="['fas', 'video']" class="fa-fw" />
              </v-btn>
            </v-col>
            <v-col cols="auto" class="align-self-center px-0 my-1">
              <v-btn depressed large icon class="call-button mx-2">
                <fa-icon :icon="['fas', 'comments']" class="fa-fw" />
              </v-btn>
              <v-btn depressed large icon class="call-button mx-2">
                <span class="font-weight-bold">911</span>
              </v-btn>
            </v-col>
          </v-row>
        </section>
        <!-- call controls when a call is active -->
        <section v-if="callStarted" class="fill-height">
          <v-row class="fill-height justify-center">
            <v-col cols="auto" class="align-self-center px-0">
              <v-btn
                depressed
                large
                icon
                color="red"
                class="call-button mx-2 my-sm-2"
                :class="{'call-anim': callStarted && !connectionStabilized}"
                @click="endCallHandler"
              >
                <fa-icon icon="phone-slash" class="fa-fw" />
              </v-btn>
              <v-btn depressed large icon class="call-button ma-2 my-sm-2">
                <fa-icon icon="phone-plus" class="fa-fw" />
              </v-btn>
            </v-col>
            <v-col cols="auto" class="align-self-center px-0">
              <v-btn depressed large icon class="call-button ma-2 my-sm-2">
                <fa-icon icon="arrow-right" class="fa-fw" />
              </v-btn>
              <v-btn depressed large icon class="call-button ma-2 my-sm-2">
                <span class="font-weight-bold">911</span>
              </v-btn>
            </v-col>
          </v-row>
        </section>
      </v-col>
    </v-row>
  </v-card>
</template>

<script lang="ts">
import Vue from 'vue';
import { mapMutations } from 'vuex';
import { ICallInformation } from '@/store/demo';

const CALL_INIT_DELAY = 3000;
const CALL_INTERVAL = 1000;
const CALL_FAILED_TIMEOUT = 2000;
const CALL_CONNECTION = {
  SUCCESS: 'SUCCESS',
  FAIL: 'FAIL',
};

interface IPerson {
  name: string;
  phone: string;
  relation: string;
  call: {
    ringTime: number;
    response: string;
  };
}

let timeout: any = null;
let interval: any = null;
// MATT to check: warning  'timeCounter' is assigned a value but never used
// eslint-disable-next-line
let timeCounter = 0;

export default Vue.extend({
  name: 'DemoTelemedicineCard',
  components: {},
  computed: {},
  watch: {},
  props: {
    storeName: String,
  },
  data() {
    const data: IPerson[] = [
      {
        name: 'Dr. Michael Vanier',
        phone: '+1 (905) 555-5424',
        relation: 'Primary Care Provider',
        call: {
          ringTime: CALL_INIT_DELAY,
          response: CALL_CONNECTION.SUCCESS,
        },
      },
      {
        name: 'Monique Alicia Caron',
        phone: '+1 (345) 888-7728',
        relation: 'Patient',
        call: {
          ringTime: CALL_INIT_DELAY,
          response: CALL_CONNECTION.SUCCESS,
        },
      },
      {
        name: 'Josh Caron',
        phone: '+1 (555) 888-5555',
        relation: 'Caregiver',
        call: {
          ringTime: 5000,
          response: CALL_CONNECTION.FAIL,
        },
      },
      {
        name: 'Sunny Side Long Term Care Facility',
        phone: '+1 (905) 555-2222',
        relation: 'Residence',
        call: {
          ringTime: CALL_INIT_DELAY,
          response: CALL_CONNECTION.SUCCESS,
        },
      },
    ];

    return {
      isCallFailed: false,
      selectedContact: data[1],
      contacts: data,
      callStarted: false,
      connectionStabilized: false,
      callCounter: '00:00',
    };
  },
  methods: {
    ...mapMutations('demo', {
      setCallInformation: 'setCallInformation',
    }),
    selectedContactHandler(selectedPerson: string): void {
      this.selectedContact = this.contacts.find(
        (item: IPerson) => item.name === selectedPerson
      );
    },
    startCallHandler(): void {
      const selectedContact: IPerson = this.selectedContact as IPerson;
      this.callStarted = true;
      if (timeout) {
        clearTimeout(timeout);
      }
      timeout = setTimeout(() => {
        this.connectionStabilized = true;
        timeout = null;

        if (selectedContact.call.response === CALL_CONNECTION.SUCCESS) {
          this.initCallCounter();
        } else {
          this.callFailed();
        }
      }, selectedContact.call.ringTime);
    },
    callFailed(): void {
      this.isCallFailed = true;
      this.resetCallCounter();
      this.connectionStabilized = false;
      this.callStarted = false;

      setTimeout(() => {
        this.isCallFailed = false;
      }, CALL_FAILED_TIMEOUT);
    },
    endCallHandler(): void {
      this.callStarted = false;
      this.connectionStabilized = false;
      this.resetCallCounter();
    },
    resetCallCounter() {
      if (interval) {
        clearInterval(interval);
      }
      timeCounter = 0;
      interval = null;
      this.callCounter = '00:00';
    },
    initCallCounter() {
      const startTime: Date = new Date();
      this.setCallData(startTime);

      interval = setInterval(() => {
        const endTime: Date = new Date();
        let timeDiff: number = endTime.getTime() - startTime.getTime();
        timeDiff = timeDiff / 1000;

        const seconds: number = Math.floor(timeDiff % 60);
        const secondsAsString: string =
          seconds < 10 ? '0' + seconds : seconds + '';

        timeDiff = Math.floor(timeDiff / 60);

        const minutes: number = timeDiff % 60;
        const minutesAsString: string =
          minutes < 10 ? '0' + minutes : minutes + '';

        timeDiff = Math.floor(timeDiff / 60);
        const hours: number = timeDiff % 24;

        timeDiff = Math.floor(timeDiff / 24);

        const days: number = timeDiff;

        const totalHours: number = hours + days * 24;
        const totalHoursAsString: string =
          totalHours < 10 ? '0' + totalHours : totalHours + '';

        let result = '00:00';
        if (totalHoursAsString === '00') {
          result = `${minutesAsString}:${secondsAsString}`;
        } else {
          result = `${totalHoursAsString}:${minutesAsString}:${secondsAsString}`;
        }

        this.callCounter = result;
      }, CALL_INTERVAL);
    },
    setCallData(startTime: Date) {
      const startTimeValue =
        startTime.getHours().toString().padStart(2, '0') +
        ':' +
        startTime.getMinutes().toString().padStart(2, '0') +
        ':' +
        startTime.getSeconds().toString().padStart(2, '0');

      const callModel: ICallInformation = {
        callStarted: true,
        time: startTimeValue,
        storeName: this.storeName,
      };
      this.setCallInformation(callModel);
    },
  },
});
</script>

<style lang='scss'>
@import '~vuetify/src/styles/main.sass';

.DemoTelemedicineCard {
  transition: background-color 200ms ease-out;
}
.dialer .disable-events.v-text-field input{
  text-overflow: ellipsis;
}
.dialer .disable-events.v-text-field > .v-input__control > .v-input__slot:before,
.dialer .disable-events.v-text-field > .v-input__control > .v-input__slot:after {
  border-style: none;
}
.dialer .v-text-field > .v-input__control > .v-input__slot:before,
.dialer .v-text-field > .v-input__control > .v-input__slot:after {
  border-color: rgba(255, 255, 255, 0.2) !important;
  border-style: thin 0 0 0 !important;
}
.call-button {
  background-color: var(--v-backgroundAlt-base);
}
.call-anim:before{
  animation: pulse 0.5s infinite;
  animation-direction: alternate;
  animation-timing-function: linear;
}
@keyframes pulse {
  0% {
    opacity: 0.5;
  }
  100% {
    opacity: 0.1;
  }
}
</style>
