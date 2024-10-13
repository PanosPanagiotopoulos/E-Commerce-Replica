export interface Product {
  id: number;
  pid: string;
  title: string;
  description: string;
  price: number;
  category: string;
  shippingCost: number;
  paymentMethods: string[];
  imageURLS: string[];
}
