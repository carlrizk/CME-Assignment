import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PolicyEditComponent } from './policy/policy-edit/policy-edit.component';
import { ClaimViewComponent } from './claim/claim-view/claim-view.component';
import { ClaimDetailsComponent } from './claim/claim-details/claim-details.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ClaimEditComponent } from './claim/claim-edit/claim-edit.component';


const routes: Routes = [
  { path: "home", component: HomeComponent },
  { path: "policies", component: PolicyEditComponent },
  { path: "claims", component: ClaimViewComponent },
  { path: 'claims/new', component: ClaimEditComponent },
  { path: 'claims/:id', component: ClaimDetailsComponent },
  { path: '404', component: NotFoundComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: '404', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
