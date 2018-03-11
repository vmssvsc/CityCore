Event = {
    Variables: {
        srcSave: '/Events/Save',
    },
    Controls: {
        btnSave: '#btnSave',
        
    }

};

$(document).ready(function () {
    $(Event.Controls.btnSave).click(function () {

        $.ajax({
            type: 'post',
            url: Event.Variables.srcUpload,
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