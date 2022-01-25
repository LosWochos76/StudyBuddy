import { GameBadge } from "./gamebadge";

export class GameBadgeList {
    count: number = 0;
    objects: GameBadge[] = [];

    constructor(result) {
        this.count = result['count'];

        for (let index in result['objects']) {
            this.objects.push(GameBadge.fromApi(result['objects'][index]));
        }
    }
}