import { Component, OnInit } from '@angular/core';
import { FcmTokenService } from 'src/app/services/fcm-token.service';

@Component({
  selector: 'app-fcm-token-list',
  templateUrl: './fcm-token-list.component.html',
  styleUrls: ['./fcm-token-list.component.css']
})
export class FcmTokenListComponent implements OnInit {
  
  $tokens = this.fcmTokenService.GetAll();

  constructor(private fcmTokenService: FcmTokenService ) { }

  ngOnInit(): void {
  }

}
