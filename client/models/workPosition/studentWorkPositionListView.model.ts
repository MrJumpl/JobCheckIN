import { Remote } from '../remote.enum';
import { MatchCategory } from './../matchCategory.enum';

export interface StudentWorkPositionListView {
    workPositionId: number;
    name: string;
    locationId: string;
    contractTypes: number[];
    remote: Remote;
    publication: string;
    expiration: string;
    companyName: string;
    companyLogo: string;
    match: MatchCategory;
    favorite: boolean;
}
