import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterUserSuccessComponent } from './register-user-success.component';

describe('RegisterUserSuccessComponent', () => {
  let component: RegisterUserSuccessComponent;
  let fixture: ComponentFixture<RegisterUserSuccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterUserSuccessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterUserSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
