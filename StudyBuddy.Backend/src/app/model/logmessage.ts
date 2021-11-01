export class LogMessage {
    occurence:string = "";
    message:string = "";
    level:number = 0;
    userId:number = 0;
    source:number = 0;

    constructor() { }

    static fromApi(obj):LogMessage {
        let result = new LogMessage();
        result.occurence = obj['occurence'];
        result.message = obj['message'];
        result.level = +obj["level"];
        result.userId = +obj["userId"];
        result.source = +obj["source"];
        return result;
    }

    toApi() {
        return { 
            'Message': this.message,
            'Level': this.level,
            'UserId': this.userId,
            'Source': this.source
        };
    }
}