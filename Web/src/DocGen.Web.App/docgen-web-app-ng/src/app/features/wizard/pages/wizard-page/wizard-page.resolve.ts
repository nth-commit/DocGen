import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Http } from '@angular/http';
import { Store } from '@ngrx/store';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { getAppSettings } from '../../../../app.settings';
import { Template } from '../../../core';
import { State, Begin } from '../../reducers';

@Injectable()
export class WizardPageResolve implements Resolve<Template> {

    constructor(
        private http: Http,
        private store: Store<State>
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Template | Observable<Template> | Promise<Template> {
        const templateId = route.paramMap.get('templateId');
        const template$ = this.http.get(`${getAppSettings().Urls.Api}/templates/${templateId}`).map(r => r.json());

        const templatePromise = template$.toPromise();
        templatePromise.then(template => this.store.dispatch(new Begin(template)));

        return templatePromise;
    }
}
