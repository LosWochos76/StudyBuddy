import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChallengeEditComponent } from './challenge-edit.component';

describe('ChallengeEditComponent', () => {
  let component: ChallengeEditComponent;
  let fixture: ComponentFixture<ChallengeEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChallengeEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChallengeEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
