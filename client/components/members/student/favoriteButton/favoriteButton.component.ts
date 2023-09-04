import { Component, Input } from '@angular/core';

import { MuniWebButtonState } from '../../../../../../_templates/muni-web/models/button-state';

@Component({
    selector: 'jobchin-student-favoriteButton',
    templateUrl: 'favoriteButton.component.html',
    styleUrls: [ './favoriteButton.component.scss' ],
})
export class StudentFavoriteButtonComponent {
    MuniWebButtonState = MuniWebButtonState;

    @Input() favorite: boolean;

    constructor() { }
}
