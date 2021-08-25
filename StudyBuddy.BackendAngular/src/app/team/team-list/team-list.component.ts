import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Team } from 'src/app/model/team';
import { LoggingService } from 'src/app/shared/loging.service';
import { TeamService } from 'src/app/shared/team.service';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit {
  objects:Team[] = [];
  selected:Team = null;
  count = 0;

  constructor(
    private logger:LoggingService, 
    private service:TeamService,
    private router:Router) { }

  async ngOnInit() {
    this.count = await this.service.getCount();
    this.objects = await this.service.getAll();
    
    this.service.changed.subscribe(async () => {
      this.count = await this.service.getCount();
      this.objects = await this.service.getAll();
    });
  }
  
  onSelect(obj:Team) {
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

}
