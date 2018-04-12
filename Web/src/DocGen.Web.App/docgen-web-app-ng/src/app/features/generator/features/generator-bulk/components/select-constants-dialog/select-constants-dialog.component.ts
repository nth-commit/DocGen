import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import { InputValueCollection, TemplateStep } from '../../../../../core';
import { State } from '../../../../../_shared';
import { DocumentUpdateConstantsAction } from '../../state/document';

@Component({
  templateUrl: './select-constants-dialog.component.html',
  styleUrls: ['./select-constants-dialog.component.scss']
})
export class SelectConstantsDialogComponent implements OnInit {

  constructor(
    private store: Store<State>
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.store.dispatch(new DocumentUpdateConstantsAction({
        'contractor.type': 'person',
        'contractor.person.name': 'asdasd',
        'contractor.person.location': 'asdasd',
        'contractor.person.occupation': 'asdasd',
        'contractor.company.name': null,
        'contractor.company.location': null,
        'disclosure_reason': 'asdasdasd',
        'disclosure_access': true,
        // 'disclosure_access.details.persons': null
      }));
    }, 2000);
  }

}
