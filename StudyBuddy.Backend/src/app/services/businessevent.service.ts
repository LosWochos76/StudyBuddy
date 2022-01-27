import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { BusinessEvent } from '../model/businessevent';
import { BusinessEventList } from '../model/businesseventlist';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class BusinessEventService {
  @Output() changed = new EventEmitter();
  private url = environment.api_url;
  
  constructor(
    private logger:LoggingService,
    private auth:AuthorizationService,
    private http:HttpClient) { }

    async getAll(page: number): Promise<BusinessEventList> {
    if (!this.auth.isLoggedIn())
      return null;

      var query = {};
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    if (this.auth.getUser().isTeacher())
      query['OwnerId'] = this.auth.getUser().id;

    let result = await this.http.get(this.url + "BusinessEvent", 
    {
      params: query,
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    return new BusinessEventList(result);
  }

  async byId(id:number):Promise<BusinessEvent> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "BusinessEvent/" + id;
    this.logger.debug("Loading BusinessEvent from " + path);
    let result = await this.http.get(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return BusinessEvent.fromApi(result);
  }

  async remove(id:number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "BusinessEvent/" + id;
    this.logger.debug("Removing BusinessEvent from " + path);
    let result = await this.http.delete(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    this.changed.emit();
  }

  async save(obj:BusinessEvent) {
    if (!this.auth.isLoggedIn())
      return null;
    
    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new BusinessEvent");
      result = await this.http.post(this.url + "BusinessEvent", data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

      obj.id = result["id"];
    } else {
      this.logger.debug("Saving existing BusinessEvent");
      result = await this.http.put(this.url + "BusinessEvent/" + obj.id, data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
    }
    
    this.changed.emit();
  }

  async execute(id:number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "BusinessEvent/" + id;
    this.logger.debug("Executing BusinessEvent from " + path);
    let result = await this.http.head(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
  }
}