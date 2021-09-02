import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { StudyProgram } from 'src/app/model/studyprogram';
import { Term } from 'src/app/model/term';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { StudyProgramService } from 'src/app/services/study-program.service';
import { TermService } from 'src/app/services/term.service';

@Component({
  selector: 'app-challenge-edit',
  templateUrl: './challenge-edit.component.html',
  styleUrls: ['./challenge-edit.component.css']
})
export class ChallengeEditComponent implements OnInit {
  id:number = 0;
  obj:Challenge = null;
  form:FormGroup;
  study_programs:StudyProgram[] = [];
  terms:Term[] = [];

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:ChallengeService,
    private auth:AuthorizationService,
    private studyprogram_service:StudyProgramService,
    private term_service:TermService) {

      this.form = new FormGroup({
        name: new FormControl("", [Validators.required, Validators.minLength(3)]),
        description: new FormControl(""),
        points: new FormControl(0, [Validators.required, Validators.pattern("^[0-9]*$")]),
        validity_start: new FormControl("", [Validators.required]),
        validity_end: new FormControl("", [Validators.required]),
        category: new FormControl("", [Validators.required]),
        prove: new FormControl("", [Validators.required]),
        study_program: new FormControl(""),
        enrolled_since: new FormControl("")
      });
  };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new Challenge();
      this.obj.owner = this.auth.getUser().id;
    }

    this.study_programs = await this.studyprogram_service.getAll();
    this.terms = await this.term_service.getAll();

    this.form.setValue({
      name: this.obj.name,
      description: this.obj.description,
      points: this.obj.points,
      validity_start: this.obj.validity_start,
      validity_end: this.obj.validity_end,
      category: this.obj.category,
      prove: this.obj.prove,
      study_program: this.obj.study_program,
      enrolled_since: this.obj.enrolled_since
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a Challenge!");

    this.obj.copyValues(this.form.value);

    if (!this.obj.isPeriodValid())
      this.form.controls.validity_end.setErrors({'invalidperiod': true});
    else
      this.form.controls.validity_end.setErrors(null);

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(this.obj);
    this.router.navigate(["challenge"]);
  }

  onCancel() {
    this.router.navigate(['challenge']);
  }
}
