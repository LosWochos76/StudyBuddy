import { Result } from "./result";

export class Score {
    learning:Result;
    organizing:Result;
    networking:Result;
    total:Result;

    static fromApi(obj): Score {
        let result = new Score();
        result.learning = Result.fromApi(obj["learning"]);
        result.organizing = Result.fromApi(obj["organizing"]);
        result.networking = Result.fromApi(obj["networking"]);
        result.total = Result.fromApi(obj["total"]);
        return result;
    }
}