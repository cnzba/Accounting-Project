import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'invoiceFilter'
})

export class InvoiceFilterPipe implements PipeTransform {

    transform(value: any[], searchString: string) {

        if (!searchString) {
            console.log('no search')
            return value
        }

        return value.filter(it => {
            const clientName = it.clientName.toLocaleLowerCase().includes(searchString.toLocaleLowerCase())
            const status = it.status.toLowerCase().includes(searchString.toLowerCase())
            const invoiceNumber = it.invoiceNumber.toLowerCase().includes(searchString.toLowerCase())
            const grandTotal = it.grandTotal.toString().toLowerCase().includes(searchString.toLowerCase())
            const dateCreated = it.dateCreated.toString().toLowerCase().includes(searchString.toLowerCase())
            const dateDue = it.dateDue.toString().toLowerCase().includes(searchString.toLowerCase())
                  
            return (clientName + status + invoiceNumber + grandTotal + dateCreated + dateDue);
        });
    }

}
