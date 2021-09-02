import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
  selector: 'app-challenge-clone',
  templateUrl: './challenge-clone.component.html',
  styleUrls: ['./challenge-clone.component.css']
})
export class ChallengeCloneComponent implements OnInit {
  id:number = 0;
  obj:Challenge = null;
  form:FormGroup;
  
  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:ChallengeService) { 

      this.form = new FormGroup({});
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.obj = await this.service.byId(this.id);
  }

  onSubmit() {
    this.router.navigate(["challenge"]);
  }

  onCancel() {
    this.router.navigate(["challenge"]);
  }
}