import { Component, OnInit } from '@angular/core';
import { LogMessage } from 'src/app/model/logmessage';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
  selector: 'app-logging-list',
  templateUrl: './logging-list.component.html',
  styleUrls: ['./logging-list.component.css']
})
export class LoggingListComponent implements OnInit {
  objects: LogMessage[] = [];
  timeout: any = null;

  constructor(
    private service: LoggingService) { }

  async ngOnInit() {
    this.objects = await this.service.getAll();
  }

  async onFlush() {
    if (!confirm("Wollen Sie die Logging-Meldungen wirklich löschen?"))
      return;

    await this.service.flush();
    this.objects = await this.service.getAll();
  }
}