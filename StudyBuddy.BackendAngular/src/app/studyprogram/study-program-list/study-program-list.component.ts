import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StudyProgram } from 'src/app/model/studyprogram';
import { LoggingService } from 'src/app/shared/loging.service';
import { StudyProgramService } from 'src/app/shared/study-program.service';

@Component({
  selector: 'app-study-program-list',
  templateUrl: './study-program-list.component.html',
  styleUrls: ['./study-program-list.component.css']
})
export class StudyProgramListComponent implements OnInit {
  objects:StudyProgram[] = [];
  selected:StudyProgram|null = null;

  constructor(
    private logger:LoggingService, 
    private service:StudyProgramService,
    private router:Router) { }

  async ngOnInit() {
    this.objects = await this.service.getAll();
    this.service.changed.subscribe(async () => {
      this.objects = await this.service.getAll();
    });
  }
  
  onSelect(obj:StudyProgram) {
    this.logger.debug("User selected a StudyProgram");
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;
    
    this.logger.debug("User wants to delete a StudyProgram");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    this.logger.debug("User wants to edit a StudyProgram");
    this.router.navigate(['/studyprogram', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a StudyProgram");
    this.router.navigate(['/studyprogram/0']);
  }
}
