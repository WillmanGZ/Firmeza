<script lang="ts" setup>
import LockIcon from '@/assets/icons/LockIcon.vue';
import MailIcon from '@/assets/icons/MailIcon.vue';
import BookIcon from '@/assets/icons/BookIcon.vue';
import ToastService from '@/utils/ToastService';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuth } from '@/composables/useAuth';
import { authService } from '@/services/auth.service';

const router = useRouter();
const { setUserInfo } = useAuth();

const email = ref('');
const password = ref('');

async function handleLogin() {
  if (!email.value || !password.value) {
    ToastService.success('Debes rellenar todos los campos');
    return;
  }
  try {
    const data = await authService.login(email.value, password.value);
    if (data.payload) {
      setUserInfo(data.payload);
      router.push('/dashboard');
    }
  } catch (error) {
    ToastService.warning(`Credenciales inválidas`);
    console.log(error);
  }
}
</script>

<template>
  <header class="flex flex-col justify-center items-center py-7 space-y-2 select-none text-center">
    <article
      class="p-2 sm:p-3 border-2 border-blue-700 rounded-xl flex items-center justify-center"
    >
      <BookIcon class="text-blue-700 w-8 h-8 sm:w-10 sm:h-10" />
    </article>

    <h1 class="text-3xl sm:text-4xl lg:text-4xl font-bold">
      <span class="text-blue-700">Codex</span><span class="text-red-600">Scholar</span>
    </h1>

    <p class="text-gray-500 text-xs sm:text-sm font-medium">Plataforma Educativa</p>
  </header>

  <main class="flex items-center justify-center">
    <section
      class="min-w-[200px] w-[80%] md:w-auto lg:w-auto bg-white p-8 pt-10 rounded-2xl border border-blue-700 shadow"
    >
      <h2 class="text-2xl font-medium text-center mb-2 text-blue-700">Iniciar sesión</h2>
      <p class="text-gray-500 text-sm text-center mb-6">
        Ingresa tus credenciales para acceder a la plataforma
      </p>

      <form @submit.prevent="handleLogin" class="space-y-4">
        <article class="relative">
          <label class="block text-sm font-semibold mb-1" for="email-input"
            >Correo electrónico</label
          >
          <MailIcon class="absolute left-3 top-8.5 w-5 h-5 text-blue-700" />
          <input
            id="email-input"
            v-model="email"
            type="email"
            placeholder="estudiante@ejemplo.com"
            class="w-full pl-10 pr-3 py-2 border rounded-lg outline-none focus:ring-2 border-gray-300 focus:ring-blue-500 text-sm"
            required
          />
        </article>

        <article class="relative">
          <label class="block text-sm font-semibold mb-1" for="password-input">Contraseña</label>
          <LockIcon class="absolute left-3 top-8.5 w-5 h-5 text-blue-700" />
          <input
            id="password-input"
            v-model="password"
            type="password"
            placeholder="********"
            class="w-full pl-10 pr-3 py-2 border rounded-lg outline-none focus:ring-2 border-gray-300 focus:ring-blue-500 text-sm"
            required
          />
        </article>

        <button
          type="submit"
          class="w-full bg-blue-700 text-white py-2 rounded-lg hover:bg-blue-800 transition cursor-pointer"
        >
          Entrar
        </button>
      </form>
    </section>
  </main>
  <footer class="flex flex-col items-center mt-6 px-4">
    <div class="flex w-full max-w-md h-2.5 rounded-full overflow-hidden shadow-sm">
      <div class="bg-yellow-400 w-1/3"></div>
      <div class="bg-blue-700 w-1/3"></div>
      <div class="bg-red-600 w-1/3"></div>
    </div>
    <p class="text-gray-500 text-xs mt-4 text-center">
      © 2025 CodexScholar. Plataforma educativa.
    </p>
  </footer>
</template>
