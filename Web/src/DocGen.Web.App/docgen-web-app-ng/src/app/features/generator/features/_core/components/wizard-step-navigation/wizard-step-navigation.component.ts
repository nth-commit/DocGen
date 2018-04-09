import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-generator-wizard-step-navigation',
  templateUrl: './wizard-step-navigation.component.html',
  styleUrls: ['./wizard-step-navigation.component.scss']
})
export class WizardStepNavigationComponent implements OnInit, OnChanges {
  @Input() completeText: string;
  @Input() hasNextStep: boolean;
  @Input() hasPreviousStep: boolean;
  @Input() currentStepValid: boolean;
  @Output() nextStepClick = new EventEmitter();
  @Output() previousStepClick = new EventEmitter();
  @Output() completeClick = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.completeText = this.completeText || 'Done!';
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
