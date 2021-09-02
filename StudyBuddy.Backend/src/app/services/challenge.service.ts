import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Challenge } from '../model/challenge';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class ChallengeService {
  @Output() changed = new EventEmitter();
  private url = environment.api_url;
  
  constructor(
    private logger:LoggingService,
    private auth:AuthorizationService,
    private http:HttpClient) { }

  async getAll():Promise<Challenge[]> {
    if (!this.auth.isLoggedIn())
      return null;

    let objects:Challenge[] = [];

    let result = await this.http.get(this.url + "Challenge", 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    for (let obj in result)
      objects.push(Challenge.fromApi(result[obj]));

    return objects;
  }

  async getCount() {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "Challenge";
    this.logger.debug("Getting count of Challenges");
    let result = await this.http.head(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    return result;
  }

  async byId(id:number):Promise<Challenge> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "Challenge/" + id;
    this.logger.debug("Loading Challenge from " + path);
    let result = await this.http.get(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return Challenge.fromApi(result);
  }

  async remove(id:number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "Challenge/" + id;
    this.logger.debug("Removing Challenge from " + path);
    let result = await this.http.delete(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    this.changed.emit();
  }

  // ToDo: Implement as server-call!
  async byText(text:string):Promise<Challenge[]> {
    let result:Challenge[] = [];
    let objects = await this.getAll();

    for (let obj of objects)
      if (obj.name.includes(text) || obj.description.includes(text))
        result.push(obj);

    return result;
  }

  async save(obj:Challenge) {
    if (!this.auth.isLoggedIn())
      return null;
    
    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new Challenge");
      result = await this.http.post(this.url + "Challenge", data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

      obj.id = result["id"];
    } else {
      this.logger.debug("Saving existing Challenge");
      result = await this.http.put(this.url + "Challenge/" + obj.id, data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
    }
    
    this.changed.emit();
  }
}
