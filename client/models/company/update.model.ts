import { CandidatesModel } from './candidates.model';
import { CompanyBranches } from './companyBranches.model';
import { GeneralInfoModel } from './generalInfo.model';
import { PresentationModel } from './presentation.model';

export class CompanyUpdateModel {
    generalInfo: GeneralInfoModel;
    candidates: CandidatesModel;
    presentation: PresentationModel;
    branches: CompanyBranches;
}
