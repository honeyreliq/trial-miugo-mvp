import { Module } from 'vuex';
import { RootState } from '../types';

export interface IIncidentDetails {
  description: string;
  issue: string;
  immediateAttention: any;
  patientLocated: IPatientLocated;
  issueResolved: any;
}

export interface IFollowUp {
  partiesContacted: {
    patient: boolean;
    caregiver: boolean;
    primaryCareProvider: boolean;
    emergencyServices: boolean;
  };
  partiesStillToBeContacted: {
    caregiver: boolean;
    primaryCareProvider: boolean;
    other: boolean;
    otherDetails: string;
  };
}

export interface ISOSFormModel {
  contactInformation: {
    confirmedIdentity: boolean;
    otherPartiesContacted: boolean;
    confirmedIdentityDetails: string;
  };
  incidentDetails: IIncidentDetails;
  followUp: IFollowUp;
}

export interface IGeofenceFormModel {
  contactInformation: {
    contactWith: {
      patient: boolean;
      caregiver: boolean;
      sunnySideLongTermCareFacility: boolean;
      emergencyServices: boolean;
    };
    otherParties: any;
    otherDetails: string;
  };
  incidentDetails: IIncidentDetails;
  followUp: IFollowUp;
}

export interface IContactInformation {
  confirmedIdentity: string;
  fallOccur: string;
  injuryIncurred: string;
}

export interface IInjury {
  patientHitHead: string;
  injured: string;
  immediateAttention: string;
}

export interface IFallIncidentDetails {
  whereFallOccurred: string;
  whereFallOccurredDetail: string;
  type: {
    value: string;
    answer: string;
  };
  whenFallOccurred: string;
  actions: string;
  description: string;
}

export interface IFallFollowUp {
  partiesContacted: string[];
  partiesToBeContacted: string[];
  answer: string;
}

export interface IFallFormModel {
  contactInformation: IContactInformation;
  injury: IInjury;
  assessmentDetails: {
    answer: string;
  };
  incidentDetails: IFallIncidentDetails;
  followUp: IFallFollowUp;
}

export interface ICallInformation {
  callStarted: boolean;
  time: string;
  storeName: string;
}

export interface IPatientLocated {
  located: string;
  time: string;
}

export interface IAlertInformation {
  triggered: boolean;
  date: string;
  time: string;
}

export interface ISOSCheckList {
  sosDetected: IAlertInformation;
  assistanceRequired: boolean;
  callEstablished: ICallInformation;
  descriptionEvent: boolean;
  followUpItems: boolean;
}

export interface IFallCheckList {
  fallDetected: IAlertInformation;
  callEstablished: ICallInformation;
  fallOccurred: string;
  assistanceRequired: string;
  headInjuryIncurred: string;
  injuryIncurred: string;
  locationFall: string;
  typeFall: string;
  descriptionEvent: boolean;
  followUpItems: boolean;
}

export interface ISurvey {
  sid: number;
  survey: string;
  status: string;
  date: number;
  time: string;
}

export interface IFile {
  fid: number;
  name: string;
  category: string;
  date: number;
  time: string;
}

export interface IDemoState {
  sosWorkspace: {
    form: ISOSFormModel;
    alert: IAlertInformation;
    checkList: ISOSCheckList;
    callInformation: ICallInformation;
    showSuccessMessage: boolean;
  };
  geofenceWorkspace: {
    form: IGeofenceFormModel;
    checklist: IGeofenceCheckList;
    alert: IAlertInformation;
    callInformation: ICallInformation;
    showSuccessMessage: boolean;
  };
  fallWorkspace: {
    form: IFallFormModel;
    checklist: IFallCheckList;
    alert: IAlertInformation;
    callInformation: ICallInformation;
    showSuccessMessage: boolean;
  };
  miugo: {
    appStarted: boolean;
    selectedSurvey: ISurvey;
    selectedFile: IFile;
    selectedSymptom: string;
  };
}
export interface IGeofenceCheckList {
  violationDetected: IAlertInformation;
  patientLocated: IPatientLocated;
  callEstablished: ICallInformation;
  isEventDescriptionCompleted: boolean;
  isFollowUpCompleted: boolean;
}

function getCleanGeofenceStore(): any {
  return {
    form: {
      contactInformation: {
        contactWith: {
          patient: false,
          caregiver: false,
          sunnySideLongTermCareFacility: false,
          emergencyServices: false,
        },
        otherParties: null,
        otherDetails: '',
      },
      incidentDetails: {
        description: '',
        issue: '',
        immediateAttention: null,
        patientLocated: {
          located: '',
          time: '',
        },
        issueResolved: null,
      },
      followUp: {
        partiesContacted: {
          patient: false,
          caregiver: false,
          primaryCareProvider: false,
          emergencyServices: false,
        },
        partiesStillToBeContacted: {
          caregiver: false,
          primaryCareProvider: false,
          other: false,
          otherDetails: '',
        },
      },
    },
    checklist: {
      violationDetected: {
        triggered: false,
        time: '',
      },
      patientLocated: {
        located: '',
        time: '',
      },
      callEstablished: {
        time: '',
        callStarted: null,
      },
      isEventDescriptionCompleted: false,
      isFollowUpCompleted: false,
    },
    alert: {
      triggered: null,
      time: '',
    },
    callInformation: {
      callStarted: false,
      time: '',
    },
    showSuccessMessage: false,
  };
}

function getCleanSOSStore(): any {
  return {
    form: {
      contactInformation: {
        confirmedIdentity: null,
        otherPartiesContacted: null,
        confirmedIdentityDetails: '',
      },
      incidentDetails: {
        description: '',
        issue: '',
        immediateAttention: null,
        patientLocated: null,
        issueResolved: null,
      },
      followUp: {
        partiesContacted: {
          patient: false,
          caregiver: false,
          primaryCareProvider: false,
          emergencyServices: false,
        },
        partiesStillToBeContacted: {
          caregiver: false,
          primaryCareProvider: false,
          other: false,
          otherDetails: '',
        },
      },
    },
    alert: {
      triggered: null,
      time: '',
    },
    checkList: {
      sosDetected: {
        time: null,
        triggered: null,
      },
      callEstablished: {
        time: null,
        callStarted: null,
      },
      assistanceRequired: null,
      descriptionEvent: null,
      followUpItems: null,
    },
    callInformation: {
      callStarted: false,
      time: '',
    },
    showSuccessMessage: false,
  };
}

function getCleanFallStore(): any {
  return {
    form: {
      contactInformation: {
        confirmedIdentity: null,
        fallOccur: null,
        injuryIncurred: null,
      },
      injury: {
        patientHitHead: null,
        injured: null,
        immediateAttention: null,
      },
      assessmentDetails: {
        answer: null,
      },
      incidentDetails: {
        whereFallOccurred: null,
        whereFallOccurredDetail: null,
        type: {
          value: null,
          answer: null,
        },
        whenFallOccurred: null,
        actions: null,
        description: null,
      },
      followUp: {
        partiesContacted: [],
        partiesToBeContacted: [],
        answer: null,
      },
    },
    alert: {
      triggered: null,
      time: '',
    },
    checklist: {
      fallDetected: {
        time: null,
        triggered: null,
      },
      callEstablished: {
        time: null,
        callStarted: null,
      },
      fallOccurred: null,
      assistanceRequired: null,
      headInjuryIncurred: null,
      injuryIncurred: null,
      locationFall: null,
      typeFall: null,
      descriptionEvent: null,
      followUpItems: null,
    },
    callInformation: {
      callStarted: false,
      time: '',
    },
    showSuccessMessage: false,
  };
}

const state: IDemoState = {
  sosWorkspace: getCleanSOSStore(),
  geofenceWorkspace: getCleanGeofenceStore(),
  fallWorkspace: getCleanFallStore(),
  miugo: {
    appStarted: false,
    selectedSurvey: {
      sid: null,
      survey: '',
      status: '',
      date: null,
      time: ''
    },
    selectedFile: {
      fid: null,
      name: '',
      category: '',
      date: null,
      time: ''
    },
    selectedSymptom: 'All',
  },
};

export const demo: Module<IDemoState, RootState> = {
  namespaced: true,
  state,
  actions: {},
  getters: {
    // SOS workspace
    sosForm(demoState: IDemoState): ISOSFormModel {
      return demoState.sosWorkspace.form;
    },
    sosFormIncidentDetails(demoState: IDemoState): IIncidentDetails {
      return demoState.sosWorkspace.form.incidentDetails;
    },
    sosFormFollowUp(demoState: IDemoState): IFollowUp {
      return demoState.sosWorkspace.form.followUp;
    },
    sosAlert(demoState: IDemoState): IAlertInformation {
      return demoState.sosWorkspace.alert;
    },
    sosCheckList(demoState: IDemoState): ISOSCheckList {
      return demoState.sosWorkspace.checkList;
    },
    sosCallInformation(demoState: IDemoState): ICallInformation {
      return demoState.sosWorkspace.callInformation;
    },

    // Geofence workspace
    geofenceForm(demoState: IDemoState): IGeofenceFormModel {
      return demoState.geofenceWorkspace.form;
    },
    geofenceFormIncidentDetails(demoState: IDemoState): IIncidentDetails {
      return demoState.geofenceWorkspace.form.incidentDetails;
    },
    geofenceFormFlowUp(demoState: IDemoState): IFollowUp {
      return demoState.geofenceWorkspace.form.followUp;
    },
    geofencePatientLocated(demoState: IDemoState): IPatientLocated {
      return demoState.geofenceWorkspace.form.incidentDetails.patientLocated;
    },
    geofenceAlert(demoState: IDemoState): IAlertInformation {
      return demoState.geofenceWorkspace.alert;
    },
    geofenceChecklist(demoState: IDemoState): IGeofenceCheckList {
      const checkList: IGeofenceCheckList = {...demoState.geofenceWorkspace.checklist};
      checkList.violationDetected = {...demoState.geofenceWorkspace.alert};
      checkList.patientLocated = {...demoState.geofenceWorkspace.form.incidentDetails.patientLocated};
      checkList.callEstablished = {...demoState.geofenceWorkspace.callInformation};
      return checkList;
    },
    geofenceCallInformation(demoState: IDemoState): ICallInformation {
      return demoState.geofenceWorkspace.callInformation;
    },

    // Fall workspace
    fallAlert(demoState: IDemoState): IAlertInformation {
      return demoState.fallWorkspace.alert;
    },
    fallFormContactInformation(demoState: IDemoState): IContactInformation {
      return demoState.fallWorkspace.form.contactInformation;
    },
    fallFormInjury(demoState: IDemoState): IInjury {
      return demoState.fallWorkspace.form.injury;
    },
    fallFormIncidentDetails(demoState: IDemoState): IFallIncidentDetails {
      return demoState.fallWorkspace.form.incidentDetails;
    },
    fallFormFollowUp(demoState: IDemoState): IFallFollowUp {
      return demoState.fallWorkspace.form.followUp;
    },
    fallCheckList(demoState: IDemoState): IFallCheckList {
      return demoState.fallWorkspace.checklist;
    },
    fallForm(demoState: IDemoState): IFallFormModel {
      return demoState.fallWorkspace.form;
    },
    fallCallInformation(demoState: IDemoState): ICallInformation {
      return demoState.fallWorkspace.callInformation;
    },

    // miugo
    selectedSurvey(demoState: IDemoState): ISurvey {
      return demoState.miugo.selectedSurvey;
    },
    selectedFile(demoState: IDemoState): IFile {
      return demoState.miugo.selectedFile;
    },
    selectedSymptom(demoState: IDemoState): string {
      return demoState.miugo.selectedSymptom;
    },
  },
  mutations: {
    // Shared
    setCallInformation(demoState: IDemoState, payload: ICallInformation): void {
      switch (payload.storeName) {
        case 'geofence':
          demoState.geofenceWorkspace.callInformation = {...payload};
          break;
        case 'sos':
          demoState.sosWorkspace.callInformation = {...payload};
          break;
        case 'fall':
          demoState.fallWorkspace.callInformation = {...payload};
          break;
      }
    },

    showSuccessMessage(demoState: IDemoState, payload: 'sosWorkspace' | 'geofenceWorkspace' | 'fallWorkspace'): void {
      demoState[payload].showSuccessMessage = true;
    },

    // SOS workspace
    setSOSFormModel(demoState: IDemoState, payload: ISOSFormModel): void {
      demoState.sosWorkspace.form = {...payload};
    },
    setSOSAlert(demoState: IDemoState, alert: IAlertInformation): void {
      demoState.sosWorkspace.alert = {...alert};
    },
    setSOSCheckList(demoState: IDemoState, checkList: ISOSCheckList): void {
      demoState.sosWorkspace.checkList = {...checkList};
    },
    cleanSOSStore(demoState: IDemoState): void {
      demoState.sosWorkspace = getCleanSOSStore();
    },

    // Geofence workspace
    setGeofenceFormModel(demoState: IDemoState, payload: IGeofenceFormModel): void {
      demoState.geofenceWorkspace.form = {...payload};
    },
    setGeofenceAlert(demoState: IDemoState, payload: IAlertInformation): void {
      demoState.geofenceWorkspace.alert = {...payload};
    },
    setGeofenceChecklist(demoState: IDemoState, payload: IGeofenceCheckList): void {
      demoState.geofenceWorkspace.checklist = {...payload};
    },
    cleanGeofenceStore(demoState: IDemoState): void {
      demoState.geofenceWorkspace = getCleanGeofenceStore();
    },

    // Fall workspace
    setFallAlert(demoState: IDemoState, alert: IAlertInformation): void {
      demoState.fallWorkspace.alert = {...alert};
    },
    setFallFormModel(demoState: IDemoState, payload: IFallFormModel): void {
      demoState.fallWorkspace.form = {...payload};
    },
    setFallCheckList(demoState: IDemoState, payload: IFallCheckList): void {
      demoState.fallWorkspace.checklist = {...payload};
    },
    cleanFallStore(demoState: IDemoState): void {
      demoState.fallWorkspace = getCleanFallStore();
    },

    // miugo
    setAppStarted(demoState: IDemoState, payload: boolean) {
      demoState.miugo.appStarted = payload;
    },
    setSelectedSurvey(demoState: IDemoState, survey: ISurvey) {
      demoState.miugo.selectedSurvey = {...survey};
    },
    setSelectedFile(demoState: IDemoState, file: IFile) {
      demoState.miugo.selectedFile = {...file};
    },
    setSelectedSymptom(demoState: IDemoState, payload: string): void {
      demoState.miugo.selectedSymptom = payload;
    },
  },
};
