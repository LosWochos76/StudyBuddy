import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChallengeSelectComponent } from './challenge-select.component';

describe('ChallengeSelectComponent', () => {
  let component: ChallengeSelectComponent;
  let fixture: ComponentFixture<ChallengeSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChallengeSelectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChallengeSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
