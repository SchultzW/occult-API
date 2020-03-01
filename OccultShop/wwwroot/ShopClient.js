const URL = "https://localhost:44372/Api/Product";

function getAllProds(onloadHandler) {
    var xhr = new XMLHttpRequest();
    xhr.onload = onloadHandler;
    xhr.onerror = errorHandler;
    xhr.open("GET", URL, true);
    xhr.send();
}

function fillTable() {
    var prods = JSON.parse(this.responseText);
    console.log(prods);
    for (var i in prods) {
        addRow(prods[i]);
    }
}

//adds info to a row
function addRow(prod) {
    var tbody = document.getElementsByTagName('tbody')[0];
    var fields = ["title", "description", "price", "tag", "imgPath"];
    var tr = document.createElement('tr');
    //add a cell for each field
    for (var i in fields) {
        var td = document.createElement('td');
        var field = fields[i];
        if (field == "title") {
            td.innerHTML = prod[field];
        }
        else if (field == "description") {
            td.innerHTML = prod[field];
        } else if (field == "price") {
            td.innerHTML = prod[field];
        } else if (field == 'tag') {
            td.innerHTML = prod[field];
        }
        else if (field == "imgPath") {
            td.innerHTML = prod[field];
        }
    
        tr.appendChild(td);
    }
    tbody.appendChild(tr);
}

function errorHandler(e) {
    window.alert("API request faild :(")
}

//retrieve data from form
function getData() {
    var data = {};
    var form = document.getElementById("prodForm");
    for (var i = 0; i < form.length; i++) {
        var input = form[i];
        if (input.name) {
            data[input.name] = input.value;
        }
    }
    return data;
}
//sends obj to db
function addProd() {
    var data = getData();

    var xhr = new XMLHttpRequest();
    // Parameters: request type, URL, async (if false, send does not return until a response is recieved)
    xhr.open("Post", URL, true);
    xhr.setRequestHeader("content-type", "Application/json");
    xhr.onerror = errorHandler;
    xhr.onreadystatechange = function () {
        // if readyState is "done" and status is "success"
        if (xhr.readyState == 4 && xhr.status == 200) {
            addRow(JSON.parse(xhr.responseText));
        }
    };
    //change to string
    var dataString = JSON.stringify(data);
    xhr.send(dataString);
}

function fillSelectList() {
    var prods = JSON.parse(this.responseText);
    var sel = document.getElementsByTagName("Select")[0];
    for (var i in prods) {
        var opt = document.createElement("option");
        opt.setAttribute("value", prods[i].productId);
        opt.innerHTML = prods[i].title;
        sel.appendChild(opt);
    }
}

function clearSelectList() {
    var select = document.getElementsByTagName("select")[0];
    var length = select.options.length;

    for (i = 1; i < length; i++) {
        select.options[i] = null;
    }
}

//called when a prod is selected from select list
function getProdById(event) {
    var xhr = new XMLHttpRequest();
    xhr.onload = fillForm;
    xhr.onerror = errorHandler;
    xhr.open("GET", URL + "/" + event.target.value, true);
    xhr.send();
}

//fills form with prod info

function fillForm() {
    var prod = JSON.parse(this.responseText);

    var inputs = document.getElementsByTagName("input");
    inputs[0].value = prod.productId;
    inputs[1].value = prod.title;
    inputs[2].value = prod.description;
    inputs[3].value = prod.price;
    inputs[4].value = prod.tag;
    inputs[5].value = prod.imgPath;
    
}

//send new data to DB

function updateProd() {
    var patchCommand = {};
    var form = document.getElementById("prodForm");
    patchCommand.value = form[4].value;
    patchCommand.op = "replace";
    patchCommand.path = "price";

    //create patch req
    var xhr = new XMLHttpRequest();
    var productId = form[1].value;
    xhr.open("PATCH", URL + "/" + productId, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onerror = errorHandler;
    // serialize the data to a string so it can be sent
    var dataString = JSON.stringify(patchCommand);
    xhr.send(dataString);
    clearSelectList();

}

function replaceProd() {
    var data = getData();

    //put req
    var xhr = new XMLHttpRequest();
    xhr.open("PUT", URL + "/" + data.productId, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onerror = errorHandler;

    // serialize the data to a string so it can be sent
    var dataString = JSON.stringify(data);
    xhr.send(dataString);
    clearSelectList();
}

//remove a prod
function deleteProd() {
    var data = getData();
    var xhr = new XMLHttpRequest();
    xhr.open("DELETE", URL + "/" + data.productId, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onerror = errorHandler;
    xhr.onreadystatechange = function () {
        // if readyState is "done" and status is "success"
        if (xhr.readyState == XMLHttpRequest.DONE && xhr.status == 204) {
            clearSelectList();
            getAllProd(fillSelectList);
        }
    };
    xhr.send();
}