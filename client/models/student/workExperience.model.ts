export interface WorkExperinceModel {
    workExperienceId: number;
    position: string;
    companyName: string;
    city: string;
    areaOfInterestId: number;
    from: Date;
    to: Date;
    paid: boolean;
    contractTypeId: number;
    contactPerson: string;
    contact: string;
    description: string;
}
