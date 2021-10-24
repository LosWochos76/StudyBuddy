import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FcmTokenListComponent } from './fcm-token-list.component';

describe('FcmTokenListComponent', () => {
  let component: FcmTokenListComponent;
  let fixture: ComponentFixture<FcmTokenListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FcmTokenListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FcmTokenListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
