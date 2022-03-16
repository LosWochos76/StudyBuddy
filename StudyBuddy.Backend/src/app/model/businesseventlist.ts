import { BusinessEvent } from "./businessevent";

export class BusinessEventList {
    count: number = 0;
    objects: BusinessEvent[] = [];

    static fromResult(result) {
        let obj = new BusinessEventList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(BusinessEvent.fromApi(result['objects'][index]));
        }

        return obj;
    }
}