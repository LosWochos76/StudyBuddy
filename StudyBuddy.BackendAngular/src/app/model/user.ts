export class User {
    id:string;
    firstname:string;
    lastname:string;
    nickname:string;
    email:string;
    role:Role;
    study_program_id:string;
    enrolled_since_term_id:string;
    firebase_user_id:string;
    
    constructor() { 
        this.id = "";
    }

    isAdmin():boolean {
        return this.role == Role.Admin;
    }

    isStudent():boolean {
        return this.role == Role.Student;
    }

    isTeacher():boolean {
        return this.role == Role.Teacher;
    }
}

export enum Role {
    Student = 1,
    Teacher = 2,
    Admin = 3
  }