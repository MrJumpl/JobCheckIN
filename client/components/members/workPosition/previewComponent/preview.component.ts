import { Component, Input, ViewChild } from '@angular/core';

import { WebApiErrorResponse } from '../../../../../../_shared/models/webapi-error-response';
import { MuniWebPopUpComponent } from '../../../../../../_templates/muni-web/components/pop-up/pop-up.component';
import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { MuniWebButtonState } from '../../../../../../_templates/muni-web/models/button-state';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { CompanyService } from '../../../../services/company.service';
import { ModelUtils } from '../../../../utils/modelUtils';

@Component({
    selector: 'jobchin-workPosition-preview',
    templateUrl: 'preview.component.html',
})
export class WorkPositionPreviewComponent {
    MuniWebButtonSize = MuniWebButtonSize;

    previewState = MuniWebButtonState.init;
    preview: string;
    previewError: string[];

    @Input() getModelFunc: () => any = () => {
        return ModelUtils.deepCopy(this.profile.company.workPosition);
    }

    @ViewChild(MuniWebPopUpComponent) popUp: MuniWebPopUpComponent;

    constructor(private profile: JobChINProfileConfig, private companyService: CompanyService) { }

    getPreview() {
        this.previewState = MuniWebButtonState.loading;
        this.preview = null;
        this.previewError = null;

        let model = this.getModelFunc();
        if (!model) {
            this.previewError = [ 'Některá pole nejsou správně vyplněna, proto není náhled k dispozici.' ];
            this.previewState = MuniWebButtonState.error;
            this.popUp.openPopup();
            return;
        }

        this.companyService.getWorkPositionPreview(this.getModelFunc()).subscribe(
            result => {
                this.preview = result;
                this.previewState = MuniWebButtonState.success;
                this.popUp.openPopup();
            },
            (err: WebApiErrorResponse) => {
                this.previewError = err.messages;
                this.previewState = MuniWebButtonState.error;
                this.popUp.openPopup();
            }
        );
    }
}
