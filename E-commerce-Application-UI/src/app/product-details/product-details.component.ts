import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { Product } from '../../models/Product';
import { ProductService } from '../../services/product-service';
import { CommonModule } from '@angular/common';
import { ImageGalleryComponent } from '../image-gallery/image-gallery.component';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../services/cart-service';
import { CartVisibilityService } from '../../services/cart-visibility-service';
import { SimpleFooterComponent } from '../simple-footer/simple-footer.component';

@Component({
  selector: 'e-com-app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ImageGalleryComponent,
    SimpleFooterComponent,
  ],
})
export class ProductDetailsComponent implements OnInit {
  product: Product | null = null;
  paymentMethods: string[] = ['cash', 'visa', 'paypal'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private titleService: Title,
    private cartService: CartService,
    private cartVisibilityService: CartVisibilityService
  ) {}

  async ngOnInit(): Promise<void> {
    this.product = this.productService.getCurrentProduct();

    if (this.product) {
      this.titleService.setTitle(this.product.title);
      return;
    }
    // Handle case when product data is not available
    // Likely from refresh
    const productId: string | null = this.route.snapshot.paramMap.get('id');
    // At worst case that product id is not in router link go to homepage
    if (!productId) {
      console.error('Product failed to be fetched from router link id');
      this.router.navigate(['/']);
    }

    this.product = this.productService.findProductLocally(productId);

    if (!this.product) {
      console.error('Product failed to be fetched with router id');
      this.router.navigate(['/']);
    }
  }

  addToCart(product: Product): void {
    this.cartService.addCartItem(product);
    this.cartVisibilityService.showCart();
    setTimeout(() => {
      this.cartVisibilityService.hideCart();
    }, 8000);
  }

  getPaymentIcon(method: string): string {
    switch (method.toLowerCase()) {
      case 'cash':
        return 'fa fa-money-bill-wave'; // Cash icon
      case 'visa':
        return 'fa fa-cc-visa'; // Visa icon
      case 'paypal':
        return 'fa fa-paypal'; // PayPal icon
      default:
        return 'fa fa-credit-card'; // Default icon
    }
  }
}
