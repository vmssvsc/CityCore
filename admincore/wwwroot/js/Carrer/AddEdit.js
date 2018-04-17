Career = {
    Variables: {
        srcDeleteDocument: '/Career/DeleteDocument',
    },
    Controls: {
        btnDeletePostDoc: '#deletePostDoc',
        btnDeleteFormDoc: '#deleteFormDoc',       
    },

    DeleteDocument: function (id, type) {
        $.ajax({
            type: 'get',
            url: Career.Variables.srcDeleteDocument + '?id=' + id + '&type=' + type,

            success: function (data) {
                if (data.success) {
                    Common.Success(data.message);
                    window.location.reload();
                }
                else {
                    Common.Error(data.message);
                }
            },
            error: function (xhr, status, errorThrown) {
                Common.Error(errorThrown);
            }
        });
    }
};

$(document).ready(function () {
    $(Career.Controls.btnDeletePostDoc).click(function () {
        var id = $(this).attr('data-id');
        Career.DeleteDocument(id, 1)
    });
    $(Career.Controls.btnDeleteFormDoc).click(function () {
        var id = $(this).attr('data-id');
        Career.DeleteDocument(id, 2)
    });
});