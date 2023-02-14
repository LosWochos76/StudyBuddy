export class User {
    id: number;
    created: string = '0000-00-00';
    firstname: string;
    lastname: string;
    nickname: string;
    password: string;
    email: string;
    role: number = 1;
    emailconfirmed: boolean = false;
    accountactive: boolean = false;

    constructor() {
        this.id = 0;
    }

    isStudent(): boolean {
        return this.role == 1;
    }

    isTeacher(): boolean {
        return this.role == 2;
    }

    isAdmin(): boolean {
        return this.role == 3;
    }

    getRoleName() {
        switch (this.role) {
            case 1: return "Student"; break;
            case 2: return "Dozent"; break;
            case 3: return "Admin"; break;
            default: return "";
        }
    }

    getStatus() {
        if (this.emailconfirmed) return "verifiziert";
        else return "nicht verifiziert";
    }

    static fromApi(obj): User {
        let result = new User();
        result.id = obj['id'];
        result.created = obj['created'];
        result.firstname = obj['firstname'];
        result.lastname = obj['lastname'];
        result.nickname = obj['nickname'];
        result.email = obj['email'];
        result.role = obj['role'];
        result.emailconfirmed = obj['emailConfirmed'];
        result.accountactive = obj['accountActive'];
        return result;
    }

    toApi() {
        return {
            "id": this.id,
            "firstname": this.firstname,
            "lastname": this.lastname,
            "nickname": this.nickname,
            "email": this.email,
            "password": this.password,
            "role": this.role,
            "emailConfirmed": this.emailconfirmed,
            "accountActive": this.accountactive
        };
    }

    fullName() {
        return this.firstname + " " +
            this.lastname +
            " (" + this.nickname + ")";
    }

    copyValues(values) {
        this.firstname = values.firstname;
        this.lastname = values.lastname;
        this.nickname = values.nickname.toLowerCase();

        if (values.hasOwnProperty('emailconfirmed'))
            this.emailconfirmed = values.emailconfirmed;

        if (values.hasOwnProperty('accountactive'))
            this.accountactive = values.accountactive;

        if (values.hasOwnProperty('password'))
            this.password = values.password;

        if (values.hasOwnProperty('email'))
            this.email = values.email.toLowerCase();

        if (values.hasOwnProperty('role'))
            this.role = +values.role;
    }
}