import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Term } from 'src/app/model/term';
import { Helper } from 'src/app/shared/helper';
import { LoggingService } from 'src/app/shared/loging.service';
import { TermService } from 'src/app/shared/term.service';

@Component({
  selector: 'app-term-edit',
  templateUrl: './term-edit.component.html',
  styleUrls: ['./term-edit.component.css']
})
export class TermEditComponent implements OnInit {
  id:string = "";
  obj:Term = null;
  form:FormGroup;

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:TermService) {

      this.form = new FormGroup({
        shortname: new FormControl("", [Validators.required, Validators.minLength(3)]),
        name: new FormControl("", [Validators.required, Validators.minLength(3)]),
        start: new FormControl("", [Validators.required]),
        end: new FormControl("", [Validators.required])
      });
  };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    if (this.id != "0") {
      this.obj = await this.service.byId(this.id);
    } else {
      let today = (new Date()).toISOString().split('T')[0];
      this.obj = new Term("", "", "", today, today);
    }

    this.form.setValue({
      shortname: this.obj.shortname,
      name: this.obj.name,
      start: this.obj.start,
      end: this.obj.end
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a Term!");

    this.obj.shortname = this.form.controls.shortname.value;
    this.obj.name = this.form.controls.name.value;
    this.obj.start = this.form.controls.start.value;
    this.obj.end = this.form.controls.end.value;

    if (!this.obj.isPeriodValid())
      this.form.controls.start.setErrors({'invalidperiod': true});
    else
      this.form.controls.start.setErrors(null);

    let objects = await this.service.getAll();
    if (!this.obj.periodHasNoOverlapsWithOthers(objects))
      this.form.controls.end.setErrors({'periodwithoveraps': true});
    else
      this.form.controls.end.setErrors(null);

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(this.obj);
    this.router.navigate(["term"]);
  }

  onCancel() {
    this.router.navigate(['term']);
  }
}