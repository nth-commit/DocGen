import { RouterReducerState } from '@ngrx/router-store';
import { GeneratorBulkState } from './generator';

export interface CoreState {
  event: EventState;
  router: RouterReducerState;
}

export interface EventState {
  actionType: string;
  reducerId: string;
}

export interface State {
  core: CoreState;
  generatorBulk: GeneratorBulkState;
}
