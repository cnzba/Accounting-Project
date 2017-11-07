export interface IInvoice {
    id: number;
    invoiceNumber: string;
    date: Date;
    issueeOrganization: string;
    issueeCareOf: string;
    gstnumber: string;
    charitiesNumber: string;
    clientContact: string;
    dueDate: Date;
   
    grandTotal: number;
    subTotal: number;
    gst: number; 

    status: IInvoiceStatus;
    invoiceLine: IInvoiceLine[];
}

export interface IInvoiceStatus {
    id: number;
    status: string;
}

export interface IInvoiceLine {
    id: number; // needed to order the lines
    invoiceId: number;
    description: string;
    amount: number;
}

