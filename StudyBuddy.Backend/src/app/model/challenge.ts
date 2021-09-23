import { Helper } from "../services/helper";

export class Challenge {
    id:number = 0;
    name:string = "";;
    description:string = "";;
    points:number = 1;
    validity_start:string;
    validity_end:string;
    category:number = 1;
    owner:number = 0;
    created:string;
    prove:number = 1;
    parent:number = 0;

    constructor() { 
        let today = (new Date()).toISOString().split('T')[0];
        this.validity_start = today;
        this.validity_end = today;
        this.created = today;
    }

    copyValues(values) {
        this.name = values.name;
        this.description = values.description;
        this.points = +values.points;
        this.validity_start = values.validity_start;
        this.validity_end = values.validity_end;
        this.category = +values.category;
        this.prove = +values.prove;

        if (values.hasOwnProperty('owner')) 
            this.owner = +values.owner;
    }

    validityStartAsDate():Date {
        return Helper.convertToDate(this.validity_start);
    }

    validityEndAsDate():Date {
        return Helper.convertToDate(this.validity_end);
    }

    isPeriodValid():boolean {
        return this.validityEndAsDate() >= this.validityStartAsDate();
    }

    isInPeriod(d:Date):boolean {
        return d >= this.validityStartAsDate() && d <= this.validityEndAsDate();
    }

    static fromApi(result):Challenge {
        let obj = new Challenge();
        obj.id = result["id"];
        obj.name = result["name"];
        obj.description = result["description"];
        obj.points = result["points"];
        obj.validity_start = result["validityStart"].split('T')[0];
        obj.validity_end = result["validityEnd"].split('T')[0];
        obj.category = result["category"];
        obj.owner = result["ownerID"];
        obj.created = result["created"].split('T')[0];
        obj.prove = result["prove"];
        obj.parent = result["seriesParentID"];
        return obj;
    }

    toApi() {
        return {
            "id": this.id,
            "name": this.name,
            "description": this.description,
            "points": this.points,
            "ValidityStart": this.validity_start,
            "ValidityEnd": this.validity_end,
            "category": this.category,
            "OwnerID": this.owner,
            "created": this.created,
            "prove": this.prove,
            "SeriesParentID": this.parent
        };
    }
}