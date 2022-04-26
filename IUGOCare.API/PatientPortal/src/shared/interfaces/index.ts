export interface IObservationType {
  text: any;
  value: string;
  order: number;
}

export interface IObservationResponse {
  isSaved: boolean;
  message: string;
}

export interface IObservationRequest {
  effectiveDate: Date;
  observationCode: string;
}

export interface IBasicMeasurement {
  code: string;
  unit: string;
  value?: number;
  label: string;
  placeholder?: string;
  rules: any[];
}

export interface IUnit {
  value: string;
  minValue: number;
  maxValue: number;
}

export interface IComplexMeasurement {
  code: string;
  units: IUnit[];
  unit?: string;
  value?: number;
  label: string;
  placeholder?: string;
  rules: any[];
}

export interface ICombinedMeasurement {
  code: string;
  units?: IUnit[];
  unit?: string;
  value: string;
  label: string;
  placeholder?: string;
}

export interface ChartPoint {
  x: string;
  y: number;
}

export interface IChartInformation {
  ChartsImages: string[];
  ChartHeaders: any[];
}
