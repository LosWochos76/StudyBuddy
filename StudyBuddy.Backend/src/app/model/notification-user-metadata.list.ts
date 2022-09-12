import { NotificationUserMetadata } from "./notification-user-metadata";

export class NotificationUserMetadataList {
    count: number = 0;
    objects: NotificationUserMetadata[] = [];

    static fromResult(result) {
        let obj = new NotificationUserMetadataList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(NotificationUserMetadata.fromApi(result['objects'][index]));
        }

        return obj;
    }
}