import {
  Component,
  OnInit,
  DoCheck,
  Input,
  KeyValueDiffers,
  SimpleChange,
  SimpleChanges,
  OnChanges,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductCardComponent } from '../product-card/product-card.component';
import { Product } from '../../models/Product';
import { AuthPageComponent } from '../auth-page/auth-page.component';
import { ProductService } from '../../services/product-service';

@Component({
  selector: 'e-com-app-products-list-page',
  standalone: true,
  imports: [CommonModule, ProductCardComponent, AuthPageComponent],
  templateUrl: './products-list-page.component.html',
  styleUrl: './products-list-page.component.scss',
})
export class ProductsListPageComponent implements OnInit, OnChanges {
  products: Product[] | null = [];
  productsOriginal: Product[] | null = [];
  categories: string[] = [];
  fadeIn = false;

  constructor(private productService: ProductService) {}
  ngOnInit() {
    this.productService.currentProductsList$.subscribe((products) => {
      this.products = products;
      this.productsOriginal = products;
      this.categories = [
        ...new Set(products.map((product) => product.category)),
      ];
    });

    this.triggerFadeIn();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['products']) {
      this.triggerFadeIn();
    }
  }

  /**
   *
   * @param scrollBar : The click event when the scrollBar option was clicked
   * Filters the products list by category
   */
  filterByCategory(scrollBarOption: Event) {
    const category = (
      scrollBarOption.target as HTMLElement
    ).innerText.toLowerCase();

    // Temporarily remove the fade-in effect and reset the product list to its initial state
    this.fadeIn = false;

    // Force a reflow to ensure the reset state is applied
    void (document.querySelector('.products-list') as HTMLElement).offsetHeight;

    // Delay the filtering and re-application of the fade-in class to ensure smooth animation
    setTimeout(() => {
      if (category === 'all') {
        this.products = [...this.productsOriginal!];
      } else {
        this.products = this.productsOriginal!.filter(
          (product) => product.category.toLowerCase() === category
        );
      }

      // Trigger the fade-in effect after filtering
      this.triggerFadeIn();
    }, 350); // Adjust the delay as needed to ensure the reset state is visible before animation
  }

  triggerFadeIn() {
    this.fadeIn = false;
    setTimeout(() => {
      this.fadeIn = true;
    }, 0); // Add a slight delay to ensure the DOM updates before the animation starts
  }
}
