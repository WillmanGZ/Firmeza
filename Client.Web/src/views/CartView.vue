<script setup lang="ts">
import { useCart } from '@/composables/useCart';
import { checkoutService } from '@/services/checkout.service';
import ToastService from '@/utils/ToastService';

const cart = useCart();

async function onCheckout() {
  if (cart.items.value.length === 0) {
    ToastService.error('El carrito está vacío');
    return;
  }

  const resp = await checkoutService.process(cart.items.value);

  if (resp.success) {
    ToastService.success('Compra realizada correctamente');
    cart.clear();
  } else {
    ToastService.error(resp.message ?? 'No se pudo llevar a cabo la compra');
  }
}
</script>

<template>
  <main class="p-6 space-y-6">
    <h1 class="text-xl font-bold">Carrito</h1>

    <!-- Carrito vacío -->
    <div v-if="cart.items.value.length === 0" class="p-6 bg-white border rounded-xl text-center">
      <p class="text-gray-500">Tu carrito está vacío</p>
    </div>

    <!-- Carrito con items -->
    <div v-else class="space-y-4">
      <div
        v-for="it in cart.items.value"
        :key="it.id"
        class="flex items-center gap-4 p-4 border rounded-lg bg-white"
      >
        <!-- Imagen -->
        <div class="w-16 h-16 bg-gray-100 rounded flex items-center justify-center text-2xl">
          📦
        </div>

        <!-- Info producto -->
        <div class="flex-1">
          <p class="font-semibold">{{ it.product.name }}</p>
          <p class="text-sm text-gray-500">{{ it.product.description }}</p>
        </div>

        <!-- Cantidad -->
        <div class="flex items-center gap-2">
          <button class="px-2" @click="cart.decrease(it.id)">-</button>
          <div class="px-3">{{ it.quantity }}</div>
          <button class="px-2" @click="cart.increase(it.id)">+</button>
        </div>

        <!-- Precio -->
        <div class="w-32 text-right">
          <p class="font-semibold">${{ (it.quantity * it.product.price).toLocaleString() }}</p>
          <button class="text-sm text-red-600" @click="cart.remove(it.id)">Eliminar</button>
        </div>
      </div>

      <!-- Total -->
      <div class="p-4 bg-white border rounded-xl flex items-center justify-between">
        <div>
          <p class="text-sm text-gray-500">Total</p>
          <p class="text-xl font-bold">${{ cart.total.value.toLocaleString() }}</p>
        </div>

        <div class="flex gap-3">
          <button class="px-4 py-2 border rounded" @click="cart.clear()">Vaciar</button>
          <button class="px-4 py-2 bg-blue-700 text-white rounded" @click="onCheckout()">
            Checkout
          </button>
        </div>
      </div>
    </div>
  </main>
</template>
