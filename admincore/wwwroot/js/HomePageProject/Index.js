SmartProjectList = {
    Variables: {
        srcEdit: '/HomePageProject/Save',
        srcList: '/HomePageProject/GetList',
        srcDelete: '/HomePageProject/Delete',
        oTable: null,
    },
    Controls: {
        table: '#tblProjects',
    },
    IntializeTable: function () {
        SmartProjectList.Variables.oTable = $(SmartProjectList.Controls.table).dataTable({
            "sAjaxSource": SmartProjectList.Variables.srcList,
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
                        return "<a href='/HomePageProject/AddEdit?id=" + full[0] + "' ><i class='fa fa-edit'></i></a><a href='javascript:void(0)' onClick=\"\SmartProjectList.DeleteSmartProject('" + full[0] + "')\"\><i class='fa fa-trash-o'></i></a>";
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
                        { name: 'srchTxt', value: encodeURIComponent($(SmartProjectList.Controls.txtSearch).val() == '' ? '' : $(SmartProjectList.Controls.txtSearch).val()) },
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
        $(SmartProjectList.Controls.table).DataTable().ajax.reload();
    },

    DeleteSmartProject: function (id) {
        bootbox.confirm("Are you sure you want to delete this  Smart Project ?",
            function (result) {
                if (result) {
                    $.ajax({
                        type: 'get',
                        url: SmartProjectList.Variables.srcDelete + '?id=' + id,
                        success: function (data) {
                            if (data.success) {
                                Common.Success(data.message);
                                SmartProjectList.reloadList();
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
            });

    }

};

$(document).ready(function () {
    SmartProjectList.IntializeTable();
});