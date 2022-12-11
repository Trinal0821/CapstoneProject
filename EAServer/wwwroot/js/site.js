function changeTitle() {
    title = document.getElementById('thisbutton');
    console.log("Hello there from the other side");

    $.ajax({
        type: "GET",
        url: "/Home/Run",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log("the list", msg)
        },
        error: function (req, status, error) {
            console.log(":(")
        }
    });
}