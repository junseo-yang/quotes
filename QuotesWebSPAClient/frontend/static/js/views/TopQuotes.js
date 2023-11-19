/* 
    TopQuotes.js
    Assignment 3
    
    Revision History
        Junseo Yang, 2023-11-19: Created
*/

import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
  constructor(params) {
    super(params);
    this.number = params.number
    this.setTitle(`Top ${this.number} Quotes`);
  }

  async getHtml() {
    return `
        <div id="topQuoteStatusMessage" style="display:none" role="alert">

        </div>
        <h1>Top ${this.number} Quotes</h1>
        <div id="numberOfQuotes" value="${this.number}" hidden></div>
        <form id="viewTopQuotes">
          <div class="form-group">
            <label for="topQuotesNumber">Number of Top Quotes to display</label>
            <input type="number" id="topQuotesNumber" name="topQuotesNumber" class="form-control" min="0"/>
          </div>

          <div class="d-flex justify-content-end">
            <button type="button" id="viewTopQuotesBtn" class="btn btn-primary">Display Top Quotes</button>
          </div>
        </div>
        </form>
        <div id="topQuotesList">

        </div>
        <div id="topQuotesListMessage" class="alert alert-primary" style="display:none" role="alert">

        </div>
    `;
  }
}
