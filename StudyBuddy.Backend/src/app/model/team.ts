export class Team {
    id:number = 0;
    name:string = "";
    owner:number = 0;

    constructor() { }

    static fromApi(result):Team {
        let obj = new Team();
        obj.id = result["id"];
        obj.name = result["name"];
        obj.owner = result["ownerID"];
        return obj;
    }

    toApi() {
        return {
            "id": this.id,
            "name": this.name,
            "ownerID": this.owner
        };
    }
}