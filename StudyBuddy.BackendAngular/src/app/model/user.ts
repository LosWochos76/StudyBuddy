export class User {
    id:string;
    firstname:string;
    lastname:string;
    nickname:string;
    email:string;
    role:string;
    study_program_id:string;
    enrolled_since_term_id:string;
    firebase_user_id:string;
    
    constructor() { 
        this.id = "";
    }

    isStudent():boolean {
        return this.role == "1";
    }

    isTeacher():boolean {
        return this.role == "2";
    }

    isAdmin():boolean {
        return this.role == "3";
    }

    getRoleName() {
        switch (this.role) {
            case "1": return "Student"; break;
            case "2": return "Dozent"; break;
            case "3": return "Admin"; break;
            default: return "";
        }
    }
}