import { DateUtils } from './../../../../core/utils/date-utils';

export class ModelUtils {
    static deepCopy(model) {
        return DateUtils.convertDates(JSON.parse(JSON.stringify(model)))
    }
}
