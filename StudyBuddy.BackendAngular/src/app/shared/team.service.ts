import { EventEmitter, Injectable, Output } from '@angular/core';
import { AngularFireDatabase } from '@angular/fire/database';
import { Team } from '../model/team';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  @Output() changed = new EventEmitter();
  
  constructor(
    private db: AngularFireDatabase,
    private logger: LoggingService) { }

  private fromSingleSnapshot(snapshot):Team {
    let val = snapshot.val();
    let id = Object.keys(val)[0];
    val = val[id];
    let p = new Team(id, val.name);
    p.members = val.members == undefined ? [] : val.members; 
    return p;
  }

  private fromSnapshot(snapshot):Team {
    let val = snapshot.val();
    let p = new Team(snapshot.key, val.name);
    p.members = val.members == undefined ? [] : val.members;
    return p;
  }

  async getCount() {
    let reference = this.db.database.ref("teams");
    let snapshot = await reference.get();
    return snapshot.numChildren();
  }

  async getAll():Promise<Team[]> {
    let result:Team[] = [];
    let reference = this.db.database.ref("teams").orderByChild("name");
    let snapshot = await reference.once("value");

    snapshot.forEach((snapshot) => {
      result.push(this.fromSnapshot(snapshot));
    });

    return result;
  }

  async remove(id:string) {
    this.db.database.ref().child("teams").child(id).remove();
    this.changed.emit();
  }

  async byId(id:string):Promise<Team> {
    let reference = this.db.database.ref().child("teams").child(id);
    let snapshot = await reference.get();

    if (snapshot.exists())
        return this.fromSnapshot(snapshot);

    return null;
  }

  async save(obj:Team) {
    let tmp = { 
      'name': obj.name,
      'members': obj.members
    };

    if (obj.id == "") {
      let ref = this.db.database.ref("teams").push();
			obj.id = ref.key;
			ref.set(tmp);
    } else {
      this.db.database.ref("teams").child(obj.id).set(tmp);
    }

    this.changed.emit();
  }
}
