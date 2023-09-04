export interface CompanyTypeModel {
    companyTypeId: number;
    name: string;
    priceProfit: number;
    priceNonProfit: number;
    numberOfWorkPosition: number | null;
    numberOfStudentsRevealed: number | null;
    companyPresentation: boolean;
    databaseSearch: boolean;
    visible: boolean;
}