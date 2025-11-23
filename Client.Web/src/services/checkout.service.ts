import type { ApiResponse } from '@/interfaces/api-response';
import { handleResponse } from '@/helpers/handle-response';
import { useAuth } from '@/composables/useAuth';
import type { CartItem } from '@/composables/useCart';
import type { SaleFullCreate } from '@/interfaces/sale-full-create';

const API_URL = 'http://localhost:5152/api/sales/full';

export const checkoutService = {
  async process(items: CartItem[]): Promise<ApiResponse<string>> {
    const { getToken } = useAuth();
    const token = getToken();

    const sale: SaleFullCreate = {
      products: items.map((i) => ({
        productId: i.product.id,
        quantity: i.quantity,
        unitPrice: i.product.price,
      })),
    };

    const res = await fetch(API_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(sale),
    });

    return handleResponse<string>(res);
  },
};
