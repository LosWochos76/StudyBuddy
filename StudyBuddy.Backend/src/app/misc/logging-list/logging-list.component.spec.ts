import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoggingListComponent } from './logging-list.component';

describe('LoggingListComponent', () => {
  let component: LoggingListComponent;
  let fixture: ComponentFixture<LoggingListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoggingListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoggingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
