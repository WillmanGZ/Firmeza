import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import LoginView from '@/views/LoginView.vue';
import DashboardLayout from '../layouts/DashboardLayout.vue';
import DashboardHomeView from '../views/DashboardHomeView.vue';
import DashboardStudentsView from '@/views/DashboardStudentsView.vue';
import DashboardSectionsView from '@/views/DashboardSectionsView.vue';
import { useAuth } from '@/composables/useAuth';
import RegisterView from '@/views/RegisterView.vue';

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: LoginView,
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterView,
  },
  {
    path: '/dashboard',
    component: DashboardLayout,
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        redirect: { name: 'DashboardHome' },
      },
      {
        path: 'inicio',
        name: 'DashboardHome',
        component: DashboardHomeView,
      },
      {
        path: 'estudiantes',
        name: 'DashboardStudents',
        component: DashboardStudentsView,
      },
      {
        path: 'secciones',
        name: 'DashboardSections',
        component: DashboardSectionsView,
      },
    ],
  },

  {
    path: '/:pathMatch(.*)*',
    redirect: '/login',
  },
];

const router = createRouter({
  history: createWebHistory('/'),
  routes,
  scrollBehavior() {
    return { top: 0 };
  },
});

router.beforeEach((to, from, next) => {
  const { isAuthenticated } = useAuth();

  if (to.meta.requiresAuth && !isAuthenticated()) {
    next({ name: 'Login' });
    return;
  }

  if (to.name === 'Login' && isAuthenticated()) {
    next({ name: 'DashboardHome' });
    return;
  }

  next();
});

export default router;
