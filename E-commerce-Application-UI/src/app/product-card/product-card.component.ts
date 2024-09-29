import { Component, Input } from '@angular/core';
import { Product } from '../../models/Product';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product-service';

@Component({
  selector: 'e-com-app-product-card',
  standalone: true,
  imports: [],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.scss',
})
export class ProductCardComponent {
  constructor(private router: Router, private productService: ProductService) {}

  @Input() productData!: Product;

  /**
   * Navigate to the product details page for the clicked product.
   * The product ID is passed as a route parameter.
   * @param product - The product object clicked by the user.
   */
  goToProductDetails(): void {
    this.productService.setCurrentProduct(this.productData);
    this.router.navigate(['/product', this.productData.pid]);
  }
}
