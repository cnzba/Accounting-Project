import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({
  name: 'invoiceFilter'
})

export class InvoiceFilterPipe implements PipeTransform {

    constructor(public datepipe: DatePipe) { }

    transform(value: any[], searchString: string) {

        if (!searchString) {
            console.log('no search')
            return value
        }

        return value.filter(it => {
            const clientName = it.clientName.toLocaleLowerCase().includes(searchString.toLocaleLowerCase())
            const status = it.status.toLowerCase().includes(searchString.toLowerCase())
            const invoiceNumber = it.invoiceNumber.toLowerCase().includes(searchString.toLowerCase())
            const grandTotal = ("$" + it.grandTotal.toFixed(2)).toLowerCase().includes(searchString.toLowerCase())

            let format_dateCreated = this.datepipe.transform(it.dateCreated, 'dd/MM/yyyy');
            const dateCreated = format_dateCreated.toString().toLowerCase().includes(searchString.toLowerCase())

            let format_dateDue = this.datepipe.transform(it.dateDue, 'dd/MM/yyyy');
            const dateDue = format_dateDue.toString().toLowerCase().includes(searchString.toLowerCase())
                  
            return (clientName + status + invoiceNumber + grandTotal + dateCreated + dateDue);
        });
    }

}
