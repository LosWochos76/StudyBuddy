
import { User } from "./user";

export class LoginResult {
    public user: User;
    public status: number;
    public token: string;

    constructor() { }

    getRequestStatus() {
        switch (this.status) {
            case 1: return "Email not verified"; 
            case 2: return "Incorrect credentials";
            case 3: return "User not found";
            default: return "Success";
        }
    }

    static fromApi(obj): LoginResult {
        let result = new LoginResult();
        result.user = obj['user'];
        result.status = obj['status'];
        result.token = obj['token'];
        return result;
    }
}