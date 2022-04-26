import { format, addSeconds, parseISO } from 'date-fns';

// Convert a date to application-specific format
// Optionally return a short date or a date + time
export const formatDate = (date: string, short = false): string => {
  const actualDate = parseISO(date);
  const dateFormat = 'MM/dd/yyyy';
  const timeFormat = ' hh:mm a';

  // TODO: Reimplement when user preference is enabled
  // const timeFormat = (this.currentPatient?.timeFormat || '24 H') === '24 H' ? ' HH:mm' : ' hh:mm a';

  if (short) {
    return date ? format(actualDate, dateFormat) : '';
  }
  return date ? format(actualDate, dateFormat + timeFormat) : '';
};

// Convert a value in seconds into hours, minutes, and seconds
export const formatSecondsToHms = (observationValue: number): string => {
  return format(
    addSeconds(new Date(2020, 1, 1), observationValue),
    'HH:mm:ss'
  );
};

// Convert a value in seconds into minutes, and seconds
export const formatSecondsToMinutes = (observationValue: number): string => {
  return format(
    addSeconds(new Date(2020, 1, 1), observationValue),
    'mm:ss'
  );
};

// Splice an @ symbol into a date + time string
export const formatSplicedDate = (date: string): string => {
  const formattedDate = formatDate(date);
  const splicedDate = [formattedDate.slice(0, 11), '@ ', formattedDate.slice(11)].join('');
  return formattedDate ? splicedDate : '';
}