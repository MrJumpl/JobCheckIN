import { PaginationInfo } from '../paginationInfo.model';
import { StudentWorkPositionListView } from './studentWorkPositionListView.model';

export interface StudentWorkPositionsPaged {
    paginationInfo: PaginationInfo;
    workPositions: StudentWorkPositionListView[];
}
