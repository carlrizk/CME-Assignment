import { IBeneficiary } from '../beneficiary/beneficiary'

export interface IPolicy {
    policyNumber?: string;
    effectiveDate?: Date;
    expiryDate?: Date;
    beneficiaries?: IBeneficiary[];
}
