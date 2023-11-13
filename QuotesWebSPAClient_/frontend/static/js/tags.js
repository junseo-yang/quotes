$(document).ready(function () {
    let _tagsList = $('#tagsList');
    let _tagsListMessage = $('#tagsListMessage');

    let _tagsLastModified = new Date(1970, 0, 1);

    let _tagsUrl = 'https://localhost:7223/api/tags';

    let loadTags = async function () {
        // call out to the Web API using fetch (enabling CORS) to get our tags:
        let resp = await fetch(_tagsUrl, {
            mode: "cors",
            headers: {
                'Accept': 'application/json'
            }
        }).catch(function () {
            _tagsList.empty();
            _tagsListMessage.text('Hmmm, there was a problem loading the Tags. Check the API server.');
            _tagsListMessage.attr('class', 'alert alert-danger');
            _tagsListMessage.show()
            _tagsListMessage.fadeOut(3000);
        }
        );

        if (resp.status === 200) {
            let tagsResult = await resp.json();
            let tags = tagsResult.tags;

            if (tags.length === 0) {
                _tagsListMessage.text('No tags to display - use the form to add some.');
                _tagsListMessage.show()
            } else {
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
                                <a class="btn btn-sm btn-danger">Delete</a>
                            </td>
                        </tr>
                        `)
                    }
                }
            }
        } else {
            _tagsListMessage.text('Hmmm, there was a problem loading the tags. Check the API server.');
            _tagsListMessage.attr('class', 'alert alert-danger');
            _tagsListMessage.show()
            _tagsListMessage.fadeOut(3000);
        }
    };

    // first a 1 time call and then set up a timer to call load todos fn:
    loadTags();
    setInterval(loadTags, 1000);
});