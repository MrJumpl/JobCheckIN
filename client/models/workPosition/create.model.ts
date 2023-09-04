import { BasicInfoModel } from './basicInfo.model';
import { CandidateRequestModel } from './candidateRequest.model';
import { CandidatesModel } from './candidates.model';
import { DetailModel } from './detail.model';
import { VisibilityModel } from './visibility.model';


export class WorkPositionCreateModel {
    visibility: VisibilityModel;
    basicInfo: BasicInfoModel;
    detail: DetailModel;
    candidates: CandidatesModel;
    candidateRequest: CandidateRequestModel;
}
