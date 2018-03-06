import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

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
