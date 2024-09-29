import { Injectable } from '@angular/core';
import { Product } from '../models/Product';
import { BehaviorSubject } from 'rxjs';

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

  initializeProducts() {
    // For testing purposes
    const prevProducts = sessionStorage.getItem('products');
    if (!prevProducts) {
      const products: Product[] = [];
      for (var i = 0; i < 20; i++) {
        products.push(this.generateRandomProduct());
      }

      this.productsListSource.next(products);
      sessionStorage.setItem('products', JSON.stringify(products));
      return;
    }

    this.productsListSource.next(JSON.parse(prevProducts));
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

  /**
   * Helper function to create sample fake product values
   */
  private generateRandomProduct(): Product {
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
      url: [
        getRandomValue(urls),
        getRandomValue(urls),
        getRandomValue(urls),
      ].flat(), // Randomly pick one URL for all 3 slots
      title: getRandomValue(titles),
      description: getRandomValue(descriptions),
      price: Math.floor(Math.random() * (100 - 5 + 1)) + 5, // Random price between 5 and 100
      category: getRandomValue(categories),
      shippingCost: Math.floor(Math.random() * (10 - 1 + 1)) + 1, // Random shipping cost between 1 and 10
      paymentMethods: ['cash', 'card', 'paypal'], // Always these 3 payment methods
    };

    return product;
  }
}
