import { NotificationFrequency } from '../notificationFrequency.enum';

export interface CompanyNotificationSettings {
    notificationFrequency: NotificationFrequency;
    notificationEmail: string;
}
