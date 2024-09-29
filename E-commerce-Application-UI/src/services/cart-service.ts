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
  // Check if the component is first time initialised
  // Testing purposes
  private isFirstInitialized: boolean = true; // Add this flag to ensure one-time initialization

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
    if (this.isFirstInitialized) {
      try {
        // Actual backend connection for the initialisation of the cart items
        // this.cartDataSubject.next(await this.fetchCartData());
        this.cartDataSubject.next(this.generateRandomCartItems());
      } catch (error) {
        const axiosError = error as AxiosError<CartItem[]>;
        throw axiosError;
      }

      this.isFirstInitialized = false;
    }
  }

  /**
   * Fetches cart data for the authenticated user from the server.
   * @returns A Promise that resolves to an array of CartItem objects.
   */
  private async fetchCartData(): Promise<CartItem[]> {
    try {
      const userID: string = sessionStorage.getItem('authToken') || '';
      if (!userID) {
        return [];
      }
      const endpoint: string = `http://localhost:5000/api/cart`;
      const params = {
        user: userID,
      };
      // Define custom headers
      const headers = {
        'Content-Type': 'application/json',
      };
      // Request the users cart data if any
      const response: AxiosResponse<CartItem[]> = await axios.get<CartItem[]>(
        endpoint,
        {
          params,
          headers,
          responseType: 'json',
        }
      );

      return response.data;
    } catch (error) {
      const axiosError = error as AxiosError<CartItem>;
      throw axiosError;
    }
  }

  async removeCartItem(item: CartItem) {
    try {
      item.quantity--;
      if (!(item.quantity > 0)) {
        this.cartDataSubject.next(
          this.cartDataSubject.value.filter(
            (cartItem) => cartItem.product.id != item.product.id
          )
        );
      }

      // Final update for the server
      // await this.modifyCartItem(item);
    } catch (error) {
      throw error as AxiosError<string>;
    }
  }

  async modifyCartItem(item: CartItem): Promise<void> {
    if (!item) {
      throw new Error('No item given');
    }

    try {
      const userID: string = sessionStorage.getItem('authToken') || '';
      if (!userID) {
        throw new Error('No user authenticated found');
      }

      const endpoint: string = `http://localhost:5000/api/cart`;
      const body = {
        user: userID,
        cartItem: item,
      };
      // Define custom headers
      const headers = {
        'Content-Type': 'application/json',
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
        } as AxiosError<string>;
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
      const cartItem = this.cartDataSubject.value.find(
        (cartItem) => cartItem.product.id === item.id
      );

      if (cartItem) {
        cartItem.quantity++;
        return;
      }

      const newCartItem: CartItem = {
        product: item,
        quantity: 1,
      };

      this.cartDataSubject.value.push(newCartItem);

      // Update the server with the new item
      // await this.saveCartItem(newCartItem);
    } catch (error) {
      const axiosError = error as AxiosError<string>;
      console.error(axiosError);
    }
  }

  async saveCartItem(item: CartItem): Promise<void> {
    if (!item) {
      throw new Error('No item given');
    }

    try {
      const userID: string = sessionStorage.getItem('authToken') || '';
      if (!userID) {
        throw new Error('No user authenticated found');
      }

      const endpoint: string = `http://localhost:5000/api/cart`;
      const body = {
        user: userID,
        cartItem: item,
      };
      // Define custom headers
      const headers = {
        'Content-Type': 'application/json',
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
        } as AxiosError<string>;
      }
    } catch (error) {
      const axiosError = error as AxiosError;
      throw axiosError;
    }
  }

  private generateRandomCartItems(): CartItem[] {
    var cartItems: CartItem[] = [];

    for (var i = 0; i < 5; i++) {
      cartItems.push(this.generateRandomCartItem(cartItems));
    }

    return cartItems;
  }

  /**
   * Helper function to create sample fake cart item values
   */
  private generateRandomCartItem(currentItems: CartItem[]): CartItem {
    // Possible values for URLs, titles, and categories
    const urls = [
      'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThRXuidqRiC6fOuxBYenmi8knrzZ1qLxNjcchJVm31NKeelz_iVAXQ3llZhspgVR8sOXs&usqp=CAU',
      'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRKEskkEsgaaMnUV6zBHUvPreeAMP2vxv1E2A&usqp=CAU',
      'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSF9M_-P3YiCb4xbfeKG87GH_oI6raUliRViQ&usqp=CAU',
    ];

    const titles = ['Running Shoes', 'Comfortable Trousers', 'Stylish Shirt'];
    const descriptions = [
      'Running Shoes designed to provide ultimate comfort and performance for athletes. With cushioned soles and lightweight material, these shoes are perfect for long-distance runners and everyday joggers.',
      'Comfortable Trousers crafted from breathable cotton fabric. These trousers are ideal for casual wear, offering flexibility and style while keeping you comfortable throughout the day.',
      'Stylish Shirt made from high-quality, wrinkle-resistant fabric. It is perfect for both casual and semi-formal occasions, providing a smart, trendy look that stands out.',
      'Running Shoes engineered with advanced traction for enhanced grip on various surfaces. These shoes offer excellent arch support and are suitable for outdoor trail running and gym workouts.',
      'Comfortable Trousers with a relaxed fit, featuring an elastic waistband and adjustable drawstring. Ideal for lounging or casual outings, they provide both style and ease of movement.',
      'Stylish Shirt with a slim fit design, crafted from soft, breathable fabric. This shirt pairs well with both jeans and trousers, making it a versatile addition to any wardrobe.',
      'Running Shoes featuring breathable mesh uppers to keep your feet cool during intense runs. The shoes provide excellent cushioning to reduce impact on joints and prevent injuries.',
      'Comfortable Trousers with a modern cut, perfect for casual Fridays or weekend outings. The fabric blend ensures durability and comfort, making them an essential part of any wardrobe.',
    ];

    const categories = ['Shoes', 'Trousers', 'Shirts'];

    // Helper function to generate a random string ID
    const generateRandomId = () => Math.random().toString(36).substring(2, 8);

    // Helper function to select a random element from an array
    const getRandomValue = <T>(array: T[]): T =>
      array[Math.floor(Math.random() * array.length)];

    // Generate the product with random values
    const product: Product = {
      id: Math.floor(Math.random() * 1000),
      pid: generateRandomId(),
      url: Array(3).fill(getRandomValue(urls)), // Randomly pick one URL for all 3 slots
      title: getRandomValue(titles),
      description: getRandomValue(descriptions),
      price: Math.floor(Math.random() * (100 - 5 + 1)) + 5, // Random price between 5 and 100
      category: getRandomValue(categories),
      shippingCost: Math.floor(Math.random() * (10 - 1 + 1)) + 1, // Random shipping cost between 1 and 10
      paymentMethods: ['cash', 'card', 'paypal'], // Always these 3 payment methods
    };

    const quantity = Math.floor(Math.random() * 6 + 1);

    return {
      product: product,
      quantity: quantity,
    };
  }
}
