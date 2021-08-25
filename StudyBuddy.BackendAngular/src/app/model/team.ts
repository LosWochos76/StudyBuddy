import { User } from "./user";

export class Team {
    members:[] = [];

    constructor(
        public id:string,
        public name:string
    ) { }
}