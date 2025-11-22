<template>
  <div>
    <!-- BOTON PRINCIPAL -->
    <button @click="openPdf" class="btn">Ver PDF</button>

    <!-- DIALOG -->
    <dialog ref="dialogRef" class="fixed inset-0 border-gray-400 border-2 m-auto w-svw max-w-4xl rounded-xl p-0 shadow-xl">
      <div class="flex flex-col p-6 bg-gray-300 rounded-xl">
        <h2 class="text-2xl font-bold mb-4 text-gray-800">PDF del estudiante</h2>

        <!-- VISOR PDF -->
        <iframe v-if="pdfUrl" :src="pdfUrl" class="w-full h-[70vh] border rounded-md mb-4"></iframe>

        <div class="flex justify-between mt-4">
          <button
            @click="downloadPdf"
            class="bg-green-600 hover:bg-green-700 text-white font-medium py-2 px-4 rounded-lg transition shadow"
          >
            Descargar
          </button>
          <button
            @click="closeDialog"
            class="bg-red-600 hover:bg-red-700 text-white font-medium py-2 px-4 rounded-lg transition shadow"
          >
            Cerrar
          </button>
        </div>
      </div>
    </dialog>
  </div>
</template>

<script setup lang="ts">
import { studentService } from '@/services/estudiante.service';
import ToastService from '@/utils/ToastService';
import { ref } from 'vue';

const pdfUrl = ref<string | null>(null);
const dialogRef = ref<HTMLDialogElement | null>(null);

const props = defineProps<{
  id: number;
}>();

// Abrir visor PDF
const openPdf = async () => {
  const res = await studentService.getPdfById(props.id);

  if (!res.success) {
    ToastService.error(res.message ?? 'No se pudo obtener el PDF');
    return;
  }

  // Convertir base64 a URL para iframe
  pdfUrl.value = `data:application/pdf;base64,${res.payload}`;

  dialogRef.value?.showModal();
};

const closeDialog = () => {
  dialogRef.value?.close();
};

const downloadPdf = () => {
  const link = document.createElement('a');
  link.href = pdfUrl.value!;
  link.download = `inscripcion-${props.id}.pdf`;
  link.click();
};
</script>

<style scoped>
.pdf-dialog {
  width: 80vw;
  max-width: 900px;
  border: none;
  border-radius: 10px;
  padding: 0;
}

.dialog-content {
  padding: 20px;
}

.pdf-frame {
  width: 100%;
  height: 70vh;
  border: 1px solid #ccc;
  border-radius: 6px;
}

.actions {
  display: flex;
  justify-content: space-between;
  margin-top: 15px;
}

.btn,
.btn-close,
.btn-download {
  padding: 8px 14px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  background-color: #4f46e5;
  color: white;
}

.btn-close {
  background-color: #dc2626;
}

.btn-download {
  background-color: #059669;
}
</style>
