import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { AuthToken } from '../models/AuthToken';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root',
})
/**
 * Service to inject when we wanna have authentication in our website
 */
export class LoginService {
  // Always have the url of the previous component to route back to it
  private returnUrl: string = '';

  constructor(private router: Router) {}

  // Call this method to navigate to the auth page
  /**
   * Navigates to the authentication page with the specified return URL.
   * If no return URL is provided, it navigates to the authentication page without any return URL.
   * @param {string} returnUrl - The return URL to navigate back to after authentication.
   * @returns None
   */
  navigateToAuthPage(returnUrl: string) {
    this.returnUrl = returnUrl ? returnUrl : '';
    this.router.navigate(['/auth']);
  }

  /**
   *
   * @param email The email of the user
   * @param password The password of the user
   * @returns The axios response of type AuthToken with the JTW token from the server or error value
   */
  async login(
    email: string,
    password: string
  ): Promise<AxiosResponse<AuthToken>> {
    // API call
    try {
      const endpoint: string = `/auth/login`;
      const body = {
        email,
        password,
      };
      // Define custom headers
      const headers = {
        'Content-Type': 'application/json',
      };

      const response: AxiosResponse<AuthToken> = await axios.post(
        endpoint,
        body,
        { headers, responseType: 'json' }
      );

      if (!(response.status >= 200 && response.status < 300)) {
        console.error(
          'Error in login\n' +
            response.statusText +
            '\nError Data : ' +
            response.data
        );

        const axiosError: AxiosError = {
          name: 'AxiosError',
          message: response.data.token,
          config: response.config,
          code: '' + response.status, // Optional, typically used for specific error codes like ECONNABORTED
          request: response.request,
          response,
        } as AxiosError;

        throw axiosError;
      }
      return response;
    } catch (error) {
      throw error as AxiosError;
    }
  }

  /**
   *
   * @param user The user object that is going to be saved
   * @returns the promise of the response with the JWT data inside or error value
   */
  async register(user: User): Promise<AxiosResponse<AuthToken>> {
    // API call
    try {
      const endpoint: string = `/api/User`;
      const body = user;
      // Define custom headers
      const headers = {
        'Content-Type': 'application/json',
      };

      const response = await axios.post(endpoint, body, {
        headers,
        responseType: 'json',
      });

      if (!(response.status >= 200 && response.status < 300)) {
        const axiosError: AxiosError = {
          name: 'AxiosError',
          message: response.data,
          config: response.config,
          code: '' + response.status, // Optional, typically used for specific error codes like ECONNABORTED
          request: response.request,
          response,
        } as AxiosError;

        throw axiosError;
      }

      return await this.login(user.email, user.password!);
    } catch (error) {
      console.log(
        'Server Register Response Error:\n' + JSON.stringify(error, null, 2)
      );
      const axiosError: AxiosError = error as AxiosError;
      throw axiosError;
    }
  }
  /**
   * Call this method after successful
   *  authentication to navigate back
   * */
  handleAuthTraceback() {
    this.router.navigate([this.returnUrl]);
    this.returnUrl = ''; // Clear the return URL after navigation
  }

  get previousUrl(): string {
    return this.previousUrl;
  }
}
