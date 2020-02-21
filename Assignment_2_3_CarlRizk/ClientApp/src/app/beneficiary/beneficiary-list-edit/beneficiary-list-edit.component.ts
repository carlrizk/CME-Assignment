import { Component, Input, Output, EventEmitter } from '@angular/core';
import { IBeneficiary } from '../beneficiary';

@Component({
  selector: 'app-beneficiary-list-edit',
  templateUrl: './beneficiary-list-edit.component.html',
  styleUrls: ['./beneficiary-list-edit.component.css'],
})
export class BeneficiaryListEditComponent {

  @Input() beneficiaries: IBeneficiary[] = [];
  @Output() onStateChange: EventEmitter<boolean> = new EventEmitter();

  rowsValid: boolean[] = [];

  constructor() { }

  onAddClick(): void {
    this.beneficiaries.push({});
    this.rowsValid.push(false);
    this.onStateChange.emit(this.isValid())
  }

  onDeleteClick(id): void {
    this.beneficiaries.splice(id, 1);
    this.rowsValid.splice(id, 1);
    this.onStateChange.emit(this.isValid())
  }

  onBeneficiariesChange(id: number, state: boolean) {
    this.rowsValid[id] = state;
    this.onStateChange.emit(this.isValid())
  }

  isValid(): boolean {
    if(this.beneficiaries.length < 1) return false;
    return this.rowsValid.every(r => r == true);
  }
}
