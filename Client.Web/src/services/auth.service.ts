import type { ApiResponse } from '@/interfaces/api-response';
import { handleResponse } from '@/helpers/handle-response';

const API_URL = 'http://localhost:5152/api/auth/login';

export const authService = {
  async login(email: string, password: string): Promise<ApiResponse<string>> {
    const res = await fetch(API_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email: email, password: password }),
    });
    return handleResponse<string>(res);
  },
};
