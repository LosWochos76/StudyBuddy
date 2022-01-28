import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Tag } from '../model/tag';
import { TagList } from '../model/taglist';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private url = environment.api_url;
  @Output() changed = new EventEmitter();

  constructor(
    private logger: LoggingService,
    private http: HttpClient,
    private auth: AuthorizationService) { }

  async getAll(page: number = -1): Promise<TagList> {
    var query = {};

    if (page != -1) {
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    }

    let result = await this.http.get(this.url + "Tag", {
      params: query,
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    return new TagList(result);
  }

  async remove(id: number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "Tag/" + id;
    this.logger.debug("Removing Tag from " + path);
    let result = await this.http.delete(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    this.changed.emit();
  }

  async byId(id: number): Promise<Tag> {
    let path = this.url + "Tag/" + id;
    this.logger.debug("Loading Tag from " + path);
    let result = await this.http.get(this.url + "Tag/" + id,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return Tag.fromApi(result);
  }

  async save(obj: Tag) {
    if (!this.auth.isLoggedIn())
      return null;

    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new Tag");
      result = await this.http.post(this.url + "Tag", data,
        {
          headers: new HttpHeaders({ Authorization: this.auth.getToken() })
        }).toPromise();

      obj.id = result["id"];
    } else {
      this.logger.debug("Saving existing Tag");
      result = await this.http.put(this.url + "Tag/" + obj.id, data,
        {
          headers: new HttpHeaders({ Authorization: this.auth.getToken() })
        }).toPromise();
    }

    this.changed.emit();
  }

  async CreateOrFind(tags: string): Promise<TagList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Creating or finding tags");
    let result = await this.http.post(this.url + "Tag/CreateOrFind/", tags,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return new TagList(result);
  }

  async setForChallenge(challenge_id: number, tag_string: string) {
    let data = {
      'ChallengeId': challenge_id,
      'Tags': tag_string
    };

    this.http.post(this.url + "Tag/Challenge/", data, {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    this.changed.emit();
  }

  async setForBadge(badge_id: number, tag_string: string) {
    let data = {
      'BadgeId': badge_id,
      'Tags': tag_string
    };

    this.http.post(this.url + "Tag/Badge/", data, {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    this.changed.emit();
  }
}