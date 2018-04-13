import { Component, OnInit, Input } from '@angular/core';

import { Document } from '../../../../../_core';

@Component({
  selector: 'app-generator-bulk-documents-table',
  templateUrl: './documents-table.component.html',
  styleUrls: ['./documents-table.component.scss']
})
export class DocumentsTableComponent implements OnInit {
  @Input() completedDocuments: Document[];
  @Input() draftDocuments: Document[];

  constructor() { }

  ngOnInit() {
  }

}
