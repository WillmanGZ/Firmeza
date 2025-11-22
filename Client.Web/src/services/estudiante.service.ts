import type { Estudiante } from '@/interfaces/Estudiante';
import type { ApiResponse } from '@/interfaces/api-response';
import { handleResponse } from '@/helpers/handle-response';
import type { DetalleInscripcion } from '@/interfaces/DetalleInscripcion';

const API_URL = 'http://localhost:8080/api/estudiantes';

export const studentService = {
  async getAll(): Promise<ApiResponse<Estudiante[]>> {
    const res = await fetch(API_URL);
    return handleResponse<Estudiante[]>(res);
  },

  async getById(id: number): Promise<ApiResponse<Estudiante>> {
    const res = await fetch(`${API_URL}/${id}`);
    return handleResponse<Estudiante>(res);
  },

  async getPdfById(id: number): Promise<ApiResponse<string>> {
    const res = await fetch(`http://localhost:8080/api/inscripciones/pdf/${id}`);
    return handleResponse<string>(res);
  },

  async getInscripcionCompleta(id: number): Promise<ApiResponse<DetalleInscripcion>> {
    const res = await fetch(`http://localhost:8080/api/inscripciones/detalle/${id}`);
    return handleResponse<DetalleInscripcion>(res);
  },

  async create(data: Estudiante): Promise<ApiResponse<Estudiante>> {
    const res = await fetch(API_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse<Estudiante>(res);
  },

  async update(id: number, data: Estudiante): Promise<ApiResponse<Estudiante>> {
    const res = await fetch(`${API_URL}/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse<Estudiante>(res);
  },

  async remove(id: number): Promise<ApiResponse<null>> {
    const res = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
    return handleResponse<null>(res);
  },
};
