import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';

import { State, Document } from '../../../_core';
import { WizardResumeAction, WizardBeginAction } from '../_core';
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

  selectedDocuments: Document[] = [];

  constructor(
    private store: Store<State>
  ) { }

  get allSelectedAreCompleted(): boolean {
    return this.selectedDocuments.every(d => !!d.creationTime);
  }

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

  create() {
    this.template$.first().subscribe(template => {
      this.store.dispatch(new WizardBeginAction(REDUCER_ID, { template }));
    });
  }

  deleteDocument(document: Document) {
    this.store.dispatch(new DocumentDeleteDocumentAction({
      id: document.id
    }));
  }

  deleteSelectedDocuments() {
    this.selectedDocuments.forEach(d => this.deleteDocument(d));
    this.selectedDocuments = [];
  }

  downloadDocument(document: Document) {
    alert('Download');
  }

  downloadSelectedDocuments() {
    this.selectedDocuments.forEach(d => this.downloadDocument(d));
  }

  createFromDocument(document: Document) {
    this.store.dispatch(new DocumentCreateFromAction({
      id: document.id
    }));
  }

  updateSelectedDocuments(selectedDocuments: Document[]) {
    this.selectedDocuments = selectedDocuments;
  }
}
