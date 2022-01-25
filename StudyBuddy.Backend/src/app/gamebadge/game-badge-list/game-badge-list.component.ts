import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameBadge } from 'src/app/model/gamebadge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-game-badge-list',
  templateUrl: './game-badge-list.component.html',
  styleUrls: ['./game-badge-list.component.css']
})
export class GameBadgeListComponent implements OnInit {
  count: number;
  objects: GameBadge[] = [];
  selected: GameBadge = null;
  user: User = null;
  owners_cache = new Map();

  constructor(
    private logger: LoggingService,
    private service: GameBadgeService,
    private router: Router,
    private auth: AuthorizationService,
    private user_service: UserService) {
    this.user = this.auth.getUser();
  }

  async ngOnInit() {
    var result = await this.service.getAll();
    this.objects = result.objects;
    this.count = result.count;

    this.service.changed.subscribe(async () => {
      var result = await this.service.getAll();
      this.objects = result.objects;
      this.count = result.count;
    });

    if (this.user.isAdmin())
      this.loadOwners();
  }

  onSelect(obj: GameBadge) {
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;

    if (!confirm("Wollen Sie das Abzeichen wirklich löschen?"))
      return

    this.logger.debug("User wants to delete a GameBadge");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to edit a GameBadge");
    this.router.navigate(['/gamebadge', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a GameBadge");
    this.router.navigate(['/gamebadge/0']);
  }

  private async loadOwners() {
    for (let obj of this.objects) {
      this.owners_cache.set(obj.owner, await this.user_service.byId(obj.owner));
    }
  }

  getOwnerName(id: number) {
    if (this.owners_cache.has(id)) {
      let owner = this.owners_cache.get(id);
      if (owner != null)
        return owner.fullName();
    }

    return "";
  }

  onSuccess() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to see success of GameBadge");
    this.router.navigate(['/gamebadgesuccess/', this.selected.id]);
  }
}