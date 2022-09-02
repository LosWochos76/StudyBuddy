import { Tag } from "./tag";

export class TagList {
    count: number = 0;
    objects: Tag[] = [];

    static fromResult(result) {
        let obj = new TagList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(Tag.fromApi(result['objects'][index]));
        }
        
        return obj;
    }
}