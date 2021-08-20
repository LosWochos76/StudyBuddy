export class StudyProgram {
    constructor(
        public id:string,
        public acronym:string,
        public name:string
    ) { }

    isArconymUnique(objects:StudyProgram[]):boolean {
        let result = objects.find(obj => obj.acronym == this.acronym && obj.id != this.id);
        return result == undefined;
    }
}