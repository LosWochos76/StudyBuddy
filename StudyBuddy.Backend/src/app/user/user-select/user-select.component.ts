import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

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
  @Input() selected:number[] = [];
  @Input() mode = 'multiple';
  all:User[] = [];
  all_copy:User[] = [];
  selected_objects:User[] = [];
  timeout: any = null;
  onChanged: any = () => { };
  onTouched: any = () => { };
  disabled = false;
  name = "";
  
  constructor(
    private service:UserService) { 
      this.name = "select_user_" + Math.floor(Math.random() * 10000);
    }

  async ngOnInit() {
    var result = await this.service.getAll(1);
    this.all = result.objects;
    this.all_copy = this.all.slice();
  }

  writeValue(ids:number[]) {
    this.selected = [];
    for (let id of ids)
      this.onChange(id);
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

  isSingleMode():boolean {
    return this.mode == "single";
  }

  getName() {
    return this.name;
  }

  onChange(id:number) {
    if (this.isSingleMode()) {
      let index = this.all.findIndex(obj => obj.id == id);
      if (index > -1) {
        this.selected_objects = [this.all[index]];
        this.selected = [id];
        this.onChanged(this.selected);
      }
    }
    else {
      let index = this.selected_objects.findIndex(obj => obj.id == id);
      if (index > -1) {
        this.selected_objects.splice(index, 1);
        this.selected.splice(this.selected.indexOf(id), 1);
        this.onChanged(this.selected);
        return;
      }

      index = this.all.findIndex(obj => obj.id == id);
      if (index > -1) {
        this.selected_objects.push(this.all[index]);
        this.selected.push(id);
        this.onChanged(this.selected);
      }
    }
  }

  onKeySearch(event: any) {
    clearTimeout(this.timeout);
    var $this = this;
    this.timeout = setTimeout(function () {
      if (event.keyCode != 13) {
        $this.onSearch(event.target.value);
      }
    }, 1000);
  }

  private findByText(value:string) {
    let result:User[] = [];

    for (let obj of this.all_copy)
      if (JSON.stringify(obj).search(value) > -1)
        result.push(obj);

    return result;
  }

  isChecked(id:number) {
    return this.selected_objects.findIndex(xyz => xyz.id == id) > -1;
  }

  private async onSearch(value: string) {
    if (value == "")
      this.all = this.all_copy.slice();
    else
      this.all = this.findByText(value);
  }

  selectAll() {
    this.selected = [];
    this.selected_objects = [];
    
    for (let obj of this.all) {
      this.selected.push(obj.id);
      this.selected_objects.push(obj);
    }

    this.onChanged(this.selected);
  }

  selectNone() {
    this.selected_objects = [];
    this.selected = [];
    this.onChanged(this.selected);
  }
}