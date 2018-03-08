import { TemplateStep, TemplateStepInput } from '../core';

export const Utility = {

    getTemplateStepInputId: (templateStep: TemplateStep, templateStepInput: TemplateStepInput) => {
        let inputId = templateStep.id;
        if (templateStepInput.key) {
          inputId += `.${templateStepInput.key}`;
        }
        return inputId;
    }
};
