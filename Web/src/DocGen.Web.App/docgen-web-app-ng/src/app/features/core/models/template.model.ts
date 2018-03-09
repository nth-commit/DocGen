

export interface Template {
    id: string;
    name: string;
    verions: number;
    description: string;
    markup: string;
    markupVersion: number;
    steps: TemplateStep[];
}

export interface TemplateStep {
    id: string;
    name: string;
    description: string;
    conditionType?: TemplateStepConditionType;
    conditionTypeData?: TemplateStepConditionTypeData_EqualsPreviousInputValue;
    inputs: TemplateStepInput[];
    parentReference: string;
}

export enum TemplateStepConditionType {
    Unknown = 0,
    EqualsPreviousInputValue
}

export interface TemplateStepConditionTypeData_EqualsPreviousInputValue {
    PreviousInputId: string;
    PreviousInputValue: any;
}

export interface TemplateStepInput {
    id: string;
    key: string;
    name: string;
    description: string;
    hint: string;
    type: TemplateStepInputType;
    typeData?: TemplateStepInputTypeData_Radio;
}

export enum TemplateStepInputType {
    Unknown = 0,
    Text,
    Radio,
    Checkbox
}

export interface TemplateStepInputTypeData_Radio {
    name: string;
    value: string;
}
