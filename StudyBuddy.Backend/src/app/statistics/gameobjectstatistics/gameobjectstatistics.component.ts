import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { UserStatisticsService } from 'src/app/services/userstatistics.service';

@Component({
    selector: 'app-gameobjectstatistics',
    templateUrl: './gameobjectstatistics.component.html',
    styleUrls: ['./gameobjectstatistics.component.css']
})

export class GameObjectStatisticsComponent implements OnInit {

    constructor(private stats: UserStatisticsService) {

        this.stats.GetGameObjectStatistics()
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