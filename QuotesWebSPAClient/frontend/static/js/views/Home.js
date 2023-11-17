import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle("Home");
    }

    async getHtml() {
        return `
            <h1>Home</h1>
            <p>
                PROG3170 Assignment 3 Junseo Yang
            </p>
            <p>
                Homepage of QuotesWebSPAClient
            </p>
        `;
    }
}