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

function createBody(data) {

    const jsonData = JSON.stringify(data);
    const parsedData = JSON.parse(jsonData);

    console.log(parsedData);

    //get old body, prepare new body
    const bodyOld = document.getElementById('table-body');
    const body = document.createElement('tbody');
    body.setAttribute('id', 'table-body');

    //append data to new body
    if (parsedData.length > 0) {
        for (var i = 0; i < parsedData.length; i++) {
            var row = body.insertRow();

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

        const content = "There are no files in this folder.";
        var cell = row.insertCell();
        cell.textContent = content;
    }

    //replace old body
    bodyOld.parentNode.replaceChild(body, bodyOld);
}

window.onload = function () {
    fetchData();
};