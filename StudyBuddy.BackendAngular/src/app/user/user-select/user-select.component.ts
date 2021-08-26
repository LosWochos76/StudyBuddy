import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-user-select',
  templateUrl: './user-select.component.html',
  styleUrls: ['./user-select.component.css'],
  providers: [
    {
       provide: NG_VALUE_ACCESSOR,
       useExisting: forwardRef(() => UserSelectComponent),
       multi: true
    }
 ]
})
export class UserSelectComponent implements OnInit, ControlValueAccessor {
  @Input() selected:string[] = [];
  all:User[] = [];
  selected_objects:User[] = [];
  onChanged: any = () => { };
  onTouched: any = () => { };
  disabled = false;
  all_selected = null;
  selected_selected = null;

  constructor(
    private service:UserService,
    private library:FaIconLibrary) { 
      library.addIcons(faArrowLeft, faArrowRight)
    }

  writeValue(ids:[]) {
    for (let id of ids)
      this.selected.push(id);
  }

  registerOnChange(fn: any): void {
    this.onChanged = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  async ngOnInit() {
    this.all = await this.service.getAll();
    for (let id of this.selected)
      this.add(id, false);
  }

  onAllSelected(id:string) {
    this.all_selected = id;
  }

  onSelectedSelected(id:string) {
    this.selected_selected = id;
  }

  add(id:string, add_to_selected = true) {
    if (id == null || id == "")
      return;
    
    if (add_to_selected)
      this.selected.push(id);

    let index = this.all.findIndex(x => x.id == id);
    this.selected_objects.push(this.all[index]);
    this.all.splice(index, 1);
    this.all_selected = null;
    this.selected_selected = null;

    this.onChanged(this.selected);
  }

  remove(id:string) {
    if (id == null || id == "")
      return;

    let index = this.selected.findIndex(x => x == id);
    this.all.push(this.selected_objects[index]);
    this.selected.splice(index, 1);
    this.selected_objects.splice(index, 1);
    this.all_selected = null;
    this.selected_selected = null;

    this.onChanged(this.selected);
  }
}