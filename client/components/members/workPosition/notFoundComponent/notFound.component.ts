import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { jobchinRoutes } from '../../../../jobchin.routes';

@Component({
    selector: 'jobchin-workPosition-notFound',
    templateUrl: 'notFound.component.html',
})
export class WorkPositionNotFoundComponent {
    MuniWebButtonSize = MuniWebButtonSize;

    constructor(protected router: Router, private route: ActivatedRoute) { }

    redirectList() {
        this.router.navigate([ `../../${jobchinRoutes.myOffres}` ], { relativeTo: this.route });
    }
}
