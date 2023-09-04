import { OrderBy } from '../orderBy.enum';
import { Remote } from '../remote.enum';

export class WorkPositionFilterDto {
    pageNo: number;
    areaOfInterests: number[];
    locations: string[];
    contractTypes: number[];
    name: string;
    remotes: Remote[];
    languages: number[];
    orderBy: OrderBy;
}
