import { Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { AuthPageComponent } from './auth-page/auth-page.component';
import { ProductDetailsComponent } from './product-details/product-details.component';

export const routes: Routes = [
  { path: '', component: HomepageComponent }, // Set HomepageComponent as the default route
  { path: 'auth', component: AuthPageComponent }, // Auth route
  { path: 'product/:id', component: ProductDetailsComponent },
  { path: '**', redirectTo: '' }, // Fallback route
];
