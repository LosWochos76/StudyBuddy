import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Score } from '../model/score';
import { ScoreTrend } from '../model/scoretrend';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
    providedIn: 'root'
})
export class UserStatisticsService {
    @Output() changed = new EventEmitter();
    private url = environment.api_url;

    constructor(
        private logger: LoggingService,
        private http: HttpClient,
        private auth: AuthorizationService) { }

    async getScore(user_id: number): Promise<Score> {
        if (!this.auth.isLoggedIn())
            return null;

        this.logger.debug("Getting Score of user " + user_id);

        let result = await this.http.get(this.url + "Score/" + user_id,
            {
                headers: new HttpHeaders({ Authorization: this.auth.getToken() })
            }).toPromise();

        return Score.fromApi(result);
    }

    async getTrend(user_id: number): Promise<any> {
        if (!this.auth.isLoggedIn())
            return null;

        this.logger.debug("Getting Trend of user " + user_id);

        let result = await this.http.get(this.url + "Trend/" + user_id,
        {
            headers: new HttpHeaders({ Authorization: this.auth.getToken() })
        }).toPromise();

        return ScoreTrend.fromApi(result);
    }
}