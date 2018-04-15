AlbumList = {
    Variables: {
        srcEdit: '/Gallery/Save',
        srcList: '/Gallery/GetList',
        srcDelete: '/Gallery/Delete',
        oTable: null,
    },
    Controls: {
        table: '#tblAlbums',
    },
    IntializeTable: function () {
        AlbumList.Variables.oTable = $(AlbumList.Controls.table).dataTable({
            "sAjaxSource": AlbumList.Variables.srcList,
            //"aaSorting": [[1, "desc"]],// default sorting
            "sDom": "frtlip",
            "autoWidth": false,
            "bLengthChange": true,
            "fixedHeader": true,
            "aoColumnDefs": [
                {
                    "aTargets": [0],
                    "visible": false
                },
                {
                    "aTargets": [1],
                },
                {
                    "aTargets": [2],
                },
                {
                    "aTargets": [3],
                },
                {
                    "aTargets": [4],
                    "mRender": function (data, type, full) {
                        return "<a href='/Gallery/AddEdit?id=" + full[0] + "' ><i class='fa fa-edit'></i></a><a href='javascript:void(0)' onClick=\"\AlbumList.DeleteAlbum('" + full[0] + "')\"\><i class='fa fa-trash-o'></i></a>";
                    },
                    "sortable": false,
                    "className": "text-center",
                }

            ],

            "oLanguage": {
                "sEmptyTable": $('#hdnNodataavailable').val(),
                "sLengthMenu": "Page Size: _MENU_",
                "oPaginate": {
                    "sFirst": $('#hdnFirst').val(),
                    "sLast": $('#hdnLast').val(),
                    "sNext": $('#hdnNext').val(),
                    "sPrevious": $('#hdnPrevious').val()
                }
            },
            "fnServerParams": function (aoData) {
                $("div").data("srchParams",
                    [
                        //{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
                        { name: 'srchTxt', value: encodeURIComponent($(AlbumList.Controls.txtSearch).val() == '' ? '' : $(AlbumList.Controls.txtSearch).val()) },
                        { name: 'srchBy', value: 'ALL' },


                        //{
                        //    name: 'date', value: $('#txtDate').val().toString().trim() == "" ? "" : common.formatDate($('#txtDate').val().toString().trim()), //($('#ddlStationId option:selected').val() == "Select" || $('#ddlStationId option:selected').val() == "0") ? "" : $('#ddlStationId option:selected').val()
                        //},
                    ]);

                var srchParams = $("div").data("srchParams");
                if (srchParams) {
                    for (var i = 0; i < srchParams.length; i++) {
                        aoData.push({ "name": "" + srchParams[i].name + "", "value": "" + srchParams[i].value + "" });
                    }
                }
            },
            "fnInfoCallback": function (oSettings, iStart, iEnd, iMax, iTotal, sPre) {
                // return common.pagingText(iStart, iEnd, iTotal, $('#hdnRecordsText').val(), oSettings._iDisplayLength);

            },
        });
    },

    reloadList: function () {
        $(AlbumList.Controls.table).DataTable().ajax.reload();
        },

    DeleteAlbum: function (id) {
        bootbox.confirm("Are you sure you want to delete this album? All the photos inside it will be deleted.",
            function (result) {
                if (result) {
                    $.ajax({
                        type: 'get',
                        url: AlbumList.Variables.srcDelete + '?id=' + id,
                        success: function (data) {
                            if (data.success) {
                                Common.Success(data.message);
                                AlbumList.reloadList();
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
            });        
    }

};

$(document).ready(function () {
    AlbumList.IntializeTable();
});