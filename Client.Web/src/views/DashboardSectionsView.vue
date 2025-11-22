<script setup lang="ts">
import PlusIcon from '@/assets/icons/PlusIcon.vue';
import StackIcon from '@/assets/icons/StackIcon.vue';
import type { Seccion } from '@/interfaces/Seccion';
import { seccionService } from '@/services/seccion.service';
import ToastService from '@/utils/ToastService';
import Swal from 'sweetalert2';
import { ref, onMounted } from 'vue';
import { RouterLink } from 'vue-router';

const sections = ref<Seccion[]>([]);
const loading = ref(true);

const editMode = ref(false);
const seccionEditando = ref<Seccion | null>(null);
const nuevaDescripcion = ref('');

onMounted(() => {
  obtenerSecciones();
});

async function obtenerSecciones() {
  try {
    const r = await seccionService.getAll();
    if (r.success && r.payload) sections.value = r.payload;
  } catch (e) {
    console.error('Error cargando secciones', e);
  } finally {
    loading.value = false;
  }
}

function abrirEditar(seccion: Seccion) {
  seccionEditando.value = { ...seccion };
  nuevaDescripcion.value = seccion.descripcion || '';
  editMode.value = true;
}

async function guardarCambios() {
  if (!seccionEditando.value) return;

  const updateData: Seccion = {
    idSeccion: seccionEditando.value.idSeccion,
    descripcion: nuevaDescripcion.value,
    inscripciones: seccionEditando.value.inscripciones ?? [],
  };

  const r = await seccionService.update(updateData.idSeccion!, updateData);

  if (r.success) {
    ToastService.success('Sección actualizada');
    editMode.value = false;
    obtenerSecciones();
  } else {
    ToastService.error('Error actualizando la sección');
  }
}

async function eliminarSeccion(id: number) {
  Swal.fire({
    title: 'Desea eliminar esta sección?',
    icon: 'question',
    confirmButtonText: 'Eliminar',
    confirmButtonColor: 'red',
    cancelButtonText: 'Cancelar',
    showCancelButton: true,
    showCloseButton: true,
    preConfirm: async () => {
      const r = await seccionService.remove(id);

      if (r.success) {
        ToastService.success('Sección eliminada');
        await obtenerSecciones();
        return true;
      } else {
        ToastService.error('Error eliminando sección');
        return false;
      }
    },
  });
}
</script>

<template>
  <main class="p-6 space-y-6">
    <!-- HEADER -->
    <header class="flex flex-col md:flex-row items-center justify-between gap-4">
      <div>
        <h1 class="text-xl md:text-2xl font-bold">Secciones</h1>
        <p class="text-sm text-gray-500">Organiza a los estudiantes en grupos</p>
      </div>

      <RouterLink
        to="/dashboard/crear-seccion"
        class="inline-flex items-center gap-2 bg-blue-700 hover:bg-blue-800 text-white text-sm font-medium py-2 px-3 rounded-lg shadow-sm transition"
      >
        <PlusIcon class="w-5 h-5" />
        Crear Sección
      </RouterLink>
    </header>

    <!-- CONTENT -->
    <section class="flex-1">
      <div class="p-8 border h-full border-gray-200 rounded-xl bg-white shadow-sm">
        <!-- Cargando -->
        <div v-if="loading" class="flex items-center justify-center py-12">
          <p class="text-gray-500">Cargando secciones...</p>
        </div>

        <!-- Sin secciones -->
        <div
          v-else-if="sections.length === 0"
          class="flex flex-col items-center justify-center text-center py-8"
        >
          <div class="flex items-center justify-center w-20 h-20 bg-gray-50 border rounded-full">
            <StackIcon class="w-15 h-15 text-gray-600" />
          </div>

          <h3 class="text-lg font-semibold mt-3">No hay secciones creadas</h3>
          <p class="text-sm text-gray-500 mb-6 max-w-md">
            Crea secciones para organizar a los estudiantes.
          </p>

          <RouterLink
            to="/dashboard/crear-seccion"
            class="inline-flex items-center gap-2 bg-blue-700 hover:bg-blue-800 text-white text-sm font-medium py-2 px-4 rounded-lg"
          >
            <PlusIcon class="w-5 h-5" />
            Crear Primera Sección
          </RouterLink>
        </div>

        <!-- Tabla -->
        <div v-else class="overflow-x-auto">
          <table class="min-w-full border border-gray-200 rounded-lg">
            <thead class="bg-gray-100 text-gray-700 text-sm uppercase">
              <tr>
                <th class="py-3 px-4 text-left">Descripcion</th>
                <th class="py-3 px-4 text-left">Inscripciones</th>
                <th class="py-3 px-4 text-center">Editar</th>
                <th class="py-3 px-4 text-center">Eliminar</th>
              </tr>
            </thead>

            <tbody>
              <tr
                v-for="seccion in sections"
                :key="seccion.idSeccion"
                class="border-b border-gray-200 hover:bg-gray-50"
              >
                <td class="py-3 px-4 text-sm font-medium">{{ seccion.descripcion }}</td>
                <td class="py-3 px-4 text-sm">
                  {{ seccion.inscripciones.length }}
                </td>

                <td class="py-3 px-4 text-center">
                  <button class="text-blue-600 hover:text-blue-800" @click="abrirEditar(seccion)">
                    Editar
                  </button>
                </td>

                <td class="py-3 px-4 text-center">
                  <button
                    class="text-red-600 hover:text-red-800"
                    @click="eliminarSeccion(seccion.idSeccion!)"
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>

    <!-- MODAL EDITAR -->
    <div
      v-if="editMode"
      class="fixed inset-0 bg-black/40 flex items-center justify-center"
    >
      <div class="bg-white p-6 rounded-lg w-full max-w-sm space-y-4">
        <h2 class="text-lg font-semibold">Editar sección</h2>

        <input
          v-model="nuevaDescripcion"
          type="text"
          class="w-full border rounded-lg px-3 py-2"
          placeholder="Descripcion"
        />

        <div class="flex justify-end gap-3">
          <button class="px-3 py-2 rounded bg-gray-200" @click="editMode = false">Cancelar</button>

          <button class="px-3 py-2 rounded bg-blue-700 text-white" @click="guardarCambios">
            Guardar
          </button>
        </div>
      </div>
    </div>
  </main>
</template>
