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
                            <div class="card" style="width: auto;">
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
                                <div class="d-flex justify-content-between">
                                    <button class="btn btn-primary btn-sm btn-like" value="${quotes[i].quoteId}">
                                        Likes <span class="badge bg-light text-dark">${quotes[i].like}</span>
                                    </button>
                                    <a class="btn btn-sm btn-primary" href="/quotes/${quotes[i].quoteId}">Edit</a>
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
            _quoteStatusMessage.fadeOut(5000);
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
                _quoteStatusMessage.text('The description is empty or the tags are not supported. Try again.');
                _quoteStatusMessage.attr('class', 'alert alert-danger');
            } else {
                _quoteStatusMessage.text('Hmmm, there was a problem adding the quote');
                _quoteStatusMessage.attr('class', 'alert alert-danger');
            }
        }).catch(error => {
            console.error(error);
            _quoteStatusMessage.empty();
            _quoteStatusMessage.text('Hmmm, there was a problem adding the Tags. Check the API server.');
            _quoteStatusMessage.attr('class', 'alert alert-danger');
        });

        _quoteStatusMessage.show()
        _quoteStatusMessage.fadeOut(5000);
    });

    // Autocomplete for Add Quote
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

    $("body").on("click", "#editQuoteBtn", async function() {
        let _quoteStatusMessage = $('#quoteStatusMessage')

        // Update an existing tag by reading the form input fields:
        let updatedQuote = {
            quoteId: $('#quoteId').val(),
            description: $('#quoteDescription').val(),
            author: $('#quoteAuthor').val(),
            tags: $('#quoteTags').val().split(", ").filter(n => n)
        };

        await fetch(_quotesUrl, {
            mode: "cors",
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedQuote)
        }).then(resp => {
            _quoteStatusMessage.empty();
            if (resp.status === 200) {
                _quoteStatusMessage.text('The quote has been updated successfully');
                _quoteStatusMessage.attr('class', 'alert alert-success');
            } else if(resp.status === 400) {
                _quoteStatusMessage.text('The description is empty or the tags are not supported. Try again.');
                _quoteStatusMessage.attr('class', 'alert alert-danger');
            } else if(resp.status === 500) {
                _quoteStatusMessage.text('You might have some duplicate tags. Try again.');
                _quoteStatusMessage.attr('class', 'alert alert-danger');
            } else {
                _quoteStatusMessage.text('Hmmm, there was a problem editing the quotes');
                _quoteStatusMessage.attr('class', 'alert alert-danger');
            }
        }).catch(error => {
            console.error(error);
            _quoteStatusMessage.empty();
            _quoteStatusMessage.text('Hmmm, there was a problem editing the Tags. Check the API server.');
            _quoteStatusMessage.attr('class', 'alert alert-danger');
        });

        _quoteStatusMessage.show()
        _quoteStatusMessage.fadeOut(5000);
    });

    // Autocomplete for Edit Quote
    $("body").on("keydown", "#quoteTags", async function() {
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
    });

    // Like by quoteId
    $("body").on("click", ".btn-like", async function() {
        let _quoteStatusMessage = $('#quoteStatusMessage')
        let quoteId = $(this).val();

        // Make put request to the quoteId
        await fetch(_quotesUrl + `/${quoteId}`, {
            mode: "cors",
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(resp => {
            _quoteStatusMessage.empty();
            if (resp.status === 200) {
                _quoteStatusMessage.text('The quote has been updated successfully');
                _quoteStatusMessage.attr('class', 'alert alert-success');
            } else {
                _quoteStatusMessage.text('Hmmm, there was a problem editing the quotes');
                _quoteStatusMessage.attr('class', 'alert alert-danger');
            }
        }).catch(error => {
            console.error(error);
            _quoteStatusMessage.empty();
            _quoteStatusMessage.text('Hmmm, there was a problem editing the Tags. Check the API server.');
            _quoteStatusMessage.attr('class', 'alert alert-danger');
        });

        _quoteStatusMessage.show()
        _quoteStatusMessage.fadeOut(5000);
    });

    // first a 1 time call and then set up a timer to call load quotes fn:
    loadQuotes();
    setInterval(loadQuotes, 1000);
});