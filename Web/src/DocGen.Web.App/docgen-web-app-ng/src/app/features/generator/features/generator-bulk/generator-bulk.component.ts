import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import { State } from '../../../_shared';

import { GeneratorBulkState } from './state';

@Component({
  selector: 'app-generator-bulk',
  templateUrl: './generator-bulk.component.html',
  styleUrls: ['./generator-bulk.component.scss']
})
export class GeneratorBulkComponent implements OnInit {

  template$ = this.store.select(s => s.generatorBulk.documents.template);

  constructor(
    private store: Store<State>
  ) { }

  ngOnInit() {
  }

}
