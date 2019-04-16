var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("ReceiveMessage", (message) => {
    var template = $('#announcement-template').html();
    var html = Mustache.render(template, {
        Content: message.content,
        DateCreated: moment(message.DateCreated).fromNow(),
        Id: message.id,
        Title: message.title, 
    });
    $('#annoncementList').prepend(html);
    
    var totalAnnounce = parseInt($('#totalAnnouncement').text()) + 1;

    $('#totalAnnouncement').text(totalAnnounce);

    loadAnnouncement();
});

