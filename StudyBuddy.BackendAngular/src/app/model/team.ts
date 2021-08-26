import { User } from "./user";

export class Team {
    members:string[] = [];

    constructor(
        public id:string,
        public name:string
    ) { }
}