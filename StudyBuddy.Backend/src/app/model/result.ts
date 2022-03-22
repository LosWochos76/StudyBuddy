export class Result {
    category: number = 0;
    points: number = 0;
    count: number = 0;

    static fromApi(obj): Result {
        let result = new Result();
        result.category = obj["category"];
        result.points = obj["points"];
        result.count = obj["count"];
        return result;
    }
}