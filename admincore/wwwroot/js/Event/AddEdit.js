Event = {
    Variables: {
        srcSave: '/Event/Save',
    },
    Controls: {
        btnSave: '#btnSave',
        hdnID: '#hdnId',
        txteventTitle: '#txtETitle',
        txteventDescription: '#txtEDescription'
        
    },

    

};

$(document).ready(function () {
    $(Event.Controls.btnSave).click(function () {

        var postObj = new Object();

        postObj.Id = $(Event.Controls.hdnID).val();
        postObj.Title = $(Event.Controls.txteventTitle).val();
        postObj.Description = $(Event.Controls.txteventDescription).val();




        $.ajax({
            type: 'post',
            url: Event.Variables.srcSave,
            data: postObj,
            
            
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