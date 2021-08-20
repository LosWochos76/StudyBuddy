import { Helper } from "../shared/helper";

export class Term {
    constructor(
        public id:string,
        public shortname:string,
        public name:string,
        public start:string,
        public end:string
    ) { }

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

    periodHasNoOverlapsWithOthers(objects:Term[]):boolean {
        for (let obj of objects)
            if (obj.isInPeriod(this.startAsDate()) || obj.isInPeriod(this.endAsDate()))
                return false;

        return true;
    }
}