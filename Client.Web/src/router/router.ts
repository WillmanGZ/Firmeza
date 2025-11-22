import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import LoginView from '@/views/LoginView.vue';
import DashboardLayout from '../layouts/DashboardLayout.vue';
import DashboardHomeView from '../views/DashboardHomeView.vue';
import DashboardStudentsView from '@/views/DashboardStudentsView.vue';
import DashboardGradesView from '@/views/DashboardGradesView.vue';
import DashboardSectionsView from '@/views/DashboardSectionsView.vue';
import { useAuth } from '@/composables/useAuth';
import DashboardCreateStudent from '@/views/DashboardCreateStudent.vue';
import DashboardCreateSectionView from '@/views/DashboardCreateSectionView.vue';
import DashboardEditStudentView from '@/views/DashboardEditStudentView.vue';
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
        path: 'crear-estudiante',
        name: 'DashboardCreateStudents',
        component: DashboardCreateStudent,
      },
      {
        path: 'editar-estudiante/:id',
        name: 'DashboardEditStudent',
        component: DashboardEditStudentView,
      },
      {
        path: 'notas',
        name: 'DashboardGrades',
        component: DashboardGradesView,
      },
      {
        path: 'secciones',
        name: 'DashboardSections',
        component: DashboardSectionsView,
      },
      {
        path: 'crear-seccion',
        name: 'DashboardCreateSection',
        component: DashboardCreateSectionView,
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
