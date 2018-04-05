import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Http, Headers, RequestOptionsArgs, URLSearchParams } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { getAppSettings } from '../../../../app.settings';
import { InputValueCollection, SerializableDocument, HtmlDocument } from '../../../core';
import { DocumentResult, SerializableDocumentResult, TextDocumentResult, DocumentType, HtmlDocumentResult } from '../../models';

@Injectable()
export class DocumentResolve implements Resolve<DocumentResult> {

  constructor(
    private http: Http
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<DocumentResult> {
    const templateId = route.paramMap.get('templateId');
    const templateVersion = route.queryParamMap.get('version') || '1';
    const documentType: DocumentType = (<DocumentType>route.queryParamMap.get('type')) || 'pdf';

    const key = `drafts:${templateId}:${templateVersion}:values`;
    const inputValuesJson = localStorage.getItem(key);

    if (inputValuesJson) {
      const inputValues = JSON.parse(inputValuesJson);
      const params = new URLSearchParams();

      params.append('templateId', templateId);
      params.append('templateVersion', templateVersion);
      Object.keys(inputValues).forEach(k => {
        params.append(`v_${k}`, inputValues[k]);
      });

      const correlationId = localStorage.getItem(`drafts:${templateId}:${templateVersion}:correlationId`);
      return this.getDocumentResultBody(documentType, params, inputValues, correlationId);
    }

    throw new Error('Input values expected in local storage');
  }

  private getDocumentResultBody(
    documentType: DocumentType,
    documentCreationParams: URLSearchParams,
    inputValues: InputValueCollection,
    correlationId: string): Promise<DocumentResult> {

    const documentUrl = `${getAppSettings().Urls.Api}/documents`;

    let contentType: string = null;
    if (documentType === 'text') {
      contentType = 'text/plain';
    } else if (documentType === 'pdf') {
      contentType = 'application/vnd+document+json';
    } else if (documentType === 'html') {
      contentType = 'application/vnd+document+html';
    } else {
      throw new Error('Unrecognised document type');
    }

    const document$ = this.http.post(documentUrl, {}, {
      headers: new Headers({ 'Content-Type': contentType }),
      params: documentCreationParams
    });

    return document$
      .map(r => {
        if (documentType === 'text') {
          return new TextDocumentResult(r.text(), inputValues, correlationId);
        } else if (documentType === 'pdf') {
          return new SerializableDocumentResult(r.json(), inputValues, correlationId);
        } else if (documentType === 'html') {
          return new HtmlDocumentResult(r.json(), inputValues, correlationId);
        }
      })
      .toPromise();
  }
}
