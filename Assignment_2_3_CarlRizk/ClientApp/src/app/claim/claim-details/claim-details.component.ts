import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IClaim } from '../claim';
import { DataService } from 'src/app/core/data.service';

@Component({
  selector: 'app-claim-details',
  templateUrl: './claim-details.component.html',
  styleUrls: ['./claim-details.component.css']
})
export class ClaimDetailsComponent implements OnInit {

  claim: IClaim;

  constructor(private route: ActivatedRoute, private router: Router, private dataService: DataService) { }

  ngOnInit() {
    let id = this.route.snapshot.params['id'];
    this.getClaim(id);
  }

  getClaim(id: number) {
    this.dataService.getClaim(id)
      .subscribe((claim: IClaim) => {
        this.claim = claim;
      },
        (err) => {
          this.router.navigate(['/404']);
        }
      );
  }
}

