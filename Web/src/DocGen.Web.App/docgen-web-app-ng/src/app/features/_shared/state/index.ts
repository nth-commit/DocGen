import { GeneratorBulkState } from '../../generator';

export interface CoreState {
  event: EventState;
}

export interface EventState {
  actionType: string;
  reducerId: string;
}

export interface State {
  core: CoreState;
  generatorBulk: GeneratorBulkState;
}
