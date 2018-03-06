import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Http } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';


import { getAppSettings } from '../../../../app.settings';
import { Template } from '../../../core';

@Injectable()
export class WizardPageResolve implements Resolve<Template> {

    constructor(
        private http: Http
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Template | Observable<Template> | Promise<Template> {
        const templateId = route.paramMap.get('templateId');
        return this.http.get(`${getAppSettings().Urls.Api}/templates/${templateId}`)
            .map(r => r.json());
    }
}
