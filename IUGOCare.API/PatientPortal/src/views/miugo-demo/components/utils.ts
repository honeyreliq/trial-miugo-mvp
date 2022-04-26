import moment, { Moment } from 'moment';

// This function subtracts the provided time span from today
// and returns it as a UNIX timestamp
// e.g. 1318874398
export function subtractFromToday(minutes = 0, days = 0, months = 0): number {
  const result: Moment = moment().subtract(months, 'months');
  result.subtract(days, 'days');
  result.subtract(minutes, 'minutes');

  let daysToSubtract = 0;

  // If a generated date falls on a weekend, set it to Friday
  // to simulate normal business hours
  daysToSubtract += (result.isoWeekday() === 6) ? 1 : 0;  // Saturday
  daysToSubtract += (result.isoWeekday() === 7) ? 2 : 0;  // Sunday

  if (daysToSubtract > 0) {
    result.subtract(daysToSubtract, 'days');
  }
  return result.unix();
}

// This function adds the provided time span to today
// and returns it as a UNIX timestamp
// e.g. 1318874398
export function addToToday(minutes = 0, days = 0, months = 0): number {
  const result: Moment = moment().add(months, 'months');
  result.add(days, 'days');
  result.add(minutes, 'minutes');

  let daysToSubtract = 0;

  // If a generated date falls on a weekend, set it to Friday
  // to simulate normal business hours
  daysToSubtract += (result.isoWeekday() === 6) ? 1 : 0;  // Saturday
  daysToSubtract += (result.isoWeekday() === 7) ? 2 : 0;  // Sunday

  if (daysToSubtract > 0) {
    result.add(daysToSubtract, 'days');
  }
  return result.unix();
}

// This function takes in a timestamp and returns a string with the given format
// e.g. 02/14/2021
export function timestampToString(timestamp: number, format: string): string {
  return moment.unix(timestamp).format(format);
}
