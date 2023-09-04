import { Remote } from '../remote.enum';

export interface BasicInfoModel {
    expiration: Date;
    publication: Date;
    jobBeginning: Date;
    name: string;
    language: string;
    remote: Remote;
    locationId: string;
    branchId: number;
    contractTypes: number[];
    users: number[];
    public: boolean;
}


