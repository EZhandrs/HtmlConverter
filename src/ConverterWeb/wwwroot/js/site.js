// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notification")
    .build();

hubConnection.on("PdfConvertion", function (data) {
    console.log("signal");
    var pElement = $('<p></p>');
    pElement.text(data.fileName + ' conversion');
    pElement.attr('data-file-id', data.fileId);
    $('#progress').append(pElement);
});

hubConnection.on("PdfConverted", function (data) {
    var aElement = $('<a></a>');
    aElement.attr('href', '/download/' + data.fileId);
    aElement.text(' download');
    var pElement = $('#progress').find('[data-file-id="' + data.fileId + '"]');
    pElement.text(data.fileName + " ");
    pElement.append(aElement);
});

hubConnection.start();

$('form').submit(function (e) {
    e.preventDefault();

    var formData = new FormData(this);
    formData.append('connectionId', hubConnection.connection.connectionId)

    var action = $(this).attr("action");

    $.ajax({
        url: action,
        type: 'POST',
        data: formData,
        cache: false,
        contentType: false,
        processData: false
    });
});