

export interface Template {
    id: string;
    name: string;
    version: number;
    description: string;
    markup: string;
    markupVersion: number;
    signingType: TemplateSigningType;
    steps: TemplateStep[];
}

export enum TemplateSigningType {
    Unknown = 0,
    NotSigned,
    Optional,
    Required
}

export interface TemplateStep {
    id: string;
    name: string;
    description: string;
    conditions: TemplateStepCondition[];
    inputs: TemplateStepInput[];
    parentReference: string;
}

export interface TemplateStepCondition {
    type: TemplateStepConditionType,
    typeData: TemplateStepConditionTypeData_EqualsPreviousInputValue;
}

export enum TemplateStepConditionType {
    Unknown = 0,
    EqualsPreviousInputValue,
    IsDocumentSigned
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
