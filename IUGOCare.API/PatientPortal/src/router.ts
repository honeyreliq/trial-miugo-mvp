import Vue from 'vue';
import Router from 'vue-router';
import { getInstance } from './auth/index';
import goTo from 'vuetify/es5/services/goto';
import routerIugocare from './router-iugocare';
import routerMiugo from './router-miugo';

Vue.use(Router);

const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,

  // This is for the scroll top when click on any router link
  scrollBehavior: (to, from, savedPosition) => {
    let scrollTo = 0;

    if (to.hash) {
      scrollTo = parseFloat(to.hash);
    } else if (savedPosition) {
      scrollTo = savedPosition.y;
    }

    return goTo(scrollTo);
  },

  routes: [
    ...routerIugocare,
    ...routerMiugo,
    // Pages
    {
      path: '/',
      redirect: 'demos',
    },
    {
      path: '/demos',
      // @ts-ignore
      component: () => import('@/layouts/simple/Index.vue'),
      children: [
        {
          name: 'iUGO Demos',
          path: '/demos',
          // @ts-ignore
          component: () => import('@/views/Demos.vue'),
        },
      ],
    },
    {
      path: '/login',
      // @ts-ignore
      component: () => import('@/layouts/auth/Index.vue'),
      children: [
        {
          name: 'authentication',
          path: '/login',
          // @ts-ignore
          component: () => import('@/views/account/Auth.vue'),
        },
      ],
    },
    {
      path: '*',
      // @ts-ignore
      component: () => import('@/layouts/simple/Index.vue'),
      children: [
        {
          name: 'error404',
          path: '',
          // @ts-ignore
          component: () => import('@/views/PagesError.vue'),
        },
      ],
    },
  ],
});

// const publicRoutes = ['authentication', 'error404'];

// router.beforeEach(async (to, from, next) => {
//   if (publicRoutes.includes(to.name)) {
//     next();
//   } else {
//     const $auth = getInstance();
//     const isAuthenticated = await $auth.isAuthenticatedAsync();
//     if (isAuthenticated) {
//       next();
//     } else {
//       next({ name: 'authentication' });
//     }
//   }
// });

export default router;
