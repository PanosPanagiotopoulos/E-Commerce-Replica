export interface Product {
  id: number,
  pid: string;
  url: string[];
  title: string;
  description: string;
  price: number;
  category: string;
  shippingCost: number;
  paymentMethods: string[];
}
