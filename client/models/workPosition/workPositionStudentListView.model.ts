import { MatchCategory } from '../matchCategory.enum';

export interface WorkPositionStudentListView {
    studentId: number;
    fullName: string;
    shownInterest: Date;
    photoLink: string;
    match: MatchCategory;
}
