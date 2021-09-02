export class StudyProgram {
    id:number = 0;
    acronym:string = "";
    name:string = "";

    constructor() { }

    // ToDo: Als serverbasierte variante implementieren!
    isArconymUnique(objects:StudyProgram[]):boolean {
        let result = objects.find(obj => obj.acronym == this.acronym && obj.id != this.id);
        return result == undefined;
    }

    static fromApi(result):StudyProgram {
        let obj = new StudyProgram();
        obj.id = result['id'];
        obj.acronym = result['acronym'];
        obj.name = result['name'];
        return obj;
    }

    toApi() {
        return {
            'ID': this.id,
            'Acronym': this.acronym,
            'Name': this.name
        };
    }
}