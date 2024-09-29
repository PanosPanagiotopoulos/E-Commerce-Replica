import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthResultPopupComponent } from './auth-result-popup.component';

describe('AuthResultPopupComponent', () => {
  let component: AuthResultPopupComponent;
  let fixture: ComponentFixture<AuthResultPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthResultPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthResultPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
