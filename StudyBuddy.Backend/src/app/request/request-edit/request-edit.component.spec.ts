import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestEditComponent } from './request-edit.component';

describe('RequestEditComponent', () => {
  let component: RequestEditComponent;
  let fixture: ComponentFixture<RequestEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RequestEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
