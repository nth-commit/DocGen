export { CoreModule } from './core.module';

export
{
    Template,
    TemplateStep,
    TemplateStepConditionType,
    TemplateStepConditionTypeData_EqualsPreviousInputValue,
    TemplateStepInput,
    TemplateStepInputType,
    TemplateStepInputTypeData_Radio,

    InputValue,
    InputValueCollection,

    SerializableDocument,
    Instruction,
    InstructionType,

    BeginWriteListInstruction,
    BeginWriteListItemInstruction,
    BeginWritePageInstruction,
    EndWriteListInstruction,
    EndWriteListItemInstruction,
    EndWritePageInstruction,
    WriteBreakInstruction,
    WriteParagraphBreakInstruction,
    WriteSignaturePlaceholder,
    WriteTextInstruction,

    isBeginWriteList,
    isBeginWriteListItem,
    isBeginWritePage,
    isEndWriteList,
    isEndWriteListItem,
    isEndWritePage,
    isWriteBreak,
    isWriteParagraphBreak,
    isWriteText
} from './models';

export { InputValueCollectionUtility, TemplateUtility } from './utility';

export { TemplateService } from './services/templates/template.service';
