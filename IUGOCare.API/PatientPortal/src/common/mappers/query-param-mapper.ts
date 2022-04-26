import { IDataTableQueryParameters } from "../interfaces";

export const mapToDataTableQueryParameters = (data: any): IDataTableQueryParameters => {

    const filterBy = [];

    // TODO: This constant conversion to-and-from this object literal
    // is only to keep from breaking the current UI functionality, but it
    // should be replaced with one common interface, e.g. IFilterObject
    // when there is an API and the client-side filtering is removed
    for (const [key, value] of Object.entries(data.filterBy)) {
        filterBy.push({ column: key, values: value });
    }

    return {
        sortBy: data.sortBy,
        sortDesc: data.sortDesc,
        filterBy: filterBy,
        pageNumber: data.page,
        itemsPerPage: data.itemsPerPage
    } as IDataTableQueryParameters;
};