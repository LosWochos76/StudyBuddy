import { Challenge } from "./challenge";

export class ChallengeList {
    count: number = 0;
    objects: Challenge[] = [];

    constructor(result) {
        this.count = result['count'];

        for (let index in result['objects']) {
            this.objects.push(Challenge.fromApi(result['objects'][index]));
        }
    }
}