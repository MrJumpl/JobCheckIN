import { Sector } from '../sector.enum';

export interface GeneralInfoModel {
    ico: string;
    dic: string;
    companyName: string;
    street: string;
    city: string;
    zipCode: string;
    countryId: number;
    correspondenceStreet: string;
    correspondenceCity: string;
    correspondenceZipCode: string;
    correspondenceCountryId: number;
    workerCountRangeId: number;
    sector: Sector;
    gdprAgreement: boolean;
}
