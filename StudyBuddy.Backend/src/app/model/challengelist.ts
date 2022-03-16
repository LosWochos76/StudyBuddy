import { Challenge } from "./challenge";

export class ChallengeList {
    count: number = 0;
    objects: Challenge[] = [];

    static fromResult(result) {
        let obj = new ChallengeList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(Challenge.fromApi(result['objects'][index]));
        }

        return obj;
    }
}