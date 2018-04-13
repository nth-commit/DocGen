import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { TemplateService, Template } from '../../../_core';

@Injectable()
export class TemplateResolve implements Resolve<Template> {

    constructor(
        private templateService: TemplateService
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Template | Observable<Template> | Promise<Template> {
        const templateId = route.paramMap.get('templateId');
        const templateVersion = route.queryParamMap.get('version') || '1';

        return this.templateService.getTemplate(templateId, parseInt(templateVersion, 10));
    }
}
