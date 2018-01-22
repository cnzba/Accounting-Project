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
    
    invoiceLine: IInvoiceLine[];
}

export interface IInvoiceLine {
    itemOrder: number; 
    description: string;
    amount: number;
}

