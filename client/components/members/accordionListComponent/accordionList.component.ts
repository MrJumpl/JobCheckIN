import { Component, EventEmitter, Input, Output, TemplateRef } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';

import { MuniWebButtonSize } from '../../../../../_templates/muni-web/models/button-size';


@Component({
    selector: 'jobchin-accordionList',
    templateUrl: 'accordionList.component.html',
})
export class AccordionListComponent {
    MuniWebButtonSize = MuniWebButtonSize;

    @Input() itemForm: TemplateRef<any>;
    @Input() addButtonLabel: string;
    @Input() removeButtonLabel: string;
    @Input() formArray: FormArray;
    @Input() maxItems: number;
    @Input() getTitle: (x: FormGroup) => string = _ => '';
    @Input() isNew: (x: FormGroup) => boolean = _ => true;

    @Output() add: EventEmitter<any> = new EventEmitter<any>();
    @Output() remove: EventEmitter<number> = new EventEmitter<number>();

    constructor() {
    }

    getContext(i: number, item: FormGroup) {
        return {
            $implicit: i,
            item: item,
        };
    }

    showAccordion() {
        return this.formArray.length > 0;
    }

    showAdd() {
        if (this.maxItems) {
            return this.formArray.length < this.maxItems;
        }
        return true;
    }

    createOpen(group: FormGroup, i: number): boolean {
        return this.isNew(group);
    }

    onAdd() {
        this.add.emit();
    }

    onRemove(i: number) {
        this.remove.emit(i);
    }

    getItemClass(group: FormGroup): string {
        return group.invalid ? 'accordion-item-error' : '';
    }
}
