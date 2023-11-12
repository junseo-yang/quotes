import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
  constructor(params) {
    super(params);
    this.setTitle("Quotes");
  }

  async getHtml() {
    return `
            <h1>Quotes</h1>
            <ul id="quotesList">

            </ul>
        `;
  }
}