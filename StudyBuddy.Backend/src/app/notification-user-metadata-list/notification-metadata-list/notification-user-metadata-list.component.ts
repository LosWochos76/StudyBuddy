import { Component, OnInit } from '@angular/core';
import { NotificationUserMetadataService } from '../../services/notification-user-metadata.service';
import { ActivatedRoute } from '@angular/router';
import { NotificationUserMetadata } from '../../model/notification-user-metadata';

@Component({
  selector: 'app-notification-metadata-list',
  templateUrl: './notification-user-metadata-list.component.html',
  styleUrls: ['./notification-user-metadata-list.component.css']
})
export class NotificationUserMetadataListComponent implements OnInit {
  page = 1;
  total = 0;
  objects: NotificationUserMetadata[] = [];
  selected: NotificationUserMetadata = null;
  user_cache = new Map();

  constructor(
    private notificationUserMetadataService: NotificationUserMetadataService,
    private activatedRoute: ActivatedRoute
  ) {
    const notificationId = activatedRoute.snapshot.params.id;
    notificationUserMetadataService.getAll(notificationId, this.page).then( response => {
      this.objects = response.objects;
      this.total = response.count;
    })
  }

  ngOnInit(): void { }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;

    this.notificationUserMetadataService.remove(this.selected.id);
    this.objects.splice(this.objects.indexOf(this.selected), 1)
    this.selected = null;
  }

  onSelect(obj: NotificationUserMetadata) {
    this.selected = obj;
  }
  async onTableDataChange(event) {
    this.page = event;
    var fullList = await this.notificationUserMetadataService.getAll(event);
    this.objects = fullList.objects;
  }

}
