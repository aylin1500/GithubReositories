

function SaveMark(obj) {
    var imageSrc = $(obj).closest('tr').find('img')[0].src;
    var nameRep = $(obj).closest('tr').find('span')[0].innerText;
    var idRep = $(obj).closest('tr')[0].id;

    var jsonObject = {
        "Id": idRep,
        "Name": nameRep,
        "ImageSrc": imageSrc
    };

    $.ajax({
        url: "Home/AddRepositoryMark",
        type: "POST",
        data: JSON.stringify(jsonObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (response) {
        },
        success: function (response) {
            $(obj).removeClass("btn-info");
            $(obj).addClass("btn-warning");
        }
    });
}