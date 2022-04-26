import Vue from 'vue';
import createAuth0Client from '@auth0/auth0-spa-js';
import axios from 'axios';

/** Define a default action to perform after authentication */
// eslint-disable-next-line
const DEFAULT_REDIRECT_CALLBACK = (_: any) =>
  window.history.replaceState({}, document.title, window.location.pathname);

let instance: any;

/** Returns the current instance of the SDK */
export const getInstance = () => instance;

/** Creates an instance of the Auth0 SDK. If one has already been created, it returns that instance */
export const useAuth0 = ({
  onRedirectCallback = DEFAULT_REDIRECT_CALLBACK,
  redirectUri = window.location.origin,
}) => {
  if (instance) { return instance; }

  // The 'instance' is simply a Vue object
  instance = new Vue({
    data() {
      return {
        loading: true,
        isAuthenticated: false,
        user: {} as {[key: string]: any},
        auth0Client: null,
        popupOpen: false,
        error: null,
      };
    },
    methods: {
      isAuthenticatedAsync() {
        return new Promise((resolve) => {

          if (this.loading) {
            this.$watch('loading', () => {
              if (! this.loading) {
                resolve(this.isAuthenticated);
              }
            });
          } else {
            resolve(this.isAuthenticated);
          }
        });
      },
      /** Authenticates the user using a popup window */
      async loginWithPopup(o: any) {
        this.popupOpen = true;

        try {
          await this.auth0Client.loginWithPopup(o);
        } catch (e) {
          // eslint-disable-next-line
          // console.error(e);
        } finally {
          this.popupOpen = false;
        }

        this.user = await this.auth0Client.getUser();
        this.isAuthenticated = true;
      },
      /** Handles the callback when logging in using a redirect */
      async handleRedirectCallback() {
        this.loading = true;
        try {
          await this.auth0Client.handleRedirectCallback();
          this.user = await this.auth0Client.getUser();
          this.isAuthenticated = true;
        } catch (e) {
          this.error = e;
        } finally {
          this.loading = false;
        }
      },
      /** Authenticates the user using the redirect method */
      loginWithRedirect(o: any) {
        return this.auth0Client.loginWithRedirect(o);
      },
      /** Returns all the claims present in the ID token */
      getIdTokenClaims(o: any) {
        return this.auth0Client.getIdTokenClaims(o);
      },
      /** Returns the access token. If the token is invalid or missing, a new one is retrieved */
      getTokenSilently(o: any) {
        return this.auth0Client.getTokenSilently(o);
      },
      /** Gets the access token using a popup window */

      getTokenWithPopup(o: any) {
        return this.auth0Client.getTokenWithPopup(o);
      },
      /** Logs the user out and removes their session on the authorization server */
      logout(o: any) {
        return this.auth0Client.logout(o);
      },
    },
    /** Use this lifecycle method to instantiate the SDK client */
    async created() {
      const settings = await axios.get(`${process.env.VUE_APP_API_URL}/api/Authentication/settings`);
      // Create a new instance of the SDK client using members of the given options object
      /* eslint-disable @typescript-eslint/camelcase */
      this.auth0Client = await createAuth0Client({
        domain: settings.data.domain,
        client_id: settings.data.clientId,
        audience: settings.data.audience,
        redirect_uri: redirectUri,
      });
      /* eslint-enable @typescript-eslint/camelcase */

      try {
        // If the user is returning to the app after authentication...
        if (
          window.location.search.includes('code=') &&
          window.location.search.includes('state=')
        ) {
          // handle the redirect and retrieve tokens
          const { appState } = await this.auth0Client.handleRedirectCallback();

          // Notify subscribers that the redirect callback has happened, passing the appState
          // (useful for retrieving any pre-authentication state)
          onRedirectCallback(appState);
        }
      } catch (e) {
        this.error = e;
      } finally {
        // Initialize our internal authentication state
        this.isAuthenticated = await this.auth0Client.isAuthenticated();
        if (this.isAuthenticated) {
          this.user = await this.auth0Client.getUser();
          window.localStorage['auth0:token'] = await this.auth0Client.getTokenSilently();
          window.localStorage.username = this.user.nickname;
        }
        this.loading = false;
      }
    },
  });

  return instance;
};

// Create a simple Vue plugin to expose the wrapper object throughout the application
export const Auth0Plugin = {
  install(vue: any, options: any) {
    vue.prototype.$auth = useAuth0(options);
  },
};
