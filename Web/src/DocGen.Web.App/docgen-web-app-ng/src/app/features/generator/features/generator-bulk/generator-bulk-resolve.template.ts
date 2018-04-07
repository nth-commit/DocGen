import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Http } from '@angular/http';
import { Store } from '@ngrx/store';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { getAppSettings } from '../../../../app.settings';
import { TemplateService, Template } from '../../../core';

import { GeneratorBulkState, DocumentBeginAction } from './state';

@Injectable()
export class GeneratorBulkResolveTemplate implements Resolve<Template> {

    constructor(
        private templateService: TemplateService,
        private store: Store<GeneratorBulkState>
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<Template> {
        const templateId = route.paramMap.get('templateId');

        // TODO: Refresh from local storage

        const templatePromise = this.templateService.getLatestTemplate(templateId);
        templatePromise.then(t => this.store.dispatch(new DocumentBeginAction(t)));
        return templatePromise;
    }
}
