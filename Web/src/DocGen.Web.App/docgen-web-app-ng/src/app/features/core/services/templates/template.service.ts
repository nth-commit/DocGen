import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { getAppSettings } from '../../../../app.settings';

import { Template } from '../../models';

@Injectable()
export class TemplateService {

  constructor(
    private http: Http
  ) { }

  getLatestTemplate(id: string): Promise<Template> {
    return this.getTemplate(id, null);
  }

  getTemplate(id: string, version: number): Promise<Template> {
    if (version !== null) {
      const templateJson = localStorage.getItem(this.getTemplateKey(id, version));
      if (templateJson) {
        return new Promise(resolve => resolve(JSON.parse(templateJson)));
      }
    }

    const template$ = this.http.get(`${getAppSettings().Urls.Api}/templates/${id}`).map(r => {
      const result: Template = r.json();

      result.steps.forEach(s => {
          s.inputs.forEach(i => {
              let inputId = s.id;
              if (i.key) {
                inputId += `.${i.key}`;
              }
              i.id = inputId;
          });
      });

      return result;
    });

    const templatePromise = template$.toPromise();
    templatePromise.then(t => localStorage.setItem(this.getTemplateKey(t.id, t.version), JSON.stringify(t)));
    return templatePromise;
  }

  private getTemplateKey(id: string, version: number) {
    return `templates:${id}:${version}`;
  }
}
