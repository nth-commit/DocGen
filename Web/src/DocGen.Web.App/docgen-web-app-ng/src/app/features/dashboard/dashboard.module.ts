import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatDialogModule, MatAutocompleteModule, MatFormFieldModule, MatInputModule } from '@angular/material';

import { CoreModule } from '../_core';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { TemplateSelectDialogComponent } from './components/template-select-dialog/template-select-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DashboardRoutingModule,
    MatButtonModule,
    MatDialogModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatInputModule,
    CoreModule
  ],
  declarations: [DashboardComponent, TemplateSelectDialogComponent],
  entryComponents: [TemplateSelectDialogComponent]
})
export class DashboardModule { }
