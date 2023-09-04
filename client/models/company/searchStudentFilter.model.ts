export class SearchStudentFilterDto {
    pageNo: number;
    workPositionId: number;
    contractTypes: number[];
    activeDriver: boolean;
    drivingLicense: boolean;
    areaOfInterests: number[];
    hardSkills: number[];
    softSkills: number[];
    faculties: string[];
}