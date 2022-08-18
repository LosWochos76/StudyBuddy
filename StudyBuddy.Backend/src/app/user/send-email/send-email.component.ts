import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserList } from 'src/app/model/userlist';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-send-email',
  templateUrl: './send-email.component.html',
  styleUrls: ['./send-email.component.css']
})
export class SendEmailComponent implements OnInit {
  form: FormGroup;
  users: UserList = new UserList();
  status: number = 0;

  constructor(
    private logger: LoggingService,
    private user_service: UserService) {
    this.form = new FormGroup({
      recipient: new FormControl(0),
      subject: new FormControl("", [Validators.required, Validators.minLength(3)]),
      message: new FormControl("", [Validators.required, Validators.minLength(3)])
    });
  }

  async ngOnInit() {
    this.users = await this.user_service.getAll();
  }

  async onSubmit() {
    let recipient = this.form.controls.recipient.value;
    let subject = this.form.controls.subject.value;
    let message = this.form.controls.message.value;

    let success = await this.user_service.sendMail(recipient, subject, message);
    if (success) {
      this.form.controls.recipient.setValue(0);
      this.form.controls.subject.setValue("");
      this.form.controls.message.setValue("");
      this.status = 1;
    } else {
      this.status = 2;
    }
  }
}
