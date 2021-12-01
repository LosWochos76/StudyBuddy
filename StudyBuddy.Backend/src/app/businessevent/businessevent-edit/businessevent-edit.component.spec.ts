import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BusinessEventEditComponent } from './businessevent-edit.component';

describe('BusinesseventsEditComponent', () => {
  let component: BusinessEventEditComponent;
  let fixture: ComponentFixture<BusinessEventEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BusinessEventEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BusinessEventEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
