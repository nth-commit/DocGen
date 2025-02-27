import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import './extensions';

import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/withLatestFrom';
import 'rxjs/add/operator/scan';
import 'rxjs/add/operator/toArray';
import 'rxjs/add/operator/switchMap';

import 'rxjs/add/observable/empty';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/timer';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/zip';
import 'rxjs/add/observable/race';

const bootstrap = () => platformBrowserDynamic().bootstrapModule(AppModule).catch(err => console.log(err));

if (environment.production) {
  enableProdMode();
  bootstrap();
} else {
  fetch('http://localhost:52541/config')
    .then(response => response.json())
    .then(x => {
      window['DOCGEN_ENVIRONMENT_SETTINGS'] = x;
      bootstrap();
    });
}
