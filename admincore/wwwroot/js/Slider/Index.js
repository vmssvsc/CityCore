Slider = {
    Variables: {
        srcUpload: '/Slider/Save',
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
        var fileInput = $(this).parent().parent().find('input')[0];
        var file = fileInput.files[0];
        fdata.append("file", file);

        $.ajax({
            type: 'post',
            url: Slider.Variables.srcUpload,
            data: fdata,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.success) {
                    Common.Success(data.message);
                }
                else {
                    Common.Error(data.message);
                }
            },
            error: function() {
                Common.Error(data.message);
            }
        });
    });
});