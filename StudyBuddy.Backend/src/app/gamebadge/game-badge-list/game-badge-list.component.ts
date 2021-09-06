import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GameBadge } from 'src/app/model/gamebadge';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
  selector: 'app-game-badge-list',
  templateUrl: './game-badge-list.component.html',
  styleUrls: ['./game-badge-list.component.css']
})
export class GameBadgeListComponent implements OnInit {
  objects:GameBadge[] = [];
  selected:GameBadge = null;

  constructor(
    private logger:LoggingService, 
    private service:GameBadgeService,
    private router:Router) { }

  async ngOnInit() {
    this.objects = await this.service.getAll();
    this.service.changed.subscribe(async () => {
      this.objects = await this.service.getAll();
    });
  }

  onSelect(obj:GameBadge) {
    this.logger.debug("User selected a GameBadge");
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;

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
}