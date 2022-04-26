<template>
  <BaseCard class="flex">
    <v-row v-if="showChart || showWorkoutChart">
      <div v-for="head in chartHeaders"
           :key="head.key"
           style="padding-left: 75px; padding-bottom: 10px;">
        <h3 style="margin-top: 5px;">{{ head.valueString }}</h3>
        <span>{{ head.key }}</span>
      </div>
    </v-row>
    <v-row class="chart-container">
      <canvas id="myChart"></canvas>
    </v-row>
    <v-row v-if="observationType === 'workouts'" class="bottom-chart-container">
      <canvas id="myBottomChart" style="display: none;"></canvas>
    </v-row>
  </BaseCard>
</template>
<script lang="ts">
  import Vue, { PropType } from 'vue';
  import { mapState } from 'vuex';
  import { PatientObservationDto, IPatientObservationDataDto } from '@/IUGOCare-api';
  import { ChartPoint, IChartInformation } from '@/shared/interfaces';
  import Chart from 'chart.js';
  import { format, parse } from 'date-fns';
  import { convertToF, convertToLb, convertToMgdl, secondsToHms } from '../../common/conversion-helpers';

  export default Vue.extend({
    name: 'ObservationsCharts',
    props: {
      observationType: String,
      observations: {
        type: Array as PropType<PatientObservationDto[]>,
      },
    },
    data() {
      return {
        chartHeaders: [],
        showChart: false,
        showWorkoutChart: false,
        chart: {} as Chart,
        bottomChart: {} as Chart,
      };
    },
    methods: {
      getLineChartOptions(label: string, maxValue: number, minValue: number) {
        const beginAtZero = minValue === 0;

        const options = {
          responsive: true,
          maintainAspectRatio: false,
          legend: {
            display: false,
          },
          scales: {
            yAxes: [
              {
                position: 'left',
                scaleLabel: {
                  display: true,
                  labelString: label,
                },
                ticks: {
                  beginAtZero: { beginAtZero },
                  stepSize: 10,
                  steps: 10,
                  stepValue: 5,
                  max: maxValue,
                  min: minValue,
                },
                gridLines: {
                  color: this.getChartGridLinesColor,
                  zeroLineColor: this.getChartGridLinesColor,
                },
              },
            ],
            xAxes: [
              {
                position: 'center',
                scaleLabel: {
                  display: true,
                  labelString: this.$t('dateAndTime'),
                },
                ticks: {
                  maxRotation: 60,
                  minRotation: 60,
                },
                gridLines: {
                  color: this.getChartGridLinesColor,
                  zeroLineColor: this.getChartGridLinesColor,
                },
                distribution: 'linear',
                type: 'time',
                time: {
                  unit: 'day',
                  displayFormats: {
                    day: this.getPatientDateFormat,
                    hour: this.getPatientTimeFormat,
                  },
                  tooltipFormat: this.getPatientDateAndTimeFormat,
                },
                bounds: 'ticks',
              },
            ],
          },
          tooltips: {
            mode: 'index',
            intersect: true,
            position: 'average',
          },
        };

        return options;
      },
      getBarChartOptions(label: string, maxValue: number, minValue: number) {
        const options = {
          responsive: true,
          maintainAspectRatio: false,
          legend: {
            display: true,
            position: 'top',
          },
          scales: {
            yAxes: [
              {
                position: 'left',
                scaleLabel: {
                  display: true,
                  labelString: label,
                },
                stacked: true,
                ticks: {
                  beginAtZero: true,
                  min: minValue,
                  max: maxValue,
                  stepSize: 7200,
                  callback: (value: any) => {
                    const hours = value / 3600;
                    return `${Math.floor(hours)}:00:00`;
                  },
                },
                gridLines: {
                  color: this.getChartGridLinesColor,
                  zeroLineColor: this.getChartGridLinesColor,
                },
              },
            ],
            xAxes: [
              {
                scaleLabel: {
                  display: true,
                  labelString: this.$t('date'),
                  position: 'center',
                },
                ticks: {
                  maxRotation: 60,
                  minRotation: 60,
                },
                gridLines: {
                  color: this.getChartGridLinesColor,
                  zeroLineColor: this.getChartGridLinesColor,
                },
                stacked: true,
                distribution: 'linear',
                type: 'time',
                time: {
                  unit: 'day',
                  displayFormats: {
                    day: this.getPatientDateFormat,
                    hour: this.getPatientTimeFormat,
                  },
                  tooltipFormat: this.getPatientDateAndTimeFormat,
                },
                bounds: 'ticks',
              },
            ],
          },
          tooltips: {
            callbacks: {
              label: (tooltipItem: any, data: any) => {
                const value = data.datasets[tooltipItem.datasetIndex].data?.[tooltipItem.index]?.y || 0;
                const labelValue = data.datasets[tooltipItem.datasetIndex].label || '';
                return `${labelValue}: ${secondsToHms(value)}`;
              },
            },
          },
        };

        return options;
      },
      getDataChartTemplate() {
        return {
          label: '',
          borderColor: '#007EC7CC',
          borderWidth: 2,
          fill: false,
          pointBackgroundColor: '#007EC7CC',
          pointHoverBackgroundColor: '#007EC7CC',
          lineTension: 0,
          data: [] as any,
        };
      },
      getBarDataChartTemplate() {
        return {
          label: '',
          borderWidth: 2,
          backgroundColor: '#fff',
          order: 0,
          data: [] as any,
          categoryPercentage: 1.0,
          barPercentage: 1.0,
          maxBarThickness: 65,
        };
      },
      getA1cDataChart() {
        const dataTemplate = this.getDataChartTemplate();

        const unit = '%';
        const a1cDataset = JSON.parse(JSON.stringify(dataTemplate));

        const weightLabel = this.$t('weight');

        a1cDataset.label = `${weightLabel} (${unit})`;

        const totalReadings = this.observations.length;

        if (this.observations.length > 0) {

          this.observations.forEach((o: any) => {
            if (o.observationsData) {
              const date = o.effectiveDate.toString();

              o.observationsData.forEach((od: any) => {
                const tData: ChartPoint = { x: date, y: od.value };
                a1cDataset.data.push(tData);
              });
            }
          });
        }

        const labels: string[] = a1cDataset.data.map((d: ChartPoint) => d.x);
        const values: number[] = a1cDataset.data.map((d: ChartPoint) => d.y);
        const total = values.reduce((sum: number, next: number) => sum + next, 0);
        const max = values.reduce((prev: number, next: number) => Math.max(prev, next), 0);
        const min = values.reduce((prev: number, next: number) => Math.min(prev, next), Infinity);
        const average = (total / totalReadings).toFixed(1);

        this.chartHeaders = [
          {
            key: this.$t('high'),
            valueString: `${max} ${unit}`,
          },
          {
            key: this.$t('low'),
            valueString: `${min} ${unit}`,
          },
          {
            key: this.$t('average'),
            valueString: `${average} ${unit}`,
          },
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];

        return {
          type: 'line',
          labels,
          datasets: [a1cDataset],
          options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
              display: false,
            },
            scales: {
              yAxes: [
                {
                  position: 'left',
                  scaleLabel: {
                    display: true,
                    labelString: `${this.$t('a1c')} (${unit})`,
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                  ticks: {
                    beginAtZero: true,
                    stepSize: 0.5,
                    steps: 1,
                    stepValue: 0.5,
                    max: Math.floor(max + 1) as number,
                    min: Math.ceil(min - 1) as number,
                  },
                },
              ],
              xAxes: [
                {
                  position: 'center',
                  scaleLabel: {
                    display: true,
                    labelString: this.$t('dateAndTime'),
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                  ticks: {
                    maxRotation: 60,
                    minRotation: 60,
                  },
                  distribution: 'linear',
                  type: 'time',
                  time: {
                    unit: 'day',
                    displayFormats: {
                      day: this.getPatientDateFormat,
                      hour: this.getPatientTimeFormat,
                    },
                    tooltipFormat: this.getPatientDateAndTimeFormat,
                  },
                  bounds: 'ticks',
                },
              ],
            },
            tooltips: {
              callbacks: {
                label: (tooltipItem: any) => {
                  const {value} = tooltipItem;
                  return `${this.$t('a1c')}: ${value}${unit}`;
                },
              },
            },
          },
        };
      },
      getBloodPressureData() {
        const dataTemplate = this.getDataChartTemplate();

        const bloodPressureUnit = 'mmHg';
        const heartRateUnit = 'bpm';
        const systolicData = JSON.parse(JSON.stringify(dataTemplate));
        const diastolicData = JSON.parse(JSON.stringify(dataTemplate));
        const heartRateData = JSON.parse(JSON.stringify(dataTemplate));

        const systolicLabel = this.$t('systolic');
        const diastolicLabel = this.$t('diastolic');
        const heartRateLabel = this.$t('heartRate');

        systolicData.label = `${systolicLabel} (${bloodPressureUnit})`;
        diastolicData.label = `${diastolicLabel} (${bloodPressureUnit})`;
        heartRateData.label = `${heartRateLabel} (${heartRateUnit})`;

        const labels: string[] = [];

        let max = 0;
        let sumSystolic = 0;
        let sumDiastolic = 0;
        let totalReadings = 0;

        heartRateData.borderColor = '#DA4250CC';
        heartRateData.pointHoverBackgroundColor = '#DA4250CC';
        heartRateData.pointBackgroundColor = '#DA4250CC';
        if (this.observations.length > 0) {

          totalReadings += this.observations.length;

          this.observations.forEach((o: any) => {
            if (o.observationsData) {
              o.observationsData.forEach((od: any) => {
                let tData;
                const date = o.effectiveDate.toString();

                if (od.observationCode) {
                  tData = {
                    x: date,
                    y: od.value,
                  };

                  if (od.value > max) {
                    max = od.value;
                  }

                  if (!labels.includes(date)) {
                    labels.push(date);
                  }

                  if (od.observationCode === 'systolic') {
                    systolicData.data.push(tData);
                    sumSystolic += od.value;
                  }
                  if (od.observationCode === 'diastolic') {
                    diastolicData.data.push(tData);
                    sumDiastolic += od.value;
                  }
                  if (od.observationCode === 'heart-rate') {
                    heartRateData.data.push(tData);
                  }
                }
              });
            }
          });
        }

        const averageLabel = this.$t('average');
        this.chartHeaders = [
          {
            key: `${averageLabel} ${this.$t('systolic')}`,
            valueString: `${totalReadings > 0 ? Math.round(sumSystolic / totalReadings) : 0} ${bloodPressureUnit}`,
          },
          {
            key: `${averageLabel} ${this.$t('diastolic')}`,
            valueString: `${totalReadings > 0 ? Math.round(sumDiastolic / totalReadings) : 0} ${bloodPressureUnit}`,
          },
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];

        return {
          type: 'line',
          labels: labels as string[],
          datasets: [systolicData, diastolicData, heartRateData],
          options: this.getLineChartOptions(
            `${bloodPressureUnit} (${heartRateUnit})`,
            Math.floor(max + 10),
            0,
          ),
        };
      },
      getOxygenDataChart() {
        const dataTemplate = this.getDataChartTemplate();

        const oxygenData = JSON.parse(JSON.stringify(dataTemplate));
        const heartRateData = JSON.parse(JSON.stringify(dataTemplate));
        const oxygenSaturationUnit = '%';

        oxygenData.label = this.$t('oxygenSaturationLbl');
        heartRateData.label = this.$t('heartRateLbl');

        const labels: string[] = [];

        let max = 0;
        const totalReadings = this.observations.length;
        let sumOxygenSat = 0;

        heartRateData.borderColor = '#DA4250CC';
        heartRateData.pointHoverBackgroundColor = '#DA4250CC';
        heartRateData.pointBackgroundColor = '#DA4250CC';
        if (this.observations.length > 0) {

          this.observations.forEach((o: any) => {
            if (o.observationsData) {
              o.observationsData.forEach((od: any) => {
                let tData;
                const date = o.effectiveDate.toString();

                if (od.observationCode) {
                  tData = {
                    x: date,
                    y: od.value,
                  };

                  if (od.value > max) {
                    max = od.value;
                  }

                  if (!labels.includes(date)) {
                    labels.push(date);
                  }

                  if (od.observationCode === 'oxygen-saturation') {
                    oxygenData.data.push(tData);
                    sumOxygenSat += od.value;
                  }
                  if (od.observationCode === 'heart-rate') {
                    heartRateData.data.push(tData);
                  }
                }
              });
            }
          });
        }

        this.chartHeaders = [
          {
            key: `${this.$t('average')} ${this.$t('oxygenSaturationLbl')}`,
            valueString: `${Math.round(sumOxygenSat / totalReadings)} ${oxygenSaturationUnit}`,
          },
          {
            key: `${this.$t('readingsTaken')}`,
            valueString: totalReadings,
          },
        ];

        return {
          type: 'line',
          labels: labels as string[],
          datasets: [oxygenData, heartRateData],
          options: this.getLineChartOptions(`${this.$t('oxygenSaturationYAxisChartLabel')}`,
            Math.floor(max + 10),
            0),
        };
      },
      getBloodGlucoseDataChart() {
        const dataTemplate = this.getDataChartTemplate();

        const bloodGlucoseUnit = 'mg/dL';
        const bloodGlucoseData = JSON.parse(JSON.stringify(dataTemplate));

        const bloodGlucoseLabel = this.$t('bloodGlucose');
        bloodGlucoseData.label = `${bloodGlucoseLabel} (${bloodGlucoseUnit})`;

        const labels: string[] = [];

        let max = 0;
        let min = 0;
        let totalReadings = 0;

        if (this.observations.length > 0) {

          totalReadings += this.observations.length;

          this.observations.forEach((o: PatientObservationDto) => {
            if (o.observationsData) {
              o.observationsData
                .filter((od) => od.observationCode === 'blood-glucose')
                .forEach((od) => {
                  let tData;
                  const date = o.effectiveDate.toString();
                  const value = convertToMgdl(od.value, od.unit);

                  if (od.observationCode) {
                    tData = {
                      x: date,
                      y: value,
                    };

                    max = value > max ? value : max;
                    min = (value <= min || min === 0) ? value : min;

                    if (!labels.includes(date)) {
                      labels.push(date);
                    }

                    bloodGlucoseData.data.push(tData);
                  }
                });
            }
          });
        }

        this.chartHeaders = [
          {
            key: this.$t('high'),
            valueString: `${max} ${bloodGlucoseUnit}`,
          },
          {
            key: this.$t('low'),
            valueString: `${min} ${bloodGlucoseUnit}`,
          },
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];

        return {
          type: 'line',
          labels: labels as string[],
          datasets: [bloodGlucoseData],
          options: this.getLineChartOptions(
            bloodGlucoseUnit,
            Math.floor((max + 10) / 10) * 10,
            Math.ceil((min - 10) / 10) * 10,
          ),
        };
      },
      getWeightDataChart() {
        const dataTemplate = this.getDataChartTemplate();

        const weightUnit = 'lbs';
        const weightData = JSON.parse(JSON.stringify(dataTemplate));

        const weightLabel = this.$t('weight');

        weightData.label = `${weightLabel} (${weightUnit})`;

        const labels: string[] = [];

        let max = 0;
        let min = 0;
        const totalReadings = this.observations.length;

        if (this.observations.length > 0) {

          this.observations.forEach((o: any) => {
            if (o.observationsData) {
              const date = o.effectiveDate.toString();

              o.observationsData.forEach((od: any) => {
                const value = convertToLb(od.value, od.unit);

                const tData: ChartPoint = {
                  x: date,
                  y: value,
                };

                if (value > max) {
                  max = value;
                }
                if (value <= min || min === 0) {
                  min = value;
                }

                if (!labels.includes(date)) {
                  labels.push(date);
                }

                weightData.data.push(tData);
              });
            }
          });
        }

        const total = weightData.data.map((d: ChartPoint) => d.y).reduce((sum: number, next: number) => sum + next, 0);
        const average = (total / totalReadings).toFixed(1);

        this.chartHeaders = [
          {
            key: this.$t('high'),
            valueString: `${max} ${weightUnit}`,
          },
          {
            key: this.$t('low'),
            valueString: `${min} ${weightUnit}`,
          },
          {
            key: this.$t('average'),
            valueString: `${average} ${weightUnit}`,
          },
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];

        return {
          type: 'line',
          labels: labels as string[],
          datasets: [weightData],
          options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
              display: false,
            },
            scales: {
              yAxes: [
                {
                  position: 'left',
                  scaleLabel: {
                    display: true,
                    labelString: weightUnit,
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                  ticks: {
                    beginAtZero: true,
                    stepSize: 10,
                    steps: 10,
                    stepValue: 5,
                    max: Math.floor((max + 10) / 10) * 10 as number,
                    min: Math.ceil((min - 10) / 10) * 10 as number,
                  },
                },
              ],
              xAxes: [
                {
                  position: 'center',
                  scaleLabel: {
                    display: true,
                    labelString: this.$t('dateAndTime'),
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                  ticks: {
                    maxRotation: 60,
                    minRotation: 60,
                  },
                  distribution: 'linear',
                  type: 'time',
                  time: {
                    unit: 'day',
                    displayFormats: {
                      day: this.getPatientDateFormat,
                      hour: this.getPatientTimeFormat,
                    },
                    tooltipFormat: this.getPatientDateAndTimeFormat,
                  },
                  bounds: 'ticks',
                },
              ],
            },
            tooltips: {
              callbacks: {
                label: (tooltipItem: any, data: any) => {
                  let labelStr = '';

                  const obser = this.observations.find((x: any) => {
                    const efectiveSrt = new Date(
                      x.effectiveDate.getTime() - (x.effectiveDate.getTimezoneOffset() * 60000))
                      .toISOString().substr(0, 16).replace('T', ' ');
                    const [year, month, day] = efectiveSrt.split(' ')[0].split('-');
                    const [hours, minutes] = efectiveSrt.split(' ')[1].split(':');
                    const dateFormat = `${month}/${day}/${year} ${hours}:${minutes}`;
                    return dateFormat === tooltipItem.label;
                  });

                  if (obser) {
                    const value = obser.observationsData[0].value || 0;
                    const labelValue = obser.observationsData[0].unit || '';
                    labelStr = `${this.$t('weight')} (${labelValue}): ${value}`;
                  } else {
                    const value = data.datasets[tooltipItem.datasetIndex].data?.[tooltipItem.index]?.y || 0;
                    const labelValue = data.datasets[tooltipItem.datasetIndex].label || '';
                    labelStr = `${labelValue}: ${value}`;
                  }
                  return labelStr;
                },
              },
            },
          },
        };
      },
      getRespiratoryRateDataChart() {
        const dataTemplate = this.getDataChartTemplate();

        const respiratoryRateUnit = 'rpm';
        const respiratoryRateData = JSON.parse(JSON.stringify(dataTemplate));
        const respiratoryRateLabel = this.$t('respiratoryRate');
        respiratoryRateData.label = `${respiratoryRateLabel} (${respiratoryRateUnit})`;
        const labels: string[] = [];

        let max = 0;
        let min = 0;
        let totalReadings = 0;

        if (this.observations.length > 0) {

          totalReadings += this.observations.length;

          this.observations.forEach((o: any) => {
            if (o.observationsData) {
              o.observationsData.forEach((od: any) => {
                let tData;
                const date = o.effectiveDate.toString();

                if (od.observationCode) {
                  tData = {
                    x: date,
                    y: od.value,
                  };
                  if (od.value > max) {
                    max = od.value;
                  }
                  if (od.value <= min || min === 0) {
                    min = od.value;
                  }

                  if (!labels.includes(date)) {
                    labels.push(date);
                  }

                  if (od.observationCode === 'respiratory-rate') {
                    respiratoryRateData.data.push(tData);
                  }
                }
              });
            }
          });
        }

        this.chartHeaders = [
          {
            key: this.$t('high'),
            valueString: `${max} ${respiratoryRateUnit}`,
          },
          {
            key: this.$t('low'),
            valueString: `${min} ${respiratoryRateUnit}`,
          },
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];

        return {
          type: 'line',
          labels: labels as string[],
          datasets: [respiratoryRateData],
          options: this.getLineChartOptions(
            this.$t('respiratoryRateYAxisChartLabel').toString(),
            70,
            0,
          ),
        };
      },
      getHeartRateDataChart() {
        const dataTemplate = this.getDataChartTemplate();

        const heartRateUnit = 'bpm';
        const heartRateData = JSON.parse(JSON.stringify(dataTemplate));
        const heartRateLabel = this.$t('heartRate');
        heartRateData.label = `${heartRateLabel} (${heartRateUnit})`;
        const labels: string[] = [];

        let max = 0;
        let min = 0;
        let totalReadings = 0;

        if (this.observations.length > 0) {

          totalReadings += this.observations.length;

          this.observations.forEach((o: any) => {
            if (o.observationsData) {
              o.observationsData.forEach((od: any) => {
                let tData;
                const date = o.effectiveDate.toString();

                if (od.observationCode) {
                  tData = {
                    x: date,
                    y: od.value,
                  };
                  if (od.value > max) {
                    max = od.value;
                  }
                  if (od.value <= min || min === 0) {
                    min = od.value;
                  }

                  if (!labels.includes(date)) {
                    labels.push(date);
                  }

                  if (od.observationCode === 'heart-rate') {
                    heartRateData.data.push(tData);
                  }
                }
              });
            }
          });
        }

        this.chartHeaders = [
          {
            key: this.$t('high'),
            valueString: `${max} ${heartRateUnit}`,
          },
          {
            key: this.$t('low'),
            valueString: `${min} ${heartRateUnit}`,
          },
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];

        return {
          type: 'line',
          labels: labels as string[],
          datasets: [heartRateData],
          options: this.getLineChartOptions(
            this.$t('heartRateYAxisChartLabel').toString(),
            max + 10,
            min - 10,
          ),
        };
      },
      buildObservationsChart() {
        let chartData = null;
        let bottomChart = null;

        switch (this.observationType) {
          case 'a1c':
            chartData = this.getA1cDataChart();
            break;
          case 'blood-glucose':
            chartData = this.getBloodGlucoseDataChart();
            break;
          case 'blood-pressure':
            chartData = this.getBloodPressureData();
            break;
          case 'body-temperature':
            chartData = this.getBodyTemperatureData();
            break;
          case 'weight':
            chartData = this.getWeightDataChart();
            break;
          case 'oxygen-saturation':
            chartData = this.getOxygenDataChart();
            break;
          case 'respiratory-rate':
            chartData = this.getRespiratoryRateDataChart();
            break;
          case 'heart-rate':
            chartData = this.getHeartRateDataChart();
            break;
          case 'workouts':
            chartData = this.getWorkoutTopData();
            bottomChart = this.getWorkoutBottomData();
            break;
          case 'sleep':
            chartData = this.getSleepDataChart();
            break;
        }

        if (chartData) {
          this.showChart = true;
          this.buildChart(chartData);
        }

        if (bottomChart) {
          this.showWorkoutChart = true;
          this.buildBottomChart(bottomChart);
        }
      },
      buildChart(chartConfig: any) {
        Chart.plugins.register(this.getSaveToPngChartPlugin());

        const ctx: any = document.getElementById('myChart');
        if (ctx) {
          this.chart = new Chart(ctx, {
            type: chartConfig.type,
            data: {
              labels: chartConfig.labels,
              datasets: chartConfig.datasets,
            },
            options: chartConfig.options,
          });
        }
      },
      buildBottomChart(chartConfig: any) {
        const bottomCtx: any = document.getElementById('myBottomChart');
        if (bottomCtx) {
          bottomCtx.style.display = '';
          this.bottomChart = new Chart(bottomCtx, {
            type: chartConfig.type,
            data: {
              labels: chartConfig.labels,
              datasets: chartConfig.datasets,
            },
            options: chartConfig.options,
          });
        }
      },
      getBodyTemperatureData() {
        const dataTemplate = this.getDataChartTemplate();

        const bodyTemperatureUnit = 'ºF';
        const temperatureData = JSON.parse(JSON.stringify(dataTemplate));

        const bodyTemperatureLabel = this.$t('bodyTemperature');

        temperatureData.label = `${bodyTemperatureLabel} (${bodyTemperatureUnit})`;

        const labels: string[] = [];

        let max = 0;
        let min = 200;
        let totalReadings = 0;

        if (this.observations.length > 0) {

          totalReadings += this.observations.length;

          this.observations.forEach((o: any) => {
            if (o.observationsData) {
              o.observationsData.forEach((od: any) => {
                let tData;
                const date = o.effectiveDate.toString();
                const value = convertToF(od.value, od.unit);

                if (od.observationCode) {
                  tData = {
                    x: date,
                    y: value,
                  };

                  if (value > max) {
                    max = value;
                  }

                  if (value < min) {
                    min = value;
                  }

                  if (!labels.includes(date)) {
                    labels.push(date);
                  }

                  temperatureData.data.push(tData);
                }
              });
            }
          });
        }

        this.chartHeaders = [
          {
            key: this.$t('high'),
            valueString: `${max} ${bodyTemperatureUnit}`,
          },
          {
            key: this.$t('low'),
            valueString: `${min} ${bodyTemperatureUnit}`,
          },
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];

        return {
          type: 'line',
          labels: labels as string[],
          datasets: [temperatureData],
          options: this.getLineChartOptions(
            '°F',
            Math.floor(max + 10),
            Math.floor(min - 10),
          ),
        };
      },
      getWorkoutTopData() {
        const dataTemplate = {
          id: '',
          label: '',
          barThickness: 'flex',
          data: [] as any,
          borderColor: '#3ba6e4',
          backgroundColor: '#007EC7CC',
          yAxisID: '',
        };

        const distanceUnit = 'mi';
        const stepsUnit = this.$t('steps');

        const distanceData = JSON.parse(JSON.stringify(dataTemplate));
        const stepsData = JSON.parse(JSON.stringify(dataTemplate));

        distanceData.id = 'distance';
        distanceData.label = distanceUnit;
        distanceData.yAxisID = 'distance';
        stepsData.id = 'steps';
        stepsData.label = stepsUnit;
        stepsData.yAxisID = 'steps';

        const labelsTopChart: string[] = [];

        stepsData.borderColor = '#7558B3CC';
        stepsData.backgroundColor = '#7558B3CC';

        let totalReadings = 0;
        let maxSteps = 0;
        let maxDistance = 0;

        if (this.observations.length > 0) {

          totalReadings += this.observations.length;

          this.observations.forEach((o: any) => {

            if (o.observationsData) {
              o.observationsData.forEach((od: any) => {
                const date = format(o.effectiveDate, this.getPatientDateFormatNoUpper);

                if (od.observationCode) {

                  if (!labelsTopChart.includes(date)) {
                    labelsTopChart.push(date);
                  }

                  switch (od.observationCode) {
                    case 'distance': {
                      const value = od.value;

                      if (maxDistance < value) {
                        maxDistance = value;
                      }

                      distanceData.data.push(
                        {
                          x: date,
                          y: value,
                        },
                      );
                      break;
                    }
                    case 'steps':
                      if (maxSteps < od.value) {
                        maxSteps = od.value;
                      }
                      stepsData.data.push(
                        {
                          x: date,
                          y: od.value,
                        },
                      );
                      break;
                    default:
                      break;
                  }
                }
              });
            }
          });
        }
        labelsTopChart.sort((a: string, b: string) =>
          parse(a, this.getPatientDateFormatNoUpper, new Date()).getTime() -
          parse(b, this.getPatientDateFormatNoUpper, new Date()).getTime());

        this.chartHeaders = [
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];
        const dataSets = [];

        if (distanceData.data.length > 0) {
          this.validateWorkoutsData(labelsTopChart, distanceData);
          dataSets.push(distanceData);
        }

        if (stepsData.data.length > 0) {
          this.validateWorkoutsData(labelsTopChart, stepsData);
          dataSets.push(stepsData);
        }

        return {
          type: 'bar',
          responsive: true,
          maintainAspectRatio: false,
          labels: labelsTopChart as string[],
          datasets: dataSets,
          options: {
            legend: {
              display: false,
            },
            barValueSpacing: 10,
            scales: {
              yAxes: [
                {
                  id: 'distance',
                  position: 'left',
                  scaleLabel: {
                    display: true,
                    labelString: `${distanceData.label}`,
                  },
                  ticks: {
                    min: 0,
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                },
                {
                  id: 'steps',
                  position: 'right',
                  scaleLabel: {
                    display: true,
                    labelString: `${stepsData.label}`,
                  },
                  ticks: {
                    min: 0,
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                },
              ],
              xAxes: [
                {
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                },
              ],
            },
            tooltips: {
              enabled: false,
              custom: this.getWorkoutsCustomToolTips(),
              callbacks: this.getWorkoutsTooltipsCallbacks(),
            },
          },
        };
      },
      getWorkoutBottomData() {
        const dataTemplate = {
          id: '',
          label: '',
          barThickness: 'flex',
          data: [] as any,
          borderColor: '#3ba6e4',
          backgroundColor: '#007EC7CC',
          yAxisID: '',
        };

        const caloriesUnit = 'kcal';
        const durationUnit = 'hh:mm';
        const heartRateUnit = 'bpm';

        const caloriesData = JSON.parse(JSON.stringify(dataTemplate));
        const durationData = JSON.parse(JSON.stringify(dataTemplate));
        const heartRatedata = JSON.parse(JSON.stringify(dataTemplate));

        caloriesData.id = 'calories-burned';
        caloriesData.label = caloriesUnit;
        caloriesData.yAxisID = 'calories-burned';
        durationData.id = 'fitness-duration';
        durationData.label = durationUnit;
        durationData.yAxisID = 'fitness-duration';
        heartRatedata.id = 'heart-rate';
        heartRatedata.label = heartRateUnit;
        heartRatedata.yAxisID = 'heart-rate';

        const labelsBottonChart: string[] = [];

        let totalReadings = 0;

        heartRatedata.borderColor = '#DA4250CC';
        heartRatedata.backgroundColor = '#DA4250CC';

        caloriesData.borderColor = '#007EC7CC';
        caloriesData.backgroundColor = '#007EC7CC';

        durationData.borderColor = '#7558B3CC';
        durationData.backgroundColor = '#7558B3CC';

        if (this.observations.length > 0) {

          totalReadings += this.observations.length;

          this.observations.forEach((o: any) => {

            if (o.observationsData) {
              o.observationsData.forEach((od: any) => {
                const date = format(o.effectiveDate, this.getPatientDateFormatNoUpper);

                if (od.observationCode) {

                  if (!labelsBottonChart.includes(date)) {
                    labelsBottonChart.push(date);
                  }

                  switch (od.observationCode) {
                    case 'fitness-duration':
                      durationData.data.push(
                        {
                          x: date,
                          y: od.value,
                        },
                      );
                      break;
                    case 'calories-burned':
                      caloriesData.data.push(
                        {
                          x: date,
                          y: od.value,
                        },
                      );
                      break;
                    case 'heart-rate':
                      heartRatedata.data.push(
                        {
                          x: date,
                          y: od.value,
                        },
                      );
                      break;
                    default:
                      break;
                  }
                }
              });
            }
          });
        }

        labelsBottonChart.sort((a: string, b: string) =>
          parse(a, this.getPatientDateFormatNoUpper, new Date()).getTime() -
          parse(b, this.getPatientDateFormatNoUpper, new Date()).getTime());

        this.chartHeaders = [
          {
            key: this.$t('readingsTaken'),
            valueString: totalReadings,
          },
        ];
        const dataSets = [];

        if (heartRatedata.data.length > 0) {
          this.validateWorkoutsData(labelsBottonChart, heartRatedata);
          dataSets.push(heartRatedata);
        }

        if (caloriesData.data.length > 0) {
          this.validateWorkoutsData(labelsBottonChart, caloriesData);
          dataSets.push(caloriesData);
        }

        if (durationData.data.length > 0) {
          this.validateWorkoutsData(labelsBottonChart, durationData);
          dataSets.push(durationData);
        }

        return {
          type: 'bar',
          responsive: true,
          maintainAspectRatio: false,
          labels: labelsBottonChart as string[],
          datasets: dataSets,
          options: {
            legend: {
              display: false,
            },
            barValueSpacing: 10,
            scales: {
              yAxes: [
                {
                  id: 'calories-burned',
                  position: 'left',
                  scaleLabel: {
                    display: true,
                    labelString: `${caloriesData.label}`,
                  },
                  ticks: {
                    min: 0,
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                },
                {
                  id: 'heart-rate',
                  position: 'left',
                  scaleLabel: {
                    display: true,
                    labelString: `${heartRatedata.label}`,
                  },
                  ticks: {
                    min: 0,
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                },
                {
                  id: 'fitness-duration',
                  position: 'right',
                  scaleLabel: {
                    display: true,
                    labelString: `${durationData.label}`,
                  },
                  ticks: {
                    beginAtZero: true,
                    min: 0,
                    max: 86400,
                    callback: (durationLabel: number): string => {
                      const h = Math.floor(durationLabel / 3600);
                      const m = Math.floor(durationLabel % 3600 / 60);
                      const hDisplay = h > 0 ? (h < 10 ? `0${h}:` : `${h}:`) : '00:';
                      const mDisplay = m > 0 ? (m < 10 ? `0${m}` : `${m}`) : '00';
                      return `${hDisplay}${mDisplay}`;
                    },
                  },
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                },
              ],
              xAxes: [
                {
                  gridLines: {
                    color: this.getChartGridLinesColor,
                    zeroLineColor: this.getChartGridLinesColor,
                  },
                },
              ],
            },
            tooltips: {
              enabled: false,
              custom: this.getWorkoutsCustomToolTips(),
              callbacks: this.getWorkoutsTooltipsCallbacks(),
            },
          },
        };
      },
      getSleepDataChart() {
        const dataTemplate = this.getBarDataChartTemplate();
        this.chartHeaders = null;

        const totalData = JSON.parse(JSON.stringify(dataTemplate));
        const remData = JSON.parse(JSON.stringify(dataTemplate));
        const deepData = JSON.parse(JSON.stringify(dataTemplate));
        const lightData = JSON.parse(JSON.stringify(dataTemplate));
        const awakeData = JSON.parse(JSON.stringify(dataTemplate));

        totalData.label = this.$t('total').toString();
        remData.label = this.$t('rem').toString();
        deepData.label = this.$t('deep').toString();
        lightData.label = this.$t('light').toString();
        awakeData.label = this.$t('awake').toString();

        totalData.order = 0;
        remData.order = 4;
        deepData.order = 3;
        lightData.order = 2;
        awakeData.order = 1;

        totalData.backgroundColor = '#007EC7CC';
        remData.backgroundColor = '#7558B3CC';
        lightData.backgroundColor = '#EEC138CC';
        deepData.backgroundColor = '#DA4250CC';
        awakeData.backgroundColor = '#35BC9BCC';

        const labels: string[] = [];
        let maxTotalSleep = 0;

        if (this.observations.length > 0) {
          this.observations.forEach((o: any) => {
            if (o.observationsData) {
              const date = o.effectiveDate.toString();

              const total = o.observationsData.find((d: IPatientObservationDataDto) =>
                d.observationCode === 'total')?.value || 0;
              const rem = o.observationsData.find((d: IPatientObservationDataDto) =>
                d.observationCode === 'rem')?.value || 0;
              const deep = o.observationsData.find((d: IPatientObservationDataDto) =>
                d.observationCode === 'deep')?.value || 0;
              const light = o.observationsData.find((d: IPatientObservationDataDto) =>
                d.observationCode === 'light')?.value || 0;
              const awake = o.observationsData.find((d: IPatientObservationDataDto) =>
                d.observationCode === 'awake')?.value || 0;

              if (!labels.includes(date)) {
                labels.push(date);
              }

              maxTotalSleep = total > maxTotalSleep ? total : maxTotalSleep;

              totalData.data.push({ x: date, y: (rem === 0 && deep === 0 && light === 0 && awake === 0) ? total : 0 });
              remData.data.push({ x: date, y: rem });
              deepData.data.push({ x: date, y: deep });
              lightData.data.push({ x: date, y: light });
              awakeData.data.push({ x: date, y: awake });
            }
          });
        }

        let sumTotalValues = 0;
        totalData.data.forEach((item: any) => sumTotalValues += item.y);

        let totalHours = (maxTotalSleep + 7200) / 3600;
        const modHours = totalHours % 2;
        totalHours += modHours > 0 ? 2 - modHours : 0;
        const maxTotalSeconds = Math.floor(totalHours) * 60 * 60;

        const chartDataSets = sumTotalValues > 0 ?
          [totalData, awakeData, lightData, deepData, remData] :
          [awakeData, lightData, deepData, remData];

        this.chartHeaders = [];

        return {
          type: 'bar',
          labels: labels as string[],
          datasets: chartDataSets,
          options: this.getBarChartOptions(
            this.$t('timeHours').toString(),
            maxTotalSeconds,
            0,
          ),
        };
      },
      validateWorkoutsData(labels: string[], data: any): void {
        if (labels.length > 0) {

          labels.forEach((l) => {
            let d = data.data.find((o: any) => o.x === l);

            if (!d) {
              d = {
                x: l,
                y: 0,
              };
              data.data.push(d);
            }
          });
          this.sortWorkoutsData(data);
        }
      },
      sortWorkoutsData(data: any) {
        data.data.sort((a: any, b: any) => {
          return parse(a.x, this.getPatientDateFormatNoUpper, new Date()).getTime() -
            parse(b.x, this.getPatientDateFormatNoUpper, new Date()).getTime();
        });
      },
      getWorkoutsCustomToolTips() {
        const customTooltips = function(tooltipModel: any) {
          // Tooltip Element
          let tooltipEl = document.getElementById('chartjs-tooltip');

          // Create element on first render
          if (!tooltipEl) {
            tooltipEl = document.createElement('div');
            tooltipEl.id = 'chartjs-tooltip';
            tooltipEl.style.opacity = '1';
            tooltipEl.innerHTML = '<table></table>';
            document.body.appendChild(tooltipEl);
          }

          // Hide if no tooltip
          if (tooltipModel.opacity === 0) {
            tooltipEl.style.opacity = '0';
            return;
          }

          // Set caret Position
          tooltipEl.classList.remove('above', 'below', 'no-transform');
          if (tooltipModel.yAlign) {
            tooltipEl.classList.add(tooltipModel.yAlign);
          } else {
            tooltipEl.classList.add('no-transform');
          }

          function getBody(bodyItem: any) {
            return bodyItem.lines;
          }

          // Set Text
          if (tooltipModel.body) {
            const titleLines = tooltipModel.title || [];
            const bodyLines = tooltipModel.body.map(getBody);

            let innerHtml = '<thead>';

            titleLines.forEach((title: any) => {
              innerHtml += '<tr><th colspan="2">' + title + '</th></tr>';
            });
            innerHtml += '</thead><tbody>';

            bodyLines.forEach((body: string) => {
              innerHtml += body;
            });
            innerHtml += '</tbody>';

            const tableRoot = tooltipEl.querySelector('table');
            tableRoot.innerHTML = innerHtml;
          }

          // `this` will be the overall tooltip
          // @ts-ignore - Support for Vuetify form validation for TS set for v3
          const position = this._chart.canvas.getBoundingClientRect();

          // Display, position, and set styles for font
          tooltipEl.style.opacity = '1';
          tooltipEl.style.position = 'absolute';
          tooltipEl.style.left = position.left + window.pageXOffset + tooltipModel.caretX + 'px';
          tooltipEl.style.top = position.top + window.pageYOffset + tooltipModel.caretY - 80 + 'px';
          tooltipEl.style.fontFamily = tooltipModel._bodyFontFamily;
          tooltipEl.style.fontSize = tooltipModel.bodyFontSize + 'px';
          tooltipEl.style.fontStyle = tooltipModel._bodyFontStyle;
          tooltipEl.style.padding = tooltipModel.yPadding + 'px ' + tooltipModel.xPadding + 'px';
          tooltipEl.style.pointerEvents = 'none';
        };
        return customTooltips;
      },
      getWorkoutsTooltipsCallbacks() {
        return {
          label: (tooltipItem: any, data: any): any => {
            let htmlLabel = '';
            const datasetId = data.datasets[tooltipItem.datasetIndex].id || '';

            const strDate = tooltipItem.label;
            const date = parse(strDate, this.getPatientDateFormatNoUpper, new Date());

            const dateObservation: any = this.observations.find((o: any) =>
              o.effectiveDate.getFullYear() === date.getFullYear() &&
              o.effectiveDate.getMonth() === date.getMonth() &&
              o.effectiveDate.getDate() === date.getDate(),
            );

            const duration = dateObservation.observationsData.find((o: any) => {
              return o.observationCode === 'fitness-duration';
            });
            const distance = dateObservation.observationsData.find((o: any) => {
              return o.observationCode === 'distance';
            });
            const steps = dateObservation.observationsData.find((o: any) => {
              return o.observationCode === 'steps';
            });
            const heartRate = dateObservation.observationsData.find((o: any) => {
              return o.observationCode === 'heart-rate';
            });
            const calories = dateObservation.observationsData.find((o: any) => {
              return o.observationCode === 'calories-burned';
            });
            const durationStyle = 'background: #7558B3CC; border-color: #ff00003d; border-width: 2px';
            const durationSpan = `<span class="chartjs-tooltip-key" style="${durationStyle}"></span>`;
            const distanceStyle = 'background: #007EC7CC; border-color: #ff00003d; border-width: 2px';
            const distanceSpan = `<span class="chartjs-tooltip-key" style="${distanceStyle}"></span>`;
            const stepsStyle = 'background: #7558B3CC; border-color: #ff00003d; border-width: 2px';
            const stepsSpan = `<span class="chartjs-tooltip-key" style="${stepsStyle}"></span>`;
            const heartRateStyle = 'background: #DA4250CC; border-color: #ff00003d; border-width: 2px';
            const heartRateSpan = `<span class="chartjs-tooltip-key" style="${heartRateStyle}"></span>`;
            const caloriesBurnedStyle = 'background: #007EC7CC; border-color: #ff00003d; border-width: 2px';
            const caloriesBurnedSpan = `<span class="chartjs-tooltip-key" style="${caloriesBurnedStyle}"></span>`;

            if (duration) {
              const labelStyle = datasetId === 'fitness-duration' ? 'style="font-weight: bold;"' : '';
              const h = Math.floor(duration.value / 3600);
              const m = Math.floor(duration.value % 3600 / 60);
              const hDisplay = h > 0 ? (h < 10 ? `0${h}:` : `${h}:`) : '00:';
              const mDisplay = m > 0 ? (m < 10 ? `0${m}` : `${m}`) : '00';
              const units = h > 0 ? 'hrs' : 'min';
              htmlLabel += `<tr><td ${labelStyle}>${durationSpan}${this.$t('duration')}</td><td ${labelStyle}>${hDisplay}${mDisplay} ${units}</td></tr>`;
            }

            if (distance) {
              const labelStyle = datasetId === 'distance' ? 'style="font-weight: bold;"' : '';
              htmlLabel += `<tr><td ${labelStyle}>${distanceSpan}${this.$t('distance')}</td><td ${labelStyle}>${distance.value} ${distance.unit}</td></tr>`;
            }

            if (steps) {
              const labelStyle = datasetId === 'steps' ? 'style="font-weight: bold;"' : '';
              htmlLabel += `<tr><td ${labelStyle}>${stepsSpan}${this.$t('steps')}</td><td ${labelStyle}>${steps.value} ${this.$t(steps.unit.toLowerCase())}</td></tr>`;
            }

            if (heartRate) {
              const labelStyle = datasetId === 'heart-rate' ? 'style="font-weight: bold;"' : '';
              htmlLabel += `<tr><td ${labelStyle}>${heartRateSpan}${this.$t('average')} ${this.$t('heartRate')}</td><td ${labelStyle}>${heartRate.value} ${heartRate.unit}</td></tr>`;
            }

            if (calories) {
              const labelStyle = datasetId === 'calories-burned' ? 'style="font-weight: bold;"' : '';
              htmlLabel += `<tr><td ${labelStyle}>${caloriesBurnedSpan}${this.$t('caloriesBurned')}</td><td ${labelStyle}>${calories.value} ${calories.unit}</td></tr>`;
            }
            return htmlLabel;
          },
        };
      },
      reRenderCharts(): void {
        const chart = this.chart as Chart;
        if (chart && typeof chart.destroy === 'function') {
          chart.destroy();
        }
        const bottomChart = this.bottomChart as Chart;
        if (bottomChart && typeof bottomChart.destroy === 'function') {
          bottomChart.destroy();
        }
        this.buildObservationsChart();
      },
      getSaveToPngChartPlugin(): any {
        const plugin = {
          id: 'savetopng',
          afterRender: () => this.emitChartsInformation(),
        };
        return plugin;
      },
      emitChartsInformation(): void {
        const chartsInformation: IChartInformation = {
          ChartsImages: this.getChartsImages,
          ChartHeaders: this.chartHeaders,
        };
        this.$emit('sendChartInformation', chartsInformation);
      },
    },
    computed: {
      ...mapState('patients', ['currentPatient']),
      getChartGridLinesColor(): string {
        return this.$vuetify.theme.dark ? 'rgba(255, 255, 255, 0.12)' : 'rgba(0, 0, 0, 0.12)';
      },
      getChartsImages(): string[] {
        const chart = this.chart as Chart;
        const charts = [];
        if (chart && chart.canvas && typeof chart.toBase64Image === 'function') {
          charts.push(chart.toBase64Image());
        }
        const bottomChart = this.bottomChart as Chart;
        if (this.observationType === 'workouts' &&
          bottomChart &&
          bottomChart.canvas &&
          typeof bottomChart.toBase64Image === 'function') {
          charts.push(bottomChart.toBase64Image());
        }
        return charts;
      },
      getPatientDateFormat(): string {
        return this.currentPatient?.dateFormat?.toUpperCase() || 'MM/DD/YYYY';
      },
      getPatientDateFormatNoUpper(): string {
        return this.currentPatient?.dateFormat || 'MM/dd/yyyy';
      },
      getPatientTimeFormat(): string {
        return (this.currentPatient?.timeFormat || '24 H') === '24 H' ?
          'HH:mm' : 'hh:mm a';
      },
      getPatientDateAndTimeFormat(): string {
        return `${this.getPatientDateFormat} ${this.getPatientTimeFormat}`;
      },
    },
    watch: {
      observations(val) {
        this.showChart = false;
        this.showWorkoutChart = false;
        this.chartHeaders = null;

        const bottomCtx: any = document.getElementById('myBottomChart');
        if (bottomCtx) {
          bottomCtx.style.display = 'none';
        }

        if (this.chart && typeof this.chart.destroy === 'function') {
          this.chart.destroy();
        }

        if (this.bottomChart && typeof this.bottomChart.destroy === 'function') {
          this.bottomChart.destroy();
        }

        if (val.length > 0) {
          this.buildObservationsChart();
        } else {
          this.showChart = false;
        }
      },
    },
    mounted() {
      this.$watch('$vuetify.theme.dark', this.reRenderCharts);
      this.$watch('$i18n.locale', this.reRenderCharts);
      this.$watch('currentPatient.dateFormat', this.reRenderCharts);
      this.$watch('currentPatient.timeFormat', this.reRenderCharts);
    },
  });
</script>

<style lang="scss">
.chart-container {
  position: relative;
  height: calc(100vh - 400px);
  width: 100%;
}

.bottom-chart-container {
  position: relative;
  height: calc(105vh - 400px);
  width: 100%;
  padding-top: 5vh;
}
</style>
