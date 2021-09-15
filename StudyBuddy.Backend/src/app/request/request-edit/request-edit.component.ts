import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Request } from 'src/app/model/request';
import { LoggingService } from 'src/app/services/loging.service';
import { RequestService } from 'src/app/services/request.service';

@Component({
  selector: 'app-request-edit',
  templateUrl: './request-edit.component.html',
  styleUrls: ['./request-edit.component.css']
})
export class RequestEditComponent implements OnInit {
  id:number = 0;
  obj:Request = null;
  form:FormGroup;

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:RequestService) {

      this.form = new FormGroup({
        sender: new FormControl([], [Validators.required]),
        recipient: new FormControl([], [Validators.required]),
        type: new FormControl(0, [Validators.required]),
        challenge: new FormControl([])
      });
  };

  async ngOnInit() {
    this.obj = new Request();

    this.form.setValue({
      sender: [],
      recipient: [],
      type: 1,
      challenge: []
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a Request!");

    this.obj.sender = this.form.controls.sender.value[0];
    this.obj.recipient = this.form.controls.recipient.value[0];
    this.obj.type = +this.form.controls.type.value;

    if (this.obj.type == 2)
      this.obj.challenge = this.form.controls.challenge.value[0];
    else
      this.obj.challenge = null;

    if (this.obj.sender == this.obj.recipient) {
      this.form.setErrors({ 'senderequalsrecipient': true });
      return;
    }

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(this.obj);
    this.router.navigate(["request"]);
  }

  onCancel() {
    this.router.navigate(['request']);
  }
}
