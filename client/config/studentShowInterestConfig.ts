import { Injectable } from '@angular/core';

@Injectable()
export class StudentShowInterestConfig {
    workPositionId: number;
    coverLetter: boolean;
    additionalQuestions: string;
    favorite: boolean;
    hasShownInterest: boolean;
    showInterestText: string;
    profileRoute: string;
}
