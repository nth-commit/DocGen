import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Http } from '@angular/http';
import { Store } from '@ngrx/store';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { getAppSettings } from '../../../../app.settings';
import { TemplateService, Template } from '../../../core';
import { State, WizardState, Refresh, Begin, BeginPayload, WizardMode } from '../../reducers';

@Injectable()
export class WizardPageResolve implements Resolve<Template> {

    constructor(
        private templateService: TemplateService,
        private store: Store<State>
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<Template> {
        const templateId = route.paramMap.get('templateId');

        const wizardStateJson = localStorage.getItem(`drafts:${templateId}:wizard`);
        if (wizardStateJson) {
            // TODO: Ensure template version is latests
            const wizardState: WizardState = JSON.parse(wizardStateJson);
            this.store.dispatch(new Refresh(wizardState));
            return new Promise(resolve => resolve(wizardState.template));
        } else {
            const templatePromise = this.templateService.getLatestTemplate(templateId);
            templatePromise.then(t => this.store.dispatch(new Begin({
                template: t,
                mode: WizardMode.PreSigning
            })));
            return templatePromise;
        }
    }
}
