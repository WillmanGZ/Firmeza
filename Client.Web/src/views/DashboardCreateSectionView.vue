<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { seccionService } from '@/services/seccion.service';
import ToastService from '@/utils/ToastService';

const router = useRouter();

const descripcion = ref('');
const loading = ref(false);

async function crearSeccion() {
  if (descripcion.value.trim().length === 0) {
    ToastService.error('La descripción es obligatoria');
    return;
  }

  loading.value = true;
  try {
    const resp = await seccionService.create({
      descripcion: descripcion.value,
      inscripciones: [],
    });

    if (resp.success) {
      ToastService.success('Sección creada correctamente');
      router.push('/dashboard/secciones');
    } else {
      ToastService.error(resp.message ?? 'No se pudo crear la sección');
    }
  } catch (e) {
    console.error(e);
    ToastService.error('Error creando la sección');
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <main class="p-6 space-y-6">
    <header>
      <h1 class="text-xl font-bold">Crear Sección</h1>
      <p class="text-sm text-gray-500">Ingresa la información básica de la sección</p>
    </header>

    <section class="bg-white border rounded-xl p-6 shadow-sm space-y-4">
      <div>
        <label class="block text-sm font-medium mb-1">Descripción</label>
        <input
          v-model="descripcion"
          type="text"
          class="w-full border rounded-lg px-3 py-2 focus:ring focus:ring-gray-300"
          placeholder="Ej: Sección A, Preescolar 1, etc."
        />
      </div>

      <button
        @click="crearSeccion"
        :disabled="loading"
        class="bg-blue-700 hover:bg-blue-800 text-white px-4 py-2 rounded-lg disabled:opacity-50"
      >
        {{ loading ? 'Creando...' : 'Crear Sección' }}
      </button>
    </section>
  </main>
</template>
