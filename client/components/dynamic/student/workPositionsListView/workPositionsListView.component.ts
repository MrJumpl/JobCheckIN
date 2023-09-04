import { Component, Input } from '@angular/core';

import { UmbracoService } from '../../../../../../_shared/services/umbraco.service';
import { StudentOffersConfig } from '../../../../config/studentOffersConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { Remote } from '../../../../models/remote.enum';
import { StudentWorkPositionListView } from '../../../../models/workPosition/studentWorkPositionListView.model';
import { StudentService } from '../../../../services/student.service';

@Component({
    selector: 'jobchin-student-workPositionsListView',
    templateUrl: 'workPositionsListView.component.html',
    styleUrls: [ './workPositionsListView.component.scss', '../../../../assets/match.scss' ],
    providers: [
        UmbracoService.typedConfig(StudentOffersConfig)
    ],
})
export class StudentWorkPositionsListViewComponent {

    @Input() workPositions: StudentWorkPositionListView[];

    constructor(public umbraco: UmbracoService, private studentService: StudentService, private config: StudentOffersConfig) { }

    getLocationName(locationId: string): string {
        return this.config.localAdministrativeUnits.find(x => x.value === locationId)?.displayValue;
    }

    getContractsNames(contractTypes: number[]): string {
        return contractTypes.map(x => this.getContractName(x)).filter(x => x).join(', ');
    }

    getRemote(remote: Remote) {
        if (remote === Remote.No) {
            return '';
        }
        return `| ${jobchinSettings.remoteTypes.find(x => x.value === remote)?.displayValue}`;
    }

    getLogo(logo: string) {
        return logo || this.config.companyNoLogoLink;
    }

    onFavoriteClick(workPosition: StudentWorkPositionListView) {
        let fav = !workPosition.favorite;
        workPosition.favorite = null;
        this.studentService.favoriteWorkPosition(workPosition.workPositionId, fav).subscribe(
            _ => workPosition.favorite = fav,
            _ => workPosition.favorite = !fav,
        );
    }

    private getContractName(contractTypeId: number) {
        return this.config.contractTypes.find(x => x.contractTypeId === contractTypeId)?.name;
    }
}
