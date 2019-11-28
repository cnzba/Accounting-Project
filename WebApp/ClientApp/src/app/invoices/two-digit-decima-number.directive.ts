import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
    selector: '[appTwoDigitDecimaNumber]'
})
export class TwoDigitDecimaNumberDirective {
    // Allow decimal numbers and positive values
    private regex: RegExp = new RegExp(/^\d+\.?\d{0,4}$/g);
    // Allow key codes for special events. Reflect :
    // Backspace, tab, end, home
    private specialKeys: Array<string> = ['Backspace', 'Tab', 'End', 'Home', 'ArrowRight', 'ArrowLeft','Delete'];

    constructor(private el: ElementRef) {
    }
    @HostListener('keydown', ['$event'])
    onKeyDown(event: KeyboardEvent) {
        console.log(this.el.nativeElement.value);
        // Allow Backspace, tab, end, and home keys
        if (this.specialKeys.indexOf(event.key) !== -1) {
            return;
        }
        let current: string = this.el.nativeElement.value;
        let next: string = current.concat(event.key);
        if (next && !String(next).match(this.regex)) {
            event.preventDefault();
        }
    }
}
