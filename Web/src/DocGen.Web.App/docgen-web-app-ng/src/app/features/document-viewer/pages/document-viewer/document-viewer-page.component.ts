import { Component, OnInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { Template, LocalStorageDocumentService, DocumentCreate } from '../../../core';
import { TextDocumentResult, SerializableDocumentResult, DocumentType } from '../../models';

@Component({
  selector: 'app-document-viewer-page',
  templateUrl: './document-viewer-page.component.html',
  styleUrls: ['./document-viewer-page.component.scss']
})
export class DocumentViewerPageComponent implements OnInit {

  template$: Observable<Template>;
  document$: Observable<TextDocumentResult | SerializableDocumentResult>;
  documentType$: Observable<DocumentType>;

  lastScrollDirection: 'up' | 'down' = null;
  isMouseOverToolbar = false;
  isToolbarVisible = true;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private documentService: LocalStorageDocumentService
  ) { }

  ngOnInit() {
    this.template$ = this.route.data.map(d => d.template);
    this.document$ = this.route.data.map(d => d.document);
    this.documentType$ = this.document$.map(d => d.type);
  }

  onEditClick() {
    this.template$.first().subscribe(t => {
      this.router.navigateByUrl(t.id);
    });
  }

  onSaveClick() {
    Observable
      .combineLatest([this.template$, this.document$])
      .first()
      .subscribe(([template, document]: [Template, SerializableDocumentResult]) => {
        this.documentService.createOrUpdate(document.correlationId, {
          inputValues: document.inputValues,
          templateId: template.id,
          templateName: template.name,
          templateVersion: template.version
        });
      });
  }

  @HostListener('window:mousewheel', ['$event'])
  onWindowScroll(ev: WheelEvent) {
    this.lastScrollDirection = ev.deltaY > 0 ? 'down' : 'up';
    this.updateIsToolbarVisible();
  }

  @HostListener('window:mousemove', ['$event'])
  onWindowMouseMove(ev: MouseEvent) {
    this.isMouseOverToolbar = ev.clientY < 100;
    if (this.isMouseOverToolbar) {
      this.lastScrollDirection = 'up';
    }
    this.updateIsToolbarVisible();
  }

  private updateIsToolbarVisible() {
    setTimeout(() => {
      this.isToolbarVisible = (
        !this.lastScrollDirection ||
        this.lastScrollDirection === 'up' ? true : this.isMouseOverToolbar
      );
    }, 1000);
  }
}
