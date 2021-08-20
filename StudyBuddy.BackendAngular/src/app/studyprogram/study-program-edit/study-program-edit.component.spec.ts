import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudyProgramEditComponent } from './study-program-edit.component';

describe('StudyProgramEditComponent', () => {
  let component: StudyProgramEditComponent;
  let fixture: ComponentFixture<StudyProgramEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudyProgramEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudyProgramEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
