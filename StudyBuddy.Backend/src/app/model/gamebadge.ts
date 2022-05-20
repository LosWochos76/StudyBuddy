import { User } from "./user";

export class GameBadge {
    id:number = 0;
    name:string = "";
    owner_id:number = 0;
    created:string;
    required_coverage:number;
    description: string = "";
    iconkey: string = "";
    tags:string = "";
    owner: User = null;
    
    constructor() { 
        let today = (new Date()).toISOString().split('T')[0];
        this.created = today;
    }

    copyValues(values) {
        this.name = values.name;
        this.description = values.description;
        this.required_coverage = +values.required_coverage;
        this.tags = values.tags;
        this.iconkey = values.icon_key;
        if (values.hasOwnProperty('owner_id')) 
            this.owner_id = +values.owner_id;
    }

    static fromApi(result):GameBadge {
        let obj = new GameBadge();
        obj.id = result["id"];
        obj.name = result["name"];
        obj.owner_id = result["ownerID"];
        obj.created = result["created"];
        obj.required_coverage = +result["requiredCoverage"];
        obj.description = result["description"];
        obj.tags = result["tags"];
        obj.iconkey = result["iconKey"];
        

        if (result["owner"] != null)
            obj.owner = User.fromApi(result["owner"]);

        return obj;
    }

    toApi() {
        return {
            "id": this.id,
            "name": this.name,
            "ownerId": this.owner_id,
            "created": this.created,
            "requiredCoverage": +this.required_coverage,
            "description": this.description,
            "tags": this.tags,
            "iconKey": this.iconkey
        };
    }
}