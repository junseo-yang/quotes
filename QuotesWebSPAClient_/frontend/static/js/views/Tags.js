import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle("Tags");
    }

    async getHtml() {
        return `
            <h1>Tags</h1>
            <p>You are viewing the tags!</p>
        `;
    }
}