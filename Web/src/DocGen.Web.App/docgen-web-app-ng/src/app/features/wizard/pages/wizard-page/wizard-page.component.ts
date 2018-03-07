import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';

import { Observable } from 'rxjs';

import { Template } from '../../../core';

import { InputValueCollection } from '../../models';
import { WizardState, Begin, UpdateValues } from '../../reducers';

@Component({
  selector: 'app-wizard-page',
  templateUrl: './wizard-page.component.html',
  styleUrls: ['./wizard-page.component.scss']
})
export class WizardPageComponent implements OnInit {

  template$: Observable<Template>;

  constructor(
    private route: ActivatedRoute,
    private wizardStore: Store<WizardState>
  ) { }

  ngOnInit() {
    this.template$ = this.route.data.map((data => data.template));

    this.template$.subscribe(template => {
      this.wizardStore.dispatch(new Begin(template));
    });
  }

  updateStepValues(values: InputValueCollection) {
    this.wizardStore.dispatch(new UpdateValues(values));
  }

}
