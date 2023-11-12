import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
    constructor(params) {
        super(params);
        this.quoteId = params.id;
        this.setTitle("Quote");
    }

    async getHtml() {
        return `
            <h1>Quote</h1>
            <p>You are viewing post #${this.quoteId}.</p>
        `;
    }
}
