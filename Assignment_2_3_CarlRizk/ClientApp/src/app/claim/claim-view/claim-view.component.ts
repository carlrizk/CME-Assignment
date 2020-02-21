import { Component, OnInit, Inject } from '@angular/core';
import { IClaim } from '../claim';
import { DataService } from 'src/app/core/data.service';
import { IApiResponse } from 'src/app/core/apiresponse';
import { IClaimFilter } from '../claimFilter';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, ValidatorFn, ValidationErrors, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-claim-view',
  templateUrl: './claim-view.component.html',
  styleUrls: ['./claim-view.component.css']
})
export class ClaimViewComponent implements OnInit {

  displayedColumns: string[] = ["id", "incurredDate", "policyNumber", "claimedAmount", "details", "delete"]

  claims: IClaim[] = [];
  filter: IClaimFilter = {};
  errorMessages: string[] = [];

  claimFilterForm: FormGroup;

  constructor(private dataService: DataService, private dialog: MatDialog, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.buildForm();
    this.refreshTable();
  }

  buildForm() {
    this.claimFilterForm = this.formBuilder.group({
      policyNumber: [this.filter.policyNumber],
      amountFrom: [this.filter.amountFrom, this.negativeValidator],
      amountTo: [this.filter.amountTo, this.negativeValidator]
    }, { validators: this.claimFilterValidator });
  }

  negativeValidator(control: AbstractControl) {
    if (control.value == null) return null;
    if (control.value < 0) {
      return { "negative": true };
    } else {
      return null;
    }
  }

  claimFilterValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
    const amountFrom = control.get("amountFrom")
    const amountTo = control.get("amountTo")

    if (amountFrom.value != null && amountTo.value != null)
      if (amountTo.value < amountFrom.value)
        if (!amountTo.getError('negative'))
          amountTo.setErrors({ 'amountToSmallerThanAmountFrom': true });

    return null;
  };

  refreshTable(): void {
    this.filter.policyNumber = this.claimFilterForm.value.policyNumber
    this.filter.amountFrom = this.claimFilterForm.value.amountFrom
    this.filter.amountTo = this.claimFilterForm.value.amountTo

    this.dataService.getClaims(0, 10000, this.filter)
      .subscribe((data) => {
        this.claims = data;
      }, (err: IApiResponse) => {
        this.errorMessages = err.data;
      });
  }

  onDeleteClick(id: number) {
    const dialogRef = this.dialog.open(DeleteConfirmationDialog, {
      width: '250px',
      data: id
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        this.dataService.deleteClaim(result)
          .subscribe(() => {
            this.refreshTable();
          }, (err: IApiResponse) => {
            this.errorMessages = err.data;
          });
      }
    });
  }
}

@Component({
  selector: 'dialog-delete-confirmation',
  templateUrl: 'confirmation-delete.dialog.html',
})
export class DeleteConfirmationDialog {

  constructor(
    public dialogRef: MatDialogRef<DeleteConfirmationDialog>,
    @Inject(MAT_DIALOG_DATA) public id: number) { }

}