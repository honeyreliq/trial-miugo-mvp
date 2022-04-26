module.exports = {
  moduleFileExtensions: [
    'ts',
    'js',
    'json',
    'vue',
  ],
  transform: {
    '^.+\\.jsx?$': 'babel-jest',
    '.*\\.vue$': 'vue-jest',
    '^.+\\.tsx?$': 'ts-jest',
    '.+\\.(css|styl|less|sass|scss|svg|png|jpg|ttf|woff|woff2)$': 'jest-transform-stub',
  },
  transformIgnorePatterns: [],
  moduleNameMapper: {
    '^@/(.*)$': '<rootDir>/src/$1',
  },
  testRegex: "__tests__/.*\\.spec\\.ts",
  testURL: 'http://localhost/',
}
