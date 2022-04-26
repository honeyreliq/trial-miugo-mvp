// These should be investigated for consistency with current terminology

export const ObservationTypes = Object.freeze({
    A1C: "a1c",
    ACTIVITY: "activity",
    BLOOD_GLUCOSE: "blood-glucose",
    BLOOD_PRESSURE: "blood-pressure",
    BODY_TEMPERATURE: "body-temperature",
    HEART_RATE: "heart-rate",
    OXYGEN_SATURATION: "oxygen-saturation",
    RESPIRATORY_RATE: "respiratory-rate",
    SLEEP: "sleep",
    WEIGHT: "weight",
});

export const ObservationDataTypes = Object.freeze({
    BLOOD_PRESSURE_SYSTOLIC: "blood-pressure-systolic",
    BLOOD_PRESSURE_DIASTOLIC: "blood-pressure-diastolic",
    BLOOD_GLUCOSE_FASTING: "blood-glucose-fasting",
    BLOOD_GLUCOSE_NON_FASTING: "blood-glucose-non-fasting",
})

// some of these values are duplicated by ObservationTypes but should remain separate for context
export const ObservationCodes = Object.freeze({
    // vital signs
    BLOOD_GLUCOSE: "blood-glucose",
    SYSTOLIC: "systolic",
    DIASTOLIC: "diastolic",
    RESPIRATORY_RATE: "respiratory-rate",
    MEAL_CODE_FASTING: "meal-code-fasting",
    MEAL_CODE_NON_FASTING: "meal-code-non-fasting",
    OXYGEN_SATURATION: "oxygen-saturation",
    // activity
    CALORIES_BURNED: "calories-burned",
    DISTANCE: "distance",
    FITNESS_DURATION: "fitness-duration",
    STEPS: "steps",
    // sleep
    AWAKE: "awake",
    DEEP: "deep",
    LIGHT: "light",
    REM: "rem",
    TOTAL: "total",
    // multiple categories
    HEART_RATE: "heart-rate",
});

export const ObservationChanges = Object.freeze({
    FALLING: "Falling",
    RISING: "Rising",
    STEADY: "Steady",
});

export const ObservationStatus = Object.freeze({
    AT_RISK: "At-Risk",
    CRITICAL: "Critical",
    TARGET: "Target",
});

export const ObservationUnits = Object.freeze({
    BPM: "bpm",
    DEGREES_CELSIUS: "°C",
    DEGREES_FAHRENHEIT: "°F",
    KG: "kg",
    LBS: "lbs",
    MGDL: "mg/dL",
    MMHG: "mmHg",
    MMOLL: "mmol/L",
    PERCENT: "%",
    RPM: "rpm",
    SPO2: "SpO2",
});