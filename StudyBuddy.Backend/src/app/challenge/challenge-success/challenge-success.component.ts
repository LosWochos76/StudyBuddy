import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-success',
  templateUrl: './challenge-success.component.html',
  styleUrls: ['./challenge-success.component.css']
})
export class ChallengeSuccessComponent implements OnInit {
  id: number = 0;
  challenge: Challenge = new Challenge();
  objects: User[] = [];
  selected: User = null;
  current_user: User = null;
  all_users: User[] = [];

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private user_service: UserService,
    private challenge_service: ChallengeService,
    private router: Router,
    private auth: AuthorizationService,) { 
      this.current_user = this.auth.getUser();
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.challenge = await this.challenge_service.byId(this.id);
    await this.loadObjects();
  }

  async loadObjects() {
    this.objects = (await this.user_service.getUsersThatAcceptedChallenge(this.id)).objects;

    if (!this.current_user.isAdmin())
      return;

    this.all_users = [];
    var users = (await this.user_service.getAll()).objects;
    
    for (var user of users) {
      if (!this.hasChallenge(user.id))
        this.all_users.push(user);
    }
  }

  hasChallenge(user_id:number) {
    return (this.objects.find(obj => obj.id == user_id) != undefined);
  }

  onSelect(obj: User) {
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  async onRemove() {
    if (!this.current_user.isAdmin || !this.isSelected())
      return;

    this.logger.debug("User wants to remove the acceptance of a user from a challenge");
    await this.challenge_service.removeAcceptance(this.id, this.selected.id);
    await this.loadObjects();
  }

  goBack() {
    this.router.navigate(['challenge']);
  }

  async onAddUser(user_id:number) {
    await this.challenge_service.addAcceptance(this.id, user_id);
    await this.loadObjects();
  }
}
