import { Score } from "./score";

export class ScoreTrend {
    values:{ [key: string]: Score } = {};

    static fromApi(obj): ScoreTrend {
        let result = new ScoreTrend();
        for (let key in obj['values']) {
            result.values[key] = Score.fromApi(obj['values'][key]);
        }

        return result;
    }

    getXAxis() {
        return Object.keys(this.values);
    }

    getLearningSeries() {
        let data = [];
        this.getXAxis().forEach((key) => { data.push(this.values[key].getLearning().points); });
        return data;
    }

    getOrganizingSeries() {
        let data = [];
        this.getXAxis().forEach((key) => { data.push(this.values[key].getOrganizing().points); });
        return data;
    }

    getNetworkingSeries() {
        let data = [];
        this.getXAxis().forEach((key) => { data.push(this.values[key].getNetworking().points); });
        return data;
    }
}