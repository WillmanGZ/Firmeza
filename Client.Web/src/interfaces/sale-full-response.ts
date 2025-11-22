export interface SaleFullResponse {
  sale: {
    id: string;
    clientId: string;
    date: string;
  };
  products: {
    id: string;
    saleId: string;
    productId: string;
    quantity: number;
    unitPrice: number;
  }[];
}
