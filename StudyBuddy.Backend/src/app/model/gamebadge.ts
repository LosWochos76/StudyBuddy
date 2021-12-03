export class GameBadge {
    id:number = 0;
    name:string = "";
    owner:number = 0;
    created:string;
    required_coverage:number = 0.3;
    description:string = "";
    tags:string = "";
    
    constructor() { 
        let today = (new Date()).toISOString().split('T')[0];
        this.created = today;
    }

    copyValues(values) {
        this.name = values.name;
        this.description = values.description;
        this.required_coverage = +values.required_coverage;
        this.tags = values.tags;

        if (values.hasOwnProperty('owner')) 
            this.owner = +values.owner;
    }

    static fromApi(result):GameBadge {
        let obj = new GameBadge();
        obj.id = result["id"];
        obj.name = result["name"];
        obj.owner = result["ownerID"];
        obj.created = result["created"];
        obj.required_coverage = +result["requiredCoverage"];
        obj.description = result["description"];
        obj.tags = result["tags"];
        return obj;
    }

    toApi() {
        return {
            "id": this.id,
            "name": this.name,
            "ownerId": this.owner,
            "created": this.created,
            "requiredCoverage": +this.required_coverage,
            "description": this.description,
            "tags": this.tags
        };
    }
}