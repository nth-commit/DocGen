import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/fromPromise';

import { TemplateService, Template } from '../core';
import { TemplateSelectDialogComponent } from './components/template-select-dialog/template-select-dialog.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  templates$: Observable<Template[]>;

  private currentDialogRef: MatDialogRef<TemplateSelectDialogComponent, any>;

  constructor(
    private router: Router,
    private matDialog: MatDialog,
    private templateService: TemplateService
  ) { }

  ngOnInit() {
    this.templates$ = Observable.fromPromise(this.templateService.listTemplates());
  }

  createDocument() {
    this.router.navigateByUrl('create');
  }

}
