import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'e-com-app-auth-prompt',
  templateUrl: './auth-prompt.component.html',
  styleUrls: ['./auth-prompt.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class AuthPromptComponent implements OnInit {
  isVisible = true; // Control visibility of the Auth Prompt

  constructor(private router: Router) {}

  ngOnInit(): void {
    // Subscribe to router events to detect route changes
    this.router.events.subscribe(() => {
      // If the current URL is /auth, make the prompt invisible
      this.isVisible = !(this.router.url === '/auth');
    });
  }

  // Navigate to the auth page when the component is clicked
  navigateToAuthPage() {
    this.router.navigate(['/auth']);
  }
}
