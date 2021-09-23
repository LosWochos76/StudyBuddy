export class Tag {
    id:number = 0;
    public name:string = "";

    constructor() { }

    static fromApi(obj):Tag {
        let result = new Tag();
        result.id = obj['id'];
        result.name = obj['name'];
        return result;
    }

    toApi() {
        return { 
            'ID': this.id, 
            'Name': this.name
          };
    }

    static toTagString(tags:Tag[]):string {
        return tags.map(obj => "#" + obj.name).join(' ');
    }
}