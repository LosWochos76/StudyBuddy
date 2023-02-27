import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
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

    GetGameObjectStatistics(orderAsc: boolean = true) {

        var query = {};
        query["orderAscending"] = orderAsc;

        return this.http.get(this.url + "Statistics/GetGameObjectStatistics",
            {
                params: query,
                headers: new HttpHeaders({ Authorization: this.auth.getToken() }),
                observe:'response',
                responseType: 'blob'
            })
    }
    GetItemCompletionStatistics(orderAsc: boolean = true) {
        var query = {};
        query["orderAscending"] = orderAsc;

        return this.http.get(this.url + "Statistics/GetItemCompletionStatistics",
            {
                params: query,
                headers: new HttpHeaders({ Authorization: this.auth.getToken() }),
                observe: 'response',
                responseType: 'blob'
            })
    }
}