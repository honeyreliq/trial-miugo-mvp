import { PatientToursClient } from '../IUGOCare-api';
import axiosClient from '@/auth/axiosInstance';

const client = new PatientToursClient(process.env.VUE_APP_API_URL, axiosClient);

export const OBSERVATIONS_TOUR = 'observationsTour';

export const tourCallbacks = {
  onSkip: (tour: string) => () => {
    client.skipTour(tour);
  },
  onFinish: (tour: string) => () => {
    client.finishTour(tour);
  },
};

export const shouldDisplayTour = async (tourKey: string): Promise<boolean> =>
  await client.shouldStartTour(tourKey);
