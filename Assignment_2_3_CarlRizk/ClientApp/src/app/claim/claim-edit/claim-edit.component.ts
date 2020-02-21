import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AsyncValidatorFn, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { IClaim } from '../claim';
import { IPolicy } from 'src/app/policy/policy';
import { Observable, of } from 'rxjs';
import { DataService } from 'src/app/core/data.service';
import { map, catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { IApiResponse } from 'src/app/core/apiresponse';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-claim-edit',
  templateUrl: './claim-edit.component.html',
  styleUrls: ['./claim-edit.component.css']
})
export class ClaimEditComponent implements OnInit {

  claim: IClaim = {};
  policy: IPolicy = {};
  canSubmit: boolean = true;
  errorMessages: string[] = [];

  claimForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private dialog: MatDialog, private dataService: DataService, private router: Router) { }

  ngOnInit() {
    this.claimForm = this.formBuilder.group({
      policyNumber: [this.claim.policyNumber,
      {
        validators: [Validators.required],
        asyncValidators: [this.policyValidator()],
        updateOn: 'blur'
      }],
      incurredDate: [this.claim.incurredDate, [Validators.required, this.claimEditValidator]],
      claimedAmount: [this.claim.claimedAmount, [Validators.required, Validators.min(1)]]
    })
    this.disableInput();
  }

  claimEditValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {

    if (control != null && control.value != null) {
      if (control.value > new Date(this.policy.expiryDate)) {
        return { 'expired': true };
      }
      if (control.value < new Date(this.policy.effectiveDate))
        return { 'notStarted': true };
    }

    return null;
  };

  disableInput() {
    this.claimForm.controls["incurredDate"].disable()
    this.claimForm.controls["claimedAmount"].disable()
  }
  enableInput() {
    this.claimForm.controls["incurredDate"].enable()
    this.claimForm.controls["claimedAmount"].enable()
  }

  policyValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      this.disableInput();
      return this.dataService.getPolicy(control.value).pipe(
        map(res => {
          this.policy = res;
          this.enableInput();
          return null;
        }),
        catchError((err) => of({ "notFound": true })
        )
      )
    }
  }

  onSubmitClick() {
    this.claim.claimedAmount = this.claimForm.value.claimedAmount;
    this.claim.policyNumber = this.claimForm.value.policyNumber;
    this.claim.incurredDate = this.claimForm.value.incurredDate;

    this.canSubmit = false;
    const dialogRef = this.dialog.open(CreateConfirmationDialog, {
      width: '250px'
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result != null) {
        this.dataService.insertClaim(this.claim)
          .subscribe(() => {
            this.router.navigate(['/claims']);
          }, (err: IApiResponse) => {
            this.errorMessages = err.data;
            this.canSubmit = true;
          });
      }else{
        this.canSubmit = true;
      }
    });
  }

}

@Component({
  selector: 'dialog-create-confirmation',
  templateUrl: 'confirmation-create.dialog.html',
})
export class CreateConfirmationDialog {

  constructor(public dialogRef: MatDialogRef<CreateConfirmationDialog>) { }

}