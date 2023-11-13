import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle("Tags");
    }

    async getHtml() {
        return `
            <h1>Tags</h1>

            <div class="row">
              <div class="col-sm-12 col-md-6">
                <h4>Tags:</h4>
                <table class="table">
                  <thead>
                    <tr>
                      <th scope="col">TagId</th>
                      <th scope="col">TagName</th>
                      <th scope="col">Action</th>
                    </tr>
                  </thead>
                  <tbody id="tagsList">
                    
                  </tbody>
                </table>
                <div id="tagsListMessage" class="alert alert-primary" style="display:none" role="alert">

                </div>
              </div>
              <div class="col-sm-12 col-md-6">
                <h4>Add a Tag:</h4>
                <form>
                  <div class="form-group">
                    <label for="tagName">Name</label>
                    <input type="text" id="tagName" name="tagName" class="form-control" />
                  </div>

                  <button type="button" id="addTagBtn" class="btn btn-primary">Add tag</button>
                </form>
                <p id="newTagMessage"></p>
              </div>
            </div>
        `;
    }
}