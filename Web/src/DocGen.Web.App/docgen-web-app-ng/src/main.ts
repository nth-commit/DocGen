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
