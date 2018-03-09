import { Component, OnInit, Input } from '@angular/core';

import { TextDocument } from '../../../core';

@Component({
  selector: 'app-document-viewer-text-document',
  templateUrl: './text-document.component.html',
  styleUrls: ['./text-document.component.scss']
})
export class TextDocumentComponent implements OnInit {
  @Input() textDocument: TextDocument;

  constructor() { }

  ngOnInit() {
  }

}
