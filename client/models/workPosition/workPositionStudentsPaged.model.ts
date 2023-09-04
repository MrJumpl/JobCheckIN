import { PaginationInfo } from '../paginationInfo.model';
import { WorkPositionStudentListView } from './workPositionStudentListView.model';

export interface WorkPositionStudentsPaged {
    paginationInfo: PaginationInfo;
    students: WorkPositionStudentListView[];
}
