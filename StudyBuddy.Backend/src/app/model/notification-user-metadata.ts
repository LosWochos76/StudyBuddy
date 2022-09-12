export class NotificationUserMetadata {
    id: number;
    notificationId: number;
    ownerId: number;
    liked: boolean | null;
    seen: boolean | null;
    shared: boolean | null;
    created: string;
    updated: string;
    

    static fromApi(obj): NotificationUserMetadata {
        return Object.assign(new NotificationUserMetadata(), obj)
    }

    toApi() {
        return this;
    }
}