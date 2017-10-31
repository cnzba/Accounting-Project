export class IInvoice {
    id: number;
    date: string;
    client: string;
    amount: number;

    due: string;
    status: string;
    GST: string;
    charnum: string;
    des: string;



}


const INVOICES: IInvoice[] = [
    {
        id: 50, date: '10/5/2016', client: 'Electrocal Commission c/o Glen Clarke', amount: 25, due: 'Paid', status: 'sent',
        GST: '$-712-551', charnum: '21479', des: 'Fundraising Dinner'
    },
    {
        id: 51, date: '20/6/2016', client: 'John Smith', amount: 15, due: '15/6/2017', status: 'sent', GST: '96-345-234',
        charnum: '234578', des: 'Donation'
    }


];

