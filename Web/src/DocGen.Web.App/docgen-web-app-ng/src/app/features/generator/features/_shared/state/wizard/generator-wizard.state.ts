import { TemplateStep, InputValueCollection } from '../../../../../core';

export interface GeneratorWizardState {
  id: string;

  steps: TemplateStep[];

  step: TemplateStep;
  stepIndex: number;
  stepIndexHistory: number[];

  nextStepIndex: number;
  hasNextStep: boolean;
  hasPreviousStep: boolean;

  values: InputValueCollection;
  stepValues: InputValueCollection;

  valid: boolean;
  stepValid: boolean;
  stepsValid: boolean[];
  stepInputsValid: boolean[][];

  empty: boolean;
  progress: number;
}
