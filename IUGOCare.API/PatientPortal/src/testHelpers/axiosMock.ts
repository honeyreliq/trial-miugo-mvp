// server instance cache, initialized by the hoisted jest.mock
let serverInstance: IServer = null;

interface IResponses {
  url: string;
  status: number;
  data: any;
  delay: number;
}

interface IMethods {
  GET: IResponses[];
  POST: IResponses[];
  PUT: IResponses[];
  PATCH: IResponses[];
  DELETE: IResponses[];
}

export interface IServerResponse {
  method: string;
  url: string;
  status: number;
  payload: object;
  delay: number;
}

export interface IServer {
  init: () => void;
  createServerResponse: (response: IServerResponse) => void;
  reset: () => void;
}

// returning the server cached
export function getServer(): IServer {
  return serverInstance;
}

// the server instance us cached in the serverInstance variable, since jest.mock us hoisted, the reference could be lost
export function server(): IServer {
  let axiosMethods: IMethods;

  return {
    init(): any {
      axiosMethods = {
        GET: [],
        PUT: [],
        PATCH: [],
        POST: [],
        DELETE: [],
      };
      serverInstance = this;

      return {
        create: () => ({
          interceptors: {
            request: {use: jest.fn()},
            response: {use: jest.fn()},
          },
          request: jest.fn((request: any) => {
            // @ts-ignore
            const {method, url} = request;

            // @ts-ignore
            const methodFound: IResponses = axiosMethods[method];
            // @ts-ignore
            let response: IResponses = methodFound.find((customResponse: IResponses) => {
              return (customResponse.url === url);
            });

            if (!response) {
              response = {
                url: '/',
                data: null,
                status: 500,
                delay: 0,
              };
            }
            return new Promise((resolve: any) => {
              setTimeout(() => {
                resolve({status: response.status, data: response.data});
              }, response.delay);
            });
          }),
        }),
      };
    },
    createServerResponse(response: IServerResponse): void {
      // first reset the existing call (if exists)
      // @ts-ignore
      const index: number = axiosMethods[response.method].findIndex((customResponse: IResponses) => {
        return (customResponse.url === response.url);
      });
      if (index >= 0) {
        // @ts-ignore
        axiosMethods[response.method].splice(index, 1);
      }
      // @ts-ignore
      axiosMethods[response.method].push({
        url: response.url,
        data: response.payload || null,
        status: response.status || 200,
        delay: response.delay || 0,
      } as IResponses);
    },
    // should reset the fake server on afterEach...
    reset(): void {
      axiosMethods.GET = [];
      axiosMethods.PUT = [];
      axiosMethods.PATCH = [];
      axiosMethods.POST = [];
      axiosMethods.DELETE = [];
    },
  };
}

