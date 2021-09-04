import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
  selector: 'app-create-series',
  templateUrl: './create-series.component.html',
  styleUrls: ['./create-series.component.css']
})
export class CreateSeriesComponent implements OnInit {
  id:number = 0;
  obj:Challenge = new Challenge();
  form:FormGroup;
  
  constructor(private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:ChallengeService) { 

      this.form = new FormGroup({
        days_add: new FormControl(7),
        count: new FormControl(10)
      });
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.obj = await this.service.byId(this.id);
  }

  async onSubmit() {
    let days_add = this.form.controls.days_add.value;
    let count = this.form.controls.count.value;

    await this.service.createSeries(this.id, days_add, count);
    this.router.navigate(['challenge']);
  }

  onCancel() {
    this.router.navigate(['challenge']);
  }
}