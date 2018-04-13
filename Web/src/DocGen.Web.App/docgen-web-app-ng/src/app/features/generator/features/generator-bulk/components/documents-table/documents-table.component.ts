import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatCheckboxChange } from '@angular/material';

import { Document } from '../../../../../_core';

@Component({
  selector: 'app-generator-bulk-documents-table',
  templateUrl: './documents-table.component.html',
  styleUrls: ['./documents-table.component.scss']
})
export class DocumentsTableComponent implements OnInit {
  @Input() completedDocuments: Document[];
  @Input() draftDocuments: Document[];
  @Input() selectedDocumentIds: string[];
  @Output() documentClicked = new EventEmitter<string>();

  displayedColumns = ['selected', 'title', 'values', 'creationTime', 'options'];
  documentsSelectedById: { [documentId: string]: boolean } = {};

  constructor() { }

  ngOnInit() {
  }

  get documents(): Document[] {
    return [
      ...this.completedDocuments,
      ...this.draftDocuments
    ];
  }

  openDocumentWizard(document: Document) {
    this.documentClicked.emit(document.id);
  }

  onChange(inputId: string, change: MatCheckboxChange) {

  }

}
