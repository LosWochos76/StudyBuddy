import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";
import { BroadcastService } from "../../services/broadcast.service";

@Component({
  selector: 'app-broadcast',
  templateUrl: './broadcast.component.html',
  styleUrls: ['./broadcast.component.css']
})
export class BroadcastComponent implements OnInit {

  form: ReturnType<typeof BroadcastComponent.prototype.createForm>

  constructor(
    private formBuilder: FormBuilder,
    private notificationService: BroadcastService
  ) {
    this.form = this.createForm()
  }

  ngOnInit(): void {
  }

  createForm() {
    return this.formBuilder.group({
      title: ['', Validators.required],
      body: ['', Validators.required]
    })
  }

  async handleSubmit() {
    const { title, body } = this.form.value
    const result = await this.notificationService.sendBroadcast(title, body)
  }
}
