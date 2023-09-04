import { Directive, OnInit } from '@angular/core';

import { JobChINProfileConfig } from '../../../config/profileConfig';
import { StudentUpdateModel } from '../../../models/student/update.model';
import { StudentService } from '../../../services/student.service';
import { BaseForm } from '../baseForm/baseForm';

/**
 * Base class for custom implementations of data overview actions (e.g. some operations with selected records which require some UI)
 */
@Directive()
export abstract class StudentBaseForm<T> extends BaseForm<T, StudentUpdateModel> implements OnInit {

    constructor(protected readonly studentService: StudentService, protected readonly profile: JobChINProfileConfig) {
        super();
    }

    ngOnInit(): void {
        if (this.standAlone) {
            this.model = this.profile.student.model[this.propertyName];
            this.onCallServer = (model) => {
                let dto = new StudentUpdateModel();
                dto[this.propertyName] = model;
                return this.studentService.updateStudent(dto);
            };
        }
    }

    abstract get propertyName(): string;

    override onServerCallback(result: StudentUpdateModel, model: T): void {
        this.model = result[this.propertyName];
    }
}
