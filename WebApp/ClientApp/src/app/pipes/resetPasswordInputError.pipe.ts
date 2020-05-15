 import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'resetpassworderrtranslate'})
export class ResetPasswordErrTranslate implements PipeTransform {
    transform(value: any): any {
        console.log(value+" : resetp");
        if (!value) return;        
        if (value.required) return "The field is required.";
        if (value.minlength) return "The required length is "+value.minlength.requiredLength;
        if (value.maxlength) return "The required length is "+value.maxlength.requiredLength;
        if (value.passwordMismatch) return "Mismatch confirmed password";
        if (value.pattern) return "Invalid input, please check tips."
        return "Unexpected error: "+value;
    }
}

