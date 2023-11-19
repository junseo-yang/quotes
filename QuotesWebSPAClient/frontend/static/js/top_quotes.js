/* 
    top_quotes.js
    Assignment 3
    
    Revision History
        Junseo Yang, 2023-11-19: Created
*/

$(document).ready(function () {
    let _topQuotesLastModified = new Date(1970, 0, 1);

    let _topQuotesUrl = 'https://localhost:7223/api/topquotes';

    let loadTopQuotes = async function () {
        let _topQuotesList = $('#topQuotesList');
        let _topQuoteStatusMessage = $('#topQuoteStatusMessage');
        let _topQuotesListMessage = $('#topQuotesListMessage');
        let _availableTags = '';
        let _numberOfQuotes = $("#numberOfQuotes").attr('value');;

        // call out to the Web API using fetch (enabling CORS) to get our quotes:
        await fetch(_topQuotesUrl + `/${_numberOfQuotes}`, {
            mode: "cors",
            headers: {
                'Accept': 'application/json'
            }
        }).then(resp => {
            if (!resp.ok) {
                throw new Error('Network error');
            }
            return resp.json();
        }).then(data => {
            let topQuotesResult = data;
            let topQuotes = topQuotesResult.quotes;

            // Populate availableTags
            _availableTags = topQuotesResult.tags;

            if (topQuotes.length === 0) {
                _topQuotesListMessage.text('No Top Quotes to display.');
                _topQuotesListMessage.show()
            } else {
                _topQuotesListMessage.hide()

                let latestLastModified = new Date(topQuotesResult.quotesLastModified);

                if (latestLastModified.getTime() >= _topQuotesLastModified.getTime()) {
                    _topQuotesLastModified = latestLastModified;

                    // loop thru the quotes and add them to the Cards...

                    _topQuotesList.empty();

                    for (let i = 0; i < topQuotes.length; i++) {
                        _topQuotesList.append(`
                            <div class="card m-2" style="width: auto;">
                                <div class="card-body">
                                    <h5 class="card-title">${topQuotes[i].author}</h5>
                                    <p class="card-text">"${topQuotes[i].description}"</p>
                                    <div>
                                    ${function displayTags() {
                                        let tags = '';
                                        for (let j = 0; j < topQuotes[i].tags.length; j++) {
                                            tags += `<span class="badge rounded-pill bg-secondary">${topQuotes[i].tags[j]}</span>`;
                                        }
                                        return tags;
                                    }()}
                                    </div>
                                    <br/ >
                                    <div class="d-flex justify-content-between">
                                        <button class="btn btn-primary btn-sm btn-like" value="${topQuotes[i].quoteId}">
                                            Likes <span class="badge bg-light text-dark">${topQuotes[i].like}</span>
                                        </button>
                                        <a class="btn btn-sm btn-primary" href="/quotes/${topQuotes[i].quoteId}">Edit</a>
                                    </div>
                                </div>
                            </div>
                        `)
                    }
                }
            }
        }).catch(error => {
            console.error(error);
            _topQuotesList.empty();
            _topQuoteStatusMessage.text('Hmmm, there was a problem loading the quotes. Check the API server.');
            _topQuoteStatusMessage.attr('class', 'alert alert-danger');
            _topQuoteStatusMessage.show();
            _topQuoteStatusMessage.fadeOut(5000);
        });
    };

    // add a click handler to Redirect to a top quotes page:
    $("body").on("click", "#viewTopQuotesBtn", async function() {
        let _topQuoteStatusMessage = $('#topQuoteStatusMessage');

        // Update an existing tag by reading the form input fields:
        let numberOfTopQuotes = $('#topQuotesNumber').val();
        
        // Validation Number Of Quotes
        if (numberOfTopQuotes < 0 || isNaN(numberOfTopQuotes)) {
            _topQuoteStatusMessage.empty();
            _topQuoteStatusMessage.text("Number of Top Quotes should be a number greater than or equal to 0.");
            _topQuoteStatusMessage.attr('class', 'alert alert-danger');
            _topQuoteStatusMessage.show();
            _topQuoteStatusMessage.fadeOut(5000);
        } else {
            // 
            window.location.replace(window.location.protocol + "//" + window.location.host + "/topquotes/" + numberOfTopQuotes);
        }
    });

    // first a 1 time call and then set up a timer to call load quotes fn:
    loadTopQuotes();
    setInterval(loadTopQuotes, 1000);
});