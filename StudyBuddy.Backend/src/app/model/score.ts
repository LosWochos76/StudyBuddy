import { Result } from "./result";

export class Score {
    values: { [key: string]: Result } = {};

    static fromApi(obj): Score {
        let result = new Score();

        for (let key in obj['values']) {
            result.values[key] = Result.fromApi(obj['values'][key]);
        }

        return result;
    }

    public getLearning() {
        return this.values['Learning'] == undefined ? new Result() : this.values['Learning'];
    }

    public getOrganizing() {
        return this.values['Organizing'] == undefined ? new Result() : this.values['Organizing'];
    }

    public getNetworking() {
        return this.values['Networking'] == undefined ? new Result() : this.values['Networking'];
    }

    public getTotal() {
        let learning = this.getLearning();
        let organizing = this.getOrganizing();
        let networking = this.getNetworking();

        let result = new Result();
        result.count = learning.count + organizing.points + networking.count;
        result.points = learning.points + organizing.points + networking.points;
        return result;
    }
}