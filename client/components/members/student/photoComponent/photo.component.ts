import { Component, EventEmitter, Input, Output } from '@angular/core';


@Component({
    selector: 'jobchin-student-photo',
    templateUrl: 'photo.component.html',
    styleUrls: [ './photo.component.scss' ],
})
export class PhotoComponent {
    photoPopUp = false;

    @Input() size = 50;
    @Input() cardClickable = false;
    @Input() photoClickable = false;
    @Input() fullName: string;
    @Input() studentId: number;
    @Input() photoLink: string;

    @Output() onCardClick = new EventEmitter<any>();
    @Output() onPhotoClick = new EventEmitter<any>();

    constructor() { }

    photoClick() {
        this.onPhotoClick?.emit();
    }

    cardClick() {
        this.onCardClick?.emit();
    }
}
