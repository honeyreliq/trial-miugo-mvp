<template>
  <section class="BaseGoogleMap">
    <v-row class="whiteX mx-0">
      <v-col class="map pa-0 pr-2" cols="12" :md="(useDirectionsPanel)?'8':'12'">
        <div class="wrapper">
          <!-- custom controls layer in front of map -->
          <div class="map-custom-ui">
            <section class="buttons d-flex align-end flex-column">
              <v-btn
                  v-if="apiLoaded && openFn"
                  small
                  top
                  right
                  class="ma-2 btn white black--text"
                  title="Expand Map"
                  @click="openFn()"
              >
                <fa-icon :icon="['far', 'expand']" class="fa-fw" />
              </v-btn>
              <v-btn
                  v-if="apiLoaded && useSwitchLocation"
                  small
                  top
                  right
                  class="ma-2 btn white black--text"
                  title="Swap Directions"
                  @click="switchDirections()"
              >
                <fa-icon :icon="['far', 'exchange']" class="fa-fw swap-icon" />
              </v-btn>
            </section>
          </div>
          <!-- google map layer -->
          <div :class="(useDirectionsPanel)?'map-container mt-2' : 'map-container'" ref="googleMap"></div>
        </div>
      </v-col>
      <v-col cols="12" md="4" v-if="useDirectionsPanel">
        <!-- direction details container -->
        <div class="map-directions" ref="googleMapDirections"></div>
      </v-col>
    </v-row>
    <!-- bottom details -->
    <div v-if="!hideDetails" class="caption event-date pa-2">{{ eventTime }}</div>
  </section>
</template>

<script lang="ts">
import Vue from 'vue';
import loadGoogleMapsApi from 'load-google-maps-api';

interface ILocation {
  lat: number;
  lng: number;
}

/* global google */
const API_KEY = 'AIzaSyCpU0ERo1F1HCVFBUf3jt9OGp8fZ8wIME8';
const MAP_CONFIG = {
  // api uses metrical units (km) by default
  scaleControl: true,
  fullscreenControl: false, // custom zoom control for modal
  streetViewControl: false, // no pacman
  center: {},
  styles: null as any[],
};

const DARK_MODE_STYLES = [
  { elementType: "geometry", stylers: [{ color: "#242f3e" }] },
  { elementType: "labels.text.stroke", stylers: [{ color: "#242f3e" }] },
  { elementType: "labels.text.fill", stylers: [{ color: "#746855" }] },
  {
    featureType: "administrative.locality",
    elementType: "labels.text.fill",
    stylers: [{ color: "#d59563" }],
  },
  {
    featureType: "poi",
    elementType: "labels.text.fill",
    stylers: [{ color: "#d59563" }],
  },
  {
    featureType: "poi.park",
    elementType: "geometry",
    stylers: [{ color: "#263c3f" }],
  },
  {
    featureType: "poi.park",
    elementType: "labels.text.fill",
    stylers: [{ color: "#6b9a76" }],
  },
  {
    featureType: "road",
    elementType: "geometry",
    stylers: [{ color: "#38414e" }],
  },
  {
    featureType: "road",
    elementType: "geometry.stroke",
    stylers: [{ color: "#212a37" }],
  },
  {
    featureType: "road",
    elementType: "labels.text.fill",
    stylers: [{ color: "#9ca5b3" }],
  },
  {
    featureType: "road.highway",
    elementType: "geometry",
    stylers: [{ color: "#746855" }],
  },
  {
    featureType: "road.highway",
    elementType: "geometry.stroke",
    stylers: [{ color: "#1f2835" }],
  },
  {
    featureType: "road.highway",
    elementType: "labels.text.fill",
    stylers: [{ color: "#f3d19c" }],
  },
  {
    featureType: "transit",
    elementType: "geometry",
    stylers: [{ color: "#2f3948" }],
  },
  {
    featureType: "transit.station",
    elementType: "labels.text.fill",
    stylers: [{ color: "#d59563" }],
  },
  {
    featureType: "water",
    elementType: "geometry",
    stylers: [{ color: "#17263c" }],
  },
  {
    featureType: "water",
    elementType: "labels.text.fill",
    stylers: [{ color: "#515c6d" }],
  },
  {
    featureType: "water",
    elementType: "labels.text.stroke",
    stylers: [{ color: "#17263c" }],
  },
];

export default Vue.extend({
  name: 'BaseGoogleMap',
  components: {},
  computed: {},
  watch: {
    async '$vuetify.theme.dark'(): Promise<void> {
      await this.renderMap();
    }
  },
  props: {
    homeLocation: Object, // ILocation
    patientLocation: Object, // ILocation
    eventTime: String,
    hideDetails: Boolean, // hide bottom details
    openFn: Function, // if given, the option will be displayed
    useDirectionsPanel: Boolean, // show directions panel
    useSwitchLocation: Boolean,
    // these prop can be passed through, they will they will override the defaults
    mapOptions: Object,
    boundaryOptions: Object,
    markerOptions: Object,
  },
  data() {
    return {
      mapData: {
        origin: this.homeLocation,
        destination: this.patientLocation,
      },
      apiLoaded: false,
      map: null,
      directionsRenderer: null,
    };
  },
  methods: {
    createBoundaries(): void {
      const boundaryOptions = {
        ...{
          strokeColor: '#ff0000',
          strokeOpacity: 1,
          strokeWeight: 1,
          fillColor: '#ff0000',
          fillOpacity: 0.1,
          map: this.map,
          center: this.homeLocation,
          radius: 100,

        }, ...this.boundaryOptions,
      };

      new google.maps.Circle(boundaryOptions);
    },
    centerMapView(googleApi: any): void {
      const allPositions: ILocation[] = [this.mapData.origin, this.mapData.destination];
      const googlePositions = new googleApi.LatLngBounds();
      // there is not zoom, required, here we are setting an automatic zoom depending of all positions
      allPositions.forEach((position) => {
        googlePositions.extend(position);
      });

      this.map.fitBounds(googlePositions);
    },
    createMarkers(): void {
      const markerOptions = {
        ...{
          animation: google.maps.Animation.DROP,
        },
        ...this.markerOptions,
      };

      // delaying markers animations
      setTimeout(() => {
        const homeMarkerOptions = {
          ...markerOptions, ...{
            position: this.homeLocation,
            map: this.map,
            title: 'Patient\'s Home',
          },
        };
        new google.maps.Marker(homeMarkerOptions);

        const patientMarkerOptions = {
          ...markerOptions, ...{
            position: this.patientLocation,
            map: this.map,
            title: 'Patient\'s Last Location',
          },
        };
        new google.maps.Marker(patientMarkerOptions);

      }, 200);
    },
    createDirectionsPanel(): void {
      const mapDirectionsContainer: HTMLElement = this.$refs.googleMapDirections as HTMLElement;
      if (!this.directionsRenderer) {
        this.directionsRenderer = new google.maps.DirectionsRenderer({
          map: this.map,
          panel: mapDirectionsContainer,
          suppressMarkers: true,
        });
      }

      const directionsService = new google.maps.DirectionsService();
      directionsService.route({
            origin: this.mapData.origin,
            destination: this.mapData.destination,
            travelMode: google.maps.TravelMode.WALKING,
            unitSystem: google.maps.UnitSystem.IMPERIAL,
          },
          (response, status) => {
            if (status === 'OK') {
              this.directionsRenderer.setDirections(response);
            }
          },
      );

    },
    switchDirections(): void {
      // swapping directions before re-render map
      const origin: any = Object.assign({}, this.mapData.origin);
      const destination: any = Object.assign({}, this.mapData.destination);

      this.mapData.origin = destination;
      this.mapData.destination = origin;

      this.createDirectionsPanel();
    },
    async renderMap(): Promise<void> {
      const googleApi: any = await loadGoogleMapsApi({
        key: API_KEY,
      });

      MAP_CONFIG.center = this.homeLocation;
      MAP_CONFIG.styles = this.$vuetify.theme.dark ? DARK_MODE_STYLES : null;
      this.apiLoaded = true;

      const mapContainer: HTMLElement = this.$refs.googleMap as HTMLElement;
      this.map = new googleApi.Map(mapContainer, {...MAP_CONFIG, ...this.mapOptions});
      this.createBoundaries();
      this.createDirectionsPanel();
      this.createMarkers();
      this.centerMapView(googleApi);
    },
  },
  async mounted() {
    await this.renderMap();
  },
  destroyed() {
    this.apiLoaded = false;
  },
});
</script>
<style lang="scss">
.BaseGoogleMap {
  position: relative;
  // border: 1px solid rgba(0, 0, 0, 0.12);

  .map {
    // there is no reversed exchange font-awesome icon
    .swap-icon{
      transform: rotate(90deg);
    }

    .wrapper {
      height: 100%;
      position: relative;
    }

    .map-custom-ui {
      position: absolute;
      width: 100%;
      height: 100%;

      .btn {
        z-index: 1;
      }

      .buttons {
        height: 100%;
      }
    }

    .map-container {
      min-height: 300px;
      width: 100%;
    }
  }

  // this hack removes warning messages due to google.maps.TravelMode.WALKING is in beta
  .warnbox-content, .adp-warnbox {
    display: none;
  }

  .adp, .adp table {
    color:inherit;
  }

  .map-directions {
    .adp-placemark {
      border: 0px solid silver !important;
      background-color: transparent;
    }
    .adp-directions .adp-substep {
      padding: 8px !important;
    }
    .adp-marker2 {
      margin: 0 16px !important;
    }
    .adp-text {
      font-size: 16px;
      font-weight: 500;
      padding-right: 16px;
    }
  }

}
</style>
