import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/model/user';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { ResetPasswordData } from '../../model/resetpassworddata';
import { AuthorizationService } from '../../services/authorization.service';

@Component({
    selector: 'app-resetpassword',
    templateUrl: './resetpassword.component.html',
    styleUrls: ['./resetpassword.component.css']
})
export class ResetpasswordComponent implements OnInit {
    resetPasswordForm: FormGroup;
    showSuccess: boolean;
    showError: boolean;

    private _token: string;
    private _email: string;

    constructor(
        private route: ActivatedRoute,
        private logger: LoggingService,
        private router: Router,
        private service: AuthorizationService) {
    };

    ngOnInit() {
        this.resetPasswordForm = new FormGroup({
            password: new FormControl("", [Validators.required, Validators.minLength(3)]),
            confirm: new FormControl("", [Validators.required, Validators.minLength(3)])
        });

        this._token = this.route.snapshot.queryParams['token'];
        this._email = this.route.snapshot.queryParams['email'];

    }
    public onSubmit = (resetPasswordFormValue) => {
        this.showError = this.showSuccess = false;
        const resetPass = { ...resetPasswordFormValue };

        const resetPassData: ResetPasswordData = {
            password: resetPass.password,
            token: this._token,
            email: this._email
        }

        if (resetPass.password != "") {
            if (resetPass.password.length < 6) {
                this.resetPasswordForm.setErrors({ 'passwordunsafe': true });
                return;
            }

            if (resetPass.password != resetPass.confirm) {
                this.resetPasswordForm.setErrors({ 'passwordmismatch': true });
                return;
            }
        }
        
        let result = this.service.resetPassword(resetPassData);
        if (!result)
            this.showError = true;
        else
            this.showSuccess = true;
    }
}