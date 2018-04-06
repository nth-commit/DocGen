import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import { Observable } from 'rxjs/Observable';

import { State } from '../../../_shared';

import { GeneratorBulkState, BeginDraft } from './state';

@Component({
  selector: 'app-generator-bulk',
  templateUrl: './generator-bulk.component.html',
  styleUrls: ['./generator-bulk.component.scss']
})
export class GeneratorBulkComponent implements OnInit {

  template$ = this.store.select(s => s.generatorBulk.documents.template);
  completedDocuments$ = this.store.select(s => s.generatorBulk.documents.completedDocuments);
  draftDocuments$ = this.store.select(s => s.generatorBulk.documents.draftDocuments);

  constructor(
    private store: Store<State>
  ) { }

  ngOnInit() {
    Observable.combineLatest(this.completedDocuments$, this.draftDocuments$)
      .first()
      .subscribe(([completedDocuments, draftDocuments]) => {
        if (!completedDocuments.length && !draftDocuments.length) {
          this.store.dispatch(new BeginDraft());
        }
      });

  }

}
