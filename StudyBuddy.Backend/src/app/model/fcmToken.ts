
export class FcmToken {
    public id: number;
    public token: string;
    public userID: number;
    created: string;
    lastSeen: string;

    static fromApi(result):FcmToken {
        let obj = new FcmToken();
        obj.id = result["id"];
        obj.token = result["token"];
        obj.userID = result["userID"];
        obj.created = result["created"];
        obj.lastSeen = result["lastSeen"].split('T')[0];
        return obj;
    }
}