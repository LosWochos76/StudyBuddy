import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChallengeCloneComponent } from './challenge-clone.component';

describe('ChallengeCloneComponent', () => {
  let component: ChallengeCloneComponent;
  let fixture: ComponentFixture<ChallengeCloneComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChallengeCloneComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChallengeCloneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
