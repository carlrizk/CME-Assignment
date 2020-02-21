import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PolicyEditComponent } from './policy-edit/policy-edit.component';
import { BeneficiaryModule } from '../beneficiary/beneficiary.module';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';



@NgModule({
  declarations: [
    PolicyEditComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BeneficiaryModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule
  ],
  exports: [
    PolicyEditComponent,
  ]
})
export class PolicyModule { }
