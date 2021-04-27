import { createRouter, createWebHistory } from 'vue-router';
import Dashboard from '../views/Dashboard.vue';

const routes = [
  {
    path: '/dashboard',
    alias: '/',
    name: 'Dashboard',
    component: Dashboard,
    children: [
      {
          path: '/dashboard',
          alias: '/',
          component: () => import('../components/tabs/RankingReceivedLikeIt.vue'),
      },
      {
          path: '/dashboard/tabs/ranking/sentlikeit',
          component: () => import('../components/tabs/RankingSentLikeIt.vue'),
      }
    ],
  },
  {
    path: '/master/group',
    name: 'MasterGroup',
    component: () => import('../views/MasterGroup.vue')
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

export default router;
