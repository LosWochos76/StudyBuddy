import { BusinessEvent } from "./businessevent";

export class BusinessEventList {
    count: number = 0;
    objects: BusinessEvent[] = [];

    constructor(result) {
        this.count = result['count'];

        for (let index in result['objects']) {
            this.objects.push(BusinessEvent.fromApi(result['objects'][index]));
        }
    }
}