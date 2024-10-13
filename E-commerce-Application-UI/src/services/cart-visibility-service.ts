/**
 * Service to manage the visibility of the cart.
 * @class CartVisibilityService
 */
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartVisibilityService {
  private cartVisibleSubject = new BehaviorSubject<boolean>(false);
  /**
   * Observable that emits the visibility state of the cart.
   * @type {Observable<boolean>}
   */
  cartVisibility$ = this.cartVisibleSubject.asObservable();

  /**
   * Toggles the visibility of the cart by updating the cartVisibleSubject value.
   * It negates the current value of cartVisibleSubject to show/hide the cart.
   * It also logs the current cart visibility status to the console.
   * @returns void
   */
  toggleCartVisibility(): void {
    this.cartVisibleSubject.next(!this.cartVisibleSubject.value);
  }

  showCart(): void {
    this.cartVisibleSubject.next(true);
  }

  /**
   * Hides the cart by updating the cart visibility subject to false.
   * Also logs the current cart visibility status.
   * @returns None
   */
  hideCart(): void {
    this.cartVisibleSubject.next(false);
  }

  isCartVisible(): boolean {
    return this.cartVisibleSubject.value;
  }
}
