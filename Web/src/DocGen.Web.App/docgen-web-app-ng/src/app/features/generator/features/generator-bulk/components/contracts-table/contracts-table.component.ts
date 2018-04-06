import { Component, OnInit, Input } from '@angular/core';

import { Document } from '../../state';

@Component({
  selector: 'app-generator-bulk-contracts-table',
  templateUrl: './contracts-table.component.html',
  styleUrls: ['./contracts-table.component.scss']
})
export class ContractsTableComponent implements OnInit {
  @Input() documents: Document[];
  @Input() draftDocument: Document;

  constructor() { }

  ngOnInit() {
  }

}
