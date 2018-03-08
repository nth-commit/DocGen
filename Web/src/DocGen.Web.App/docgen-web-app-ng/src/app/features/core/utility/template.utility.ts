import { TemplateStep, TemplateStepInput } from '../models';

export const TemplateUtility = {

    getTemplateStepInputId: (templateStep: TemplateStep, templateStepInput: TemplateStepInput) => {
        let inputId = templateStep.id;
        if (templateStepInput.key) {
          inputId += `.${templateStepInput.key}`;
        }
        return inputId;
    }
};
