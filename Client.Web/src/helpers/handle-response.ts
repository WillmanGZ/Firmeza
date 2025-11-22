import type { ApiResponse } from '@/interfaces/api-response';

export async function handleResponse<T>(response: Response): Promise<ApiResponse<T>> {
  const json = await response.json();
  return json;
}
