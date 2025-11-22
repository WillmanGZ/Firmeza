<script lang="ts" setup>
import LockIcon from '@/assets/icons/LockIcon.vue';
import MailIcon from '@/assets/icons/MailIcon.vue';
import ToastService from '@/utils/ToastService';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { authService } from '@/services/auth.service';
import type { UserRegister } from '@/interfaces/user-register';

const router = useRouter();

const userName = ref('');
const phoneNumber = ref('');
const email = ref('');
const password = ref('');

async function handleRegister() {
  if (!userName.value || !phoneNumber.value || !email.value || !password.value) {
    ToastService.warning('Debes rellenar todos los campos');
    return;
  }

  try {
    const payload: UserRegister = {
      UserName: userName.value,
      Email: email.value,
      Password: password.value,
      PhoneNumber: phoneNumber.value,
    };

    const data = await authService.register(payload);

    if (data.payload) {
      ToastService.success('Se ha creado la cuenta exitosamente');
      router.push('/login');
    } else {
      ToastService.warning(`No fue posible crear la cuenta ${data.message}`);
    }
  } catch (error) {
    ToastService.warning(`No fue posible crear la cuenta`);
    console.log(error);
  }
}
</script>

<template>
  <header class="flex flex-col justify-center items-center py-7 space-y-2 select-none text-center">
    <h1 class="text-3xl sm:text-4xl lg:text-4xl font-bold">
      <span class="text-blue-700">Firmeza</span>
    </h1>

    <p class="text-gray-500 text-xs sm:text-sm font-medium">Plataforma de ventas</p>
  </header>

  <main class="flex items-center justify-center">
    <section
      class="min-w-[200px] w-[80%] md:w-auto lg:w-auto bg-white p-8 pt-10 rounded-2xl border border-blue-700 shadow"
    >
      <h2 class="text-2xl font-medium text-center mb-2 text-blue-700">Crear cuenta</h2>
      <p class="text-gray-500 text-sm text-center mb-6">
        Registra tu cuenta para acceder a la plataforma
      </p>

      <form @submit.prevent="handleRegister" class="space-y-4">
        <article class="relative">
          <label class="block text-sm font-semibold mb-1" for="username-input"> Usuario </label>
          <MailIcon class="absolute left-3 top-8.5 w-5 h-5 text-blue-700" />
          <input
            id="username-input"
            v-model="userName"
            type="text"
            placeholder="Tu nombre de usuario"
            class="w-full pl-10 pr-3 py-2 border rounded-lg outline-none focus:ring-2 border-gray-300 focus:ring-blue-500 text-sm"
            required
          />
        </article>

        <article class="relative">
          <label class="block text-sm font-semibold mb-1" for="phone-input"> Teléfono </label>
          <MailIcon class="absolute left-3 top-8.5 w-5 h-5 text-blue-700" />
          <input
            id="phone-input"
            v-model="phoneNumber"
            type="text"
            placeholder="3001234567"
            class="w-full pl-10 pr-3 py-2 border rounded-lg outline-none focus:ring-2 border-gray-300 focus:ring-blue-500 text-sm"
            required
          />
        </article>

        <article class="relative">
          <label class="block text-sm font-semibold mb-1" for="email-input">
            Correo electrónico
          </label>
          <MailIcon class="absolute left-3 top-8.5 w-5 h-5 text-blue-700" />
          <input
            id="email-input"
            v-model="email"
            type="email"
            placeholder="tucorreo@ejemplo.com"
            class="w-full pl-10 pr-3 py-2 border rounded-lg outline-none focus:ring-2 border-gray-300 focus:ring-blue-500 text-sm"
            required
          />
        </article>

        <article class="relative">
          <label class="block text-sm font-semibold mb-1" for="password-input"> Contraseña </label>
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
          Registrarse
        </button>
      </form>

      <RouterLink
        to="/login"
        class="block text-center mt-5 text-sm font-medium text-blue-700 hover:text-blue-800 transition underline underline-offset-2 cursor-pointer"
      >
        Ya tengo cuenta
      </RouterLink>
    </section>
  </main>

  <footer class="flex flex-col items-center mt-6 px-4">
    <div class="flex w-full max-w-md h-2.5 rounded-full overflow-hidden shadow-sm">
      <div class="bg-yellow-400 w-1/3"></div>
      <div class="bg-blue-700 w-1/3"></div>
      <div class="bg-red-600 w-1/3"></div>
    </div>
    <p class="text-gray-500 text-xs mt-4 text-center">© 2025 Firmeza. Plataforma de ventas.</p>
  </footer>
</template>
