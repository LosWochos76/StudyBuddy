import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermEditComponent } from './term-edit.component';

describe('TermEditComponent', () => {
  let component: TermEditComponent;
  let fixture: ComponentFixture<TermEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TermEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TermEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
