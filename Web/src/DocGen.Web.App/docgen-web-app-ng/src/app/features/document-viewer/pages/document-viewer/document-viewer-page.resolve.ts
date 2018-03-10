import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Http, Headers, RequestOptionsArgs, URLSearchParams } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { getAppSettings } from '../../../../app.settings';
import { TextDocument, InputValueCollection } from '../../../core';

@Injectable()
export class DocumentViewerPageResolve implements Resolve<TextDocument> {

    constructor(
        private http: Http
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): TextDocument | Observable<TextDocument> | Promise<TextDocument> {
        const templateId = route.paramMap.get('templateId');
        const templateVersion = route.queryParamMap.get('version') || '1';

        const key = `documents:${templateId}:${templateVersion}:values`;
        const inputValuesJson = localStorage.getItem(key);

        if (inputValuesJson) {
            const inputValues = JSON.parse(inputValuesJson);

            const documentUrl = `${getAppSettings().Urls.Api}/documents`;

            const params = new URLSearchParams();
            params.append('templateId', templateId);
            params.append('templateVersion', templateVersion);
            Object.keys(inputValues).forEach(k => {
                params.append(k, inputValues[k]);
            });

            const headers = new Headers({
                'Content-Type': 'text/plain'
            });

            const document$ = this.http.post(documentUrl, {}, { headers, params });

            const documentPromise = document$
                .map(r => <TextDocument>{
                    text: r.text()
                })
                .toPromise();

            return documentPromise;
        }

        throw new Error('Input values expected in local storage');
    }
}
