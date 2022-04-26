<template>
  <BaseCard>
    <v-form>
       <section>
        <div class="text-h6">Test Fields</div>
        <div class="text-subtitle-2">Test Fields</div>
        <v-text-field
          label="Name"
          required
        ></v-text-field>

        <v-text-field
          label="E-mail"
          required
        ></v-text-field>
      </section>
      <section>
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Test Fields Columns</div>
        <div class="text-subtitle-2">Test Fields Columns</div>
        <v-row>
          <v-col cols="12" sm="6" lg="6">
            <v-text-field label="First Name"></v-text-field>
            <v-text-field label="Email"></v-text-field>
          </v-col>
          <v-col cols="12" sm="6" lg="6">
            <v-text-field label="Last Name"></v-text-field>
            <v-text-field label="Contact"></v-text-field>
          </v-col>
        </v-row>
      </section>
      <section>
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Checkboxes</div>
        <div class="text-subtitle-2">Checkboxes</div>
        <v-checkbox
          label="Checkbox 1"
          hide-details
        ></v-checkbox>
        <v-checkbox
          label="Checkbox 2"
          hide-details
        ></v-checkbox>
        <v-checkbox
          label="Checkbox 3"
          hide-details
        ></v-checkbox>
      </section>
      <section>
        <v-divider class="my-6"></v-divider>
        <div class="text-h6">Radio Buttons</div>
        <div class="text-subtitle-2">Radio Buttons</div>
        <v-row>
          <v-col cols="12" sm="6" md="6">
            <v-radio-group column>
              <v-radio
                label="red"
                color="red"
                value="red"
              ></v-radio>
              <v-radio
                label="red darken-3"
                color="red darken-3"
                value="red darken-3"
              ></v-radio>
              <v-radio
                label="indigo"
                color="indigo"
                value="indigo"
              ></v-radio>
            </v-radio-group>
          </v-col>
          <v-col cols="12" sm="6" md="6">
            <v-radio-group column>
              <v-radio
                label="primary"
                color="primary"
                value="primary"
              ></v-radio>
              <v-radio
                label="secondary"
                color="secondary"
                value="secondary"
              ></v-radio>
              <v-radio
                label="success"
                color="success"
                value="success"
              ></v-radio>
            </v-radio-group>
          </v-col>
        </v-row>
      </section>
    </v-form>
  </BaseCard>
</template>
<script>
export default {
  name: 'DemoFormTemplate',
  data: () => ({

  }),
};
</script>
