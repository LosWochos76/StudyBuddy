import { Tag } from "./tag";

export class TagList {
    count: number = 0;
    objects: Tag[] = [];

    constructor(result) {
        this.count = result['count'];

        for (let index in result['objects']) {
            this.objects.push(Tag.fromApi(result['objects'][index]));
        }
    }
}