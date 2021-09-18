import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Tag } from 'src/app/model/tag';
import { LoggingService } from 'src/app/services/loging.service';
import { TagService } from 'src/app/services/tag.service';

@Component({
  selector: 'app-tag-edit',
  templateUrl: './tag-edit.component.html',
  styleUrls: ['./tag-edit.component.css']
})
export class TagEditComponent implements OnInit {
  id:number = 0;
  obj:Tag = null;
  form:FormGroup;

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:TagService) {

      this.form = new FormGroup({
        name: new FormControl("", [Validators.required, Validators.minLength(3)])
      });
  };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new Tag();
    }

    this.form.setValue({
      name: this.obj.name
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a Tag!");

    this.obj.name = this.form.controls.name.value;

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(this.obj);
    this.router.navigate(["tag"]);
  }

  onCancel() {
    this.router.navigate(['tag']);
  }
}
