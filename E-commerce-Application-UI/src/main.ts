import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

bootstrapApplication(AppComponent, {
  providers: [
    ...appConfig.providers,
    { provide: BrowserAnimationsModule, useValue: BrowserAnimationsModule },
  ],
}).catch((err) => console.error(err));
