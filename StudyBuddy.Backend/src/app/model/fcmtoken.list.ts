import { FcmToken } from "./fcmToken";

export class FcmTokenList {
    count: number = 0;
    objects: FcmToken[] = [];

    static fromResult(result) {
        let obj = new FcmTokenList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(FcmToken.fromApi(result['objects'][index]));
        }

        return obj;
    }
}