import { Component, ComponentRef, OnInit, ViewContainerRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
    selector: 'app-forgotpassword',
    templateUrl: './forgotpassword.component.html',
    styleUrls: ['./forgotpassword.component.css']
})
export class ForgotpasswordComponent implements OnInit {
    form: FormGroup;
    _email: string;
    
    constructor(
        private route: ActivatedRoute,
        private logger: LoggingService,
        private auth: AuthorizationService) {
        this.form = new FormGroup({
            email: new FormControl("", [Validators.required, Validators.email])
        });

    };
    ngOnInit(): void {
        this._email = this.route.snapshot.paramMap.get('email');
    }
    async onPasswordReset() {
        let email = this.form.controls.email.value;
        console.log(email)
        console.log(this._email)
        let result = await this.auth.sendPassworResetMail(email);
        if (!result) {
            this.logger.debug("Password kann nicht zurückgesetzt werden.")
        }
        alert("Sollten Sie einen Account bei uns haben, haben wir eine E-Mail zur Wiederherstellug ihres Passworts an Ihre E-Mail-Adresse verschickt.");
    }
}