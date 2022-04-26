<template>
  <v-sheet class="flex fill-height">
    <div id="chart">
      <apexchart
        type="line"
        ref="realtimeChart"
        :options="chartOptions"
        :series="series"
        :class="observationType"
      ></apexchart>
    </div>
  </v-sheet>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  name: "ObservationChart",
  data(): any {
    return {
      showChart: false,
      series: [
        {
          name: 'label A',
          data: [0],
        },
        {
          name: 'label B',
          data: [0],
        },
      ],
      chartOptions: {
        chart: {
          height: 350,
          type: "line",
          dropShadow: {
            enabled: true,
            color: "#000",
            top: 18,
            left: 7,
            blur: 10,
            opacity: 0.2,
          },
          toolbar: {
            show: true,
            autoSelected: "zoom",
            tools: {
              download: false,
            },
          },
        },
        annotations: {
          yaxis: [
            {
              y: 32,
              y2: 20,
              fillColor: "var(--v-primary-base)",
              opacity: 0.1,
            },
          ],
        },
        colors: ["var(--v-primary-base)", "var(--v-secondary-base)"],
        dataLabels: {
          enabled: true,
          offsetX: 0,
          offsetY: 1,
          style: {
            fontSize: "12px",
            fontFamily: '"Open Sans", Tahoma, sans-serif !important',
          },
        },
        stroke: {
          curve: "smooth",
        },
        title: {
          text: this.$t(this.observationType),
          align: "left",
        },
        grid: {
          borderColor: "var(--v-lightGrey-base)",
          row: {
            colors: ["var(--v-backgroundAlt-base)", "transparent"], // takes an array which will be repeated on columns
            opacity: 1,
          },
        },
        markers: {
          size: 6,
          hover: {
            size: undefined,
            sizeOffset: 1,
          },
        },
        xaxis: {
          categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
          title: {
            text: "Month",
          },
        },
        yaxis: {
          title: {
            text: this.$t(this.observationType),
          },
          // min: 5,
          // max: 40,
        },
        legend: {
          fontFamily: '"Open Sans", Tahoma, sans-serif !important',
          position: "bottom",
          horizontalAlign: "left",
          floating: true,
          offsetY: 8,
          offsetX: 0,
        },
      },
    };
  },
  mounted() {
    this.showChart = true;
    this.updateChart();
    // console.log(this.observations);
  },
  updated() {
    this.updateChart();
  },
  props: {
    observationType: String,
    observations: {
      type: Array,
    },
  },
  methods: {
    updateChart() {
      this.$refs.realtimeChart.updateOptions({
        title: {
          text: this.$t(this.observationType),
        },
        yaxis: {
          title: {
            text: this.$t(this.observationType),
          },
        },
      });
      this.$refs.realtimeChart.updateSeries([
        {
          name: "Systolic",
          data: this.getDataA(),
        },
        {
          name: "Diastolic",
          data: this.getDataB(),
        },
      ]);
      // this.getData();
    },
    getDataA(): any {
      // console.log(this.observations);
      const newObsA = [];
      const obs = this.observations.slice(0, 10);
      for (const ob of obs) {
        newObsA.push(ob.observationsData[0].value);
      }
      // console.log(newObs);
      return newObsA;
    },
    getDataB(): any {
      // console.log(this.observations);
      const newObsB = [];
      const obs = this.observations.slice(0, 10);
      for (const ob of obs) {
        newObsB.push(ob.observationsData[1].value);
      }
      // console.log(newObs);
      return newObsB;
    },
  },
  computed: {

  },
});
</script>
