import { Component } from '@angular/core';

@Component({
    selector: 'jobchin-error',
    template: `
        <muniweb-notice
            noticeType="error"
        >
            <p>
                Nastala chyba na serveru
            </p>
        </muniweb-notice>
    ` ,
})
export class ErrorComponent {
}
