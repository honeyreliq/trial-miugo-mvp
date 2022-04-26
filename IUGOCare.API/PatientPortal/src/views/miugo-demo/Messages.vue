<template>
  <v-container id="messages" fluid class="fill-height">
    <v-row class="fill-height">
      <v-col cols="12" md="3" class="d-flex pr-0">
        <v-sheet class="flex rounded-l border-right">
          <div
            class="d-flex flex-column justify-start align-stretch align-content-stretch fill-height"
          >
            <div
              class="flex-grow-0 msg-search d-flex px-4 align-center border-bottom"
            >
              <!---/Left search -->
              <v-text-field
                hide-details
                label="Search"
                class="mb-2"
                v-model="search"
              >
                <fa-icon
                  :icon="['far', 'search']"
                  class="fa-fw fa-button"
                  slot="append"
                />
              </v-text-field>
              <!---/Left search -->
            </div>
            <div class="flex-grow-1 msg-contacts">
              <!---/Left chat list -->
              <v-list two-line class="py-0">
                <v-list-item-group
                  mandatory
                  active-class="primary white--text"
                  v-model="selectedIndex"
                >
                  <template v-for="(item, index) in contact">
                    <v-list-item
                      :dark="isActive(item)"
                      :key="`survey-item-${index}`"
                      class="d-flex justify-end align-start border-bottom"
                      :class="{ 'disable-events': !item.messages.length > 0 }"
                      active-class="primary white--text"
                      @click="selectHandler(item)"
                    >
                      <v-list-item-avatar
                        size="42"
                        class="white--text darkGrey mr-4"
                        v-text="computedText(item.name)"
                      />
                      <v-list-item-content :three-line="true">
                        <v-list-item-title
                          v-text="item.name"
                        ></v-list-item-title>
                        <v-list-item-subtitle
                          v-text="item.preview"
                        ></v-list-item-subtitle>
                      </v-list-item-content>
                      <v-list-item-action-text
                        v-text="item.date"
                        class="mt-4"
                      ></v-list-item-action-text>
                    </v-list-item>
                  </template>
                </v-list-item-group>
              </v-list>
              <!---/Left chat list -->
            </div>
          </div>
        </v-sheet>
      </v-col>
      <v-col cols="9" class="d-md-flex pl-0 hidden-sm-and-down">
        <v-sheet class="flex rounded-r">
          <div
            class="d-flex flex-column justify-start align-stretch align-content-stretch fill-height"
          >
            <div
              class="flex-grow-0 msg-info d-flex px-4 align-center border-bottom"
            >
              <!---conversation header-->
              <v-list-item-avatar
                v-if="this.contact[this.selectedItem.id].image"
                size="42"
                class="white--text darkGrey mr-4"
                ><img
                  :src="
                    require(`@/assets/${
                      this.contact[this.selectedItem.id].image
                    }`)
                  "
                  :alt="this.contact[this.selectedItem.id].image"
              /></v-list-item-avatar>
              <v-list-item-avatar
                size="42"
                class="white--text darkGrey mr-4"
                v-text="computedText(this.contact[this.selectedItem.id].name)"
              />
              <div>
                {{ this.contact[this.selectedItem.id].name }} -
                {{ this.contact[this.selectedItem.id].information }}
              </div>
              <!---conversation header-->
            </div>
            <div class="flex-grow-1 pa-4 msg-messages border-bottom" ref="msgMessages" v-bind:style="msgStyle">
              <!---Chat Room-->
              <v-row
                v-for="(msg, index) in this.contact[this.selectedItem.id].messages"
                :key="index"
              >
                <v-col
                  cols="12"
                  class="d-flex"
                  :class="msg.type === 'send' ? 'justify-end' : 'justify-start'"

                >
                  <div class="text-caption mb-4">
                    {{ msg.date }}
                  </div>
                </v-col>
                <v-col
                  cols="12"
                  class="d-flex"
                  :class="msg.type === 'send' ? 'justify-end' : 'justify-start'"
                >
                  <BaseCard
                    :color="
                      msg.type === 'send'
                        ? 'primary white--text'
                        : 'var(--v-backgroundAlt-base)'
                    "
                    class="rounded-xl"
                    max-width="fit-content"
                  >
                    <div v-html="msg.message"></div>
                  </BaseCard>
                </v-col>
              </v-row>
              <!---Chat Room-->
            </div>
            <div class="pa-4 flex-grow-0 msg-input">
              <!---Send-->
              <BaseButton
                v-model="sendButton"
                class="mr-4 mb-8"
                color="primary"
                dark
                x-large
                absolute
                right
                bottom
                @click="sendNewMessage"
              >
                <fa-icon :icon="['fal', 'paper-plane']" class="fa-fw" />
              </BaseButton>
              <v-textarea
                rows="1"
                auto-grow
                placeholder="Type message"
                hide-details
                v-model="newMessage"
                class="text-body-1 mb-3"
              >
              </v-textarea>
              <!---Send-->
            </div>
          </div>
        </v-sheet>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue';
import moment from 'moment';
import { timestampToString, subtractFromToday } from '@/views/miugo-demo/components/utils';

interface IMessages {
  message: string;
  date: string;
  type: string;
}

interface IContact {
  id: number;
  name: string;
  information: string;
  preview: string;
  date: string;
  messages: IMessages[];
}

const data: IContact[] = [
  {
    id: 0,
    name: 'Dr. Michael Vanier',
    information: '+1 (905) 555-5424 - mvanier@goodhealth.com',
    preview: `That is normal, your blood pressure has a natural variability but when we consistently measure your
    bloodpressure...`,
    date: `${timestampToString(subtractFromToday(10), 'hh:mm A')}`,
    messages: [
      {
        message:
          'My blood pressure changes even when I take a measurement one after the other',
        date: `${timestampToString(subtractFromToday(0, 10), 'L')} @ 9:21 AM`,
        type: 'send',
      },
      {
        message:
          'That is normal, your blood pressure has a natural variability but when we consistently measure your blood pressure, it can tell us if you are consistently in the target range we are working towards. There are multiple factors that can contribute to the variation which is why it is important to take measurements once a day as your doctor has prescribed.',
        date: `${timestampToString(subtractFromToday(0, 9), 'L')} @ 11:03 AM`,
        type: 'receive',
      },
    ],
  },
  {
    id: 1,
    name: 'Jackie Jones',
    information: '+1 (905) 555-6789 - jjones@goodhealth.com',
    preview: 'No problem!',
    date: `${timestampToString(subtractFromToday(0, 2), 'ddd')}`,
    messages: [
      {
        message:
          'Hi Alicia, just a reminder that Dr. Smith would like you to do bloodwork before your appointment next month. Can you please come by the clinic this week? No fasting necessary.',
        date: `${timestampToString(subtractFromToday(0, 9), 'L')} @ 12:23 AM`,
        type: 'receive',
      },
      {
        message:
          'Hi Jackie, of course. I will come by next Tuesday after work around 3. Thanks for letting me know!',
        date: `${timestampToString(subtractFromToday(0, 10), 'L')} @ 1:11 PM`,
        type: 'send',
      },
      {
        message:
          'No problem!',
        date: `${timestampToString(subtractFromToday(0, 9), 'L')} @ 1:13 PM`,
        type: 'receive',
      },
    ],
  },
  {
    id: 2,
    name: 'Dr. Andrea Rodriguez',
    information: '+1 (905) 555-5424 - mvanier@goodhealth.com',
    preview: 'Please let me know if you have any questions.',
    date: `${timestampToString(subtractFromToday(0, 10), 'MMM D')}`,
    messages: [],
  },

  {
    id: 3,
    name: 'Anita Gomez',
    information: '+1 (905) 555-5424 - mvanier@goodhealth.com',
    preview: 'Your nutrition plan has been emailed to you. Thanks.',
    date: `${timestampToString(subtractFromToday(0, 20), 'MMM D')}`,
    messages: [],
  },
  {
    id: 4,
    name: 'Blaire Simmons',
    information: '+1 (905) 555-5424 - mvanier@goodhealth.com',
    preview: 'Your appointment has been cancelled. Please request a new one when you are ready.',
    date: `${timestampToString(subtractFromToday(0, 0, 13), 'l')}`,
    messages: [],
  },
];

export default Vue.extend({
  name: 'DemoMessages',
  components: {},
  props: {},
  data: () => ({
    appBarColor: {
      class: 'colorMessages',
      dark: true,
    },
    search: '',
    timeout: null,
    selectedItem: data[0],
    selectedIndex: 0, // vuetify v-list-item-group selected item control
    contact: data,
    newMessage: '',
    sendButton: false,
    msgStyle: {
      maxHeight: null,
    },
  }),
  methods: {
    selectHandler(item: IContact): void {
        this.selectedItem = item;
        this.$nextTick(() => {
          this.scrollToEnd();
        });
    },
    isActive(item: IContact): boolean {
      return (this.selectedItem && this.selectedItem.id === item.id);
    },
    sendNewMessage(): void {
      const mDate = moment().format('MM/DD/YYYY');
      const mTime = moment().format('hh:mm A');
      const formattedMessage = this.newMessage.replace(/\n/gi, '</br>');
      if (formattedMessage !== '</br>' && formattedMessage !== '') {
        const newMessage: any = {
          message: formattedMessage,
          date: mDate + ' @ ' + mTime,
          type: 'send',
        };
        this.newMessage = '';
        this.contact[this.selectedItem.id].messages.push(newMessage);
        this.$nextTick(() => {
          this.scrollToEnd();
        });
      }
    },
    scrollToEnd() {
      const content = this.$refs.msgMessages;
      // @ts-ignore
      content.scrollTop = content.scrollHeight;
    },
    setMsgSize() {
      // WIP. Needs to update on window resize. Not responsive
      const content = this.$refs.msgMessages;
      // @ts-ignore
      this.msgStyle.maxHeight = `${content.clientHeight}px`;
    },
    computedText(name: any) {
      let text = '';
      const regex = /\bdr[.\s]\s?/gi;
      name = name.replace(regex, '');

      name.split(' ').forEach((val: any) => {
        text += val.substring(0, 1);
      });

      return text;
    },
  },
  watch: {
    search(value: string): void {
      if (this.timeout) {
        clearTimeout(this.timeout);
        this.timeout = null;
      }
      if (value) {
        this.timeout = setTimeout(() => {
          this.selectedItem = data[0];
          this.selectedIndex = 0;
          this.contact = data.filter((item: IContact) => {
            return (item.name.toLocaleLowerCase().includes(value));
          });
        }, 400); // delay between key inputs
      } else {
        this.selectedItem = data[0];
        this.selectedIndex = 0;
        this.contact = data;
      }
    },
  },
  mounted() {
    this.$root.$emit('setAppBarColor', this.appBarColor);
    this.$nextTick(() => {
      this.setMsgSize();
    });

  },
  destroyed(): void {
    this.appBarColor.class = '';
    this.appBarColor.dark = false;
    this.$root.$emit('setAppBarColor', this.appBarColor);
  },
});
</script>
<style lang="scss" scoped>
.msg-search,
.msg-info{
  height: 92px;
}
.msg-input{
  min-height: 92px;
  margin-right: 60px;
}
.msg-messages{
  overflow: scroll;
}
</style>
