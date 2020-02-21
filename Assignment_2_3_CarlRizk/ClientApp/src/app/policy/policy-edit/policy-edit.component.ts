import { Component, OnInit } from '@angular/core';
import { IPolicy } from '../policy';
import { DataService } from 'src/app/core/data.service';
import { Router } from '@angular/router';
import { FormGroup, Validators, ValidatorFn, ValidationErrors, FormBuilder } from '@angular/forms';
import { IApiResponse } from 'src/app/core/apiresponse';

@Component({
  selector: 'app-policy-edit',
  templateUrl: './policy-edit.component.html',
  styleUrls: ['./policy-edit.component.css']
})
export class PolicyEditComponent implements OnInit {

  policy: IPolicy = {
    beneficiaries: [],
  };
  errorMessages: string[] = [];
  canSubmit: boolean = true;

  policyForm: FormGroup;
  beneficariesValid: boolean = false;

  constructor(private router: Router, private dataService: DataService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    this.policyForm = this.formBuilder.group({
      policyNumber: { value: this.policy.policyNumber, disabled: true },
      effectiveDate: [this.policy.effectiveDate, Validators.required],
      expiryDate: [this.policy.expiryDate, Validators.required],
    }, { validators: this.policyValidator })
  }

  policyValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
    const effectiveDate = control.get("effectiveDate")
    const expiryDate = control.get("expiryDate")

    let now = new Date();

    if (effectiveDate.value != null)
      if (effectiveDate.value < now)
        if (!effectiveDate.getError('required'))
          effectiveDate.setErrors({ "smallerThanToday": true });


    if (effectiveDate.value != null && expiryDate.value != null)
      if (expiryDate.value <= effectiveDate.value)
        if (!expiryDate.getError('required'))
          expiryDate.setErrors({ 'smallerThanEffectiveDate': true });

    return null;
  };

  onSubmitClick() {
    this.policy.effectiveDate = this.policyForm.value.effectiveDate;
    this.policy.expiryDate = this.policyForm.value.expiryDate;
    this.policy.policyNumber = this.policyForm.value.policyNumber;
    if (this.policy.policyNumber) {
      //Update not part of this assignment
      console.log("Update not part of this assignment");
    } else {
      this.canSubmit = false;
      this.dataService.insertPolicy(this.policy)
        .subscribe(() => {
          this.router.navigate(['/']);
        }, (err: IApiResponse) => {
          this.errorMessages = err.data;
          this.canSubmit = true;
        });
    }
  }

  onPolicyChange(state: boolean) {
    this.beneficariesValid = state;
  }

}
