import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchService } from '../../services/search-service';
import { FormsModule } from '@angular/forms'; // Import FormsModule
import { ShoppingCartPopupComponent } from '../shopping-cart-popup/shopping-cart-popup.component';
import { CartVisibilityService } from '../../services/cart-visibility-service';

@Component({
  selector: 'e-com-app-navigation-search',
  standalone: true,
  imports: [FormsModule, CommonModule, ShoppingCartPopupComponent],
  templateUrl: './navigation-search.component.html',
  styleUrl: './navigation-search.component.scss',
})
export class NavigationSearchComponent {
  // Call search service for input
  constructor(
    private searchService: SearchService,
    private cartVisibilityService: CartVisibilityService
  ) {}
  @Output() cartVisibility: EventEmitter<boolean> = new EventEmitter<boolean>();
  isCartVisible: boolean = false;

  searchQuery: string = '';

  /**
   * Function that calls the search service to find products based on query
   * and updates the main products page with the new filtered products data
   */
  searchProducts() {
    // Call search service for input
    this.searchService.search(this.searchQuery);
  }

  /**
   * Function to redirect to cart when cart button is clicked
   */
  goToCart() {}

  toggleCartPopup() {
    this.cartVisibilityService.toggleCartVisibility();
  }

  showCartPopup() {
    this.cartVisibilityService.showCart();
  }

  hideCartPopup() {
    this.cartVisibilityService.hideCart();
  }
}
