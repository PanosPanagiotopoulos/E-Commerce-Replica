import { Injectable } from '@angular/core';
import { Product } from '../models/Product';
import { BehaviorSubject } from 'rxjs';
import axios, { AxiosResponse } from 'axios';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  // Observable object to help pass data to the details page
  private productSource = new BehaviorSubject<Product | null>(null);
  currentProduct$ = this.productSource.asObservable();

  // Current products lastly requested
  private productsListSource: BehaviorSubject<Product[]> = new BehaviorSubject<
    Product[]
  >([]);
  currentProductsList$ = this.productsListSource.asObservable();

  constructor() {
    this.initializeProducts(); // Initialize products on service start
  }

  async initializeProducts(): Promise<void> {
    const endpoint: string = `/api/Product/products`;
    const params = {
      page: 1,
      pagesize: 20,
    };
    // Define custom headers
    const headers = {
      'Content-Type': 'application/json',
    };
    // Request the users cart data if any
    const response: AxiosResponse = await axios.get(endpoint, {
      params,
      headers,
      responseType: 'json',
    });

    this.productsListSource.next(response.data.products);
  }

  //TODO Backend Connection for data requests (uses search service)
  findProductLocally(id: string | null): Product | null {
    return (
      this.productsListSource.value.find((product) => product.pid === id) ||
      null
    );
  }

  setCurrentProduct(product: Product) {
    this.productSource.next(product);
  }

  getCurrentProduct(): Product | null {
    return this.productSource.value;
  }

  getProducts(): Product[] {
    return this.productsListSource.value;
  }
}
