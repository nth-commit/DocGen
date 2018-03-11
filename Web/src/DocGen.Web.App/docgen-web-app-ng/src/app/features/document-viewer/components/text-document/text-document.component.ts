import { Component, OnInit, Input } from '@angular/core';

import { TextDocumentResult } from '../../models';

@Component({
  selector: 'app-document-viewer-text-document',
  templateUrl: './text-document.component.html',
  styleUrls: ['./text-document.component.scss']
})
export class TextDocumentComponent implements OnInit {
  @Input() textDocument: TextDocumentResult;

  constructor() { }

  ngOnInit() {
  }

}
