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

    var query = { };
    if (this.auth.getUser().isTeacher())
      query['OwnerId'] = this.auth.getUser().id;

    let objects:Challenge[] = [];
    let result = await this.http.get(this.url + "Challenge", 
    {
      params: query,
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

  async byText(text:string):Promise<Challenge[]> {
    if (!this.auth.isLoggedIn())
      return null;

    var query = { 'SearchText': text };
    if (this.auth.getUser().isTeacher())
      query['OwnerId'] = this.auth.getUser().id;

    let objects:Challenge[] = [];
    let result = await this.http.get(this.url + "Challenge", 
    {
      params: query,
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    for (let obj in result)
      objects.push(Challenge.fromApi(result[obj]));

    return objects;
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

  async createSeries(id:number, days_add:number, count:number) {
    if (!this.auth.isLoggedIn())
      return null;
    
    let data = { 'challengeId':id, 'daysAdd':days_add, 'count':count };
    let result = null;
    this.logger.debug("Creating series of Challenges");
    result = await this.http.post(this.url + "Challenge/CreateSeries/", data, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    this.changed.emit();
  }

  async ofBadge(id:number):Promise<number[]> {
    let ids:number[] = [];

    this.logger.debug("Loading Challenges of Badge + " + id);
    let result = await this.http.get(this.url + "Challenge/ofBadge/" + id,
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    for (let index in result) {
      ids.push(result[index].id);
    }

    return ids;
  }

  async getQrCode(id:number):Promise<Blob> {
    this.logger.debug("Loading QR-Code of Challenge " + id);

    let result = await this.http.get(this.url + "Challenge/" + id  + "/QrCode/",
    {
      responseType: 'blob',
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    return result;
  }

  async removeAcceptance(challenge_id:number, user_id:number) {
    this.logger.debug("Removeing challenge acceptance");

    let result = await this.http.put(this.url + "Challenge/" + challenge_id + "/RemoveAcceptance/" + user_id, "", 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    console.log(result);
  }
}
