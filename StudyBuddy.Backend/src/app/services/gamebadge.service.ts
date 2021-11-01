import { query } from '@angular/animations';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GameBadge } from '../model/gamebadge';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class GameBadgeService {
  @Output() changed = new EventEmitter();
  private url = environment.api_url;
  
  constructor(
    private logger:LoggingService,
    private http:HttpClient,
    private auth:AuthorizationService) { }

  async getCount():Promise<number> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "GameBadge/Count";
    this.logger.debug("Getting count of GameBadge");
    let result = await this.http.get(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    return +result;
  }

  async getAll():Promise<GameBadge[]> {
    if (!this.auth.isLoggedIn())
      return null;

    var query = { };
    if (this.auth.getUser().isTeacher)
      query['OwnerId'] = this.auth.getUser().id;

    let objects:GameBadge[] = [];
    this.logger.debug("Getting all GameBadge");
    let result = await this.http.get(this.url + "GameBadge", 
    {
      params: query,
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    for (let obj in result)
      objects.push(GameBadge.fromApi(result[obj]));

    return objects;
  }

  async remove(id:number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "GameBadge/" + id;
    this.logger.debug("Removing GameBadge from " + path);
    let result = await this.http.delete(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    this.changed.emit();
  }

  async byId(id:number):Promise<GameBadge> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "GameBadge/" + id;
    this.logger.debug("Loading GameBadge from " + path);
    let result = await this.http.get(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return GameBadge.fromApi(result);
  }

  async save(obj:GameBadge) {
    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new GameBadge");
      result = await this.http.post(this.url + "GameBadge", data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

      obj.id = result["id"];
    } else {
      this.logger.debug("Saving existing GameBadge");
      result = await this.http.put(this.url + "GameBadge/" + obj.id, data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
    }
    
    this.changed.emit();
  }
}