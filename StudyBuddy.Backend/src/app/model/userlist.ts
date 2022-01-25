import { User } from "./user";

export class UserList {
    count: number = 0;
    objects: User[] = [];

    constructor(result) {
        this.count = result['count'];

        for (let index in result['objects']) {
            this.objects.push(User.fromApi(result['objects'][index]));
        }
    }
}