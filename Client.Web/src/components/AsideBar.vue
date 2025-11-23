<script lang="ts" setup>
import { ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import BookIcon from '@/assets/icons/BookIcon.vue';
import HomeIcon from '@/assets/icons/HomeIcon.vue';
import CartIcon from '@/assets/icons/CartIcon.vue';
import LogOutIcon from '@/assets/icons/LogOutIcon.vue';
import MenuIcon from '@/assets/icons/MenuIcon.vue';
import { useAuth } from '@/composables/useAuth';
import ToastService from '@/utils/ToastService';

const router = useRouter();
const route = useRoute();
const isOpen = ref(false);
const { removeUserInfo } = useAuth();

const navigation = [
  { name: 'Inicio', icon: HomeIcon, path: '/tienda/inicio' },
  { name: 'Productos', icon: BookIcon, path: '/tienda/productos' },
  { name: 'Carrito', icon: CartIcon, path: '/tienda/carrito' },
];

const isActive = (path: string) => route.path.startsWith(path);

const navigateTo = (route: string) => {
  router.push(route);
  isOpen.value = false;
};

const logOut = () => {
  removeUserInfo();
  ToastService.info('Esperamos volverte a ver pronto!');
  router.push('/login');
};
</script>

<template>
  <aside>
    <button
      @click="isOpen = !isOpen"
      class="md:hidden fixed top-4 left-4 z-50 p-2 bg-white border border-gray-300 rounded-lg shadow-md"
    >
      <MenuIcon class="w-6 h-6" />
    </button>

    <div
      :class="[
        'fixed md:static top-0 left-0 h-screen w-64 bg-white border-r border-gray-300 flex flex-col z-40 transform transition-transform duration-300',
        isOpen ? 'translate-x-0' : '-translate-x-full md:translate-x-0',
      ]"
    >
      <header
        class="flex flex-col sm:flex-col md:flex-row lg:flex-row justify-center border-b border-gray-300 items-center py-5 space-y-2 select-none text-center"
      >
        <div>
          <h1 class="text-2xl font-bold">
            <span class="text-blue-700">Firmeza</span>
          </h1>

          <p class="text-gray-500 text-xs sm:text-sm font-medium">Plataforma de ventas</p>
        </div>
      </header>

      <nav class="flex-1 py-4 space-y-1">
        <button
          v-for="item in navigation"
          :key="item.name"
          @click="navigateTo(item.path)"
          :class="[
            'flex items-center gap-3 px-4 py-3 rounded-lg text-sm font-medium text-foreground hover:bg-blue-700 hover:text-white transition-colors cursor-pointer w-full text-left',
            isActive(item.path) ? 'bg-blue-700 text-white' : '',
          ]"
        >
          <component :is="item.icon" class="h-7 w-7 text-current" />
          {{ item.name }}
        </button>
      </nav>

      <div class="space-y-3 border-t border-gray-300 pt-4">
        <button
          @click="logOut()"
          class="w-[95%] flex items-center justify-start gap-3 text-gray-500 hover:bg-red-700 hover:text-white border border-gray-300 cursor-pointer rounded-lg py-1 px-4 transition bg-transparent"
        >
          <LogOutIcon class="h-7 w-7 text-current" />
          Cerrar Sesión
        </button>
        <div
          class="w-[95%] max-w-md h-1 mb-2 rounded-full shadow-sm bg-linear-to-r from-yellow-400 via-blue-700 to-red-600"
        ></div>
      </div>
    </div>

    <div
      v-if="isOpen"
      @click="isOpen = false"
      class="fixed inset-0 bg-black/40 z-30 md:hidden"
    ></div>
  </aside>
</template>
