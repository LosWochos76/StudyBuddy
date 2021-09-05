import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { StudyProgram } from '../model/studyprogram';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class StudyProgramService {
  private url = environment.api_url;
  @Output() changed = new EventEmitter();

  constructor(
    private logger: LoggingService,
    private http: HttpClient,
    private auth: AuthorizationService) { }

  async getAll():Promise<StudyProgram[]> {
    let objects:StudyProgram[] = [];

    this.logger.debug("Getting all StudyPrograms");
    let result = await this.http.get(this.url + "StudyProgram").toPromise();

    for (let obj in result)
      objects.push(StudyProgram.fromApi(result[obj]));

    return objects;
  }

  async remove(id:number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "StudyProgram/" + id;
    this.logger.debug("Removing StudyProgram from " + path);
    let result = await this.http.delete(path, 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
    
    this.changed.emit();
  }

  async byId(id:number):Promise<StudyProgram> {
    let path = this.url + "StudyProgram/" + id;
    this.logger.debug("Loading StudyProgram from " + path);
    let result = await this.http.get(path).toPromise();

    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return StudyProgram.fromApi(result);
  }

  async save(obj:StudyProgram) {
    if (!this.auth.isLoggedIn())
      return null;
    
    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new StudyProgram");
      result = await this.http.post(this.url + "StudyProgram", data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
    } else {
      this.logger.debug("Saving existing StudyProgram");
      result = await this.http.put(this.url + "StudyProgram/" + obj.id, data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
    }
    
    this.changed.emit();
  }
}