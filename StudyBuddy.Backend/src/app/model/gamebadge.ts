import { Helper } from "../services/helper";

export class GameBadge {
    id:number = 0;
    name:string = "";
    owner:number = 0;
    created:string;
    required_coverage:number = 0.5;
    
    constructor() { 
        let today = (new Date()).toISOString().split('T')[0];
        this.created = today;
    }

    copyValues(values) {
        this.name = values.name;
        this.required_coverage = +values.required_coverage;
    }

    static fromApi(result):GameBadge {
        let obj = new GameBadge();
        obj.id = result["id"];
        obj.name = result["name"];
        obj.owner = result["ownerId"];
        obj.created = result["created"];
        obj.required_coverage = +result["requiredCoverage"];
        return obj;
    }

    toApi() {
        return {
            "id": this.id,
            "name": this.name,
            "ownerId": this.owner,
            "created": this.created,
            "requiredCoverage": +this.required_coverage
        };
    }
}