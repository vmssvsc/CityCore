VideoList = {
    Variables: {
        srcEdit: '/Video/Save',
        srcList: '/Video/GetList',
        srcDelete: '/Video/Delete',
        oTable: null,
    },
    Controls: {
        table: '#tblVideos',
    },
    IntializeTable: function () {
        VideoList.Variables.oTable = $(VideoList.Controls.table).dataTable({
            "sAjaxSource": VideoList.Variables.srcList,
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
                    "mRender": function (data, type, full) {
                        return "<a href='/Video/AddEdit?id=" + full[0] + "' ><i class='fa fa-edit'></i></a><a href='javascript:void(0)' onClick=\"\VideoList.DeleteVideo('" + full[0] + "')\"\><i class='fa fa-trash-o'></i></a>";
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
                        { name: 'srchTxt', value: encodeURIComponent($(VideoList.Controls.txtSearch).val() == '' ? '' : $(VideoList.Controls.txtSearch).val()) },
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
        $(VideoList.Controls.table).DataTable().ajax.reload();
    },

    DeleteVideo: function (id) {
        bootbox.confirm("Are you sure you want to delete this Video?",
            function (result) {
                if (result) {
                    $.ajax({
                        type: 'get',
                        url: VideoList.Variables.srcDelete + '?id=' + id,
                        success: function (data) {
                            if (data.success) {
                                Common.Success(data.message);
                                VideoList.reloadList();
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
    VideoList.IntializeTable();
});