import type { ApiResponse } from '@/interfaces/api-response';
import { handleResponse } from '@/helpers/handle-response';
import { useAuth } from '@/composables/useAuth';
import type { CartItem } from '@/composables/useCart';

const API_URL = 'https://jsonplaceholder.typicode.com/posts';

export const checkoutService = {
  async process(items: CartItem[]): Promise<ApiResponse<string>> {
    const { getToken } = useAuth();
    const token = getToken();

    const res = await fetch(API_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        items: items.map((i) => ({
          productId: i.product.id,
          quantity: i.quantity,
        })),
      }),
    });

    return handleResponse<string>(res);
  },
};
