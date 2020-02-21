import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { RelationshipEnum } from '../relationship.enum';
import { GenderEnum } from '../gender.enum';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { IBeneficiary } from '../beneficiary';

@Component({
  selector: 'app-beneficiary-edit',
  templateUrl: './beneficiary-edit.component.html',
  styleUrls: ['./beneficiary-edit.component.css'],
})
export class BeneficiaryEditComponent implements OnInit {

  @Input() beneficiary: IBeneficiary = {};
  @Output() onStateChange : EventEmitter<boolean> = new EventEmitter();
  beneficiaryFormGroup: FormGroup;

  relationships: string[] = Object.keys(RelationshipEnum)
  genders: string[] = Object.keys(GenderEnum)

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.beneficiaryFormGroup = this.formBuilder.group({
      name: [this.beneficiary.name, Validators.required],
      gender: [this.beneficiary.gender, Validators.required],
      relationship: [this.beneficiary.relationship, Validators.required],
      dateOfBirth: [this.beneficiary.dateOfBirth, Validators.required]
    })
  }

  onBeneficiaryChange() {
    this.beneficiary.name = this.beneficiaryFormGroup.value.name
    this.beneficiary.dateOfBirth = this.beneficiaryFormGroup.value.dateOfBirth
    this.beneficiary.gender = this.beneficiaryFormGroup.value.gender
    this.beneficiary.relationship = this.beneficiaryFormGroup.value.relationship
    this.onStateChange.emit(this.beneficiaryFormGroup.valid);
  }


}
