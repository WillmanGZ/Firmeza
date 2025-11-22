<script setup lang="ts">
import PlusIcon from '@/assets/icons/PlusIcon.vue';
import type { Product } from '@/interfaces/product';
import { productService } from '@/services/products.service';
import ToastService from '@/utils/ToastService';
import { ref, onMounted } from 'vue';

const products = ref<Product[]>([]);
const loading = ref(true);

onMounted(() => {
  obtenerProductos();
});

async function obtenerProductos() {
  try {
    const r = await productService.getAll();
    if (r.success && r.payload) products.value = r.payload;
  } catch (e) {
    console.error('Error cargando productos', e);
  } finally {
    loading.value = false;
  }
}

function addToCart(product: Product) {
  console.log('Producto añadido al carrito:', product);
  ToastService.success(product.name + ' añadido al carrito');
}
</script>

<template>
  <main class="p-6 space-y-6">
    <!-- HEADER -->
    <header class="flex flex-col md:flex-row items-center justify-between gap-4">
      <div>
        <h1 class="text-xl md:text-2xl font-bold">Productos</h1>
        <p class="text-sm text-gray-500">Explora y gestiona los productos disponibles</p>
      </div>

      <RouterLink
        to="/dashboard/crear-producto"
        class="inline-flex items-center gap-2 bg-blue-700 hover:bg-blue-800 text-white text-sm font-medium py-2 px-3 rounded-lg shadow-sm transition"
      >
        <PlusIcon class="w-5 h-5" />
        Crear Producto
      </RouterLink>
    </header>

    <!-- CONTENT -->
    <section class="flex-1">
      <div class="p-8 border h-full border-gray-200 rounded-xl bg-white shadow-sm">
        <!-- Cargando -->
        <div v-if="loading" class="flex items-center justify-center py-12">
          <p class="text-gray-500">Cargando productos...</p>
        </div>

        <!-- Sin productos -->
        <div
          v-else-if="products.length === 0"
          class="flex flex-col items-center justify-center text-center py-8"
        >
          <div class="flex items-center justify-center w-20 h-20 bg-gray-50 border rounded-full">
            <span class="text-4xl">🛒</span>
          </div>

          <h3 class="text-lg font-semibold mt-3">No hay productos disponibles</h3>
          <p class="text-sm text-gray-500 mb-6 max-w-md">
            Crea productos para mostrarlos en esta sección.
          </p>

          <RouterLink
            to="/dashboard/crear-producto"
            class="inline-flex items-center gap-2 bg-blue-700 hover:bg-blue-800 text-white text-sm font-medium py-2 px-4 rounded-lg"
          >
            <PlusIcon class="w-5 h-5" />
            Crear Primer Producto
          </RouterLink>
        </div>

        <!-- GRID DE PRODUCTOS -->
        <div v-else class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
          <div
            v-for="p in products"
            :key="p.id"
            class="border border-gray-200 rounded-xl shadow-sm hover:shadow-md transition bg-white flex flex-col"
          >
            <!-- Imagen (placeholder por ahora) -->
            <div
              class="h-40 bg-gray-100 rounded-t-xl flex items-center justify-center text-gray-400 text-5xl"
            >
              📦
            </div>

            <div class="p-4 flex flex-col grow">
              <h3 class="text-lg font-semibold">{{ p.name }}</h3>
              <p class="text-gray-500 text-sm line-clamp-2">
                {{ p.description }}
              </p>

              <p class="text-blue-700 font-bold text-lg mt-3">${{ p.price.toLocaleString() }}</p>

              <button
                @click="addToCart(p)"
                class="mt-auto bg-green-600 hover:bg-green-700 text-white text-sm py-2 px-3 rounded-lg w-full transition"
              >
                Añadir al carrito
              </button>
            </div>
          </div>
        </div>
      </div>
    </section>
  </main>
</template>
