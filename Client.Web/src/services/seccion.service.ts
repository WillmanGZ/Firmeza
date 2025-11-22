import type { ApiResponse } from '@/interfaces/api-response';
import { handleResponse } from '@/helpers/handle-response';
import type { Seccion } from '@/interfaces/Seccion';

const API_URL = 'http://localhost:8080/api/secciones';

export const seccionService = {
  async getAll(): Promise<ApiResponse<Seccion[]>> {
    const res = await fetch(API_URL);
    return handleResponse<Seccion[]>(res);
  },

  async getById(id: number): Promise<ApiResponse<Seccion>> {
    const res = await fetch(`${API_URL}/${id}`);
    return handleResponse<Seccion>(res);
  },

  async create(data: Seccion): Promise<ApiResponse<Seccion>> {
    const res = await fetch(API_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse<Seccion>(res);
  },

  async update(id: number, data: Seccion): Promise<ApiResponse<Seccion>> {
    const res = await fetch(`${API_URL}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse<Seccion>(res);
  },

  async remove(id: number): Promise<ApiResponse<null>> {
    const res = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
    return handleResponse<null>(res);
  },
};
