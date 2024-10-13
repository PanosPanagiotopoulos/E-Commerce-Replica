import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  AfterViewInit,
  ElementRef,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartItem } from '../../models/CartItem';
import { CartService } from '../../services/cart-service';
import { AxiosError } from 'axios';
import { Product } from '../../models/Product';
import { CartVisibilityService } from '../../services/cart-visibility-service';
import { Router } from '@angular/router';

@Component({
  selector: 'e-com-app-shopping-cart-popup',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './shopping-cart-popup.component.html',
  styleUrls: ['./shopping-cart-popup.component.scss'],
})
export class ShoppingCartPopupComponent implements OnInit, AfterViewInit {
  constructor(
    private cartService: CartService,
    private cartVisibilityService: CartVisibilityService,
    private router: Router
  ) {}

  cartItems: CartItem[] = [];

  animationClass: string = '';
  isAuthPage: boolean = false;

  @ViewChild('cartPopup', { static: true }) cartPopup!: ElementRef;

  /**
   * Lifecycle hook that is called after data-bound properties of a directive are initialized.
   * Here, it's used to initialize the shopping cart items and trigger the fade-in animation.
   */
  async ngOnInit() {
    this.checkIfAuthPage();
    // Apply the fade-in class when the component is initialized (visible)
    this.triggerFadeIn();
    // Subscribe to cart data changes
    this.cartService.cartData$.subscribe((cartItems) => {
      this.cartItems = cartItems;
    });

    if (sessionStorage.getItem('authToken')) {
      try {
        // Fetch the cart items of the user from the server
        await this.cartService.getCartData();
      } catch (error) {
        const axiosError = error as AxiosError<CartItem[]>;
        console.error(axiosError);
      }
    }
  }

  ngAfterViewInit(): void {
    this.adjustPopupPosition();
  }

  /**
   * Method to trigger the fade-in animation.
   */
  triggerFadeIn() {
    this.animationClass = 'fade-in';
  }

  /**
   * Getter method to calculate the total amount in the shopping cart.
   * This method sums up the price and shipping cost of each item multiplied by its quantity.
   */
  get totalAmount(): number {
    return this.cartItems.reduce(
      (sum, item) =>
        sum + (item.product.price + item.product.shippingCost) * item.quantity,
      0
    );
  }

  /**
   * Method to handle mouse enter event on the cart popup.
   * Keeps the cart visible when the mouse is hovering over it.
   */

  onMouseEnter(): void {
    this.cartVisibilityService.showCart();
  }

  /**
   * Method to handle mouse leave event on the cart popup.
   * Hides the cart when the mouse leaves the cart area.
   */

  onMouseLeave(): void {
    this.cartVisibilityService.hideCart();
  }

  /**
   * Method to handle the checkout process.
   * Currently, it just logs a message if there is a total amount.
   */
  goToCheckout() {
    if (!this.totalAmount) {
      return;
    }
    console.log('Proceeding to checkout');
  }

  /**
   * Method to remove an item from the cart.
   * Decreases the quantity of the item, and removes it if the quantity reaches zero.
   * Also handles errors if the removal process fails.
   */
  async removeItem(item: CartItem) {
    try {
      item.quantity--;
      if (!item.quantity) {
        this.cartItems = this.cartItems.filter(
          (cartItem) => cartItem.product.id != item.product.id
        );
      }

      await this.cartService.removeCartItem(item);
      await this.cartService.getCartData();
    } catch (error) {
      const axiosError = error as AxiosError<string>;
      console.error(axiosError);
    }
  }

  checkIfAuthPage(): void {
    // Check if the current route is /auth
    this.isAuthPage = this.router.url === '/auth';
  }

  adjustPopupPosition(): void {
    const cartButtonPosition = document
      .getElementById('cart-button-nav')!
      .getBoundingClientRect();
    const popup = this.cartPopup.nativeElement;

    if (popup && cartButtonPosition) {
      // Adjust the popup position dynamically based on the cart button
      let topOffset = 5; // Default top offset
      if (this.isAuthPage) {
        topOffset = 2; // Decrease top offset for /auth page
      }

      popup.style.top = `${cartButtonPosition.bottom + topOffset - 5}px`; // Adjust Y axis
    }
  }
}
