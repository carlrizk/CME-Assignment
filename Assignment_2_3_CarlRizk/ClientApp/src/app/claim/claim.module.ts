import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClaimViewComponent, DeleteConfirmationDialog } from './claim-view/claim-view.component';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { ClaimGridComponent } from './claim-grid/claim-grid.component';
import { ClaimDetailsComponent } from './claim-details/claim-details.component';
import { RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ClaimEditComponent, CreateConfirmationDialog } from './claim-edit/claim-edit.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatStepperModule } from '@angular/material/stepper';



@NgModule({
  declarations: [
    ClaimViewComponent,
    ClaimGridComponent,
    ClaimDetailsComponent,
    DeleteConfirmationDialog,
    CreateConfirmationDialog,
    ClaimEditComponent
  ],
  entryComponents: [
    DeleteConfirmationDialog,
    CreateConfirmationDialog
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatStepperModule
  ],
  exports: [ClaimViewComponent, ClaimEditComponent]
})
export class ClaimModule { }
