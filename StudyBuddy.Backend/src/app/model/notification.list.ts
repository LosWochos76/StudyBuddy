import { Notification } from "./notification";

export class NotificationList {
    count: number = 0;
    objects: Notification[] = [];

    static fromResult(result) {
        let obj = new NotificationList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(Notification.fromApi(result['objects'][index]));
        }

        return obj;
    }
}