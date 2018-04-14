import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';

import { State, Document } from '../../../_core';
import { WizardResumeAction } from '../_core';
import { REDUCER_ID, DocumentDeleteDocumentAction, DocumentCreateFromAction } from './state';

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
  }

  openDocumentWizard(document: Document) {
    this.store
      .select(s => s.generatorBulk)
      .first()
      .subscribe(generatorBulk => {
        const { documents, layout } = generatorBulk;

        if (layout.dialog) {
          return;
        }

        this.store.dispatch(new WizardResumeAction(REDUCER_ID, {
          id: document.id,
          template: documents.template,
          presets: document.constants,
          values: document.values
        }));
      });
  }

  deleteDocument(document: Document) {
    this.store.dispatch(new DocumentDeleteDocumentAction({
      id: document.id
    }));
  }

  downloadDocument(document: Document) {
    alert('Download');
  }

  createFromDocument(document: Document) {
    this.store.dispatch(new DocumentCreateFromAction({
      id: document.id
    }));
  }
}
