import { Helper } from "../shared/helper";

export class Challenge {
    id:string = "";
    name:string = "";;
    description:string = "";;
    points:number = 1;
    validity_start:string;
    validity_end:string;
    category:number = 1;
    owner:string = "";;
    created:string;
    prove:number = 1;
    parent:string = "";;
    study_program:string = "";
    enrolled_since:string = "";

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
        this.study_program = values.study_program;
        this.enrolled_since = values.enrolled_since;
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
}