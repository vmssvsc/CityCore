Event = {
    Variables: {
        srcDeleteDocument: '/Event/DeleteDocument',
    },
    Controls: {
        btnDeleteDoc: '#deleteDoc',
        btnDeleteImg: '#deleteImg',       
    },

    DeleteDocument: function (id, type) {
        $.ajax({
            type: 'get',
            url: Event.Variables.srcDeleteDocument + '?id=' + id + '&type=' + type,

            success: function (data) {
                if (data.success) {
                    Common.Success(data.message);
                    window.location.reload();
                }
                else {
                    Common.Error(data.message);
                }
            },
            error: function () {
                Common.Error(data.message);
            }
        });
    }
};

$(document).ready(function () {
    $(Event.Controls.btnDeleteDoc).click(function () {
        var id = $(this).attr('data-id');
        Event.DeleteDocument(id, 1)
    });
    $(Event.Controls.btnDeleteImg).click(function () {
        var id = $(this).attr('data-id');
        Event.DeleteDocument(id, 2)
    });
});