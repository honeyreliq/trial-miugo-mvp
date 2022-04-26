<template>
  <!-- TODO: Set the `server-items-length="serverItemsLength" prop when the API exists -->
  <v-data-table
    v-model="selected"
    :headers="dataHeaders"
    :items="rows"
    :item-key="itemKey"
    :item-class="rowClass"
    class="striped"
    :search="search"
    :sort-by="sortBy"
    :sort-desc="sortDesc"
    :disable-pagination="disablePagination"
    :hide-default-footer="disablePagination"
    :footer-props="footerProps"
    :show-select="showSelect"
    @click:row="rowClicked"
    :options.sync="options"
    v-bind="$attrs"
  >
    <!-- Built-in search bar -->
    <template v-slot:top v-if="showSearch">
      <v-row no-gutters>
        <v-col
          cols="auto"
          class="flex-grow-1 align-self-center"
          id="table-search"
        >
          <v-text-field
            hide-details
            filled
            clearable
            v-model="search"
            :label="$t('search')"
            ref="searchTextField"
          >
            <fa-icon
              :icon="['far', 'search']"
              class="fa-fw fa-button mr-4 ml-1 mt-1"
              slot="prepend-inner"
            />
            <template v-slot:append>
              <v-menu
                v-if="addAdvancedSearch"
                v-model="advancedSearchMenu"
                :close-on-content-click="false"
                offset-y
                left
                bottom
                transition="slide-y-transition"
                attach="#table-search"
                min-width="100%"
                @keydown.esc="advancedSearchMenu = false"
              >
                <template v-slot:activator="{ on, attrs }">
                  <BaseButton
                    class="mt-n2"
                    plain
                    icon
                    @click.stop="showAdvancedSearch"
                    v-bind="attrs"
                    v-on="on"
                  >
                    <fa-icon
                      :icon="[
                        'fas',
                        advancedSearchMenu ? 'caret-up' : 'caret-down',
                      ]"
                      class="fa-fw fa-button"
                    />
                  </BaseButton>
                </template>
                <v-card>
                  <BaseToolbar
                    :title="$t('advancedSearch')"
                    class="colorPatientChart"
                    dark
                  />
                  <v-container>
                    <v-text-field
                      hide-details
                      filled
                      clearable
                      v-model="search"
                      ref="advancedSearchTextField"
                      :label="$t('advancedSearch')"
                      class="mb-4"
                    >
                      <fa-icon
                        :icon="['far', 'search']"
                        class="fa-fw fa-button mr-4 ml-1 mt-1"
                        slot="prepend-inner"
                      />
                    </v-text-field>
                    <v-skeleton-loader
                      type="table-heading, table-row-divider@3"
                    />
                  </v-container>
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <BaseButton text @click="cancel" :disabled="isSaving">
                      {{ $t('close') }}
                    </BaseButton>
                    <BaseButton color="primary" depressed @click="submitSearch">
                      {{ $t('search') }}
                    </BaseButton>
                  </v-card-actions>
                </v-card>
              </v-menu>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
    </template>

    <!-- Filter buttons -->
    <template
      v-for="(filter, i) in filters"
      v-slot:[`header.${i}`]="{ header }"
    >
      <div :key="i" class="d-inline">
        <span>
          {{ header.text }}
        </span>
        <span>
          <v-menu
            v-model="showMenu[header.value]"
            :close-on-content-click="false"
            offset-y
            transition="slide-y-transition"
            left
            fixed
          >
            <template v-slot:activator="{ on, attrs }">
              <span v-bind="attrs" v-on="on">
                <v-icon
                  small
                  :color="
                    activeFilters[header.value] &&
                    activeFilters[header.value].length > 0
                      ? 'blue'
                      : 'default'
                  "
                >
                  mdi-filter-variant
                </v-icon>
              </span>
            </template>

            <v-card>
              <v-list flat class="pa-0">
                <v-list-item-group
                  :multiple="multiFilter"
                  v-model="activeFilters[header.value]"
                >
                  <template v-for="item in filters[header.value]">
                    <v-list-item :key="item" :value="item" active>
                      <template v-slot:default="{ active }">
                        <v-list-item-action class="mr-2" v-if="multiFilter">
                          <v-checkbox
                            :input-value="active"
                            :true-value="item"
                            color="primary"
                            :ripple="false"
                            dense
                          ></v-checkbox>
                        </v-list-item-action>
                        <v-list-item-content>
                          <v-list-item-title v-text="item"></v-list-item-title>
                        </v-list-item-content>
                      </template>
                    </v-list-item>
                  </template>
                </v-list-item-group>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <BaseButton
                    v-if="multiFilter"
                    @click="toggleAll(header.value)"
                    >Select all</BaseButton
                  >
                  <BaseButton @click="reset(header.value)">Reset</BaseButton>
                </v-card-actions>
              </v-list>
            </v-card>
          </v-menu>
        </span>
      </div>
    </template>
    <!-- Actions bar -->
    <template v-slot:[`body.prepend`]="{ headers }" v-if="showActions">
      <tr class="transparent actions-bar">
        <td :colspan="headers.length" class="border-bottom">
          <v-row align="center" class="d-flex mt-4">
            <v-col
              cols="auto"
              class="hidden-sm-and-down order-0"
              v-if="actions.some((a) => a.isQuickAction)"
            >
              <template v-for="(action, i) in quickActions">
                <v-tooltip bottom :key="action.name">
                  <template v-slot:activator="{ on, attrs }">
                    <BaseButton
                      @click="applyAction(action.name)"
                      :class="i == quickActions.length ? 'mb-4' : 'mb-4 mr-2'"
                      v-bind="attrs"
                      v-on="on"
                    >
                      <fa-icon :icon="['fal', action.icon]" class="fa-fw" />
                    </BaseButton>
                  </template>
                  <span>{{ action.name }}</span>
                </v-tooltip>
              </template>
            </v-col>
            <v-col
              cols="auto"
              class="flex-grow-1 flex-sm-grow-0 order-1"
              v-if="actions.some((a) => !a.isQuickAction)"
            >
              <v-select
                v-model="actionSelect"
                :items="actionsSelectList"
                item-text="name"
                item-value="name"
                :menu-props="{ bottom: true, offsetY: true }"
                :label="$t('actions')"
                no-data-text="No actions exist for this table"
                hide-details="true"
                single-line
                filled
                clearable
                class="action-select mb-4"
              ></v-select>
            </v-col>
            <v-col cols="auto" class="order-2" v-if="actions.some((a) => !a.isQuickAction)">
              <BaseButton
                color="primary"
                class="mb-4"
                :disabled="!actionSelect"
                @click="applyAction"
              >
                {{ $t('apply') }}
              </BaseButton>
            </v-col>
            <v-col cols="auto" class="order-3 order-sm-4">
              <BaseButton
                class="mb-4 mr-2"
                @click="
                  selected = [];
                  selectAll = false;
                "
              >
                {{ $t('clearSelection') }}
              </BaseButton>
              <BaseButton
                v-if="!selectAll"
                @click="selectAll = true"
                class="mb-4"
              >
                {{
                  serverItemsLength == 1
                    ? $tc(`${itemType}ActionBarSelect`, 1)
                    : $tc(`${itemType}ActionBarSelect`, serverItemsLength)
                }}
              </BaseButton>
            </v-col>
            <v-col cols="auto" v-if="!selectAll" class="order-4 order-sm-3">
              <span class="mb-4 d-flex align-center">
                {{ selected.length == 1 ? $tc(`${itemType}ActionBarSelected`, 1) : $tc(`${itemType}ActionBarSelected`, selected.length) }}
              </span>
            </v-col>
            <v-col cols="auto" v-if="selectAll" class="order-4 order-sm-3">
              <span class="mb-4 d-flex align-center">
                All {{ serverItemsLength == 1 ? $tc(`${itemType}ActionBarSelected`, 1) : $tc(`${itemType}ActionBarSelected`, serverItemsLength) }}
              </span>
            </v-col>
          </v-row>
        </td>
      </tr>
    </template>
    <template
      v-slot:[`header.data-table-select`]="{ on, props }"
      v-if="showSelect"
    >
      <!-- v-ripple="false" work around https://github.com/vuetifyjs/vuetify/issues/12224 -->
      <v-simple-checkbox
        color="primary"
        v-bind="props"
        v-on="on"
        v-ripple="false"
      ></v-simple-checkbox>
    </template>
    <template
      v-slot:[`item.data-table-select`]="{ isSelected, select }"
      v-if="showSelect"
    >
      <v-simple-checkbox
        color="primary"
        :value="isSelected"
        @input="select($event)"
      ></v-simple-checkbox>
    </template>

    <!-- Pass through all parent slots -->
    <template
      v-for="(_, name) in $scopedSlots"
      :slot="name"
      slot-scope="slotData"
      ><slot :name="name" v-bind="slotData"
    /></template>
  </v-data-table>
</template>
<script lang="ts">
import Vue from 'vue';
import { mapToDataTableQueryParameters } from '@/common/mappers/query-param-mapper';
import {
  IBaseDataTableAction,
} from '@/common/interfaces';
export default Vue.extend({
  name: 'BaseDataTable',
  components: {},
  props: {
    headers: {
      type: Array,
      default: (): Array<any> => [],
    },
    items: {
      type: Array,
      default: (): Array<any> => [],
    },
    serverItemsLength: {
      type: Number,
      default: 0,
    },
    sortBy: {
      type: String,
      default: undefined,
    },
    sortDesc: {
      type: Boolean,
      default: false,
    },
    disablePagination: {
      type: Boolean,
      default: false,
    },
    filters: {
      type: Object,
      default: () => ({}),
    },
    addAdvancedSearch: {
      type: Boolean,
      default: false,
    },
    multiFilter: {
      type: Boolean,
      default: false,
    },
    footerProps: {
      type: Object,
      default: () => ({}),
    },
    itemKey: {
      type: String,
      default: undefined,
    },
    actions: {
      type: Array as () => Array<IBaseDataTableAction>,
      default: (): Array<IBaseDataTableAction> => [],
    },
    itemType: {
      type: String,
      default(): any {
        return "item";
      },
    },
    activeRow: {
      type: Boolean,
      default: false,
    }
  },
  data(): any {
    return {
      search: '', // from built-in search bar
      advancedSearchMenu: false,
      options: {},
      activeFilters: {} as { [key: string]: string },
      showMenu: [],
      selected: [],
      actionSelect: '',
      showActions: false,
      selectAll: false,
      showSearch: false,
      advancedSearch: '',
      isSaving: false,
      savedSuccessfully: false,
      selectedRow: '',
      loading: true,
      loaded: false,
    };
  },
  computed: {
    rows(): any {
      return this.items;
    },
    dataHeaders(): any {
      const processedHeaders = [];
      for (const header of this.headers) {
        let copy = Object.assign({}, header);
        // if this column is being filtered
        if (
          this.filters &&
          this.filters[header.value] &&
          this.filters[header.value].length > 0
        ) {
          // attach a filter function to the header
          copy = Object.assign({}, copy, {
            filter: (value: string) =>
              this.activeFilters[header.value] &&
              this.activeFilters[header.value].length > 0
                ? this.activeFilters[header.value].includes(value)
                : true,
          });
        }
        processedHeaders.push(copy);
      }
      return processedHeaders;
    },
    isMobile(): boolean {
      return this.$vuetify.breakpoint.mdAndDown;
    },
    quickActions(): Array<IBaseDataTableAction> {
      if (this.isMobile) return [] as Array<IBaseDataTableAction>;
      else
        return this.actions.filter(
          (action: IBaseDataTableAction) => action.isQuickAction
        );
    },
    actionsSelectList(): Array<IBaseDataTableAction> {
      if (this.isMobile) return this.actions;
      else
        return this.actions.filter(
          (action: IBaseDataTableAction) => !action.isQuickAction
        );
    },
    showSelect(): boolean {
      return this.actions.length > 0;
    },
  },
  mounted() {
    this.$root.$on('search-toggled', this.toggleSearch);
  },
  destroyed() {
    this.$root.$off('search-toggled', this.toggleSearch);
  },
  watch: {
    options: {
      handler() {
        this.updateQueryParameters();
      },
      deep: true,
    },
    activeFilters: {
      handler() {
        this.updateQueryParameters();
      },
      deep: true,
    },
    selected() {
      if (this.selected.length === 0) {
        this.showActions = false;
      } else {
        this.showActions = true;
      }
    },
  },
  async created() {
    const readyHandler = () => {
      if (document.readyState == 'complete') {
        this.loading = false;
        this.loaded = true;
        document.removeEventListener('readystatechange', readyHandler);
      }
    };
    document.addEventListener('readystatechange', readyHandler);
    readyHandler();
  },
  methods: {
    rowClass(event: any) {
      if (event.id == this.selectedRow && this.activeRow) {
        return "primary white--text active-row"; 
      }
    },
    applyAction(payload: any) {
      const actionName =
        typeof payload === 'string' ? payload : this.actionSelect;
      this.$emit('action-applied', {
        name: actionName,
        selectedRows: this.selected,
      });
      this.selected = [];
      this.actionSelect = '';
    },
    toggleAll(column: any): void {
      this.activeFilters[column] = [...this.filters[column]];
    },
    reset(column: string) {
      this.activeFilters[column] = [];
      this.showMenu[column] = false;
    },
    rowClicked(event: any) {
      this.$emit('handle-row-click', event);
      this.selectedRow = event.id;
    },
    // This is usually first hit by the "options" watch method
    // just after the mounted lifecycle hook on this component
    updateQueryParameters() {
      const data = mapToDataTableQueryParameters(
        Object.assign({}, this.options, { filterBy: this.activeFilters })
      );
      this.$emit('update-query-parameters', data);
    },
    toggleSearch(state: boolean) {
      this.showSearch = state;
      this.advancedSearchMenu = !state;
      if (this.showSearch)
        this.$nextTick(() => this.$refs.searchTextField.focus());
    },
    cancel(): void {
      this.resetForm();
    },
    submitSearch(): void {
      this.resetForm();
    },
    resetForm(): void {
      this.advancedSearchMenu = false;
    },
    showAdvancedSearch() {
      this.advancedSearchMenu = true;
      // For some reason, nextTick does not work here but setTimeout does 🤷‍♂️
      setTimeout(() => this.$refs.advancedSearchTextField.focus());
    },
  },
});
</script>
<style lang="scss" scoped>
#table-search .v-menu__content {
  top: 50px !important;
  left: 0px !important;
}
::v-deep .active-row .mdi, 
::v-deep .active-row .svg-inline--fa,
::v-deep .active-row .v-input--selection-controls__input > div {
  color: white !important;
}
</style>