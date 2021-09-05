import { Helper } from "../services/helper";

export class Term {
    id:number = 0;
    public shortname:string = "";
    public name:string = "";
    public start:string;
    public end:string;

    constructor() { 
        let today = (new Date()).toISOString().split('T')[0];
        this.start = today;
        this.end = today;
    }

    startAsDate():Date {
        return Helper.convertToDate(this.start);
    }

    endAsDate():Date {
        return Helper.convertToDate(this.end);
    }

    isPeriodValid():boolean {
        return this.endAsDate() > this.startAsDate();
    }

    isInPeriod(d:Date):boolean {
        return d >= this.startAsDate() && d <= this.endAsDate();
    }

    // ToDo: Sollte als serverbasierte Variante implementiert werden!
    periodHasNoOverlapsWithOthers(objects:Term[]):boolean {
        for (let obj of objects)
            if (obj.id != this.id && (obj.isInPeriod(this.startAsDate()) || obj.isInPeriod(this.endAsDate()))) {
                return false;
            }

        return true;
    }

    static fromApi(obj):Term {
        let result = new Term();
        result.id = obj['id'];
        result.shortname = obj['shortName'];
        result.name = obj['name'];
        result.start = obj['start'].split('T')[0];
        result.end = obj['end'].split('T')[0];

        return result;
    }

    toApi() {
        return { 
            'ID': this.id, 
            'ShortName': this.shortname,
            'Name': this.name,
            'Start': this.start,
            'End': this.end
          };
    }
}