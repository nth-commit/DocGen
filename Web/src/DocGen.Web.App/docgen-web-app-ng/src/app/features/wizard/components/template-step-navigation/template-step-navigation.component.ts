import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-wizard-template-step-navigation',
  templateUrl: './template-step-navigation.component.html',
  styleUrls: ['./template-step-navigation.component.scss']
})
export class TemplateStepNavigationComponent implements OnInit {
  @Input() hasNextStep: boolean;
  @Input() hasPreviousStep: boolean;
  @Input() currentStepValid: boolean;
  @Output() nextStepClick = new EventEmitter();
  @Output() previousStepClick = new EventEmitter();
  @Output() completeClick = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onNextClick($event) {
    if (this.hasNextStep) {
      this.nextStepClick.emit();
    } else {
      this.completeClick.emit();
    }
  }

  onPreviousClick($event) {
    this.previousStepClick.emit();
  }
}
