
import { User } from "./user";

export enum LoginStatus {
    Success = 0,
    EmailNotVerified = 1,
    IncorrectCredentials = 2,
    UserNotFound = 3,
    InvalidApiResponse = 4,
    NoToken = 5,
    UndocumentedError = 6,
    AccountDisabled = 7,
    InvalidToken = 8,
    IsStudent = 999
}

export class LoginResult {
    public user: User;
    public status: LoginStatus;
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
        result.user = obj['user'] != null ? User.fromApi(obj['user']) : null;
        result.status = obj['status'] as LoginStatus;
        result.token = obj['token'];
        return result;
    }
}