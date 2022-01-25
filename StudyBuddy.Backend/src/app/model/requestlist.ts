import { Request } from "./request";

export class RequestList {
    count: number = 0;
    objects: Request[] = [];

    constructor(result) {
        this.count = result['count'];

        for (let index in result['objects']) {
            this.objects.push(Request.fromApi(result['objects'][index]));
        }
    }
}