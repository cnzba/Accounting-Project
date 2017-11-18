export interface IInvoice {
    id: number;
    invoiceNumber: string;
    dateCreated: Date;
    dateDue: Date;
    issueeOrganization: string;
    issueeCareOf: string;
    gstNumber: string;
    charitiesNumber: string;
    clientContact: string;
   
    grandTotal: number;
    subTotal: number;
    gst: number; 

    status: string;
    invoiceLine: IInvoiceLine[];
}

export interface IInvoiceLine {
    id: number; // needed to order the lines
    invoiceId: number;
    description: string;
    amount: number;
}

