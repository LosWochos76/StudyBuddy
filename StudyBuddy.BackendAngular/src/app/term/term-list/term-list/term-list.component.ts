import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Term } from 'src/app/model/term';
import { LoggingService } from 'src/app/shared/loging.service';
import { TermService } from 'src/app/shared/term.service';

@Component({
  selector: 'app-term-list',
  templateUrl: './term-list.component.html',
  styleUrls: ['./term-list.component.css']
})
export class TermListComponent implements OnInit {
  objects:Term[] = [];
  selected:Term = null;

  constructor(
    private logger:LoggingService, 
    private service:TermService,
    private router:Router) { }

  async ngOnInit() {
    this.objects = await this.service.getAll();
    this.service.changed.subscribe(async () => {
      this.objects = await this.service.getAll();
    });
  }
  
  onSelect(obj:Term) {
    this.logger.debug("User selected a Term");
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;
    
    this.logger.debug("User wants to delete a Term");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    this.logger.debug("User wants to edit a Term");
    this.router.navigate(['/term', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a Term");
    this.router.navigate(['/term/0']);
  }
}
