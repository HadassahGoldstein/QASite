$(() => {   
    setInterval(() => {
        const id = $("#current-id").val();      
        $.get("/Home/CurrentLikes", { id }, function (likes) {
            $("#likes-count").text(likes);
        })
    }, 1000)

    $("#like-btn").on('click', function () {
        const id = $(this).data("question-id");
        $(this).prop('disabled', true);
        $.post("/Home/AddLike", { id }, function (likes) {
        })       
        $(this).addClass("text-danger");
    })
})