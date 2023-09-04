import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { UmbracoService } from '../../../../../../_shared/services/umbraco.service';
import { FormConfig } from '../../../../../../_templates/muni-web/components/form/form2.component';
import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { MuniWebButtonState } from '../../../../../../_templates/muni-web/models/button-state';
import { StudentShowInterestConfig } from '../../../../config/studentShowInterestConfig';
import { ShowInterestModel } from '../../../../models/student/showInterest.model';
import { StudentService } from '../../../../services/student.service';

@Component({
    selector: 'jobchin-student-showInterest',
    templateUrl: 'showInterest.component.html',
    styleUrls: [ './showInterest.component.scss' ],
    providers: [
        UmbracoService.typedConfig(StudentShowInterestConfig)
    ]
})
export class StudentShowInterestComponent {
    MuniWebButtonSize = MuniWebButtonSize;

    favoriteState = MuniWebButtonState.init;
    formConfig: FormConfig<ShowInterestModel, any>;

    constructor(private studentService: StudentService, private config: StudentShowInterestConfig) {
        this.formConfig = {
            form: new FormGroup({
                additionalQuestion: new FormControl(null, this.config.additionalQuestions ? Validators.required : []),
                coverLetter: new FormControl(null, this.config.coverLetter ? Validators.required : []),
            }),
            onGetFormModel: form => ({
                workPositionId: this.config.workPositionId,
                additionalQuestion: form.controls.additionalQuestion.value,
                coverLetter: form.controls.coverLetter.value,
            }),
            onCallServer: (model) => this.studentService.showInterest(model),
            onServerCallback: (result, model) => this.config.hasShownInterest = true,
        }
    }

    getFavoriteLabel() {
        if (this.config.favorite === false) {
            return $localize`:@@JobChIN.studdentAddFavorite:Přidat do oblíbených`
        } else if (this.config.favorite === true) {
            return $localize`:@@JobChIN.studdentRemoveFavorite:Odebrat z oblíbených`
        } else {
            return ''
        }
    }

    hasCoverLetter() {
        return this.config.coverLetter === true;
    }
    
    hasAdditionalQuestions() {
        return this.config.additionalQuestions;
    }

    getHasShownInterest() {
        return this.config.hasShownInterest;
    }

    getInterestText() {
        return this.config.showInterestText;
    }

    getAdditionalQuestions() {
        return this.config.additionalQuestions;
    }

    onFavoriteClick() {
        this.favoriteState = MuniWebButtonState.loading;
        let interested = !this.config.favorite;
        this.studentService.favoriteWorkPosition(this.config.workPositionId, interested).subscribe(
            _ => {
                this.config.favorite = interested;
                this.favoriteState = MuniWebButtonState.success;
            },
            _ => {
                this.favoriteState = MuniWebButtonState.error;
            }
        )
    }

    redirectProfile() {
        window.location.href = this.config.profileRoute;
    }
}
