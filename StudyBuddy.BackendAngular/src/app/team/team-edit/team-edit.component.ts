import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Team } from 'src/app/model/team';
import { User } from 'src/app/model/user';
import { LoggingService } from 'src/app/shared/loging.service';
import { TeamService } from 'src/app/shared/team.service';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-team-edit',
  templateUrl: './team-edit.component.html',
  styleUrls: ['./team-edit.component.css']
})
export class TeamEditComponent implements OnInit {
  id:string = "";
  obj:Team = null;
  form:FormGroup;

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:TeamService,
    private user_service:UserService) {

      this.form = new FormGroup({
        name: new FormControl("", [Validators.required, Validators.minLength(3)]),
        members: new FormControl([], [Validators.required])
      });
    };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    if (this.id != "0") {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new Team("", "");
    }

    this.form.setValue({
      name: this.obj.name,
      members: this.obj.members
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a Team!");

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.obj.name = this.form.controls.name.value;
    this.obj.members = this.form.controls.members.value;

    this.service.save(this.obj);
    this.router.navigate(["team"]);
  }

  onCancel() {
    this.router.navigate(['team']);
  }
}
