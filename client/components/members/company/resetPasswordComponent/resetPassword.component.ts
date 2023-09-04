import { Component } from '@angular/core';

import { EntyMenuState } from '../../../../../../members/models/entry-menu-state.enum';
import { RouterUtils } from '../../../../utils/routerUtils';

@Component({
    selector: 'jobchin-resetPassword',
    templateUrl: 'resetPassword.component.html',
})
export class ResetPasswordComponent {
    EntyMenuState = EntyMenuState;
    RouterUtils = RouterUtils;

    constructor() {
    }
}
