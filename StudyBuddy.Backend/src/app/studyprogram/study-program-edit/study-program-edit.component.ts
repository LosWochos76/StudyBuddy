import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { StudyProgram } from 'src/app/model/studyprogram';
import { LoggingService } from 'src/app/services/loging.service';
import { StudyProgramService } from 'src/app/services/study-program.service';

@Component({
  selector: 'app-study-program-edit',
  templateUrl: './study-program-edit.component.html',
  styleUrls: ['./study-program-edit.component.css']
})
export class StudyProgramEditComponent implements OnInit {
  id:number = 0;
  obj:StudyProgram = null;
  form:FormGroup;

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:StudyProgramService) {

      this.form = new FormGroup({
        acronym: new FormControl("", [Validators.required, Validators.minLength(3)]),
        name: new FormControl("", [Validators.required, Validators.minLength(3)])
      });
    };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new StudyProgram();
    }

    this.form.setValue({
      acronym: this.obj.acronym,
      name: this.obj.name
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a StudyProgram!");

    this.obj.acronym = this.form.controls.acronym.value;
    this.obj.name = this.form.controls.name.value;

    let objects = await this.service.getAll();
    if (!this.obj.isArconymUnique(objects))
      this.form.controls.acronym.setErrors({'notunique': true});

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(this.obj);
    this.router.navigate(["studyprogram"]);
  }

  onCancel() {
    this.router.navigate(['studyprogram']);
  }
}