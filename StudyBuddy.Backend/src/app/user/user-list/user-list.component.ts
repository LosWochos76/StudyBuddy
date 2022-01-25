import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  objects: User[] = [];
  count: number = 0;
  selected: User = null;
  current_user: User = null;

  constructor(
    private logger: LoggingService,
    private service: UserService,
    private router: Router,
    private auth: AuthorizationService) { }

  async ngOnInit() {
    this.current_user = this.auth.getUser();

    var result = await this.service.getAll();
    this.objects = result.objects;
    this.count = result.count;

    this.service.changed.subscribe(async () => {
      var result = await this.service.getAll();
      this.objects = result.objects;
      this.count = result.count;
    });
  }

  onSelect(obj: User) {
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null && this.selected.id != this.current_user.id;
  }

  onDelete() {
    if (!this.isSelected())
      return;

    if (!confirm("Wollen Sie den Nutzer wirklich löschen?"))
      return;

    this.logger.debug("User wants to delete a User");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    if (this.selected.id == this.current_user.id)
      return;

    this.logger.debug("User wants to edit a User");
    this.router.navigate(['/user', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a User");
    this.router.navigate(['/user/0']);
  }
}
