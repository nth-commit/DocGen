import { MetaReducer } from '@ngrx/store';
import { environment } from '../../../../environments/environment';
import { DocumentViewerState } from './document-viewer.state';

export { documentViewerReducer } from './document-viewer.reducer';
export { DocumentViewerEffects } from './document-viewer.effects';

export const documentViewerMetaReducers: MetaReducer<DocumentViewerState>[] = !environment.production ? [] : [];
