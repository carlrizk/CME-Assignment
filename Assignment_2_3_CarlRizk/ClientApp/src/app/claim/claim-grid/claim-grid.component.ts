import { Component, Input, Output, EventEmitter } from '@angular/core';
import { IClaim } from '../claim';

@Component({
  selector: 'app-claim-grid',
  templateUrl: './claim-grid.component.html',
  styleUrls: ['./claim-grid.component.css']
})
export class ClaimGridComponent{

  @Input() claims: IClaim[] = [];
  @Output() deleteClicked = new EventEmitter<number>();

  displayedColumns: string[] = ["id", "incurredDate", "policyNumber", "claimedAmount", "details", "delete"]

  constructor() { }

  onDeleteClick(id : number) : void {
    this.deleteClicked.emit(id);
  }

}
