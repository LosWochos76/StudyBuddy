import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { UserStatisticsService } from 'src/app/services/userstatistics.service';

@Component({
    selector: 'app-itemcompletionstatistics',
    templateUrl: './itemcompletionstatistics.component.html',
    styleUrls: ['./itemcompletionstatistics.component.css']
})

export class ItemCompletionStatisticsComponent implements OnInit {

    constructor(private stats: UserStatisticsService) {

        this.stats.GetItemCompletionStatistics()
            .subscribe(response => {
                let fileName = response.headers.get('content-disposition')
                    ?.split(';')[1].split('=')[1];
                let blob: Blob = response.body as Blob;
                let a = document.createElement('a');
                a.download = fileName;
                a.href = window.URL.createObjectURL(blob);
                a.click();
            });
    };
    ngOnInit(): void {
    }
}