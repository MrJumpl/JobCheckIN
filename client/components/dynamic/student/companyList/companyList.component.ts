import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { UmbracoService } from '../../../../../../_shared/services/umbraco.service';
import { StudentCompanyListConfig } from '../../../../config/studentCompanyListConfig';
import { CompanyFilter } from '../../../../models/company/companyFilter.model';
import { StudentCompanyListView } from '../../../../models/company/studentCompanyListView.model';
import { StudentService } from '../../../../services/student.service';

@Component({
    selector: 'jobchin-student-companyList',
    templateUrl: 'companyList.component.html',
    styleUrls: [ './companyList.component.scss', '../../../../assets/detailedFilter.scss', '../../../../assets/button-aligment.scss' ],
    providers: [
        UmbracoService.typedConfig(StudentCompanyListConfig)
    ],
})
export class StudentCompanyListComponent {
    filterForm: FormGroup;
    detailedFilter = false;
    companies: StudentCompanyListView[];
    filter: CompanyFilter;
    loading = false;
    error = false;
    
    @ViewChild("detailedFilterRef") detailedFilterRef!: ElementRef;

    constructor(private studentService: StudentService, public config: StudentCompanyListConfig, private renderer: Renderer2) {
        this.filterForm = new FormGroup({
            areaOfInterests: new FormControl([]),
            name: new FormControl(''),
        });

        this.filter = new CompanyFilter();
        this.loadCompanies();
    }

    loadCompanies() {
        this.companies = null;
        this.loading = true;
        this.filter.areaOfInterests = this.filterForm.controls.areaOfInterests.value;
        this.filter.companyName = this.filterForm.controls.name.value;
        this.studentService.getCompanies(this.filter).subscribe(
            res => {
                this.loading = false;
                this.companies = res
            },
            err => {
                this.loading = false;
                this.error = true;
            }
        )
    }
    
    onDetailedFilterClick() {
        this.detailedFilter = ! this.detailedFilter;
        if (this.detailedFilter) {
            setTimeout(() => {
                if (this.detailedFilter)  {
                    this.renderer.addClass(this.detailedFilterRef.nativeElement, 'detailedFilter-content-overflow');
                }
            }, 700);
        } else {
            this.renderer.removeClass(this.detailedFilterRef.nativeElement, 'detailedFilter-content-overflow')
        }
    }

    getAreaOfInterests() {
        return this.config.areaOfInterests;
    }
}
