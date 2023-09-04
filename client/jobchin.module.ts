import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { NgDatepickerModule } from '../../../core/components/ng-datepicker/ng-datepicker.module';
import { DynamicComponentsModule } from '../../../core/modules/dynamic-components.module';
import { MemberAccountComponent } from '../..//members/components/account/account.component';
import { AppRoutesModule } from '../../_shared/modules/app-routes.module';
import { MuniWebModule } from '../../_templates/muni-web/muni-web.module';
import { MembersModule } from '../../members/members.module';
import { membersRoutes } from './../../members/members.routes';
import { CompanyNewUserComponent } from './components/dynamic/company/newUserComponent/newUser.component';
import { StudentCompanyListComponent } from './components/dynamic/student/companyList/companyList.component';
import { StudentCompanyListViewComponent } from './components/dynamic/student/companyListView/companyListView.component';
import {
    StudentWorkPositionsListViewComponent,
} from './components/dynamic/student/workPositionsListView/workPositionsListView.component';
import {
    StudentWorkPositionsOffersComponent,
} from './components/dynamic/student/workPositionsOffers/workPositionsOffers.component';
import { ErrorComponent } from './components/errorComponent';
import { AccordionListComponent } from './components/members/accordionListComponent/accordionList.component';
import { FormFooterComponent } from './components/members/baseForm/formFooter.component';
import { BranchesComponent } from './components/members/company/branchesComponent/branches.component';
import { CandidatesComponent } from './components/members/company/candidatesComponent/candidates.component';
import { CompanyRegisterComponent } from './components/members/company/companyRegisterComponent/companyRegister.component';
import { CompanyTypeComponent } from './components/members/company/companyTypeComponent/companyType.component';
import { ContactPersonComponent } from './components/members/company/contactPersonComponent/contactPerson.component';
import { GeneralInfoComponent } from './components/members/company/generalInfoComponent/generalInfo.component';
import { LoginCompanyComponent } from './components/members/company/loginCompanyComponent/loginCompany.component';
import {
    CompanyNotificationSettingsComponent,
} from './components/members/company/notificationSettingsComponent/notificationSettings.component';
import { CompanyOffersComponent } from './components/members/company/offersComponent/offers.component';
import { PresentationComponent } from './components/members/company/presentationComponent/presentation.component';
import { CompanyProfileComponent } from './components/members/company/profileComponent/profile.component';
import { ResetPasswordComponent } from './components/members/company/resetPasswordComponent/resetPassword.component';
import { CompanySettingsComponent } from './components/members/company/settingsComponent/settings.component';
import { CompanyStudentSearchComponent } from './components/members/company/studentSearchComponent/studentSearch.component';
import { UsersComponent } from './components/members/company/usersComponent/users.component';
import {
    WorkPositionOverviewComponent,
} from './components/members/company/workPositionOverviewComponent/workPositionOverview.component';
import { LoginHubComponent } from './components/members/loginHubComponent/loginHub.component';
import { NotificationComponent } from './components/members/notificationComponent/notification.component';
import {
    NotificationSettingsComponent,
} from './components/members/notificationSettingsComponent/notificationSettings.component';
import { OffersComponent } from './components/members/offersComponent/offers.component';
import {
    ContractTypePickerComponent,
} from './components/members/pickers/contractTypePickerComponent/contractTypePicker.component';
import { CountryPickerComponent } from './components/members/pickers/countryPickerComponent/countryPicker.component';
import { HardSkillPickerComponent } from './components/members/pickers/hardSkillPickerComponent/hardSkillPicker.component';
import { LanguagePickerComponent } from './components/members/pickers/languagePickerComponent/languagePicker.component';
import {
    NotificationFrequencyPickerComponent,
} from './components/members/pickers/notificationFrequencyPickerComponent/notificationFrequencyPicker.component';
import { ProfileHubComponent } from './components/members/profileHubComponent/profileHub.component';
import { SettingsComponent } from './components/members/settingsComponent/settings.component';
import { BasicInfoComponent } from './components/members/student/basicInfoComponent/basicInfo.component';
import { ChangePhotoComponent } from './components/members/student/changePhotoComponent/changePhoto.component';
import { ContactComponent } from './components/members/student/contactComponent/contact.component';
import { CvComponent } from './components/members/student/cvComponent/cv.component';
import { DashboardComponent } from './components/members/student/dashboardComponent/dashboard.component';
import { StudentFavoriteButtonComponent } from './components/members/student/favoriteButton/favoriteButton.component';
import { StudentHeaderComponent } from './components/members/student/headerComponent/studentHeader.component';
import { LoginStudentComponent } from './components/members/student/loginStudentComponent/loginStudent.component';
import {
    StudentNotificationSettingsComponent,
} from './components/members/student/notificationSettingsComponent/notificationSettings.component';
import { StudentOffersComponent } from './components/members/student/offersComponent/offers.component';
import { PhotoComponent } from './components/members/student/photoComponent/photo.component';
import { StudentProfileComponent } from './components/members/student/profileComponent/profile.component';
import { RegisterStudentComponent } from './components/members/student/registerStudentComponent/registerStudent.component';
import { StudentSettingsComponent } from './components/members/student/settingsComponent/settings.component';
import { StudentShowInterestComponent } from './components/members/student/showInterest/showInterest.component';
import { SkillsComponent } from './components/members/student/skillsComponent/skills.component';
import { StudyComponent } from './components/members/student/studyComponent/study.component';
import { StudentVisibilityComponent } from './components/members/student/visibilityComponent/visibility.component';
import { WorkExperiencesComponent } from './components/members/student/workExperiencesComponent/workExperiences.component';
import { WorkPositionBasicInfoComponent } from './components/members/workPosition/basicInfoComponent/basicInfo.component';
import {
    WorkPositionCandidateRequestComponent,
} from './components/members/workPosition/candidateRequestComponent/candidateRequest.component';
import { WorkPositionCandidatesComponent } from './components/members/workPosition/candidatesComponent/candidates.component';
import { WorkPositionCreateComponent } from './components/members/workPosition/createComponent/create.component';
import { WorkPositionDetailComponent } from './components/members/workPosition/detailComponent/detail.component';
import { WorkPositionDuplicateComponent } from './components/members/workPosition/duplicate.component';
import { WorkPositionExpiredComponent } from './components/members/workPosition/expired.component';
import { WorkPositionNotFoundComponent } from './components/members/workPosition/notFoundComponent/notFound.component';
import { WorkPositionPreviewComponent } from './components/members/workPosition/previewComponent/preview.component';
import { WorkPositionUpdateComponent } from './components/members/workPosition/updateComponent/update.component';
import { JobChINAngularConfig } from './config/angularConfig';
import { JobChINProfileConfig } from './config/profileConfig';
import { jobchinRoutes } from './jobchin.routes';
import { CompanyGuard } from './services/company.guard';
import { CompanyService } from './services/company.service';
import { CompanyAdminGuard } from './services/companyAdmin.guard';
import { EditWorkPositionGuard } from './services/editWorkPosition.guard';
import { PendingChangesGuard } from './services/pendingChanges.guard';
import { SettingsRedirectGuard } from './services/settingsRedirect.guard';
import { StudentGuard } from './services/student.guard';
import { StudentService } from './services/student.service';
import { UserService } from './services/user.service';
import { WorkPositionGuard } from './services/workPosition.guard';
import { JobChINZoneRootGuard } from './services/zoneRoot.guard';


@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MuniWebModule,
        MembersModule,
        NgDatepickerModule,
        DynamicComponentsModule.with([
            { name: 'CompanyNewUserComponent', type: CompanyNewUserComponent },
            { name: 'StudentWorkPositionsOffersComponent', type: StudentWorkPositionsOffersComponent },
            { name: 'StudentShowInterestComponent', type: StudentShowInterestComponent },
            { name: 'StudentCompanyListComponent', type: StudentCompanyListComponent },
        ]),
        MembersModule.with({
            angularConfigType: JobChINAngularConfig,
            registerComponent: CompanyRegisterComponent,
            resetPasswordComponent: ResetPasswordComponent,
            loginComponent: LoginHubComponent,
            profile: {
                component: undefined,
                angularConfigType: JobChINProfileConfig,
                routes: [
                    { path: '', pathMatch: 'full', redirectTo: jobchinRoutes.settings },

                    { path: jobchinRoutes.cv, component: StudentProfileComponent, canActivate: [StudentGuard], children: [
                        { path: '', pathMatch: 'full', redirectTo: jobchinRoutes.overview },
                        { path: jobchinRoutes.overview, component: CvComponent },
                        { path: jobchinRoutes.basicInfo, component: BasicInfoComponent, canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.workExperiences, component: WorkExperiencesComponent, canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.skills, component: SkillsComponent, canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.study, component: StudyComponent, canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.contact, component: ContactComponent, canDeactivate: [PendingChangesGuard] },
                    ] },
                    { path: jobchinRoutes.aboutCompany, component: CompanyProfileComponent, canActivate: [CompanyAdminGuard], children: [
                        { path: '', pathMatch: 'full', redirectTo: jobchinRoutes.generalInfo }, 
                        { path: jobchinRoutes.generalInfo, component: GeneralInfoComponent, canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.presentation, component: PresentationComponent, canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.candidates, component: CandidatesComponent, canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.branches, component: BranchesComponent, canDeactivate: [PendingChangesGuard] },
                        
                    ] },
                    { path: jobchinRoutes.studentSearch, component: CompanyStudentSearchComponent, canActivate: [CompanyAdminGuard] },
                    { path: jobchinRoutes.myOffres, children: [
                        { path: '', pathMatch: 'full', component: OffersComponent },
                        { path: jobchinRoutes.create, canActivate: [CompanyGuard], component: WorkPositionCreateComponent },
                        { path: `:id/${jobchinRoutes.duplicate}`, canActivate: [WorkPositionGuard], component: WorkPositionDuplicateComponent },
                        { path: `:id/${jobchinRoutes.overview}`, canActivate: [WorkPositionGuard], component: WorkPositionOverviewComponent },
                        { path: `:id/${jobchinRoutes.update}`, component: WorkPositionUpdateComponent, canActivate: [WorkPositionGuard], children: [
                            { path: '', pathMatch: 'full', redirectTo: jobchinRoutes.basicInfo },
                            { path: jobchinRoutes.basicInfo, component: WorkPositionBasicInfoComponent, canActivate: [EditWorkPositionGuard], canDeactivate: [PendingChangesGuard] },
                            { path: jobchinRoutes.detail, component: WorkPositionDetailComponent, canActivate: [EditWorkPositionGuard], canDeactivate: [PendingChangesGuard] },
                            { path: jobchinRoutes.candidates, component: WorkPositionCandidatesComponent, canActivate: [EditWorkPositionGuard], canDeactivate: [PendingChangesGuard] },
                            { path: jobchinRoutes.candidateRequest, component: WorkPositionCandidateRequestComponent, canActivate: [EditWorkPositionGuard], canDeactivate: [PendingChangesGuard] },
                            { path: jobchinRoutes.expired, component: WorkPositionExpiredComponent },
                        ] },
                        { path: jobchinRoutes.notFound, component: WorkPositionNotFoundComponent },
                    ] },
                    { path: jobchinRoutes.notification, component: NotificationComponent },
                    { path: jobchinRoutes.settings, component: SettingsComponent, children: [
                        { path: '', pathMatch: 'full', canActivate: [SettingsRedirectGuard], children: []  },
                        { path: jobchinRoutes.companyType, component: CompanyTypeComponent, canActivate: [CompanyAdminGuard] },
                        { path: jobchinRoutes.users, component: UsersComponent, canActivate: [CompanyAdminGuard] },
                        { path: jobchinRoutes.contactPerson, component: ContactPersonComponent, canActivate: [CompanyGuard], canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.visibility, component: StudentVisibilityComponent, canActivate: [StudentGuard], canDeactivate: [PendingChangesGuard] },
                        { path: jobchinRoutes.notification, component: NotificationSettingsComponent, canDeactivate: [PendingChangesGuard] },
                        { path: membersRoutes.account, component: MemberAccountComponent },
                    ] },
                    // { path: '**', redirectTo: '' },
                ]
            }
        }),
        AppRoutesModule.with([
            { path: membersRoutes.register, children: [
                { path: jobchinRoutes.student, component: RegisterStudentComponent },
                { path: jobchinRoutes.company, component: CompanyRegisterComponent },
            ] },
            { path: membersRoutes.login, children: [
                { path: jobchinRoutes.student, component: LoginStudentComponent },
                { path: jobchinRoutes.company, component: LoginCompanyComponent },
            ] },
        ]),
        RouterModule,
    ],
    declarations: [
        AccordionListComponent,
        ErrorComponent,
        LoginHubComponent,
        HardSkillPickerComponent,
        LanguagePickerComponent,
        CountryPickerComponent,
        ContractTypePickerComponent,
        FormFooterComponent,
        ProfileHubComponent,
        NotificationComponent,
        NotificationFrequencyPickerComponent,
        NotificationSettingsComponent,
        OffersComponent,
        SettingsComponent,

        // Company
        BranchesComponent,
        CandidatesComponent,
        CompanyRegisterComponent,
        CompanyNewUserComponent,
        CompanyNotificationSettingsComponent,
        CompanyOffersComponent,
        CompanyProfileComponent,
        CompanyTypeComponent,
        CompanySettingsComponent,
        CompanyStudentSearchComponent,
        ContactPersonComponent,
        GeneralInfoComponent,
        UsersComponent,
        LoginCompanyComponent,
        PresentationComponent,
        ResetPasswordComponent,
        WorkPositionOverviewComponent,

        // WorkPosition
        WorkPositionBasicInfoComponent,
        WorkPositionCandidateRequestComponent,
        WorkPositionCandidatesComponent,
        WorkPositionCreateComponent,
        WorkPositionDetailComponent,
        WorkPositionDuplicateComponent,
        WorkPositionExpiredComponent,
        WorkPositionNotFoundComponent,
        WorkPositionPreviewComponent,
        WorkPositionUpdateComponent,

        // Student
        BasicInfoComponent,
        ContactComponent,
        CvComponent,
        DashboardComponent,
        StudentHeaderComponent,
        ChangePhotoComponent,
        LoginStudentComponent,
        PhotoComponent,
        RegisterStudentComponent,
        SkillsComponent,
        StudentCompanyListComponent,
        StudentCompanyListViewComponent,
        StudentFavoriteButtonComponent,
        StudentNotificationSettingsComponent,
        StudentOffersComponent,
        StudentProfileComponent,
        StudentSettingsComponent,
        StudentShowInterestComponent,
        StudentVisibilityComponent,
        StudentWorkPositionsListViewComponent,
        StudentWorkPositionsOffersComponent,
        StudyComponent,
        WorkExperiencesComponent,
    ],
    exports: [
        LoginHubComponent,
        LoginHubComponent,
        LoginStudentComponent,
        RegisterStudentComponent,
        CompanyRegisterComponent,
        ResetPasswordComponent,
    ],
    providers: [
        CompanyAdminGuard,
        CompanyGuard,
        CompanyService,
        EditWorkPositionGuard,
        PendingChangesGuard,
        SettingsRedirectGuard,
        StudentGuard,
        StudentService,
        UserService,
        WorkPositionGuard,
        JobChINZoneRootGuard,
    ]
})
export class JobChINModule { }
