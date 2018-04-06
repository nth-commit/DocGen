import { Action } from '@ngrx/store';

export interface AppAction extends Action {
  reducerId?: string;
}
