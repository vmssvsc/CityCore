  TeamMemeber = {
    Variables: {
        srcDeleteDocument: '/TeamMemeber/DeleteDocument',
    },
    Controls: {

        btnDeleteImg: '#deleteImg',
    },

    DeleteDocument: function (id) {
        $.ajax({
            type: 'get',
            url: TeamMemeber.Variables.srcDeleteDocument + '?id=' + id,

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

    $(TeamMemeber.Controls.btnDeleteImg).click(function () {
        var id = $(this).attr('data-id');
        TeamMemeber.DeleteDocument(id)
    });
});