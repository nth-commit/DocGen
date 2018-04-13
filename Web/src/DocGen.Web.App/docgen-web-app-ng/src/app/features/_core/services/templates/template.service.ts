import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { getAppSettings } from '../../../../app.settings';

import { Template } from '../../models';

@Injectable()
export class TemplateService {

  constructor(
    private http: Http
  ) { }

  listTemplates(): Promise<Template[]> {
    const templatesUrl = this.getTemplateResourceUrl();
    return this.http.get(templatesUrl)
      .map(r => {
        const templates: Template[] = r.json();
        templates.forEach(t => this.processTemplate(t));
        return templates;
      })
      .toPromise();
  }

  getLatestTemplate(id: string): Promise<Template> {
    return this.getTemplate(id, null);
  }

  getTemplate(id: string, version: number): Promise<Template> {
    if (!id) {
      throw new Error('Invalid template id');
    }

    if (version !== null) {
      const templateJson = localStorage.getItem(this.getTemplateKey(id, version));
      if (templateJson) {
        return new Promise(resolve => resolve(JSON.parse(templateJson)));
      }
    }

    const template$ = this.http.get(this.getTemplateResourceUrl(id)).map(r => {
      const result: Template = r.json();
      this.processTemplate(result);
      return result;
    });

    const templatePromise = template$.toPromise();
    templatePromise.then(t => localStorage.setItem(this.getTemplateKey(t.id, t.version), JSON.stringify(t)));
    return templatePromise;
  }

  private processTemplate(template: Template) {
    template.steps.forEach(s => {
      s.inputs.forEach(i => {
          let inputId = s.id;
          if (i.key) {
            inputId += `.${i.key}`;
          }
          i.id = inputId;
      });
    });

    template.name += ' for {{organisation.name}}';
  }

  private getTemplateResourceUrl(id?: string) {
    let result = `${getAppSettings().Urls.Api}/templates`;

    if (id) {
      result += `/${id}`;
    }

    return result;
  }

  private getTemplateKey(id: string, version: number) {
    return `templates:${id}:${version}`;
  }
}
