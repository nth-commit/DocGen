import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { Template } from '../../../core';

@Component({
  selector: 'app-template-select-dialog',
  templateUrl: './template-select-dialog.component.html',
  styleUrls: ['./template-select-dialog.component.scss']
})
export class TemplateSelectDialogComponent implements OnInit {
  @Input() templates: Template[];
  @Output() templateSelected = new EventEmitter<Template>();

  constructor() { }

  ngOnInit() {
  }

}
