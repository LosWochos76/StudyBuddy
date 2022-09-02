import { User } from "./user";
import { Comment } from "./comment";

export class Notification {
    id: number;
    owner_id: number;
    owner: User;
    title: string;
    body: string;
    created: string;
    updated: string;
    liked:boolean;
    seen:boolean;
    shared:boolean;

    liked_users:User[] = [];
    comments:Comment[] = [];

    constructor() {
        this.id = 0;
    }

    static fromApi(obj): Notification {
        let result = new Notification();
        result.id = obj['id'];
        result.owner_id = obj['ownerId'];
        result.owner = User.fromApi(obj['owner']);
        result.title = obj['title'];
        result.body = obj['body'];
        result.created = obj['created'];
        result.updated = obj['updated'];
        result.liked = obj['liked'];
        result.seen = obj['seen'];
        result.shared = obj['shared'];

        return result;
    }

    toApi() {
        return { };
    }
}