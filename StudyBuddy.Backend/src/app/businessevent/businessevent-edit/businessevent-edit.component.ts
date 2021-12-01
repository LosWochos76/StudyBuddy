import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessEvent } from 'src/app/model/businessevent';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { BusinessEventService } from 'src/app/services/businessevent.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-businessevent-edit',
  templateUrl: './businessevent-edit.component.html',
  styleUrls: ['./businessevent-edit.component.css']
})
export class BusinessEventEditComponent implements OnInit {
  id: number = 0;
  obj: BusinessEvent = null;
  form: FormGroup;
  all_users: User[] = [];

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private router: Router,
    private service: BusinessEventService,
    private auth: AuthorizationService) {

    this.form = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.minLength(3)]),
      type: new FormControl(1),
      code: new FormControl("", [Validators.required, Validators.minLength(5)])
    });
  };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new BusinessEvent();
      this.obj.owner = this.auth.getUser().id;
    }

    this.form.setValue({
      name: this.obj.name,
      type: this.obj.type,
      code: this.obj.code
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a BusinessEvent!");
    this.obj.copyValues(this.form.value);

    if (this.form.invalid) {
      this.logger.debug("Data is invalid!");
      return;
    }

    await this.service.save(this.obj);
    this.router.navigate(["businessevent"]);
  }

  onCancel() {
    this.router.navigate(['businessevent']);
  }
}
