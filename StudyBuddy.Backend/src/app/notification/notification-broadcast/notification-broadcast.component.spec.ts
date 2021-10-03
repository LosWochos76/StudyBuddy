import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationBroadcastComponent } from './notification-broadcast.component';

describe('NotificationBroadcastComponent', () => {
  let component: NotificationBroadcastComponent;
  let fixture: ComponentFixture<NotificationBroadcastComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NotificationBroadcastComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationBroadcastComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
