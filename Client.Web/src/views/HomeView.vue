<script setup lang="ts">
import BookIcon from '@/assets/icons/BookIcon.vue';
import StackIcon from '@/assets/icons/CartIcon.vue';
import UserIcon from '@/assets/icons/UserIcon.vue';
</script>

<template>
  <main class="p-6 space-y-10">
    <!-- HEADER -->
    <header class="space-y-3 text-center">
      <h1 class="text-3xl font-bold text-gray-800">
        Bienvenido a <span class="text-blue-700">Firmeza</span>
      </h1>

      <p class="text-gray-600 text-sm md:text-base max-w-xl mx-auto">
        Descubre nuestros productos creados con calidad y dedicación. Compra fácil, rápido y seguro.
      </p>
    </header>

    <!-- BENEFICIOS -->
    <section class="grid grid-cols-1 sm:grid-cols-3 gap-6">
      <div class="p-6 bg-white border border-gray-200 rounded-xl shadow-sm text-center">
        <BookIcon class="w-10 h-10 mx-auto text-blue-700 mb-3" />
        <h3 class="font-semibold text-gray-800">Productos de calidad</h3>
        <p class="text-sm text-gray-500 mt-1">
          Explora nuestro catálogo cuidadosamente seleccionado.
        </p>
      </div>

      <div class="p-6 bg-white border border-gray-200 rounded-xl shadow-sm text-center">
        <UserIcon class="w-10 h-10 mx-auto text-yellow-500 mb-3" />
        <h3 class="font-semibold text-gray-800">Experiencia sencilla</h3>
        <p class="text-sm text-gray-500 mt-1">
          Compra sin complicaciones y con una navegación cómoda.
        </p>
      </div>

      <div class="p-6 bg-white border border-gray-200 rounded-xl shadow-sm text-center">
        <StackIcon class="w-10 h-10 mx-auto text-purple-600 mb-3" />
        <h3 class="font-semibold text-gray-800">Carrito rápido</h3>
        <p class="text-sm text-gray-500 mt-1">Agrega productos al carrito con un solo clic.</p>
      </div>
    </section>

    <!-- SECCIÓN EXPLORAR -->
    <div class="p-6 bg-white border border-gray-200 rounded-xl shadow-sm space-y-3 text-center">
      <h2 class="text-lg font-semibold text-gray-800">Explora la tienda</h2>
      <p class="text-sm mb-6 text-gray-500 max-w-md mx-auto">
        Empieza a navegar por todas las categorías disponibles y encuentra lo que necesitas.
      </p>

      <RouterLink
        to="/tienda/productos"
        class="mt-6 px-6 py-3 bg-blue-700 text-white rounded-lg hover:bg-blue-800 transition font-medium shadow-sm"
      >
        Ver productos
      </RouterLink>
    </div>
  </main>
</template>
