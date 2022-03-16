import { Request } from "./request";

export class RequestList {
    count: number = 0;
    objects: Request[] = [];

    static fromResult(result) {
        let obj = new RequestList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(Request.fromApi(result['objects'][index]));
        }
        
        return obj;
    }
}