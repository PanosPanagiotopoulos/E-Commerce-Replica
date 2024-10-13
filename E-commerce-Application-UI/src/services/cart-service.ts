import { Injectable } from '@angular/core';
import axios, { Axios, AxiosError, AxiosResponse } from 'axios';
import { Product } from '../models/Product';
import { CartItem } from '../models/CartItem';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
/**
 * Service to inject when we wanna have authentication in our website
 */
export class CartService {
  private cartDataSubject = new BehaviorSubject<CartItem[]>([]);
  cartData$ = this.cartDataSubject.asObservable();

  /**
   *
   * @returns : The cart product data array
   */
  /**
   * Asynchronously retrieves cart data if it is the first initialization.
   * If it is the first initialization, generates random cart items and updates the cart data subject.
   * Throws an AxiosError if there is an error during the process.
   * @returns None
   */
  async getCartData() {
    try {
      this.cartDataSubject.next(await this.fetchCartData());
    } catch (error) {
      const axiosError = error as AxiosError;
      throw axiosError;
    }
  }

  /**
   * Fetches cart data for the authenticated user from the server.
   * @returns A Promise that resolves to an array of CartItem objects.
   */
  private async fetchCartData(): Promise<CartItem[]> {
    try {
      const userId: string = sessionStorage.getItem('authToken') || '';
      if (!userId) {
        return [];
      }
      const endpoint: string = `/api/Cart`;

      // Define custom headers
      const headers = {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${userId}`,
      };

      // Request the users cart data if any
      const response: AxiosResponse<CartItem[]> = await axios.get<CartItem[]>(
        endpoint,
        {
          headers,
          responseType: 'json',
        }
      );

      return response.data;
    } catch (error) {
      const axiosError = error as AxiosError;
      throw axiosError;
    }
  }

  async removeCartItem(item: CartItem) {
    try {
      // Final update for the server
      await this.modifyCartItem(item);
      await this.getCartData();
    } catch (error) {
      throw error as AxiosError;
    }
  }

  async modifyCartItem(item: CartItem): Promise<void> {
    if (!item) {
      throw new Error('No item given');
    }

    try {
      const userId: string = sessionStorage.getItem('authToken') || '';
      if (!userId) {
        throw new Error('No user authenticated found');
      }

      const endpoint: string = `/api/Cart`;
      const body = {
        id: item.product.id,
        quantity: item.quantity,
      };
      // Define custom headers
      const headers = {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${userId}`,
      };
      // Request the users cart data if any
      const response: AxiosResponse<string> = await axios.put<string>(
        endpoint,
        body,
        { headers, responseType: 'json' }
      );

      if (!(response.status >= 200 && response.status < 300)) {
        throw {
          name: 'AxiosError',
          message: `Request failed with status code ${response.status}`, // Custom error message
          config: response.config,
          code: '' + response.status,
          request: response.request,
          response: response,
          isAxiosError: true, // This is specific to Axios errors
          toJSON: () => ({}), // Optional: Implement if needed
        } as AxiosError;
      }
    } catch (error) {
      const axiosError = error as AxiosError;
      throw axiosError;
    }
  }

  /**
   * Method to add a new item to the cart.
   * If the item is already in the cart, its quantity is increased.
   * Otherwise, a new cart item is created and added to the cart.
   * Handles errors during the process.
   */
  async addCartItem(item: Product) {
    try {
      const newCartItem: CartItem = {
        product: item,
        quantity: 1,
      };

      // Update the server with the new item
      await this.saveCartItem(newCartItem);
      await this.getCartData();
    } catch (error) {
      const axiosError = error as AxiosError;
      console.error(axiosError);
    }
  }

  async saveCartItem(item: CartItem): Promise<void> {
    if (!item) {
      throw new Error('No item given');
    }

    try {
      const userId: string = sessionStorage.getItem('authToken') || '';
      if (!userId) {
        throw new Error('No user authenticated found');
      }

      const endpoint: string = `/api/Cart`;
      const body = {
        id: item.product.id,
      };
      // Define custom headers
      const headers = {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${userId}`,
      };
      // Request the users cart data if any
      const response: AxiosResponse<string> = await axios.post<string>(
        endpoint,
        body,
        { headers, responseType: 'json' }
      );

      if (!(response.status >= 200 && response.status < 300)) {
        throw {
          name: 'AxiosError',
          message: `Request failed with status code ${response.status}`, // Custom error message
          config: response.config,
          code: '' + response.status,
          request: response.request,
          response: response,
          isAxiosError: true, // This is specific to Axios errors
          toJSON: () => ({}), // Optional: Implement if needed
        } as AxiosError;
      }
    } catch (error) {
      const axiosError = error as AxiosError;
      throw axiosError;
    }
  }
}
