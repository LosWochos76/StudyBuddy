import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationUserMetadataListComponent } from './notification-user-metadata-list.component';

describe('NotificationMetadataListComponent', () => {
  let component: NotificationUserMetadataListComponent;
  let fixture: ComponentFixture<NotificationUserMetadataListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NotificationUserMetadataListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationUserMetadataListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
