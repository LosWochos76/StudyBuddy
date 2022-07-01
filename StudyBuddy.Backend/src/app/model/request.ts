import { Helper } from "../services/helper";

export class Request {
    id:number = 0;
    public created:string;
    public sender:number;
    public recipient:number;
    public type:number;
    public challenge:number = 0;

    constructor() { 
        this.created = (new Date()).toISOString().split('T')[0];;
    }

    createdAsDate():Date {
        return Helper.convertToDate(this.created);
    }

    static fromApi(obj):Request {
        let result = new Request();
        result.id = obj['id'];
        result.created = obj['created'].split('T')[0];
        result.sender = +obj['senderID'];
        result.recipient = +obj['recipientID'];
        result.type = +obj['type']
        result.challenge = +obj['challengeID'];
        return result;
    }

    toApi() {
        return { 
            'id': this.id, 
            'created': this.created,
            'senderID': this.sender,
            'recipientID': this.recipient,
            'type': this.type,
            'challengeID': this.challenge
          };
    }
}