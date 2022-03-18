export class Result {
    category: number;
    points: number;
    count: number;

    static fromApi(obj): Result {
        let result = new Result();
        result.category = obj["category"];
        result.points = obj["points"];
        result.count = obj["count"];
        return result;
    }
}