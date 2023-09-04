import { Component } from '@angular/core';

@Component({
    selector: 'jobchin-workPosition-expired',
    template: `
        <muniweb-notice
            noticeType="error"
        >
            <p>
                Pracovní pozici není možné upravovat po expiraci
            </p>
        </muniweb-notice>
    ` ,
})
export class WorkPositionExpiredComponent {
}
