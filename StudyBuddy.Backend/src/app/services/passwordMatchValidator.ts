import { FormGroup } from "@angular/forms";

export function passwordMatchValidator(g: FormGroup) {
return g.get('password').value === g.get('password_confirm').value
    ? null : {'mismatch': true};
}