import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Team } from 'src/app/model/team';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';
import { TeamService } from 'src/app/services/team.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-team-edit',
  templateUrl: './team-edit.component.html',
  styleUrls: ['./team-edit.component.css']
})
export class TeamEditComponent implements OnInit {
  id:number = 0;
  obj:Team = null;
  form:FormGroup;

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:TeamService,
    private user_service:UserService,
    private auth:AuthorizationService) {
      this.form = new FormGroup({
        name: new FormControl("", [Validators.required, Validators.minLength(3)]),
        members: new FormControl([], [Validators.required])
      });
  };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    let members:number[] = [];

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
      members = await this.service.getMembers(this.id);
    } else {
      this.obj = new Team();
      this.obj.owner = this.auth.getUser().id;
      members.push(this.obj.owner);
    }

    this.form.setValue({
      name: this.obj.name,
      members: members
    });
  }

  checkIfOwnerIsMember():boolean {
    let members = this.form.controls.members.value;
    let owner = this.obj.owner;

    for (let pos in members) {
      if (owner == +members[pos]) {
        return true;
      }
    }
    
    return false;
  }

  async onSubmit() {
    this.logger.debug("Trying to save a Team!");

    if (!this.checkIfOwnerIsMember()) {
      this.form.setErrors({'ownerneedstobemember': true});
      return;
    }

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.obj.name = this.form.controls.name.value;
    this.service.save(this.obj);
    this.service.setMembers(this.obj.id, this.form.controls.members.value);
    this.router.navigate(["team"]);
  }

  onCancel() {
    this.router.navigate(['team']);
  }
}