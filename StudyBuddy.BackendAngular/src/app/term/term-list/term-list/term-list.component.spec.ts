import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermListComponent } from './term-list.component';

describe('TermListComponent', () => {
  let component: TermListComponent;
  let fixture: ComponentFixture<TermListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TermListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TermListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
