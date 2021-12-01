export class BusinessEvent {
    id:number = 0;
    name:string = "";;
    owner:number = 0;
    type:number = 1;
    code:string = "";

    copyValues(values) {
        this.name = values.name;
        this.type = +values.type;
        this.code = values.code;
    }

    static fromApi(result):BusinessEvent {
        let obj = new BusinessEvent();
        obj.id = result["id"];
        obj.name = result["name"];
        obj.owner = result["ownerID"];
        obj.type = result["type"];
        obj.code = result["code"];
        return obj;
    }

    toApi() {
        return {
            "id": this.id,
            "name": this.name,
            "ownerID": this.owner,
            "type": this.type,
            "code": this.code
        };
    }
}