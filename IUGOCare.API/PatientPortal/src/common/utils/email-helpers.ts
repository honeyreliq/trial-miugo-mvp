/**
 * checks a given email input if it has the correct format at //user@domain.com
 * @param input 
 * @returns true if the email has the correct format, or false if format is incorrect 
 */
export const validateEmail = (input: string): boolean => new RegExp((/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/)).test(input);
