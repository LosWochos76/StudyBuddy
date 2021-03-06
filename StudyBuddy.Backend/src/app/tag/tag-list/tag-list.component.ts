import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Tag } from 'src/app/model/tag';
import { LoggingService } from 'src/app/services/loging.service';
import { TagService } from 'src/app/services/tag.service';

@Component({
  selector: 'app-tag-list',
  templateUrl: './tag-list.component.html',
  styleUrls: ['./tag-list.component.css']
})
export class TagListComponent implements OnInit {
    page = 1;
    total = 0;
  objects: Tag[] = [];
  selected: Tag = null;

  constructor(
    private logger: LoggingService,
    private service: TagService,
    private router: Router) { }

  async ngOnInit() {
      var fullList = await this.service.getAll(this.page);
      this.objects = fullList.objects;
      this.total = fullList.count;
      this.service.changed.subscribe(async () => {
          var result = await this.service.getAll(this.page)
          this.objects = result.objects;
          this.total = result.count;

    });
    }
    async onTableDataChange(event) {
        this.page = event;
        var fullList = await this.service.getAll(event);
        this.objects = fullList.objects;
    }

  onSelect(obj: Tag) {
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;

    if (!confirm("Wollen Sie den Tag wirklich löschen?"))
      return;

    this.logger.debug("User wants to delete a Tag");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    this.logger.debug("User wants to edit a Tag");
    this.router.navigate(['/tag', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a Term");
    this.router.navigate(['/tag/0']);
  }
}
