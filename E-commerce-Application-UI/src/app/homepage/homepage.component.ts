import { Component } from '@angular/core';
import { NavigationSearchComponent } from '../navigation-search/navigation-search.component';
import { ProductsListPageComponent } from '../products-list-page/products-list-page.component';
import { SimpleFooterComponent } from '../simple-footer/simple-footer.component';
import { ShoppingCartPopupComponent } from '../shopping-cart-popup/shopping-cart-popup.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartVisibilityService } from '../../services/cart-visibility-service';

@Component({
  selector: 'e-com-app-homepage',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NavigationSearchComponent,
    ProductsListPageComponent,
    SimpleFooterComponent,
    ShoppingCartPopupComponent,
  ],
  templateUrl: './homepage.component.html',
})
export class HomepageComponent {
  constructor(private cartVisiblityService: CartVisibilityService) {}
  isCartPopupVisible: boolean = false;

  ngOnInit(): void {
    // Subscribe to cart visibility state
    this.cartVisiblityService.cartVisibility$.subscribe((isVisible) => {
      this.isCartPopupVisible = isVisible;
    });
  }
}
