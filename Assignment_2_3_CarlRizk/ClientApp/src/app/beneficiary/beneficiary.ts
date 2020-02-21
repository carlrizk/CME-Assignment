import { RelationshipEnum } from './relationship.enum';
import { GenderEnum } from './gender.enum';

export interface IBeneficiary {
    name?: string;
    relationship?: RelationshipEnum;
    gender?: GenderEnum;
    dateOfBirth?: Date;
}