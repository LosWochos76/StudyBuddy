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
  all_users:User[] = [];
  members:User[] = [];
  new_member:User = null;
  old_member:User = null;

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:TeamService,
    private user_service:UserService) {

      this.form = new FormGroup({
        name: new FormControl("", [Validators.required, Validators.minLength(3)])
      });
    };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    if (this.id != "0") {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new Team("", "");
    }

    this.all_users = await this.user_service.getAll();

    console.log(this.all_users);

    this.form.setValue({
      name: this.obj.name
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a StudyProgram!");

    this.obj.name = this.form.controls.name.value;

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(this.obj);
    this.router.navigate(["team"]);
  }

  onCancel() {
    this.router.navigate(['team']);
  }

  onNewMemberSelected(id:string) {
    this.new_member = this.all_users.filter(obj => { return obj.id == id; })[0];
  }

  onOldMemberSelected(id:string) {
    this.old_member = this.members.filter(obj => { return obj.id == id; })[0];
  }

  addMember() {
    if (this.new_member == null)
      return;

    this.members.push(this.new_member);
    let index = this.all_users.findIndex(obj => obj.id == this.new_member.id);
    console.log(index);
    this.all_users.splice(index, 1);
    this.new_member = null;
  }

  removeMember() {

  }
}
