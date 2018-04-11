import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import { ReplaySubject } from 'rxjs/ReplaySubject';

import { InputValueCollection } from '../../../../../core';
import { State } from '../../../../../_shared/state';

import { DocumentValueSelectorDialogComponent } from './document-value-selector-dialog.component';

@Injectable()
export class DocumentValueSelectorService {

  private dialog: MatDialogRef<DocumentValueSelectorDialogComponent>;
  private dialogCloseAttempts = new ReplaySubject<{ eventTimestamp: number }>();

  constructor(
    private store: Store<State>,
    private matDialog: MatDialog
  ) { }

  selectValues(): Promise<void> {
    if (this.dialog) {
      throw new Error('Currently selecting values');
    }

    return this.store
      .select(s => s.generatorBulk.documents.lastCompletedDocument)
      .do(document => {
        this.dialogCloseAttempts = new ReplaySubject<{ eventTimestamp: number }>();

        this.dialog = this.matDialog.open(DocumentValueSelectorDialogComponent);
        this.dialog.disableClose = true;

        Observable
          .combineLatest(
            this.dialog.backdropClick(),
            this.dialog.keydownEvents())
          .toArray()
          // .scan((acc, curr) => [...acc, curr], [])
          .subscribe(events => {
            console.log(events);
          });

      })
      .toPromise()
      .then(x => {});
  }
}
