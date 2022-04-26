export default [
  // MiUGO
  {
    path: '/miugo/',
    redirect: 'miugo/dashboard',
    // @ts-ignore
    component: () => import('@/layouts/miugo-demo/Index.vue'),
    children: [
      {
        name: 'miugoDashboard',
        path: '/miugo/dashboard',
        // @ts-ignore
        component: () => import('@/views/miugo-demo/Dashboard.vue'),
        props: true,
      },
      {
        name: 'observations',
        path: '/miugo/observations/:observationCategory',
        // @ts-ignore
        component: () => import('@/views/observations/Observations.vue'),
        props: true,
      },
      {
        name: 'My Care Plans - Chronic Care Management',
        path: '/miugo/careplans/ccm',
        // @ts-ignore
        component: () => import('@/views/miugo-demo/careplans/ChronicCareManagement.vue'),
        props: true,
        meta: {
          title: 'MiUGO - Chronic Care Management',
        },
      },
      {
        name: 'medications',
        path: '/miugo/medications',
        // @ts-ignore
        component: () => import('@/views/miugo-demo/Medications.vue'),
        meta: {
          title: 'MiUGO - medications',
        },
      },
      {
        name: 'surveys',
        path: '/miugo/surveys',
        // @ts-ignore
        component: () => import('@/views/miugo-demo/Surveys.vue'),
        meta: {
          title: 'MiUGO - Surveys',
        },
      },
      {
        name: 'records',
        path: '/miugo/records',
        // @ts-ignore
        component: () => import('@/views/miugo-demo/Records.vue'),
        meta: {
          title: 'MiUGO - Records',
        },
      },
      {
        name: 'education',
        path: '/miugo/education',
        // @ts-ignore
        component: () => import('@/views/miugo-demo/Education.vue'),
        meta: {
          title: 'MiUGO - Education',
        },
      },
      {
        name: 'Messages',
        path: '/miugo/messages',
        // @ts-ignore
        component: () => import('@/views/miugo-demo/Messages.vue'),
        meta: {
          title: 'MiUGO - Messages',
        },
      },
    ],
  },
];
