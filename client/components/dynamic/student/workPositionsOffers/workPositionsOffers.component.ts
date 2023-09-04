import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { UmbracoService } from '../../../../../../_shared/services/umbraco.service';
import { jobchinSettings } from '../../../../jobchin.settings';
import { OrderBy } from '../../../../models/orderBy.enum';
import { StudentWorkPositionsPaged } from '../../../../models/workPosition/studentWorkPositionPaged.model';
import { WorkPositionFilterDto } from '../../../../models/workPosition/workPositionFilter.model';
import { StudentService } from '../../../../services/student.service';
import { StudentOffersConfig } from './../../../../config/studentOffersConfig';

@Component({
    selector: 'jobchin-student-workPositionsOffers',
    templateUrl: 'workPositionsOffers.component.html',
    styleUrls: [ './workPositionsOffers.component.scss', '../../../../assets/detailedFilter.scss', '../../../../assets/accordion.scss', '../../../../assets/button-aligment.scss' ],
    providers: [
        UmbracoService.typedConfig(StudentOffersConfig)
    ]
})
export class StudentWorkPositionsOffersComponent {
    jobchinSettings = jobchinSettings;
    workPositions: StudentWorkPositionsPaged;
    filter: WorkPositionFilterDto;
    filterForm: FormGroup;
    loading = false;
    detailedFilter = false;

    @ViewChild("orderByButton") orderByButton!: ElementRef;
    @ViewChild("detailedFilterRef") detailedFilterRef!: ElementRef;

    constructor(private studentService: StudentService, private config: StudentOffersConfig, private renderer: Renderer2) {
        this.filter = new WorkPositionFilterDto();
        this.filterForm = new FormGroup({
            areaOfInterests: new FormControl([]),
            locations: new FormControl([]),
            contractTypes: new FormControl([]),
            name: new FormControl(''),
            remotes: new FormControl([]),
            languages: new FormControl([]),
        });

        this.filter.orderBy = OrderBy.Match;

        this.setFilter();
    }

    loadPage(page: number) {
        this.loading = true;

        this.filter.pageNo = page;
        this.studentService.getWorkPositionsPaged(this.filter).subscribe(
            result => {
                this.workPositions = result;
                this.loading = false;
            }
        );
    }

    onOrderByChange(val: OrderBy) {
        if (this.filter.orderBy !== val) {
            this.renderer.removeClass(this.orderByButton.nativeElement, 'is-open');
            this.filter.orderBy = val;
            this.loadPage(1);
        }
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

    setFilter() {
        this.filter.areaOfInterests = this.filterForm.controls.areaOfInterests.value;
        this.filter.locations = this.filterForm.controls.locations.value;
        this.filter.contractTypes = this.filterForm.controls.contractTypes.value;
        this.filter.name = this.filterForm.controls.name.value;
        this.filter.remotes = this.filterForm.controls.remotes.value;
        this.filter.languages = this.filterForm.controls.languages.value;

        this.loadPage(1);
    }

    getAreaOfInterests() {
        return this.config.areaOfInterests;
    }

    getLocalAdministrativeUnits() {
        return this.config.localAdministrativeUnits;
    }

    getContractTypes() {
        return this.config.contractTypes;
    }

    getLanguages() {
        return this.config.languages;
    }
}
