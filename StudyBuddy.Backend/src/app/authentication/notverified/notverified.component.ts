import { Component, ComponentRef, Input, OnInit, ViewContainerRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
    selector: 'app-notverified',
    templateUrl: './notverified.component.html',
    styleUrls: ['./notverified.component.css']
})
export class NotverifiedComponent implements OnInit {
    form: FormGroup;
    toggle: boolean = true;
    _email: string;

    constructor(
        private route: ActivatedRoute,
        private auth: AuthorizationService,
        private logger: LoggingService) {
        this.form = new FormGroup({
            email: new FormControl("", [Validators.required, Validators.email])
        });

    }

    ngOnInit(): void {
        this._email = this.route.snapshot.paramMap.get('email');
    }

    async onSubmit() {
        let email = this.form.controls.email.value;
        let result = await this.auth.sendVerificationMail(email);
        if (!result) {
            this.logger.debug("E-Mail zur Bestätigung konnte nicht gesendet werden.")
        }
        alert("Bitte prüfen Sie Ihre E-Mails und klicken Sie auf den Bestätigungslink.")
    }

    async onChangeEmail() {
        this.toggle = false;
    }
}