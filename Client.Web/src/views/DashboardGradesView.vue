<script setup lang="ts">
import { ref } from 'vue';
import PlusIcon from '@/assets/icons/PlusIcon.vue';
import DownloadIcon from '@/assets/icons/DownloadIcon.vue';
import ChevronDownIcon from '@/assets/icons/ChevronDownIcon.vue';
import DocumentIcon from '@/assets/icons/DocumentIcon.vue';

const lapso = ref('');
const materia = ref('');

const lapsos = ['I', 'II', 'III'];
const materias = ['Matemáticas', 'Ciencias', 'Lenguaje'];
</script>

<template>
  <main class="p-6 space-y-6">
    <!-- Header -->
    <header class="flex flex-col md:flex-row items-center justify-between gap-4">
      <div>
        <div class="flex items-center gap-2">
          <h1 class="text-xl md:text-2xl font-bold">Notas</h1>
        </div>
        <p class="text-sm text-gray-500">
          Gestiona las calificaciones de los estudiantes por lapso y materia
        </p>
      </div>

      <div class="flex items-center gap-3">
        <button
          class="inline-flex items-center gap-2 border border-gray-200 bg-white text-sm font-medium py-2 px-3 rounded-lg shadow-sm hover:bg-gray-200 transition cursor-pointer"
        >
          <DownloadIcon class="w-4 h-4" />
          Exportar PDF
        </button>

        <button
          class="inline-flex items-center gap-2 bg-blue-700 hover:bg-blue-800 text-white text-sm font-medium py-2 px-3 rounded-lg shadow-sm transition cursor-pointer"
        >
          <PlusIcon class="w-4 h-4" />
          Registrar Nota
        </button>
      </div>
    </header>

    <!-- Filters -->
    <div class="p-4 border border-gray-300 rounded-xl bg-white">
      <div class="mb-4">
        <h3 class="text-sm font-semibold">Filtros</h3>
        <p class="text-xs text-gray-500">Filtra las notas por lapso y materia</p>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 items-center">
        <!-- Lapso -->
        <div>
          <label class="text-sm font-semibold block mb-1">Lapso</label>
          <div class="relative">
            <select
              v-model="lapso"
              class="block w-full appearance-none bg-white border border-gray-200 rounded-md py-2 pl-3 pr-9 text-sm text-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-200"
            >
              <option value="">Seleccionar lapso</option>
              <option v-for="l in lapsos" :key="l" :value="l">{{ l }}</option>
            </select>

            <ChevronDownIcon
              class="pointer-events-none absolute right-2 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400"
            />
          </div>
        </div>

        <!-- Materia -->
        <div>
          <label class="text-sm font-semibold block mb-1">Materia</label>
          <div class="relative">
            <select
              v-model="materia"
              class="block w-full appearance-none bg-white border border-gray-200 rounded-md py-2 pl-3 pr-9 text-sm text-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-200"
            >
              <option value="">Seleccionar materia</option>
              <option v-for="m in materias" :key="m" :value="m">{{ m }}</option>
            </select>

            <ChevronDownIcon
              class="pointer-events-none absolute right-2 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400"
            />
          </div>
        </div>
      </div>
    </div>

    <!-- Main content -->
    <section class="flex-1">
      <div class="p-8 border h-full border-gray-200 rounded-xl bg-white shadow-sm">
        <div class="flex flex-col items-center justify-center py-8">
          <div class="flex items-center justify-center w-20 h-20 bg-gray-50">
            <DocumentIcon class="w-15 h-15 text-gray-600" />
          </div>

          <h3 class="text-lg font-semibold text-center text-gray-800 mb-2">
            No hay notas registradas
          </h3>
          <p class="text-sm text-gray-500 text-center mb-6 max-w-lg">
            Comienza registrando las calificaciones de los estudiantes. Podrás organizarlas por
            lapso y materia.
          </p>

          <button
            class="inline-flex items-center gap-2 bg-blue-700 hover:bg-blue-800 text-white text-sm font-medium py-2 px-4 rounded-lg transition cursor-pointer"
          >
            <PlusIcon class="w-4 h-4" />
            Registrar Primera Nota
          </button>
        </div>
      </div>
    </section>
  </main>
</template>
