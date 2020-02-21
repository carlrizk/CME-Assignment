import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from "@angular/common/http";
import { IPolicy } from "../policy/policy";
import { Observable, throwError } from "rxjs";
import { map, catchError } from 'rxjs/operators';
import { IApiResponse } from "./apiresponse";
import { IClaim } from "../claim/claim";
import { IClaimFilter } from "../claim/claimFilter";

@Injectable({
    providedIn: 'root'
})
export class DataService {

    basePoliciesUrl: string = "/api/policies"
    baseClaimsUrl: string = "/api/claims"

    headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    constructor(private http: HttpClient) { }

    insertPolicy(policy: IPolicy): Observable<IPolicy> {
        return this.http.post<IApiResponse>(this.basePoliciesUrl, policy, { headers: this.headers })
            .pipe(
                map((res: IApiResponse) => res.data),
                catchError(this.handleError)
            )
    }

    getPolicy(id : number) : Observable<IPolicy>{
        return this.http.get<IApiResponse>(this.basePoliciesUrl + "/" + id, { headers: this.headers })
            .pipe(
                map((res: IApiResponse) => res.data),
                catchError(this.handleError)
            )
    }

    insertClaim(claim: IClaim): Observable<IClaim> {
        return this.http.post<IApiResponse>(this.baseClaimsUrl, claim, { headers: this.headers })
            .pipe(
                map((res: IApiResponse) => res.data),
                catchError(this.handleError)
            )
    }

    getClaim(id: number) : Observable<IClaim> {
        return this.http.get<IApiResponse>(this.baseClaimsUrl + "/" + id, { headers: this.headers })
            .pipe(
                map((res: IApiResponse) => res.data),
                catchError(this.handleError)
            )
    }

    getClaims(startingElement: number, numberOfElements: number, filter: IClaimFilter): Observable<IClaim[]> {
        let params = {
            "StartElement" : startingElement.toString(),
            "NumberOfElements" : numberOfElements.toString(),
        }
        if (filter.policyNumber != null) params["PolicyNumber"] =  filter.policyNumber;
        if (filter.amountFrom != null) params["amountFrom"] = filter.amountFrom.toString();
        if (filter.amountTo != null) params["amountTo"] =  filter.amountTo.toString();
        return this.http.get<IApiResponse>(this.baseClaimsUrl, { params: params, headers: this.headers })
            .pipe(
                map((res: IApiResponse) => res.data),
                catchError(this.handleError)
            )
    }

    deleteClaim(id: number): Observable<boolean> {
        return this.http.delete<IApiResponse>(this.baseClaimsUrl + "/" + id)
            .pipe(
                map((res: IApiResponse) => res.data),
                catchError(this.handleError)
            )
    }

    private handleError(error: HttpErrorResponse) {
        return throwError(error.error);
    }
}