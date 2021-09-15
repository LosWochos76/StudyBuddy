export class User {
    id: number;
    firstname: string;
    lastname: string;
    nickname: string;
    password: string;
    email: string;
    role: number = 1;
    study_program_id: number;
    enrolled_since_term_id: number;

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

    static fromApi(obj): User {
        let result = new User();
        result.id = obj['id'];
        result.firstname = obj['firstname'];
        result.lastname = obj['lastname'];
        result.nickname = obj['nickname'];
        result.email = obj['email'];
        result.study_program_id = obj['programID'];
        result.enrolled_since_term_id = obj['enrolledInTermID'];
        result.role = obj['role'];
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
            "programID": this.study_program_id,
            "EnrolledInTermID": this.enrolled_since_term_id,
            "role": this.role
        };
    }

    fullName() {
        return this.firstname + " " + this.lastname + " (" + this.nickname + ")";
    }

    copyValues(values) {
        this.firstname = values.firstname;
        this.lastname = values.lastname;
        this.nickname = values.nickname.toLowerCase();

        if (values.hasOwnProperty('password'))
            this.password = values.password;

        if (values.hasOwnProperty('email'))
            this.email = values.email.toLowerCase();

        if (values.hasOwnProperty('role'))
            this.role = +values.role;

        if (values.hasOwnProperty('study_program_id'))
            this.study_program_id = +values.study_program_id;

        if (values.hasOwnProperty('enrolled_since_id'))
            this.enrolled_since_term_id = +values.enrolled_since_id;
    }
}