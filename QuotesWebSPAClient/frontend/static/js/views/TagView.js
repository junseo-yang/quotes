/* 
    TagView.js
    Assignment 3
    
    Revision History
        Junseo Yang, 2023-11-19: Created
*/

import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
  constructor(params) {
    super(params);
    this.tagId = params.id;
    this.tagName = '';
    this.setTitle("Tag");
  }

  async getHtml() {
    let _tagUrl = 'https://localhost:7223/api/tags/' + this.tagId;
    
    let _tagName = '';
    
    let loadTag = async function () {
        let resp = await fetch(_tagUrl, {
            mode: "cors",
            headers: {
                'Accept': 'application/json'
            }
        }).catch(function () {
            console.log('Error');
        }
        );

        if (resp.status === 200) {
            let tagResult = await resp.json();
            _tagName = tagResult.name;
        } else {
            console.log('error');
        }
    };

    await loadTag();
    
    return `
      <div id="tagStatusMessage" style="display:none" role="alert">

      </div>
      <h1>Tag</h1>
      <h4>Edit a Tag:</h4>
      <form>
        <div class="form-group">
          <label for="tagId">TagId</label>
          <input type="text" id="tagId" name="tagId" class="form-control" value="${this.tagId}" disabled/>
        </div>

        <div class="form-group">
          <label for="tagName">Name</label>
          <input type="text" id="tagName" name="tagName" class="form-control" value="${_tagName}" />
        </div>

        <button type="button" id="editTagBtn" class="btn btn-primary">Update tag</button>
      </form>
    `;
  }
}
