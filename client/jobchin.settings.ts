import { jobchinLocalize } from './jobchin.localize';
import { Role } from './models/company/role.enum';
import { OrderBy } from './models/orderBy.enum';
import { Remote } from './models/remote.enum';

export const jobchinSettings = {
    remoteTypes: [
        {
            value: Remote.No,
            displayValue: jobchinLocalize.noRemote,
        },
        {
            value: Remote.Partial,
            displayValue: jobchinLocalize.partialRemote,
        },
        {
            value: Remote.Full,
            displayValue: jobchinLocalize.fullRemote,
        }],
    orderByOptions: [
        {
            value: OrderBy.Match,
            displayValue: jobchinLocalize.orderByMatch,
        },
        {
            value: OrderBy.Date,
            displayValue: jobchinLocalize.orderByDate,
        }],
    roleOptions: [
        {
            value: Role.CompanyAdmin,
            displayValue: jobchinLocalize.companyAdmin,
        },
        {
            value: Role.WorkPositionAdmin,
            displayValue: jobchinLocalize.workPositionAdmin,
        }],
    

    allowedImageTypes: '.jpg, .jpeg, .png',
    allowedFileTypes: '.jpg, .jpeg, .png, .pdf, .doc, .docx',

    streetMaxLength: 64,
    cityMaxLength: 32,
    zipCodeMaxLength: 8,
    notificationEmailMaxLength: 64,
    socialMediaMaxLength: 255,
    descMaxLength: 500,
    rteMaxLength: 4000,
    rteLettersMaxLength: 2000,
    shortRteMaxLength: 2000,
    shortRteLettersMaxLength: 1000,
    contactMaxLength: 32,
    nameMaxLength: 64,
    icoLength: 8,
    dicMaxLength: 16,
    workPositionNameMaxLength: 255,
    customFieldNameMaxLength: 255,
    workExperienceMaxLength: 128,
    studentCityMaxLength: 64,
    otherStudyMaxLength: 128,
}
