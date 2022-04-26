export default [
  // iUGO Care
  {
    path: '/iugo-care/',
    redirect: 'iugo-care/patient-profile',
    // @ts-ignore
    component: () => import('@/layouts/iugo-care/Index.vue'),
    children: [
      {
        name: 'iUGO Dashboard',
        path: '/iugo-care/dashboard',
        // @ts-ignore
        component: () => import('@/views/iugo-care/Dashboard.vue'),
        props: true,
      },
      {
        name: 'Patient Profile',
        path: '/iugo-care/patient-profile',
        // @ts-ignore
        component: () => import('@/views/iugo-care/PatientProfile.vue'),
        props: true,
      },
      // iUGO Care DEMO COMPONENTS
      {
        name: 'iUGO Care Components',
        path: '/iugo-care/components',
        // @ts-ignore
        component: () => import('@/views/iugo-care/components/DemoComponents.vue'),
      },
      // iUGO Care DEMO WORKSPACES
      {
        name: 'Fall Workspace',
        path: '/iugo-care/workspace/fall',
        // @ts-ignore
        component: () => import('@/views/iugo-care/FallWorkspace.vue'),
      },
      {
        name: 'Geo Workspace',
        path: '/iugo-care/workspace/geo',
        // @ts-ignore
        component: () => import('@/views/iugo-care/GeoWorkspace.vue'),
      },
      {
        name: 'SOS Workspace',
        path: '/iugo-care/workspace/sos',
        // @ts-ignore
        component: () => import('@/views/iugo-care/SOSWorkspace.vue'),
      },
    ],
  },
];
