/* 
    Quotes.js
    Assignment 3
    
    Revision History
        Junseo Yang, 2023-11-19: Created
*/

import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
  constructor(params) {
    super(params);
    this.setTitle("Quotes");
  }

  async getHtml() {
    return `
            <div id="quoteStatusMessage" style="display:none" role="alert">

            </div>
            <h1>Quotes</h1>
            <div id="viewTopQuotes">
              <div class="form-group">
                <label for="quotesByTag">Search Quotes by Tag</label>
                <input type="text" id="quotesByTag" name="quotesByTag" class="form-control" />
              </div>
            </div>
            <div class="row">
              <div class="col-sm-12 col-md-6">
                <h4>Quotes:</h4>
                <div id="quotesList">

                </div>
                <div id="quotesListMessage" class="alert alert-primary" style="display:none" role="alert">

                </div>
              </div>
              <div class="col-sm-12 col-md-6">
                <h4>Add a Quote:</h4>
                <form>
                  <div class="form-group">
                    <label for="quoteDescription">Description</label>
                    <input type="text" id="quoteDescription" name="quoteDescription" class="form-control" />
                  </div>

                  <div class="form-group">
                    <label for="quoteAuthor">Author</label>
                    <input type="text" id="quoteAuthor" name="quoteAuthor" class="form-control" />
                  </div>

                  <div class="form-group ui-widget">
                    <label for="quoteTags">Tags</label>
                    <input type="text" id="quoteTags" name="quoteTags" class="form-control" />
                  </div>

                  <button type="button" id="addQuoteBtn" class="btn btn-primary">Add quote</button>
                </form>
              </div>
            </div>
          `;
  }
}
