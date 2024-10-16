import { Component, DoCheck, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { User } from '../../models/User';
import { AxiosError, AxiosResponse } from 'axios';
import { AuthToken } from '../../models/AuthToken';
import { AuthResultPopupComponent } from '../auth-result-popup/auth-result-popup.component';

@Component({
  selector: 'e-com-app-auth-page',
  standalone: true,
  imports: [CommonModule, FormsModule, AuthResultPopupComponent],
  templateUrl: './auth-page.component.html',
  styleUrl: './auth-page.component.scss',
})
export class AuthPageComponent implements OnInit, DoCheck {
  // User object to hold form data
  user: User = {
    firstname: '',
    lastname: '',
    email: '',
    password: '',
    phonenumber: '',
  };

  // Property to confirm the user's password
  confirmPassword: string = '';
  // Property that checks if the password and confirm password are the same
  passwordMismatch: boolean = false;
  // Message to be displayed in popup
  popupMessage: string = '';
  // Flag indicating if operation was successful or not
  isSuccess: boolean = true;
  // Flag to show/hide popup
  showPopup: boolean = false;
  // Interval time for popup visibility
  private popUpInterval: number = 2000; // Milliseconds that interval appears

  constructor(
    private loginService: LoginService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    document.body.classList.add('auth-page-active');
  }

  ngOnDestroy(): void {
    document.body.classList.remove('auth-page-active');
  }

  // Implement ngDoCheck
  ngDoCheck(): void {
    this.checkPasswords();
  }

  /**
   * Callback function for login button
   */
  async login() {
    /* TODO:
       Set animation loading till it loads and then stop it 
    */
    try {
      this.showPopup = false;
      const loginResponse: AxiosResponse<AuthToken> =
        await this.loginService.login(this.user.email, this.user.password!);
      this.popupMessage = 'Successful login';
      this.isSuccess = true;
      this.showPopup = true;
      this.hidePopupAfterDelay();

      this.saveAuthToken(loginResponse.data);
      setTimeout(() => {
        this.loginService.handleAuthTraceback();
      }, this.popUpInterval);
    } catch (error) {
      this.popupMessage = 'Unsuccessful login';
      this.isSuccess = false;
      this.showPopup = true;
      this.hidePopupAfterDelay();
    }
  }

  /**
   * Callback function for register button
   */
  async register() {
    /* TODO:
       Set animation loading till it loads and then stop it 
    */
    // Do nothing if passwords mismatch
    if (!this.passwordMismatch) {
      try {
        this.showPopup = false;

        const registerResponse: AxiosResponse<AuthToken> =
          await this.loginService.register(this.user);
        // Save the token after successfull registration and login
        this.saveAuthToken(registerResponse.data);
        this.popupMessage = 'Successful registration';
        this.isSuccess = true;
        this.showPopup = true;
        this.hidePopupAfterDelay();
        setTimeout(() => {
          this.loginService.handleAuthTraceback();
        }, this.popUpInterval);
      } catch (error) {
        const axiosError = error as AxiosError;
        console.error(axiosError.message);
        this.popupMessage = 'Unsuccessful registration';
        this.isSuccess = false;
        this.showPopup = true;
        this.hidePopupAfterDelay();
      }
    }
  }

  /**
   * Hides the popup message after a defined delay
   */
  hidePopupAfterDelay() {
    setTimeout(() => {
      this.showPopup = false;
    }, this.popUpInterval); // Hide the popup after 4 seconds
  }

  /**
   * Saves the authentication token in session storage
   * @param token - The authentication token to be saved
   */
  saveAuthToken(authData: AuthToken) {
    sessionStorage.setItem('authToken', authData.token);
    sessionStorage.setItem('role', authData.role);
  }

  checkPasswords() {
    this.passwordMismatch = this.user.password !== this.confirmPassword;
  }
}
