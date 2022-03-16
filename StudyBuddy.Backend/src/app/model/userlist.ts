import { User } from "./user";

export class UserList {
    count: number = 0;
    objects: User[] = [];

    static fromResult(result) {
        let obj = new UserList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(User.fromApi(result['objects'][index]));
        }

        return obj;
    }
}