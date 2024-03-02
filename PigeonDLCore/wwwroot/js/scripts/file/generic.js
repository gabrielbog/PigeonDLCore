/*
    data-handling functions
*/

function fetchData() {
    const searchParams = new URLSearchParams(window.location.search);
    const URL = searchParams.get("URL");

    $.ajax({
        type: 'GET',
        url: '/File/GetFilesJson',
        data: { URL: URL }, 
        dataType: 'json',
        success: function (response) {
            createBody(response);
        }
    });
}

function updateData() {
    const searchParams = new URLSearchParams(window.location.search);
    const URL = searchParams.get("URL");

    $.ajax({
        type: 'GET',
        url: '/File/GetFilesJson',
        data: { URL: URL },
        dataType: 'json',
        success: function (response) {
            updateBody(response);
        }
    });
}

function createBody(data) {

    const jsonData = JSON.stringify(data);
    const parsedData = JSON.parse(jsonData);

    //get old body, prepare new body
    const bodyOld = document.getElementById('table-body');
    const body = document.createElement('tbody');
    body.setAttribute('id', 'table-body');

    //append data to new body
    if (parsedData.length > 0) {
        for (var i = 0; i < parsedData.length; i++) {
            var row = body.insertRow();
            row.setAttribute('id', parsedData[i].url)

            //add data
            const name = parsedData[i].name;
            var cell = row.insertCell();
            cell.textContent = name;

            const size = parsedData[i].size;
            var cell = row.insertCell();
            cell.textContent = size;

            const downloads = parsedData[i].downloads;
            var cell = row.insertCell();
            cell.textContent = downloads;

            const dateUploaded = parsedData[i].dateUploaded;
            var cell = row.insertCell();
            cell.textContent = dateUploaded;

            //add actions
            const showDeleteAction = parsedData[i].showDelete;
            var cell = row.insertCell();
            var url = ""
            if (showDeleteAction == true) {

                var action1 = document.createElement('a');
                action1.setAttribute("href", '/File/Download?URL=' + parsedData[i].url + '');
                action1.innerHTML = "Download";

                const sep = " | ";

                var action2 = document.createElement('a');
                action2.setAttribute("href", '/File/Delete?URL=' + parsedData[i].url + '');
                action2.innerHTML = "Delete";

                cell.append(action1, sep, action2);
            }
            else {
                var action1 = document.createElement('a');
                action1.setAttribute("href", '/File/Download?URL=' + parsedData[i].url + '');
                action1.innerHTML = "Download";

                cell.append(action1);
            }
        }
    }
    else {
        var row = body.insertRow();
        row.setAttribute('id', 'empty-row-message')
        const content = "There are no files in this folder.";
        var cell = row.insertCell();
        cell.colSpan = 5;
        cell.textContent = content;
    }

    //replace old body
    bodyOld.parentNode.replaceChild(body, bodyOld);
}

function updateBody(data) {

    const jsonData = JSON.stringify(data);
    var parsedData = JSON.parse(jsonData);

    //get body element
    const body = document.getElementById('table-body');

    //start with the table rows, search if url exists in parsed data
    //if it does, update rows and pop entry from parsed data
    //if not, remove said row
    //if there are still elements left inside parsed data, add them at the bottom of the list
    //might be a good idea to consider sorting/filtering at the bottom of the list once the operations are done

    //append data to new body
    if (parsedData.length > 0) {

        //remove no file message if it exists
        if (body.rows[0].getAttribute('id') == 'empty-row-message') {
            body.deleteRow(0);
        }

        //update actual list
        var emptyBody = false;
        for (var i = 0, row; row = body.rows[i]; i++) {
            var rowExists = false;
            for (var j = 0; j < parsedData.length; j++) {
                if (row.getAttribute('id') == parsedData[j].url) {
                    row.cells[0].innerHTML = parsedData[j].name;
                    row.cells[1].innerHTML = parsedData[j].size;
                    row.cells[2].innerHTML = parsedData[j].downloads;
                    row.cells[3].innerHTML = parsedData[j].dateUploaded;
                    row.cells[4].innerHTML = "";

                    const showDeleteAction = parsedData[j].showDelete;
                    var url = ""
                    if (showDeleteAction == true) {

                        var action1 = document.createElement('a');
                        action1.setAttribute("href", '/File/Download?URL=' + parsedData[j].url + '');
                        action1.innerHTML = "Download";

                        const sep = " | ";

                        var action2 = document.createElement('a');
                        action2.setAttribute("href", '/File/Delete?URL=' + parsedData[j].url + '');
                        action2.innerHTML = "Delete";

                        row.cells[4].append(action1, sep, action2);
                    }
                    else {
                        var action1 = document.createElement('a');
                        action1.setAttribute("href", '/File/Download?URL=' + parsedData[j].url + '');
                        action1.innerHTML = "Download";

                        row.cells[4].append(action1);
                    }

                    rowExists = true;
                    parsedDataIndex = j;
                    break;
                }
            }

            if (rowExists) {
                parsedData.splice(parsedDataIndex, 1);
            }
            else {
                body.deleteRow(i);
                i--;
            }
        }

        //if there are new elements not in the table, add them
        if (parsedData.length > 0) {
            for (var i = 0; i < parsedData.length; i++) {
                var row = body.insertRow();
                row.setAttribute('id', parsedData[i].url)

                //add data
                const name = parsedData[i].name;
                var cell = row.insertCell();
                cell.textContent = name;

                const size = parsedData[i].size;
                var cell = row.insertCell();
                cell.textContent = size;

                const downloads = parsedData[i].downloads;
                var cell = row.insertCell();
                cell.textContent = downloads;

                const dateUploaded = parsedData[i].dateUploaded;
                var cell = row.insertCell();
                cell.textContent = dateUploaded;

                //add actions
                const showDeleteAction = parsedData[i].showDelete;
                var cell = row.insertCell();
                var url = ""
                if (showDeleteAction == true) {

                    var action1 = document.createElement('a');
                    action1.setAttribute("href", '/File/Download?URL=' + parsedData[i].url + '');
                    action1.innerHTML = "Download";

                    const sep = " | ";

                    var action2 = document.createElement('a');
                    action2.setAttribute("href", '/File/Delete?URL=' + parsedData[i].url + '');
                    action2.innerHTML = "Delete";

                    cell.append(action1, sep, action2);
                }
                else {
                    var action1 = document.createElement('a');
                    action1.setAttribute("href", '/File/Download?URL=' + parsedData[i].url + '');
                    action1.innerHTML = "Download";

                    cell.append(action1);
                }
            }
        }
        else if (emptyBody) {
            //todo: figure out why this doesn't work

            var row = body.insertRow();
            row.setAttribute('id', 'empty-row-message')
            const content = "There are no files in this folder.";
            var cell = row.insertCell();
            cell.textContent = content;
        }
    }
    else { //remove everything if parsedData is empty, means user has no files
        if (body.rows[0].getAttribute('id') != 'empty-row-message') {
            while (body.rows.length > 0) {
                body.deleteRow(0);
            }

            var row = body.insertRow();
            row.setAttribute('id', 'empty-row-message')
            const content = "There are no files in this folder.";
            var cell = row.insertCell();
            cell.colSpan = 5;
            cell.textContent = content;
        }
    }
}

/*
    page-specific functions
*/

var dropArea = document.getElementById('drop-zone')

var isFocused = false;

dropArea.addEventListener('dragstart', function (e) {
    e.preventDefault();
    e.dataTransfer.dropEffect = 'move';
});

dropArea.addEventListener('dragover', function (e) {
    e.preventDefault();
});

dropArea.addEventListener('drop', function(e) {
    e.preventDefault();

    if (e.dataTransfer.items.length > 1) {
        console.log("Only one file");
        throw new Error("Only one file");
    }
    else if (e.dataTransfer.items[0].kind != "file") {
        console.log("Not a file");
        throw new Error("Not a file");
    }
    else {
        const searchParams = new URLSearchParams(window.location.search);
        const URL = searchParams.get("URL");
        const file = e.dataTransfer.files[0];

        var formData = new FormData();
        formData.append('URL', URL);
        formData.append('file', file);

        $.ajax({
            url: '/File/UploadDragDropFiles',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                updateData();
            },
        });
    }
});

function pageIsFocused() {
    if (isFocused == false) {
        updateData(); //consider providing arguments for filtering/sorting
        isFocused = true;
    }
}

function pageIsBlurred() {
    if (isFocused == true) {
        isFocused = false;
    }
}

window.onload = function () {
    fetchData();
};

window.onfocus = pageIsFocused;
window.onblur = pageIsBlurred;
