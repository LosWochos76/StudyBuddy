import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudyProgramListComponent } from './study-program-list.component';

describe('StudyProgramListComponent', () => {
  let component: StudyProgramListComponent;
  let fixture: ComponentFixture<StudyProgramListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudyProgramListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudyProgramListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
