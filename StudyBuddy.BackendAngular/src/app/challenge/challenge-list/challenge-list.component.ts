import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { ChallengeService } from 'src/app/shared/challenge.service';
import { LoggingService } from 'src/app/shared/loging.service';

@Component({
  selector: 'app-challenge-list',
  templateUrl: './challenge-list.component.html',
  styleUrls: ['./challenge-list.component.css']
})
export class ChallengeListComponent implements OnInit {
  objects:Challenge[] = [];
  selected:Challenge = null;
  timeout: any = null;

  constructor(
    private logger:LoggingService, 
    private service:ChallengeService,
    private router:Router) { }

  async ngOnInit() {
    this.objects = await this.service.getAll();
    this.service.changed.subscribe(async () => {
      this.objects = await this.service.getAll();
    });
  }
  
  onSelect(obj:Challenge) {
    this.logger.debug("User selected a Challenge");
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  isCloneable() {
    return this.selected != null && this.selected.parent == "";
  }

  onDelete() {
    if (!this.isSelected())
      return;
    
    this.logger.debug("User wants to delete a Challenge");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    this.logger.debug("User wants to edit a Challenge");
    this.router.navigate(['/challenge', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a Challenge");
    this.router.navigate(['/challenge/0']);
  }

  // Todo: Implementieren
  onClone() {
  }

  onKeySearch(event: any) {
    clearTimeout(this.timeout);
    var $this = this;
    this.timeout = setTimeout(function () {
      if (event.keyCode != 13) {
        $this.onSearch(event.target.value);
      }
    }, 1000);
  }

  private async onSearch(value: string) {
    this.objects = await this.service.byText(value);
  }
}
