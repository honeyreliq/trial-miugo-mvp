export interface IFilterObject {
    column: String,
    values: String[]
}

export interface IDataTableQueryParameters {
	search?: String,
	sortBy: String[],
	sortDesc: Boolean[],
	filterBy: IFilterObject[],
	pageNumber: Number,
	itemsPerPage: Number,
}

export interface ITabItem {
	label: string;
	badge?: string;
	type: number;
	to?: string;
  }

/**
 * This represents the objects passed in to the 
 * BaseDataTable's `actions` prop.
 */
export interface IBaseDataTableAction {
	name: string,
	icon: string | undefined,
	isQuickAction: boolean
}
