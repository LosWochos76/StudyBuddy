import { EventEmitter, Injectable, Output } from '@angular/core';
import { AngularFireDatabase } from '@angular/fire/database';
import { StudyProgram } from '../model/studyprogram';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class StudyProgramService {
  @Output() changed = new EventEmitter();

  constructor(
    private db: AngularFireDatabase,
    private logger: LoggingService) { }

  async getAll():Promise<StudyProgram[]> {
    let result:StudyProgram[] = [];
    let reference = this.db.database.ref("studyprograms").orderByChild("acronym");
    let snapshot = await reference.once("value");

    snapshot.forEach((snapshot) => {
      let val = snapshot.val();
      let p = new StudyProgram(snapshot.key, val.acronym, val.name);
      result.push(p);
    });

    return result;
  }

  async remove(id:string) {
    this.db.database.ref().child("studyprograms").child(id).remove();
    this.changed.emit();
  }

  async byId(id:string):Promise<StudyProgram> {
    let reference = this.db.database.ref().child("studyprograms").child(id);
    let snapshot = await reference.get();

    if (snapshot.exists()) {
        let val = snapshot.val();
        return new StudyProgram(snapshot.key, val.acronym, val.name);
    }

    return null;
  }

  async save(obj:StudyProgram) {
    let tmp = { 
      'acronym': obj.acronym, 
      'name': obj.name
    };

    if (obj.id == "") {
      let ref = this.db.database.ref("studyprograms").push();
			obj.id = ref.key;
			ref.set(tmp);
    } else {
      this.db.database.ref("studyprograms").child(obj.id).set(tmp);
    }

    this.changed.emit();
  }
}
