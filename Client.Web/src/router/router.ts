import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import LoginView from '@/views/LoginView.vue';
import { useAuth } from '@/composables/useAuth';
import RegisterView from '@/views/RegisterView.vue';
import ShopLayout from '../layouts/ShopLayout.vue';
import CartView from '@/views/CartView.vue';
import HomeView from '../views/HomeView.vue';
import ProductsView from '@/views/ProductsView.vue';

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
    path: '/tienda',
    component: ShopLayout,
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        redirect: { name: 'Home' },
      },
      {
        path: 'inicio',
        name: 'Home',
        component: HomeView,
      },
      {
        path: 'productos',
        name: 'Products',
        component: ProductsView,
      },
      {
        path: 'carrito',
        name: 'Cart',
        component: CartView,
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
    next({ name: 'Home' });
    return;
  }

  next();
});

export default router;
