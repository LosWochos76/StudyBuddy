import { GameBadge } from "./gamebadge";

export class GameBadgeList {
    count: number = 0;
    objects: GameBadge[] = [];

    static fromResult(result) {
        let obj = new GameBadgeList();
        obj.count = result['count'];

        for (let index in result['objects']) {
            obj.objects.push(GameBadge.fromApi(result['objects'][index]));
        }

        return obj;
    }
}