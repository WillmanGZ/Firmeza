import type { ApiResponse } from '@/interfaces/api-response';
import { handleResponse } from '@/helpers/handle-response';
import type { UserRegister } from '@/interfaces/user-register';
import type { UserLogin } from '../interfaces/user-login';

const API_URL = 'http://localhost:5152/api/auth';

export const authService = {
  async login(user: UserLogin): Promise<ApiResponse<string>> {
    const res = await fetch(`${API_URL}/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(user),
    });
    return handleResponse<string>(res);
  },
  async register(
    user: UserRegister,
  ): Promise<ApiResponse<{ email: string; userName: string; phoneNumber: string }>> {
    const res = await fetch(`${API_URL}/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(user),
    });
    return handleResponse<{ email: string; userName: string; phoneNumber: string }>(res);
  },
};
