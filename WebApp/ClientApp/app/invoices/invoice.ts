export interface IInvoice {
    id: number;
    invoiceNumber: string;
    date: Date;
    issueeOrganization: string;
    issueeCareOf: string;
    gstnumber: string;
    charitiesNumber: string;
//    totalAmount: number;
//    subTotal: number;
//    GST: string; // GST may be 0, in which case subTotal == totalAmount

    invoiceLine: IInvoiceLine[];
}

export interface IInvoiceLine {
    id: number; // needed to order the lines
    invoiceId: number; 
    description: string;
    amount: number;
}

