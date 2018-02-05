Slider = {
    Variables: {
        srcUpload: '/Slider/Save'
    },
    Controls: {
        btnSave: '.btnUpload',
        sliderImageInput: '.sliderImage'
    },

    UploadFile: function () {

    }
}

$(document).ready(function () {
    $(Slider.Controls.btnSave).click(function () {
        var fdata = new FormData();
        var fileInput = $('.sliderImage')[0];
        var file = fileInput.files[0];
        fdata.append("file", file);

        $.ajax({
            type: 'post',
            url: Slider.Variables.srcUpload,
            data: fdata,
            processData: false,
            contentType: false
        })
    });
});