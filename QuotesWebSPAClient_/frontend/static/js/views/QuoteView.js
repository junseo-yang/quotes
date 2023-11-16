import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
  constructor(params) {
    super(params);
    this.quoteId = params.id;
    this.setTitle("Quote");
  }

  async getHtml() {
    let _quoteUrl = "https://localhost:7223/api/quotes/" + this.quoteId;

    let _quoteDescription = "";
    let _quoteAuthor = "";
    let _quoteTags = "";

    await fetch(_quoteUrl, {
      mode: "cors",
      headers: {
        Accept: "application/json",
      },
    }).then(resp => {
        if (!resp.ok) {
          throw new Error("Network error");
        }
        return resp.json();
    }) .then(data => {
        let quoteResult = data;
        _quoteDescription = quoteResult.description;
        _quoteAuthor = quoteResult.author;
        _quoteTags = quoteResult.tags.join(", ");
    }).catch(error => {
        console.error(error);
        // Redirect to quotes page when gets error to get quote by id
        window.location.replace(window.location.protocol + "//" + window.location.host + "/quotes");
    });

    return `
      <div id="quoteStatusMessage" style="display:none" role="alert">
  
      </div>
      <h1>Quote</h1>
      <h4>Edit a Quote:</h4>
      <form>
        <div class="form-group">
        <label for="quoteId">QuoteId</label>
        <input type="text" id="quoteId" name="quoteId" class="form-control" value="${this.quoteId}" disabled/>
        </div>

        <div class="form-group">
            <label for="quoteDescription">Description</label>
            <input type="text" id="quoteDescription" name="quoteDescription" class="form-control" value="${_quoteDescription}" />
        </div>

        <div class="form-group">
            <label for="quoteAuthor">Author</label>
            <input type="text" id="quoteAuthor" name="quoteAuthor" class="form-control" value="${_quoteAuthor}" />
        </div>

        <div class="form-group">
            <label for="quoteTags">Tags</label>
            <input type="text" id="quoteTags" name="quoteTags" class="form-control" value="${_quoteTags}" />
        </div>

        <button type="button" id="editQuoteBtn" class="btn btn-primary">Update quote</button>
      </form>
    `;
  }
}
