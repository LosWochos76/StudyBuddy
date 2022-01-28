import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";
import { NotificationService } from "../../services/notification.service";

@Component({
  selector: 'app-notification-broadcast',
  templateUrl: './notification-broadcast.component.html',
  styleUrls: ['./notification-broadcast.component.css']
})
export class NotificationBroadcastComponent implements OnInit {

  form: ReturnType<typeof NotificationBroadcastComponent.prototype.createForm>

  constructor(
    private formBuilder: FormBuilder,
    private notificationService: NotificationService
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
    const result = await this.notificationService.broadcastNotification(title, body)
    console.log('broadcasted Notification', result)
  }
}
