EventList = {
    Variables: {
        srcSave: '/Events/Save',
    },
    Controls: {
        btnSave: '.btnUpload',
        sliderImageInput: '.sliderImage',
        btnPreview: '.btnPreveiew',
        btnDelete: '.btnDelete',
        seqNo: '.seqNo',
        preview: '.preview',
        fileInput: '.sliderImage',
        sliderName: '.sliderName'
    }

};

$(document).ready(function () {
    $(EventList.Controls.btnSave).click(function () {
        
        $.ajax({
            type: 'post',
            url: EventList.Variables.srcUpload,
            data: fdata,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.success) {
                    //Set controls
                   
                }
                else {
                    Common.Error(data.message);
                }
            },
            error: function () {
                Common.Error(data.message);
            }
        });
    });
});