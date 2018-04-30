SmartProject = {
    Variables: {
        srcDeleteDocument: '/HomePageProject/DeleteDocument',
    },
    Controls: {
        
        btnDeleteImg: '#deleteImg',
    },

    DeleteDocument: function (id) {
        $.ajax({
            type: 'get',
            url: SmartProject.Variables.srcDeleteDocument + '?id=' + id ,

            success: function (data) {
                if (data.success) {
                    Common.Success(data.message);
                    window.location.reload();
                }
                else {
                    Common.Error(data.message);
                }
            },
            error: function (xhr,status,errorThrown) {
                Common.Error(errorThrown);
            }
        });
    }
};

$(document).ready(function () {
    
    $(SmartProject.Controls.btnDeleteImg).click(function () {
        var id = $(this).attr('data-id');
        SmartProject.DeleteDocument(id)
    });
});