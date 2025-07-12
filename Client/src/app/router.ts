import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '@/views/HomeView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    { path: '/bookmarks', name: 'bookmarks', component: () => import('../views/BookmarksView.vue') },
    { path: '/statistics', name: 'statistics', component: () => import('../views/StatisticsView.vue') },
    {
      path: '/trades',
      name: 'trades',
      component: () => import('../views/TradesView.vue'),
    },
  ],
});

export default router;
