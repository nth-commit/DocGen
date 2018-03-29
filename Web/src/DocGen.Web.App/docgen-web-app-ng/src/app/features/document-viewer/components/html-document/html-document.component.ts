import { Component, OnInit, Input, OnChanges, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

import { HtmlDocument } from '../../../core';

@Component({
  selector: 'app-document-viewer-html-document',
  templateUrl: './html-document.component.html',
  styleUrls: ['./html-document.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HtmlDocumentComponent implements OnInit, OnChanges {
  @Input()
  htmlDocument: HtmlDocument;

  pagesHtmlSanitized: SafeHtml[];

  constructor(
    private domSanitizer: DomSanitizer
  ) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.htmlDocument) {
      this.pagesHtmlSanitized = this.htmlDocument.pages.map(p => this.domSanitizer.bypassSecurityTrustHtml(p));
    }
  }
}
