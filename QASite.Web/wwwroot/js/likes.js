$(() => {   
    setInterval(() => {
        const id = $("#current-id").val();      
        $.get("/Home/CurrentLikes", { id }, function (likes) {
            $("#likes-count").text(likes);
        })
    }, 1000)

    $("#like-btn").on('click', function () {       
        const questionId = $(this).data("question-id");
        var like = $(this).data('is-liked');        
        if (like=='False') {
            $.post("/Home/AddLike", { questionId }, function (likes) {
            })
            $(this).addClass("text-danger");           
        }
        $(this).unbind('click');
       
    })
})