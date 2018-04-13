import { Injectable } from '@angular/core';
import { Router, RouterEvent, NavigationStart, RoutesRecognized } from '@angular/router';

import { Observable } from 'rxjs/Observable';

@Injectable()
export class RouteChangeService {

  private _previousUrl: string;
  private _currentUrl: string;

  constructor(
    private router: Router
  ) {
    this.router.events
      .filter(e => e instanceof NavigationStart)
      .subscribe((e: NavigationStart) => {
        this._previousUrl = this._currentUrl;
        this._currentUrl = e.url;
      });
  }

  get currentUrl(): string { return this._currentUrl; }
  get previousUrl(): string { return this._previousUrl; }
}
