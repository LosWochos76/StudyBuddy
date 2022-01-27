import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BusinessEvent } from 'src/app/model/businessevent';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { BusinessEventService } from 'src/app/services/businessevent.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-businessevent-list',
  templateUrl: './businessevent-list.component.html',
  styleUrls: ['./businessevent-list.component.css']
})
export class BusinessEventListComponent implements OnInit {
    page = 1;
    total = 0;
  objects: BusinessEvent[] = [];
  selected: BusinessEvent = null;
  timeout: any = null;
  user: User = null;
  owners_cache = new Map();
  image;

  constructor(
    private logger: LoggingService,
    private service: BusinessEventService,
    private router: Router,
    private auth: AuthorizationService,
    private user_service: UserService) {
    this.user = this.auth.getUser();
  }

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

  onSelect(obj: BusinessEvent) {
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;

    if (!confirm("Wollen Sie das Ereignis wirklich löschen?"))
      return;

    this.logger.debug("User wants to delete a BusinessEvent");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to edit a BusinessEvent");
    this.router.navigate(['/businessevent', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a BusinessEvent");
    this.router.navigate(['/businessevent/0']);
  }

  onExecute() {
    this.service.execute(this.selected.id);
  }
}