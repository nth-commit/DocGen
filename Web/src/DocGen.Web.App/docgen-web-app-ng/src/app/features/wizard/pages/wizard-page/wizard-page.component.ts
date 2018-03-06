import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { Template } from '../../../core';

@Component({
  selector: 'app-wizard-page',
  templateUrl: './wizard-page.component.html',
  styleUrls: ['./wizard-page.component.scss']
})
export class WizardPageComponent implements OnInit {

  template$: Observable<Template>;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.template$ = this.route.data.map((data => data.template));
  }

}
