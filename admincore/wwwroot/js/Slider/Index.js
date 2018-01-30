Slider = {
    Variables: {
        srcUpload: '/Slider/Save'
    },
    Controls: {
        btnSave: '.btnSave'
    },

    UploadFile: function () {

    }
}

$(document).ready(function () {
    $(Slider.Controls.btnSave).click(function () {
        $.ajax({
            type: 'post',
            url: formAction,
            data: fdata,
            processData: false,
            contentType: false
        })
    });
});