import { TemplateStep, TemplateStepInput } from '../models';

export const TemplateUtility = {

  getInputId: (step: TemplateStep, stepInput: TemplateStepInput): string => {
    let result = step.id;
    if (stepInput.id) {
      result += '.' + stepInput.id;
    }
    return result;
  },

  getInputName: (step: TemplateStep, stepInput: TemplateStepInput): string => {
    return stepInput.name || step.name;
  }
};
