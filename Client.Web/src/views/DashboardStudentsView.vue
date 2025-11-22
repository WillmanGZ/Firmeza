<script setup lang="ts">
import MagnifyingGlassIcon from '@/assets/icons/MagnifyingGlassIcon.vue';
import PlusIcon from '@/assets/icons/PlusIcon.vue';
import UserIcon from '@/assets/icons/UserIcon.vue';
import type { Estudiante } from '@/interfaces/Estudiante';
import { studentService } from '@/services/estudiante.service';
import ToastService from '@/utils/ToastService';
import Swal from 'sweetalert2';
import { computed, onMounted, ref } from 'vue';
import PDFViewer from '@/components/PDFViewer.vue';
import router from '@/router/router';

const students = ref<Estudiante[]>([]);
const loading = ref(true);
const search = ref('');

onMounted(async () => {
  await obtenerEstudiantes();
});

async function obtenerEstudiantes() {
  try {
    const response = await studentService.getAll();
    if (response.success && response.payload) {
      students.value = response.payload;
    }
  } catch (error) {
    console.error('Error cargando estudiantes:', error);
  } finally {
    loading.value = false;
  }
}

async function eliminarEstudiante(id: number) {
  Swal.fire({
    title: '¿Desea eliminar este estudiante?',
    icon: 'question',
    iconHtml: '؟',
    confirmButtonText: 'Eliminar',
    confirmButtonColor: 'red',
    cancelButtonText: 'Cancelar',
    showCancelButton: true,
    showCloseButton: true,
    preConfirm: async () => {
      const r = await studentService.remove(id);

      if (r.success) {
        ToastService.success('Estudiante eliminado exitosamente');
        await obtenerEstudiantes();
        return true;
      } else {
        ToastService.error('Error eliminando estudiante');
        return false;
      }
    },
  });
}

function editarEstudiante(id: number) {
  router.push(`/dashboard/editar-estudiante/${id}`);
}

const filteredStudents = computed(() => {
  const q = search.value.toLowerCase().trim();

  if (!q) return students.value;

  return students.value.filter((s) => {
    return (
      s.nombres?.toLowerCase().includes(q) ||
      s.apellidos?.toLowerCase().includes(q) ||
      s.cedula?.toLowerCase().includes(q) ||
      s.correo?.toLowerCase().includes(q)
    );
  });
});
</script>

<template>
  <main class="p-6 space-y-6 flex flex-col">
    <!-- Header -->
    <header
      class="flex flex-col md:flex-row lg:flex-row text-center items-center justify-between gap-4"
    >
      <div>
        <div class="flex flex-col md:flex-row lg:flex-row items-center text-center gap-2">
          <h1 class="text-xl md:text-2xl font-bold">Estudiantes</h1>
        </div>
        <p class="text-sm text-gray-500">Administra la información de todos los estudiantes</p>
      </div>

      <div class="flex items-center gap-3">
        <RouterLink
          to="/dashboard/crear-estudiante"
          class="inline-flex items-center gap-2 bg-blue-700 hover:bg-blue-800 text-white text-sm font-medium py-2 px-3 rounded-lg shadow-sm transition cursor-pointer"
        >
          <PlusIcon class="w-5 h-5" />
          Agregar Estudiante
        </RouterLink>
      </div>
    </header>

    <!-- Search bar -->
    <div class="p-4 border border-gray-300 rounded-xl bg-white">
      <div class="w-full">
        <label class="relative block">
          <span class="absolute inset-y-0 left-0 flex items-center pl-3">
            <MagnifyingGlassIcon class="text-gray-400" />
          </span>
          <input
            type="text"
            v-model="search"
            placeholder="Buscar estudiante por nombre, cédula o código..."
            class="placeholder:text-gray-400 w-full pl-10 pr-4 py-1 rounded-lg border border-gray-200 focus:outline-none focus:ring-2 focus:ring-blue-200"
          />
        </label>
      </div>
    </div>

    <!-- Main content -->
    <section class="flex-1">
      <div class="p-8 border h-full border-gray-200 rounded-xl bg-white shadow-sm overflow-y-auto">
        <!-- Cargando -->
        <div v-if="loading" class="flex items-center justify-center py-12">
          <p class="text-gray-500 text-sm animate-pulse">Cargando estudiantes...</p>
        </div>

        <!-- Sin estudiantes -->
        <div
          v-else-if="students.length === 0"
          class="flex flex-col items-center text-center justify-center py-12"
        >
          <div class="flex items-center justify-center w-20 h-20 bg-gray-50">
            <UserIcon class="w-15 h-15 text-gray-600" />
          </div>

          <h3 class="text-lg font-semibold text-gray-800 mb-2">No hay estudiantes registrados</h3>
          <p class="text-sm text-gray-500 text-center mb-6 max-w-lg">
            Comienza agregando estudiantes al sistema para poder gestionar su información y
            calificaciones
          </p>

          <RouterLink
            to="/dashboard/crear-estudiante"
            class="inline-flex items-center gap-2 bg-blue-700 hover:bg-blue-800 text-white text-sm font-medium py-2 px-3 rounded-lg shadow-sm transition cursor-pointer"
          >
            <PlusIcon class="w-5 h-5" />
            Agregar Estudiante
          </RouterLink>
        </div>

        <!-- Tabla dinámica -->
        <div v-else class="overflow-x-auto">
          <table class="min-w-full border border-gray-200 rounded-lg">
            <thead class="bg-gray-100 text-gray-700 text-sm uppercase">
              <tr>
                <th class="py-3 px-4 text-left">Nombre</th>
                <th class="py-3 px-4 text-left">Cédula</th>
                <th class="py-3 px-4 text-left">Correo</th>
                <th class="py-3 px-4 text-left">Género</th>
                <th class="py-3 px-4 text-left">Dirección</th>
                <th class="py-3 px-4 text-left">PDF</th>
                <th class="py-3 px-4 text-left">Editar</th>
                <th class="py-3 px-4 text-center">Eliminar</th>
              </tr>
            </thead>

            <tbody>
              <tr
                v-for="student in filteredStudents"
                :key="student.idEstudiante"
                class="border-b border-gray-200 hover:bg-gray-50 transition"
              >
                <td class="py-3 px-4 text-sm font-medium text-gray-800">
                  {{ student.nombres }} {{ student.apellidos }}
                </td>
                <td class="py-3 px-4 text-sm text-gray-600">{{ student.cedula }}</td>
                <td class="py-3 px-4 text-sm text-gray-600">{{ student.correo || '—' }}</td>
                <td class="py-3 px-4 text-sm text-gray-600">
                  <span v-if="student.genero === 'M'">Masculino</span>
                  <span v-else-if="student.genero === 'F'">Femenino</span>
                  <span v-else>—</span>
                </td>
                <td class="py-3 px-4 text-sm text-gray-600 truncate max-w-[200px]">
                  {{ student.direccionCompleta || '—' }}
                </td>
                <td>
                  <div>
                    <PDFViewer :id="student.idEstudiante!" />
                  </div>
                </td>
                <td class="py-3 px-4 text-center">
                  <button
                    class="text-blue-600 hover:text-blue-800 font-medium text-sm cursor-pointer"
                    title="Ver detalles"
                    @click="editarEstudiante(student.idEstudiante!)"
                  >
                    Editar
                  </button>
                </td>
                <td class="py-3 px-4 text-center">
                  <button
                    class="text-blue-600 hover:text-blue-800 font-medium text-sm cursor-pointer"
                    title="Ver detalles"
                    @click="eliminarEstudiante(student.idEstudiante!)"
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
  </main>
</template>
