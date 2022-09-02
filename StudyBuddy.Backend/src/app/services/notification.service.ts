import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Notification } from '../model/notification';
import { NotificationList } from '../model/notification.list';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  @Output() changed = new EventEmitter();
  private url = environment.api_url;

  constructor(
    private logger: LoggingService,
    private auth: AuthorizationService,
    private http: HttpClient) { }

  async getAll(page: number = -1): Promise<NotificationList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Loading all notifications");

    var query = {};
    if (page != -1) {
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    }

    let result = await this.http.get(this.url + "v2/Notification",
      {
        params: query,
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return NotificationList.fromResult(result);
  }

  async byId(id: number): Promise<Notification> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "Notification/" + id;
    this.logger.debug("Loading Notification from " + path);
    let result = await this.http.get(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return Notification.fromApi(result);
  }

  async remove(id: number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "Notification/" + id;
    this.logger.debug("Removing Notification from " + path);
    let result = await this.http.delete(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    this.changed.emit();
  }

  async save(obj: Notification) {
    if (!this.auth.isLoggedIn())
      return null;

    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new Notification");
      result = await this.http.post(this.url + "Notification", data,
        {
          headers: new HttpHeaders({ Authorization: this.auth.getToken() })
        }).toPromise();

      obj.id = result["id"];
    } else {
      this.logger.debug("Saving existing Notification");
      result = await this.http.put(this.url + "Notification/" + obj.id, data,
        {
          headers: new HttpHeaders({ Authorization: this.auth.getToken() })
        }).toPromise();
    }

    this.changed.emit();
  }
}