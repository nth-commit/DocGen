import { Component, OnInit, Input } from '@angular/core';

import { TemplateStep } from '../../../core';

@Component({
  selector: 'app-wizard-template-step',
  templateUrl: './template-step.component.html',
  styleUrls: ['./template-step.component.scss']
})
export class TemplateStepComponent implements OnInit {
  @Input() templateStep: TemplateStep;

  constructor() { }

  ngOnInit() {
  }

}
