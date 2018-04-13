import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Http } from '@angular/http';
import { Store } from '@ngrx/store';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { getAppSettings } from '../../../../app.settings';
import { TemplateService, Template } from '../../../_core';
import { State } from '../../../_core';

import { DocumentBeginAction, DocumentUpdateConstantsAction } from './state';

@Injectable()
export class GeneratorBulkResolveTemplate implements Resolve<Template> {

    constructor(
        private templateService: TemplateService,
        private store: Store<State>
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<Template> {
        const templateId = route.paramMap.get('templateId');

        // TODO: Refresh from local storage

        this.store.dispatch(new DocumentUpdateConstantsAction({
            'contractor.type': 'person',
            'contractor.person.name': 'asdasd',
            'contractor.person.location': 'asdasd',
            'contractor.person.occupation': 'asdasd',
            'contractor.company.name': null,
            'contractor.company.location': null,
            'disclosure_reason': 'asdasdasd',
            'disclosure_access': true,
            // 'disclosure_access.details.persons': null,
            'organisation.name': 'Automio',
            'organisation.location': 'New Plymouth',
            'organisation.description': 'This is a description of the organisation'
          }));

        const templatePromise = this.templateService.getLatestTemplate(templateId);
        templatePromise.then(t => this.store.dispatch(new DocumentBeginAction(t)));
        return templatePromise;
    }
}
