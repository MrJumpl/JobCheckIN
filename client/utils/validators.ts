import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';

export class JobChINValidators {
    static atLeastOneValidator(): ValidatorFn {
        return (c: AbstractControl): ValidationErrors | null => {
            let group = c as FormGroup;
            let controls = group.controls;
            if ( controls ) {
                let theOne = Object.keys(controls).find(key => controls[key].value);
                if ( !theOne ) {
                    return { atLeastOneRequired : true }
                }
            }
            return null;
        }
    }
}
