import { Component } from '@angular/core';

import { MuniWebButtonState } from '../../../../../../_templates/muni-web/models/button-state';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { DownloadResult } from '../../../../models/downloadResult';
import { StudentService } from '../../../../services/student.service';


@Component({
    selector: 'jobchin-student-header',
    templateUrl: 'studentHeader.component.html',
    styleUrls: [ './studentHeader.component.scss' ],
})
export class StudentHeaderComponent {
    photoPopUp = false;
    donwloadState: MuniWebButtonState = MuniWebButtonState.init;

    constructor(private studentService: StudentService, private profile: JobChINProfileConfig) { }

    downloadCv() {
        this.donwloadState = MuniWebButtonState.loading;
        this.studentService.downloadDocxCv().subscribe(
            (result: DownloadResult) => {
                this.donwloadState = MuniWebButtonState.success;
                const link = document.createElement('a');
                link.href = URL.createObjectURL(result.data);
                link.download = result.name;
                link.click();
            },
            () => {
                this.donwloadState = MuniWebButtonState.error;
            }
        )
    }

    getStudentId() {
        return this.profile.student.studentId;
    }

    getPhotoLink() {
        return this.profile.student.model?.photo?.photo?.link;
    }

    getFullName() {
        return this.profile.student.fullName;
    }

    lastTimeUpdatedByStudent() {
        return this.profile.student.lastTimeUpdatedByStudent;
    }

    openPhotoPopUp() {
        this.photoPopUp = true;
    }

    closePhotoPopUp() {
        this.photoPopUp = false;
    }
}
