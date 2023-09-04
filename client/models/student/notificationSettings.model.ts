import { NotificationFrequency } from '../notificationFrequency.enum';

export interface StudentNotificationSettings {
    notificationFrequency: NotificationFrequency;
    notificationEmail: string;
    workPositionNotificationFrequency: NotificationFrequency;
}
