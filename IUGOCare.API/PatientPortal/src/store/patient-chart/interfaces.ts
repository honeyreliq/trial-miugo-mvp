import { IDataTableQueryParameters, IFilterObject } from "@/common/interfaces";

export interface PatientChartState {
    patientChartInfo: PatientChartInfo,
    patientNotes: Array<PatientNote>,
    selectedNote: PatientNote,
    patientNotesQueryParameters: IDataTableQueryParameters,
    patientNoteFilters: Array<IFilterObject>,
    patientDocuments: Array<PatientDocument>,
    patientDocumentQueryParameters: IDataTableQueryParameters,
    patientDocumentFilters: Array<IFilterObject>,
    selectedDocument: PatientDocument,
    monitoringConfigurations: Array<PatientMonitoringConfiguration>,
    monitoringParameters: Array<MonitoringParameter>,
    activeConditions: Array<PatientActiveCondition>,
    carePrograms: Array<PatientCareProgram>,
    careTeam: Array<PatientCareTeamMember>,
    tsp: Array<PatientTelemonitoringServicePlan>,
    medications: Array<PatientMedication>,
    devices: Array<PatientDevice>,
    isSearchEnabled: boolean,
    recentActivity: Array<RecentActivity>,
    careProgramInformation: CareProgramInformation,
    insuranceInformation: InsuranceInformation,
    medicalInformation: MedicalInformation,
    organizationInformation: OrganizationInformation
}

export interface AddDeviceDialogForm {
    deviceName: string,
    deviceSerialNumber: number
}

export interface PatientChartInfo {
    patientId: string | undefined,
    patientEmail: string | undefined,
    patientName: PatientName | undefined,
    patientAge: string | undefined,
    patientGender: string | undefined,
    patientPreferredPhone: PatientPreferredPhone | undefined
    patientBirthDate: string | undefined
    patientLanguage: string | undefined,
    patientAllergies?: Array<string> | undefined,
    patientRace?: string,
    patientEthnicity?: string,
    patientMaritalStatus?: string,
    patientPreferredLanguage?: string,
    patientSocialSecurityNumber?: string,
    patientWorkingStatus?: string,
    patientEmployer?: string,
    patientStatus?: string,
    patientPreferredContact?: string,
    patientHomePhone?: string,
    patientCellPhone?: string,
    patientOfficePhone?: string,
    patientAddressLine1?: string,
    patientAddressLine2?: string,
    patientCity?: string,
    patientZipPostalCode?: string,
    patientState?: string,
    patientEmergencyContact?: PatientEmergencyContact
}

export interface PatientEmergencyContact {
    familyName: string,
    givenName: string,
    homePhoneNumber: string,
    cellPhoneNumber: string,
    relationship: string
}

export interface PatientNote {
    id: string,
    title: string,
    status: string,
    author: string,
    text: string,
    timeEntry: Object,
    tags: Array<PatientNoteTag>,
    isoDateCreated: string,
    isoLastUpdated: string | undefined,
}

export interface PatientNoteTag {
    id: string,
    text: string | undefined,
}

export interface PatientName {
    familyName: string | undefined,
    givenName: string | undefined,
    middleName: string | undefined,
    preferredName?: string
}

export interface PatientPreferredPhone {
    type: string | undefined,
    value: string | undefined
}

export interface PatientDocument {
    id: string,
    name: string | undefined,
    category: string | undefined,
    isoDateUploaded: string | undefined,
    absolutePath: string | undefined,
    fileType: string | undefined,
    tag: string | undefined
}

export interface PatientMonitoringConfiguration {
    id: string,
    observationType: string,
    enable: boolean,
}

export interface MonitoringParameter {
    id: string,
    patientId: string,
    observationDataType: string,
    criticalHigh: string | undefined,
    atRiskHigh: string | undefined,
    atRiskLow: string | undefined,
    criticalLow: string | undefined,
    unitOfMeasure: string
}

export interface PatientActiveCondition {
    id: string,
    activeConditions: string | undefined,
    diagnosisCode: string | undefined,
    enrolledPrograms: string[] | undefined
}

export interface PatientCareProgram {
    key: string | undefined,
    program: string | undefined,
    type: string | undefined,
    provider: string | undefined,
    timeSpent: string | undefined,
    targetTime: string | undefined,
    status: string | undefined
}

export interface PatientCareTeamMember {
    id: string,
    member: string | undefined,
    role: string | undefined
}

export interface PatientTelemonitoringServicePlan {
    id: string,
    preAuthorizationCode: string | undefined,
    diagnosis: string | undefined,
    startDate: string | undefined,
    endDate: string | undefined,
    lastUpdatedBy: string | undefined,
    status: string | undefined,
}

export interface PatientMedication {
    key: string,
    medication: string | undefined,
    dose: string | undefined,
    route: string | undefined,
    frequency: string | undefined,
    startDate: string | undefined,
    endDate: string | undefined,
    status: string | undefined,
    expandedInfo: string | undefined
}
export interface PatientDevice {
    uid: string,
    name: string | undefined,
    serial: string | undefined,
    type: string | undefined,
    dateAdded: string | undefined,
    status: string | undefined
}

export interface RecentActivity {
    id: string,
    title: string,
    dateAndTime: string,
    authorPrefix: string,
    author: string,
    type: string,
    expandedInfo: string
}

export interface CareProgramInformation {
    status: string,
    provider: string,
    providerPhone: string,
    providerType: string,
    providerStatus: string,
    providerNpi: string,
    providerAddress: string,
    providerState: string,
    providerZipPostalCode: string,
    carePrograms: Array<FakeCareProgram>
}

export interface FakeCareProgram {
    programName: string,
    provider: string,
    type: string
}

export interface MedicalInformation {
    activeConditions: string[],
    allergies: string[],
    medicalHistory: string,
    surgicalHistory: string,
    familyHistory: string,
    vaccinesImmunizations: string,
    modifiableRiskFactors: string
}

export interface InsuranceInformation {
    medicaidIdNumber: string,
    issuerID: string,
    dateCardSent: string,
    insurancePlanProvider: string,
    insurancePlanType: string,
    insurancePlanName: string,
    insurancePlanNumber: string,
    tplCode: string,
    visitsPerYear: string,
    claimOfficeNumber: string,
    yearCardIssued: string
}

export interface OrganizationInformation {
    uid: string,
    name: string,
    number: string,
    email: string,
    type: string,
    status: string,
    address: string,
    state: string,
    zipPostalCode: string
}