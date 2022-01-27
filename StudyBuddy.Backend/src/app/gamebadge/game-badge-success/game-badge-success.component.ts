import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameBadge } from 'src/app/model/gamebadge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-game-badge-success',
  templateUrl: './game-badge-success.component.html',
  styleUrls: ['./game-badge-success.component.css']
})
export class GameBadgeSuccessComponent implements OnInit {
  id: number = 0;
  badge: GameBadge = new GameBadge();
  objects: User[] = [];
  selected: User = null;
  current_user: User = null;
  all_users: User[] = [];

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private user_service: UserService,
    private badge_service: GameBadgeService,
    private router: Router,
    private auth: AuthorizationService,) { 
      this.current_user = this.auth.getUser();
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.badge = await this.badge_service.byId(this.id);

    await this.loadObjects();
  }

  async loadObjects() {
    var result = await this.user_service.getUsersHavingBadge(this.id);
    this.objects = result.objects;

    if (!this.current_user.isAdmin())
      return;

    this.all_users = [];
    var users = await this.user_service.getAll(1);
    
    for (var user of users.objects) {
      if (!this.hasBadge(user.id))
        this.all_users.push(user);
    }
  }

  hasBadge(user_id:number) {
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

    this.logger.debug("User wants to remove a user from a badge");
    await this.badge_service.removeUser(this.id, this.selected.id);
    await this.loadObjects();
  }

  goBack() {
    this.router.navigate(['gamebadge']);
  }

  async onAddUser(user_id:number) {
    await this.user_service.addBadgeToUser(user_id, this.id);
    await this.loadObjects();
  }
}