import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Term } from '../model/term';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class TermService {
  private url = environment.api_url;
  @Output() changed = new EventEmitter();

  constructor(
    private logger:LoggingService,
    private http:HttpClient,
    private auth:AuthorizationService) { }

  async getAll():Promise<Term[]> {
    let objects:Term[] = [];
    let result = await this.http.get(this.url + "Term").toPromise();

    for (let obj in result)
      objects.push(Term.fromApi(result[obj]));

    return objects;
  }

  async remove(id:number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "Term/" + id;
    this.logger.debug("Removing Term from " + path);
    let result = await this.http.delete(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    this.changed.emit();
  }

  async byId(id:number):Promise<Term> {
    let path = this.url + "Term/" + id;
    this.logger.debug("Loading Term from " + path);
    let result = await this.http.get(path).toPromise();
    
    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return Term.fromApi(result);
  }

  async save(obj:Term) {
    if (!this.auth.isLoggedIn())
      return null;
    
    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new Term");
      result = await this.http.post(this.url + "Term", data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

      obj.id = result["id"];
    } else {
      this.logger.debug("Saving existing Term");
      result = await this.http.put(this.url + "Term/" + obj.id, data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
    }
    
    this.changed.emit();
  }
}