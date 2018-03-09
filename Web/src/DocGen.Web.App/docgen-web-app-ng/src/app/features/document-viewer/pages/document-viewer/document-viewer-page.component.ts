import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { TextDocument } from '../../../core';

@Component({
  selector: 'app-document-viewer-page',
  templateUrl: './document-viewer-page.component.html',
  styleUrls: ['./document-viewer-page.component.scss']
})
export class DocumentViewerPageComponent implements OnInit {

  textDocument$: Observable<TextDocument>;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.textDocument$ = this.route.data.map(d => d.textDocument);
  }
}
