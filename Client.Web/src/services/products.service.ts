import type { ApiResponse } from '@/interfaces/api-response';
import type { Product } from '@/interfaces/product';
import { handleResponse } from '@/helpers/handle-response';
import { useAuth } from '@/composables/useAuth';

const API_URL = 'http://localhost:8081/api/products'; // ajusta si es otro puerto

export const productService = {
  async getAll(): Promise<ApiResponse<Product[]>> {
    const { getToken } = useAuth();
    const token = getToken();

    const res = await fetch(API_URL, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return handleResponse<Product[]>(res);
  },

  async getById(id: string): Promise<ApiResponse<Product>> {
    const { getToken } = useAuth();
    const token = getToken();

    const res = await fetch(`${API_URL}/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return handleResponse<Product>(res);
  },
};
