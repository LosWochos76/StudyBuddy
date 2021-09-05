import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Team } from '../model/team';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  @Output() changed = new EventEmitter();
  private url = environment.api_url;
  
  constructor(
    private logger:LoggingService,
    private http:HttpClient,
    private auth:AuthorizationService) { }

  async getCount():Promise<number> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "Team/Count";
    this.logger.debug("Getting count of Teams");
    let result = await this.http.get(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    return +result;
  }

  async getAll():Promise<Team[]> {
    if (!this.auth.isLoggedIn())
      return null;

    let objects:Team[] = [];
    this.logger.debug("Getting all Teams");
    let result = await this.http.get(this.url + "Team", 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    for (let obj in result)
      objects.push(Team.fromApi(result[obj]));

    return objects;
  }

  async remove(id:number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "Team/" + id;
    this.logger.debug("Removing Team from " + path);
    let result = await this.http.delete(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    this.changed.emit();
  }

  async byId(id:number):Promise<Team> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "Team/" + id;
    this.logger.debug("Loading Team from " + path);
    let result = await this.http.get(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return Team.fromApi(result);
  }

  async save(obj:Team) {
    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new Team");
      result = await this.http.post(this.url + "Team", data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

      obj.id = result["id"];
    } else {
      this.logger.debug("Saving existing Team");
      result = await this.http.put(this.url + "Team/" + obj.id, data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
    }
    
    this.changed.emit();
  }

  async getMembers(id:number):Promise<number[]> {
    if (!this.auth.isLoggedIn())
      return null;

    let ids:number[] = [];
    if (id == 0)
      return ids;

    this.logger.debug("Getting Members of Team " + id);
    let result = await this.http.get(this.url + "Team/Members/" + id, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    for (let index in result)
      ids.push(result[index].memberId);

    return ids;
  }

  async setMembers(id:number, members:number[]) {
    this.logger.debug("Saving members of Team " + id);

    let data = [];
    for (let pos in members)
      data.push({ "TeamId": id, "MemberId": +members[pos]});

    let result = await this.http.post(this.url + "Team/Members/" + id, data, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
  }
}