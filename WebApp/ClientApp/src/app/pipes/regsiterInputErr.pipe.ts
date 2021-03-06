 import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'regerrtranslate'})
export class RegErrTranslate implements PipeTransform {
    transform(value: any): any {
        //console.log(value);
        if (!value) return null;
        if (value.email) return "Please input an email.";
        if (value.required) return "You must enter a value.";
        if (value.minlength) return "The minimum length is "+value.minlength.requiredLength;
        if (value.maxlength) return "The maximum length is "+value.maxlength.requiredLength;
        if (value.passwordMismatch) return "Mismatch confirmed password";
        if (value.userExist) return "The user already exists";
        if (value.fixLength) return `Length must be ${value.requiredLength}`;
        return "Unexpected error: "+value;
    }
}

