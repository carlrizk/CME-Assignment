import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';

import { HomeComponent } from './home/home.component';

import { ToolbarComponent } from './toolbar/toolbar.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { BeneficiaryModule } from './beneficiary/beneficiary.module';
import { PolicyModule } from './policy/policy.module';
import { HttpClientModule } from '@angular/common/http';
import { ClaimModule } from './claim/claim.module';
import { CoreModule } from './core/core.module';

@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    HomeComponent,
  ], 
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CoreModule,
    AppRoutingModule,

    HttpClientModule,

    MatToolbarModule,
    MatButtonModule,

    BeneficiaryModule,
    PolicyModule,
    ClaimModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
