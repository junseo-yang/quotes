$(document).ready(function () {
    let _quotesList = $('#quotesList');
    let _quotesListMessage = $('#quotesListMessage');
    let _quoteStatusMessage = $('#quoteStatusMessage')

    let _quotesLastModified = new Date(1970, 0, 1);

    let _quotesUrl = 'https://localhost:7223/api/quotes';

    let _availableTags = '';

    let loadQuotes = async function () {
        // call out to the Web API using fetch (enabling CORS) to get our quotes:
        await fetch(_quotesUrl, {
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
            let quotesResult = data;
            let quotes = quotesResult.quotes;

            // Populate availableTags
            _availableTags = quotesResult.tags;

            if (quotes.length === 0) {
                _quotesListMessage.text('No quotes to display - use the form to add some.');
                _quotesListMessage.show()
            } else {
                _quotesListMessage.hide()

                let latestLastModified = new Date(quotesResult.quotesLastModified);

                if (latestLastModified.getTime() > _quotesLastModified.getTime()) {
                    _quotesLastModified = latestLastModified;

                    // loop thru the quotes and add them to the Cards...

                    _quotesList.empty();

                    for (let i = 0; i < quotes.length; i++) {
                        _quotesList.append(`
                            <div class="card" style="width: 18rem;">
                                <div class="card-body">
                                <h5 class="card-title">${quotes[i].author}</h5>
                                <p class="card-text">"${quotes[i].description}"</p>
                                <div>
                                ${function displayTags() {
                                    let tags = '';
                                    for (let j = 0; j < quotes[i].tags.length; j++) {
                                        tags += `<span class="badge rounded-pill bg-secondary">${quotes[i].tags[j]}</span>`;
                                    }
                                    return tags;
                                }()}
                                </div>
                                <br/ >
                                <a class="btn btn-primary btn-sm" href="#" role="button">
                                    Likes <span class="badge bg-light text-dark">${quotes[i].like}</span>
                                </a>
                                </div>
                            </div>
                        `)
                    }
                }
            }
        }).catch(error => {
            console.error(error);
            _quotesList.empty();
            _quoteStatusMessage.text('Hmmm, there was a problem loading the quotes. Check the API server.');
            _quoteStatusMessage.attr('class', 'alert alert-danger');
            _quoteStatusMessage.show()
            _quoteStatusMessage.fadeOut(3000);
        });
    };

    // add a click handler to POST new quotes to our API:
    $('#addQuoteBtn').click(async function () {
        // Create a new quote by reading the form input fields:
        let newQuote = {
            description: $('#quoteDescription').val(),
            author: $('#quoteAuthor').val(),
            tags: $('#quoteTags').val().split(", ").filter(n => n)
        };

        await fetch(_quotesUrl, {
            mode: "cors",
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newQuote)
        }).then(resp => {
            _quoteStatusMessage.empty();
            if (resp.status === 201) {
                _quoteStatusMessage.text('The quote was added successfully');
                _quoteStatusMessage.attr('class', 'alert alert-success');
                $('#quoteDescription').val('');
                $('#quoteAuthor').val('');
                $('#quoteTags').val('');
            } else if(resp.status === 400) {
                _quoteStatusMessage.text('The tags are not supported. Try another one with autocomplete.');
                _quoteStatusMessage.attr('class', 'alert alert-danger');
            } else {
                _quoteStatusMessage.text('Hmmm, there was a problem adding the quote');
                _quoteStatusMessage.attr('class', 'alert alert-danger');
            }
            _quoteStatusMessage.show();
            _quoteStatusMessage.fadeOut(3000);
        }).catch(error => {
            console.error(error);
            _quoteStatusMessage.empty();
            _quoteStatusMessage.text('Hmmm, there was a problem adding the Tags. Check the API server.');
            _quoteStatusMessage.attr('class', 'alert alert-danger');
            _quoteStatusMessage.show()
            _quoteStatusMessage.fadeOut(3000);
        });
    });

    // Autocomplete
    $(function () {
        function split(val) {
            return val.split(/,\s*/);
        }
        function extractLast(term) {
            return split(term).pop();
        }

        $("#quoteTags")
            // don't navigate away from the field on tab when selecting an item
            .on("keydown", function (event) {
                if (event.keyCode === $.ui.keyCode.TAB &&
                    $(this).autocomplete("instance").menu.active) {
                    event.preventDefault();
                }
            })
            .autocomplete({
                minLength: 0,
                source: function (request, response) {
                    // delegate back to autocomplete, but extract the last term
                    response($.ui.autocomplete.filter(
                        _availableTags, extractLast(request.term)));
                },
                focus: function () {
                    // prevent value inserted on focus
                    return false;
                },
                select: function (event, ui) {
                    var terms = split(this.value);
                    // remove the current input
                    terms.pop();
                    // add the selected item
                    terms.push(ui.item.value);
                    // add placeholder to get the comma-and-space at the end
                    terms.push("");
                    this.value = terms.join(", ");
                    return false;
                }
            });
    });

    // first a 1 time call and then set up a timer to call load todos fn:
    loadQuotes();
    setInterval(loadQuotes, 1000);
});