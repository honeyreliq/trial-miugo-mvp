import { ObservationStatus } from '../data/observation-constants';

export const getObservationStatusColor = (itemType: string): string => {
    switch (itemType) {
      case ObservationStatus.CRITICAL:
        return 'error';
      case ObservationStatus.AT_RISK:
        return 'warning';
      case ObservationStatus.TARGET:
        return 'success';
    }
  };