export interface IInvoice {
    invoiceNumber: string;
    dateDue: Date;
    clientName: string;
    clientContactPerson: string;
    clientContact: string;
    status: string;
   
    dateCreated?: Date;
    gstNumber?: string;
    charitiesNumber?: string;
    gstRate?: number;
    subTotal?: number;
    grandTotal?: number;
    email: string;
    paymentId: string;
    
    invoiceLine: IInvoiceLine[];
}

export class Invoice implements IInvoice {
    invoiceNumber: string;
    dateDue: Date;
    clientName: string;
    clientContactPerson: string;
    clientContact: string;
    status: string;
   
    dateCreated?: Date;
    gstNumber?: string;
    charitiesNumber?: string;
    gstRate?: number;
    subTotal?: number;
    grandTotal?: number;
    email: string;
    paymentId: string;
    
    invoiceLine: IInvoiceLine[];

    constructor(){
        this.clientName = this.clientContactPerson = this.clientContact = this.status = this.gstNumber = this.charitiesNumber = "";
        this.gstRate = this.subTotal = this.grandTotal =0;
        this.invoiceLine = [];
    }
}

export interface IInvoiceLine {
    itemOrder: number;
    description: string;
    amount: number;
    quantity: number;
    unitPrice: number;
}

export class InvoiceLine implements IInvoiceLine {
    itemOrder: number;
    description: string;
    amount: number;
    quantity: number;
    unitPrice: number;

    constructor() {
        this.description = "";
        this.amount = 0;
        this.itemOrder = 0;
        this.quantity = 0;
        this.unitPrice = 0;
    }
}


