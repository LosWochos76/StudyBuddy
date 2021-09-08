import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Team } from 'src/app/model/team';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';
import { TeamService } from 'src/app/services/team.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit {
  objects: Team[] = [];
  selected: Team = null;
  count = 0;
  user: User = null;
  owners_cache = new Map();

  constructor(
    private logger: LoggingService,
    private service: TeamService,
    private router: Router,
    private auth: AuthorizationService,
    private user_service: UserService) {
    this.user = this.auth.getUser();
  }

  async ngOnInit() {
    this.count = await this.service.getCount();
    this.objects = await this.service.getAll();

    this.service.changed.subscribe(async () => {
      this.count = await this.service.getCount();
      this.objects = await this.service.getAll();
    });

    if (this.user.isAdmin())
      this.loadOwners();
  }

  onSelect(obj: Team) {
    this.logger.debug("User selected a Team");
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to delete a Team");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    this.logger.debug("User wants to edit a Team");
    this.router.navigate(['/team', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a Team");
    this.router.navigate(['/team/0']);
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
}