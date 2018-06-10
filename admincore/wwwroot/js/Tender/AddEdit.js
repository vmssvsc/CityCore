Tender = {
    Variables: {
        srcDeleteDocument: '/Tender/DeleteDocument',
    },
    Controls: {
        btnDeletePostDoc: '#deletePostDoc',
        btnDeleteFormDoc: '#deleteFormDoc',       
    },

    DeleteDocument: function (id) {
        $.ajax({
            type: 'get',
            url: Tender.Variables.srcDeleteDocument + '?id=' + id,

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
    $(Tender.Controls.btnDeletePostDoc).click(function () {
        var id = $(this).attr('data-id');
        Tender.DeleteDocument(id, 1)
    });
    $(Tender.Controls.btnDeleteFormDoc).click(function () {
        var id = $(this).attr('data-id');
        Tender.DeleteDocument(id, 2)
    });
});