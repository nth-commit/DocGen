export { CoreModule } from './core.module';
export * from './state';

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
    isWriteText,

    HtmlDocument
} from './models';

export { InputValueCollectionUtility, TemplateUtility } from './utility';

export { TemplateService } from './services/templates/template.service';
export {
    LocalStorageDocumentService,
    DocumentCreate,
    DocumentResult,
    IDocumentService
} from './services/documents';
export { RouteChangeService } from './services/route-change/route-change.service';

export {
    TemplateSelectDialogComponent
} from './components';
