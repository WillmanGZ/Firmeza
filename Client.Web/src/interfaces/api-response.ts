export interface ApiResponse<T> {
  code: number;
  success: boolean;
  message?: string;
  payload?: T;
}
