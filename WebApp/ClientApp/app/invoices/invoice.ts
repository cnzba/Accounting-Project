export interface IInvoice {
    invoiceId: number;
    invoiceNumber: string;
    invoiceDate: Date;
    issueeOrganization: string;
    issueeCareOf: string;
    totalAmount: number;
//    subTotal: number;
//    GST: string; // GST may be 0, in which case subTotal == totalAmount

    invoiceLines: IInvoiceLine[];
}

export interface IInvoiceLine {
    id: number; // needed to order the lines
    description: string;
    amount: number;
}

