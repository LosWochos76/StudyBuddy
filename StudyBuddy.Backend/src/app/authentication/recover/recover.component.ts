import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizationService } from '../../services/authorization.service';
import { LoggingService } from '../../services/loging.service';

@Component({
    selector: 'app-recover',
    templateUrl: './recover.component.html',
    styleUrls: ['./recover.component.css']
})
/** recover component*/
export class RecoverComponent implements OnInit {
    enableAccountForm: FormGroup;
    showSuccess: boolean;
    showError: boolean;
    /** recover ctor */
    constructor(
        private logger: LoggingService,
        private auth: AuthorizationService,
        private router: Router) {
        this.enableAccountForm = new FormGroup({
            email: new FormControl("", [Validators.required, Validators.email]),
            password: new FormControl("", [Validators.required])
        });

    }
    ngOnInit(): void {
    }

    async onSubmit() {
        let email = this.enableAccountForm.controls.email.value;
        let password = this.enableAccountForm.controls.password.value;
        let result = await this.auth.enableAccount(email, password);
        if (!result)
            this.showError = true;
        else
            this.showSuccess = true;
    }
}