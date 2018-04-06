import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRoutingModule } from './core-routing.module';
import { CoreStateModule } from './state';

@NgModule({
  imports: [
    CommonModule,
    CoreRoutingModule,
    CoreStateModule
  ],
  declarations: []
})
export class CoreModule { }
