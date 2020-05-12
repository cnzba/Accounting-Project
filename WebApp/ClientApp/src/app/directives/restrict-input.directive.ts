import { Directive, ElementRef, Input } from '@angular/core';
import * as Inputmask from "inputmask";

@Directive({
  selector: '[appRestrictInput]'
})
export class RestrictInputDirective {

  private regexMap = {
    integer:'^[0-9]*$',
    float: '^[+-]?([0-9]*[.])?[0-9]+$',
  }

  constructor(private el:ElementRef) { }

  @Input('appRestrictInput')
  public set defineInputType(type:string){
    Inputmask({regex:this.regexMap[type], placeholder:''})
      .mask(this.el.nativeElement);
  }

}
