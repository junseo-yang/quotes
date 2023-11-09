$(document).ready(function () {
    let _quotesList = $('#quotesList');
    let _quotesListMessage = $('#quotesListMessage');
    let _newQuoteMessage = $('#newQuoteMessage');

    let _quotesLastModified = new Date(1970, 0, 1);

    let _quotesUrl = 'https://localhost:7223/api/quotes';

    let _availableTags = '';

    let loadQuotes = async function () {
        // call out to the Web API using fetch (enabling CORS) to get our quotes:
        let resp = await fetch(_quotesUrl, {
            mode: "cors",
            headers: {
                'Accept': 'application/json'
            }
        }).catch(function () {
            _quotesList.empty();
            _quotesListMessage.text('Hmmm, there was a problem loading the quotes. Check the API server.');
            _quotesListMessage.attr('class', 'alert alert-danger');
            _quotesListMessage.show()
            _quotesListMessage.fadeOut(3000);
        }
        );

        if (resp.status === 200) {
            let quotesResult = await resp.json();
            let quotes = quotesResult.quotes;

            // Populate availableTags
            _availableTags = quotesResult.tags;

            if (quotes.length === 0) {
                _quotesListMessage.text('No quotes to display - use the form to add some.');
                _quotesListMessage.show()
            } else {
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
                            ${(function displayTags() {
                                let tags = '';
                                for (let j = 0; j < quotes[i].tags.length; j++) {
                                    tags += `<span class="badge rounded-pill bg-secondary">${quotes[i].tags[j]}</span>`;
                                }
                                return tags;
                            }())}
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

        } else {
            _quotesListMessage.text('Hmmm, there was a problem loading the quotes. Check the API server.');
            _quotesListMessage.attr('class', 'alert alert-danger');
            _quotesListMessage.show()
            _quotesListMessage.fadeOut(3000);
        }
    };

    // add a click handler to POST new tasks to our API:
    $('#addTaskBtn').click(async function () {
        // Create a new task by reading the form input fields:
        let dueDate = new Date($('#taskDuedate').val());
        let newTask = {
            description: $('#taskDescription').val(),
            dueDate: dueDate.toISOString(),
            category: $('#taskCategory').val()
        };

        let resp = await fetch(_tasksUrl, {
            mode: "cors",
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newTask)
        });

        if (resp.status === 201) {
            _newTodoMsg.text('The task was added successfully');
            _newTodoMsg.attr('class', 'text-success');
            $('#taskDescription').val('')
        } else {
            _newTodoMsg.text('Hmmm, there was a problem loading the tasks');
            _newTodoMsg.attr('class', 'text-danger');
        }
        _newTodoMsg.fadeOut(10000);
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