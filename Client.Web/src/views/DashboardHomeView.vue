<script setup lang="ts">
import BookIcon from '@/assets/icons/BookIcon.vue';
import StackIcon from '@/assets/icons/StackIcon.vue';
import UserIcon from '@/assets/icons/UserIcon.vue';
import { studentService } from '@/services/estudiante.service';
import { onMounted, ref } from 'vue';

const numEstudiantes = ref(0);

onMounted(async () => {
  numEstudiantes.value = await obtenerNumEstudiantes();
});

async function obtenerNumEstudiantes(): Promise<number> {
  try {
    const dataEstudiantes = await studentService.getAll();
    return dataEstudiantes.payload?.length ?? 0;
  } catch (error) {
    console.error('Error al obtener estudiantes:', error);
    return 0;
  }
}
</script>

<template>
  <main class="p-6 space-y-6">
    <header>
      <div class="flex flex-row items-center text-center gap-2">
        <BookIcon class="w-7 h-7 text-blue-700" />
        <h1 class="text-xl md:text-2xl lg:md:text-2xl font-bold">
          Bienvenido a <span class="text-blue-700">Codex</span
          ><span class="text-red-600">Scholar</span>
        </h1>
      </div>

      <p class="text-gray-600">
        Sistema de gestión educativa para administrar estudiantes, notas y secciones
      </p>
    </header>

    <section class="grid grid-cols-1 sm:grid-cols-3 gap-4">
      <div class="p-4 border border-gray-300 rounded-xl shadow-sm flex flex-col items-start">
        <div class="w-full flex flex-row items-center text-center justify-between">
          <p class="text-sm text-gray-500">Total Estudiantes</p>
          <UserIcon class="w-6 h-6 text-blue-700" />
        </div>

        <div class="flex items-center justify-between w-full mt-1">
          <p class="text-3xl font-bold">{{ numEstudiantes }}</p>
        </div>
      </div>

      <div class="p-4 border border-gray-300 rounded-xl shadow-sm flex flex-col items-start">
        <div class="w-full flex flex-row items-center text-center justify-between">
          <p class="text-sm text-gray-500">Secciones Activas</p>
          <StackIcon class="w-6 h-6 text-yellow-400" />
        </div>

        <div class="flex items-center justify-between w-full mt-1">
          <p class="text-3xl font-bold">0</p>
        </div>
      </div>
    </section>

    <div class="p-6 border border-gray-300 rounded-xl bg-white shadow-sm">
      <h2 class="font-semibold text-gray-800 mb-2">Comienza a gestionar tu institución</h2>
      <p class="text-gray-500 mb-4 text-sm">
        Utiliza el menú lateral para navegar entre las diferentes secciones del sistema
      </p>

      <ul class="space-y-3">
        <li class="flex items-start gap-3">
          <UserIcon class="w-6 h-6 self-center text-blue-700" />
          <div>
            <p class="font-medium text-gray-800">Estudiantes</p>
            <p class="text-gray-500 text-sm">
              Administra la información de todos los estudiantes de tu institución
            </p>
          </div>
        </li>
        <li class="flex items-start gap-3">
          <StackIcon class="w-6 h-6 self-center text-yellow-400" />
          <div>
            <p class="font-medium text-gray-800">Secciones</p>
            <p class="text-gray-500 text-sm">
              Organiza a los estudiantes en grupos y secciones para una mejor administración
            </p>
          </div>
        </li>
      </ul>
    </div>
  </main>
</template>
