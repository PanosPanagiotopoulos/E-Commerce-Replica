import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavigationSearchComponent } from './navigation-search/navigation-search.component';
import { ProductsListPageComponent } from './products-list-page/products-list-page.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SimpleFooterComponent } from './simple-footer/simple-footer.component';
import { AuthPageComponent } from './auth-page/auth-page.component';
import { AuthPromptComponent } from './auth-prompt/auth-prompt.component';
import { ShoppingCartPopupComponent } from './shopping-cart-popup/shopping-cart-popup.component';
import { CartVisibilityService } from '../services/cart-visibility-service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'e-com-app',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    NavigationSearchComponent,
    ProductsListPageComponent,
    SimpleFooterComponent,
    AuthPageComponent,
    AuthPromptComponent,
    ShoppingCartPopupComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  constructor(private cartVisibilityService: CartVisibilityService) {}

  title = 'e-commerce-app';
  isCartVisible = false;

  ngOnInit(): void {
    this.cartVisibilityService.cartVisibility$.subscribe((isVisible) => {
      this.isCartVisible = isVisible;
    });
  }
}
