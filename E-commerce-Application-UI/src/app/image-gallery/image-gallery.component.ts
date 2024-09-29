import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'e-com-app-image-gallery',
  templateUrl: './image-gallery.component.html',
  styleUrls: ['./image-gallery.component.scss'],
  standalone: true,
  imports: [CommonModule],
})
export class ImageGalleryComponent {
  @Input() imageUrls: string[] = [];
  selectedIndex: number = 0;
  fade: boolean = false; // Add fade property

  // Function to handle image selection
  selectImage(index: number) {
    this.fade = true; // Trigger fade-out
    setTimeout(() => {
      this.selectedIndex = index; // Change the image
      this.fade = false; // Fade back in
    }, 300); // Adjust timing to match the fade-out duration
  }
}
