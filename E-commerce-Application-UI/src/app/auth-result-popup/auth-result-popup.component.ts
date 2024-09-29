import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'e-com-app-auth-result-popup',
  templateUrl: './auth-result-popup.component.html',
  styleUrls: ['./auth-result-popup.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class AuthResultPopupComponent {
  /**
   * Represents a message component with an optional success indicator.
   * @Input message - The message to be displayed in the component.
   * @Input isSuccess - A boolean flag indicating whether the message represents a success (true) or an error (false).
   */
  @Input() message: string = '';
  @Input() isSuccess: boolean = true;
}
