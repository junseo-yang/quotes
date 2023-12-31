/* 
    tags.js
    Assignment 3
    
    Revision History
        Junseo Yang, 2023-11-19: Created
*/

$(document).ready(function () {
    let _tagsList = $('#tagsList');
    let _tagsListMessage = $('#tagsListMessage');
    let _tagStatusMessage = $('#tagStatusMessage')

    let _tagsLastModified = new Date(1970, 0, 1);

    let _tagsUrl = 'https://localhost:7223/api/tags';

    let loadTags = async function () {
        // call out to the Web API using fetch (enabling CORS) to get our tags:
        await fetch(_tagsUrl, {
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
            let tagsResult = data;
            let tags = tagsResult.tags;

            if (tags.length === 0) {
                _tagsListMessage.text('No tags to display - use the form to add some.');
                _tagsListMessage.show()
            } else {
                _tagsListMessage.hide()

                let latestLastModified = new Date(tagsResult.tagsLastModified);

                if (latestLastModified.getTime() > _tagsLastModified.getTime()) {
                    _tagsLastModified = latestLastModified;

                    // loop thru the tags and add them to the Cards...

                    _tagsList.empty();

                    for (let i = 0; i < tags.length; i++) {
                        _tagsList.append(`
                        <tr>
                            <th scope="row">${tags[i].tagId}</th>
                            <td>${tags[i].name}</td>
                            <td>
                                <a class="btn btn-sm btn-primary" href="/tags/${tags[i].tagId}">Edit</a>
                            </td>
                        </tr>
                        `)
                    }
                }
            }
        }).catch(error => {
            console.error(error);
            _tagsList.empty();
            _tagStatusMessage.empty();
            _tagStatusMessage.text('Hmmm, there was a problem loading the Tags. Check the API server.');
            _tagStatusMessage.attr('class', 'alert alert-danger');
            _tagStatusMessage.show()
            _tagStatusMessage.fadeOut(5000);
        });
    };

    // add a click handler to POST new tags to our API:
    $('#addTagBtn').click(async function () {
        // Create a new tag by reading the form input fields:
        let newTag = {
            name: $('#tagName').val()
        };

        await fetch(_tagsUrl, {
            mode: "cors",
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newTag)
        }).then(resp => {
            _tagStatusMessage.empty();
            if (resp.status === 201) {
                _tagStatusMessage.text('The tag was added successfully');
                _tagStatusMessage.attr('class', 'alert alert-success');
                $('#tagName').val('')
            } else if(resp.status === 400) {
                _tagStatusMessage.text('The Name cannot be empty.');
                _tagStatusMessage.attr('class', 'alert alert-danger');
            } else if(resp.status === 409) {
                _tagStatusMessage.text('The tag already exists. Try another one.');
                _tagStatusMessage.attr('class', 'alert alert-danger');
            } else {
                _tagStatusMessage.text('Hmmm, there was a problem adding the tag');
                _tagStatusMessage.attr('class', 'alert alert-danger');
            }
        }).catch(error => {
            console.error(error);
            _tagStatusMessage.empty();
            _tagStatusMessage.text('Hmmm, there was a problem adding the Tags. Check the API server.');
            _tagStatusMessage.attr('class', 'alert alert-danger');
        });

        _tagStatusMessage.show()
        _tagStatusMessage.fadeOut(5000);
    });

    // add a click handler to PUT a updated tag to our API:
    $("body").on("click", "#editTagBtn", async function() {
        let _tagStatusMessage = $('#tagStatusMessage')

        // Update an existing tag by reading the form input fields:
        let updatedTag = {
            tagId: $('#tagId').val(),
            name: $('#tagName').val()
        };

        await fetch(_tagsUrl, {
            mode: "cors",
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedTag)
        }).then(resp => {
            _tagStatusMessage.empty();
            if (resp.status === 200) {
                _tagStatusMessage.text('The tag has been updated successfully');
                _tagStatusMessage.attr('class', 'alert alert-success');
            } else if(resp.status === 400) {
                _tagStatusMessage.text('The Name cannot be empty.');
                _tagStatusMessage.attr('class', 'alert alert-danger');
            } else if(resp.status === 409) {
                _tagStatusMessage.text('The tag already exists. Try another one.');
                _tagStatusMessage.attr('class', 'alert alert-danger');
            } else {
                _tagStatusMessage.text('Hmmm, there was a problem editing the tags');
                _tagStatusMessage.attr('class', 'alert alert-danger');
            }
        }).catch(error => {
            console.error(error);
            _tagStatusMessage.empty();
            _tagStatusMessage.text('Hmmm, there was a problem editing the Tags. Check the API server.');
            _tagStatusMessage.attr('class', 'alert alert-danger');
        });

        _tagStatusMessage.show()
        _tagStatusMessage.fadeOut(5000);
    });

    // first a 1 time call and then set up a timer to call load tags fn:
    loadTags();
    setInterval(loadTags, 1000);
});