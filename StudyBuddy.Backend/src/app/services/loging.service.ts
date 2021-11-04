import { DatePipe } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LogMessage } from '../model/logmessage';
import { AuthorizationService } from './authorization.service';

@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  private is_debug_mode:boolean = true;
  private datepipe:DatePipe = new DatePipe('en-US')
  private url = environment.api_url;
  
  constructor(
    private auth:AuthorizationService,
    private http:HttpClient) { }

  private currDate() {
    return this.datepipe.transform(new Date(), 'yyyy-MM-dd HH:mm:ss');
  }

  debug(msg:any) {
    console.log(this.currDate() + " DEBUG: " + JSON.stringify(msg));

    let obj = new LogMessage();
    var user = this.auth.getUser();
    if (user != null)
      obj.userId = user.id;

    obj.source = 3;
    obj.level = 1;
    obj.message = msg;
    this.log(obj);
  }

  error(msg:any) {
    console.log(this.currDate() + " ERROR: " + JSON.stringify(msg));

    let obj = new LogMessage();
    var user = this.auth.getUser();
    if (user != null)
      obj.userId = user.id;

    obj.source = 3;
    obj.level = 4;
    obj.message = JSON.stringify(msg);
    this.log(obj);
  }

  private async log(obj:LogMessage) {
    try {
      let data = obj.toApi();
      await this.http.post(this.url + "Logging", data).toPromise();
    } catch { }
  }

  async getAll():Promise<LogMessage[]> {
    if (!this.auth.isLoggedIn())
      return null;

    var query = { };
    let objects:LogMessage[] = [];
    let result = await this.http.get(this.url + "Logging", 
    {
      params: query,
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    for (let index in result)
      objects.push(LogMessage.fromApi(result[index]));

    return objects;
  }

  async flush() {
    let result = await this.http.delete(this.url + "Logging", 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
  }
}
