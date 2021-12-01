import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessEventListComponent } from './businessevent-list.component';

describe('BusinesseventsListComponent', () => {
  let component: BusinessEventListComponent;
  let fixture: ComponentFixture<BusinessEventListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BusinessEventListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BusinessEventListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
