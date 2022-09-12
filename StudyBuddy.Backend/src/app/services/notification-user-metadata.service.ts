import { EventEmitter, Injectable, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { LoggingService } from './loging.service';
import { AuthorizationService } from './authorization.service';
import { NotificationUserMetadataList } from '../model/notification-user-metadata.list';

@Injectable({
  providedIn: 'root'
})
export class NotificationUserMetadataService {
  @Output() changed = new EventEmitter();
  private url = environment.api_url;

  constructor(
    private http: HttpClient,
    private logger: LoggingService,
    private auth: AuthorizationService,
  ) { }

  async getAll(notificationId: number, page: number = -1): Promise<NotificationUserMetadataList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Loading all notifications");

    var query = { notificationId };
    if (page != -1) {
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    }

    let result = await this.http.get(this.url + "NotificationUserMetadata",
      {
        params: query,
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();


    return NotificationUserMetadataList.fromResult(result);
  }

  async remove(id: number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "NotificationUserMetadata/" + id;
    this.logger.debug("Removing NotificationUserMetadata from " + path);
    let result = await this.http.delete(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    this.changed.emit();
  }

}
