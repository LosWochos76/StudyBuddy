export class Helper {
    public static convertToDate(d:string):Date {
        let result = new Date(d);
        result.setHours(0,0,0);
        return result;
    }
}