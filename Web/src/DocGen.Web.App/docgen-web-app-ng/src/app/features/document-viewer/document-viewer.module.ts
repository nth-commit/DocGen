import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { MatToolbarModule, MatButtonModule, MatDialogModule } from '@angular/material';

import { environment } from '../../../environments/environment';

import { documentViewerReducer, documentViewerMetaReducers, DocumentViewerEffects } from './reducers';

import { DocumentViewerRoutingModule } from './document-viewer-routing.module';

import { DocumentRenderingService } from './services/document-rendering/document-rendering.service';

import { DocumentViewerPageComponent } from './pages/document-viewer/document-viewer-page.component';
import { TextDocumentComponent } from './components/text-document/text-document.component';
import { PdfDocumentComponent } from './components/pdf-document/pdf-document.component';
import { SignDocumentModalComponent } from './components/sign-document-modal/sign-document-modal.component';
import { HtmlDocumentComponent } from './components/html-document/html-document.component';

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('documentViewer', documentViewerReducer, { metaReducers: documentViewerMetaReducers }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    EffectsModule.forFeature([DocumentViewerEffects]),
    MatToolbarModule, MatButtonModule, MatDialogModule,
    DocumentViewerRoutingModule
  ],
  providers: [
    DocumentRenderingService
  ],
  declarations: [
    DocumentViewerPageComponent,
    TextDocumentComponent,
    PdfDocumentComponent,
    SignDocumentModalComponent,
    HtmlDocumentComponent
  ],
  entryComponents: [
    SignDocumentModalComponent
  ]
})
export class DocumentViewerModule { }
