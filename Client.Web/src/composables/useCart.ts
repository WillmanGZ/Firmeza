import { ref, computed, watch } from 'vue';
import type { Product } from '@/interfaces/product';

export interface CartItem {
  id: string;
  product: Product;
  quantity: number;
}

const STORAGE_KEY = 'firmeza_cart';

const items = ref<CartItem[]>(loadFromStorage());

// cargar desde localStorage
function loadFromStorage(): CartItem[] {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? JSON.parse(raw) : [];
  } catch {
    return [];
  }
}

// guardar en localStorage cada vez que cambie el carrito
watch(
  items,
  (val) => {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(val));
  },
  { deep: true },
);

const total = computed(() => items.value.reduce((acc, i) => acc + i.quantity * i.product.price, 0));

function add(product: Product) {
  const existing = items.value.find((i) => i.product.id === product.id);

  if (existing) {
    existing.quantity++;
  } else {
    items.value.push({
      id: crypto.randomUUID(),
      product,
      quantity: 1,
    });
  }
}

function remove(id: string) {
  items.value = items.value.filter((i) => i.id !== id);
}

function decrease(id: string) {
  const item = items.value.find((i) => i.id === id);
  if (!item) return;
  if (item.quantity > 1) item.quantity--;
  else remove(id);
}

function increase(id: string) {
  const item = items.value.find((i) => i.id === id);
  if (item) item.quantity++;
}

function clear() {
  items.value = [];
}

export function useCart() {
  return {
    items,
    total,
    add,
    remove,
    decrease,
    increase,
    clear,
  };
}
