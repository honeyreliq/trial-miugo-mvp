/**
 * Formats a given string into our phone number format: (000) 000-0000
 * 
 * @param input The string to format.
 * @returns The formatted phone number.
 */
export const formatPhoneNumber = (input: string | null): string => {
    if (!input) return '';
    const parts = input.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
    const areaCode = `(${parts[1]}${parts[1].length === 3 ? ') ' : ''}`;
    const phoneNumber = `${parts[3] ? parts[2] + '-' + parts[3] : parts[2]}`
    return areaCode + phoneNumber;
}

/**
 * Checks that a given phone number has been formatted correctly.
 * @param phoneNumber The phone number to validate.
 * @returns true if the phone number is in the correct format, false otherwise.
 */
export const validatePhoneNumber = (phoneNumber: string): boolean => new RegExp(/^\(\d{3}\)\s\d{3}-\d{4}$/).test(phoneNumber);

/**
 * Formats a complete phone number to be displayed in the UI.
 * 
 * @param number The phone number to format
 * @returns The formatted phone number
 */
export const formatPhoneNumberForDisplay = (number: string | null): string => {
    if (!number) return '';
    const parts = number.replace(/\D/g, '').match(/(1|)?(\d{0,3})(\d{0,3})(\d{0,4})/);
    if (!parts) return '';
    const intlCode = parts[1] ? '+1' : '';
    return `${intlCode} (${parts[2]}) ${parts[3]}-${parts[4]}`;
}
