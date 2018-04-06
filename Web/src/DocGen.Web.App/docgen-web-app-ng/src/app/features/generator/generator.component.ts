import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { TemplateSelectDialogComponent, Template, TemplateService } from '../core';

declare type GenerationMode = 'single' | 'bulk';

@Component({
  selector: 'app-generator',
  templateUrl: './generator.component.html',
  styleUrls: ['./generator.component.scss']
})
export class GeneratorComponent implements OnInit {

  templates$: Observable<Template[]>;

  private completed = false;
  private currentDialogRef: MatDialogRef<TemplateSelectDialogComponent, any>;
  private _selectedMode = new BehaviorSubject<GenerationMode>(null);

  constructor(
    private matDialog: MatDialog,
    private templateService: TemplateService,
    private router: Router
  ) { }

  ngOnInit() {
    this.templates$ = Observable.fromPromise(this.templateService.listTemplates());

    this._selectedMode
      .asObservable()
      .debounceTime(750)
      .subscribe(selectedMode => {
        if (selectedMode && selectedMode === this.selectedMode) {

          this.templates$.first().subscribe(templates => {

            this.currentDialogRef = this.matDialog.open(TemplateSelectDialogComponent, {
              width: '600px'
            });

            this.currentDialogRef.componentInstance.templates = templates;

            this.currentDialogRef.componentInstance.templateSelected.first().subscribe(template => {
              this.completed = true;
              setTimeout(() => {
                if (this.selectedMode === 'single') {
                  this.router.navigateByUrl(`create/${template.id}`);
                } else {
                  this.router.navigateByUrl(`create/${template.id}/bulk`);
                }
              }, 500);
            });

            this.currentDialogRef.componentInstance.templateSelectionCancelled.first().subscribe(() => {
              if (!this.completed) {
                setTimeout(() => {
                  this.selectMode(null);
                }, 500);
              }
            });

            this.currentDialogRef.afterClosed().first().subscribe(() => this.currentDialogRef = null);
          });
        }
      });
  }

  get selectedMode(): GenerationMode {
    return this._selectedMode.value;
  }

  selectMode(mode: GenerationMode) {
    if (!this.completed) {
      this._selectedMode.next(mode);
    }
  }
}
