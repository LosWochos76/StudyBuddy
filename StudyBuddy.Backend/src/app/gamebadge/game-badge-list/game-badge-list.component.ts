import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameBadge } from 'src/app/model/gamebadge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-game-badge-list',
  templateUrl: './game-badge-list.component.html',
  styleUrls: ['./game-badge-list.component.css']
})
export class GameBadgeListComponent implements OnInit {
  page = 1;
  total = 0;
  objects: GameBadge[] = [];
  selected: GameBadge = null;
  user: User = null;

  constructor(
    private logger: LoggingService,
    private navigation: NavigationService,
    private service: GameBadgeService,
    private router: Router,
    private auth: AuthorizationService) {
    this.navigation.startSaveHistory();
    this.user = this.auth.getUser();
  }

  async ngOnInit() {
    var fullList = await this.service.getAll(this.page);
    this.objects = fullList.objects;
    this.total = fullList.count;

    this.service.changed.subscribe(async () => {
      var result = await this.service.getAll(this.page);
      this.objects = result.objects;
      this.total = result.count;
    });

  }
  async onTableDataChange(event) {
    this.page = event;
    var fullList = await this.service.getAll(event);
    this.objects = fullList.objects;
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

  onSuccess() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to see success of GameBadge");
    this.router.navigate(['/gamebadgesuccess/', this.selected.id]);
  }
}