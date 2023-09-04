import { PaginationInfo } from '../paginationInfo.model';
import { CompanyWorkPositionListView } from './companyWorkPositionListView.model';

export interface CompanyWorkPositionsPaged {
    paginationInfo: PaginationInfo;
    workPositions: CompanyWorkPositionListView[];
}
