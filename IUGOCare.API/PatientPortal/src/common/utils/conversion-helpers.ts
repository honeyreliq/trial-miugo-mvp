export const convertToF = (value: number, unit: string) => {
  let degreesF = value;
  if (unit === '°C') {
      degreesF = (value * 9 / 5) + 32;
  }
  return degreesF;
};

export const convertToFt = (value: number, unit: string) => {
  let feet = value;
  switch (unit) {
    case 'mt':
      feet = Math.round(value * 3.28084);
      break;
    case 'km':
      feet = Math.round(value * 3280.84);
      break;
    case 'mi':
      feet = Math.round(value * 5280);
      break;
  }
  return feet;
};

export const convertToLb = (value: number, unit: string) => {
  let lbs = value;
  if (unit === 'kg') {
    lbs = Math.round(value * 2.20462);
  }
  return lbs;
};

export const convertToMgdl = (value: number, unit: string) => {
  let mgdl = value;
  if (unit === 'mmol/L') {
    const mMolL = 18.0182;
    mgdl = Math.round(value * mMolL);
  }
  return mgdl;
};

export const secondsToHms = (seconds: number) => {
  const h = Math.floor(seconds / 3600);
  const m = Math.floor(seconds % 3600 / 60);
  const s = Math.floor(seconds % 3600 % 60);
  const hDisplay = h.toString().padStart(2, '0');
  const mDisplay = m.toString().padStart(2, '0');
  const sDisplay = s.toString().padStart(2, '0');
  return [hDisplay, mDisplay, sDisplay].join(':');
};
