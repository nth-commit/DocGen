import { Component, OnInit, HostListener } from '@angular/core';
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

  lastScrollDirection: 'up' | 'down' = null;
  isMouseOverToolbar = false;
  isToolbarVisible = true;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.textDocument$ = this.route.data.map(d => d.textDocument);
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
