/* 
    AbstractView.js
    Assignment 3
    
    Revision History
        Junseo Yang, 2023-11-19: Created
*/

export default class {
    constructor(params) {
        this.params = params;
    }

    setTitle(title) {
        document.title = title;
    }

    async getHtml() {
        return "";
    }
}