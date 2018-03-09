import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Http } from '@angular/http';
import { Store } from '@ngrx/store';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { getAppSettings } from '../../../../app.settings';
import { Template } from '../../../core';
import { State, WizardState, Refresh, Begin } from '../../reducers';

@Injectable()
export class WizardPageResolve implements Resolve<Template> {

    constructor(
        private http: Http,
        private store: Store<State>
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<Template> {
        const templateId = route.paramMap.get('templateId');

        const wizardStateJson = localStorage.getItem(`templates:${templateId}:wizard`);
        if (wizardStateJson) {
            const wizardState: WizardState = JSON.parse(wizardStateJson);
            this.store.dispatch(new Refresh(wizardState));
            return new Promise(resolve => resolve(wizardState.template));
        } else {
            const template$ = this.http.get(`${getAppSettings().Urls.Api}/templates/${templateId}`).map(r => r.json());
            const templatePromise = template$.toPromise();
            templatePromise.then(template => this.store.dispatch(new Begin(template)));
            return templatePromise;
        }
    }
}
