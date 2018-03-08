import { Template } from '../../core';

import { Utility } from '../utility';
import { InputValue } from './input-value.model';

export interface InputValueCollection {
    [key: string]: InputValue;
}

export const createInputValueCollection = (template: Template): InputValueCollection => {
    const result: InputValueCollection = {};

    template.steps.forEach(s => {
      s.inputs.forEach(i => {
        const inputId = Utility.getTemplateStepInputId(s, i);
        result[inputId] = null;
      });
    });

    return result;
};