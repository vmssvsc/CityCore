CareerList = {
    Variables: {      
        srcList: '/Career/GetList',     
        oTable: null,
    },
    Controls: {
        table: '#tblCareerList',
    },
    IntializeTable: function () {
        CareerList.Variables.oTable = $(CareerList.Controls.table).dataTable({
            "sAjaxSource": CareerList.Variables.srcList,
            //"aaSorting": [[1, "desc"]],// default sorting
            "sDom": "frtlip",
            "autoWidth": false,
            "bLengthChange": true,
            "fixedHeader": true,
            "processing": true,
            "serverSide": true,
            //"searching": true,
           //  "dom": "ltip",  // Remove global search box
            "aoColumnDefs": [
                {
                    "aTargets": [0],
                    "visible": false
                },
                {
                    "aTargets": [1],
                    "className": "set-center",
                },
                {
                    "aTargets": [2],
                    "className": "set-center",
                },
                {
                    "aTargets": [3],
                    "className": "set-center",
                },
                {
                    "aTargets": [4],
                    "className": "set-center",
                },
                {
                    "aTargets": [5],
                    "className": "set-center",
                },

                {
                    "aTargets": [6],
                    "mRender": function (data, type, full) {
                        return "<a href='" + full[8] + "' target='_blank' title='Download pdf'><i class='far fa-file-pdf' style='font-size:20px; color:red'></i></a>";
                    },
                    "sortable": false,
                    "className": "set-center",
                },
                {
                    "aTargets": [7],
                    "mRender": function (data, type, full) {
                        return "<a href='" + full[9] + "' target='_blank' title='Download pdf'><i class='far fa-file-pdf' style='font-size:20px; color:red'></i></a>";
                    },
                    "sortable": false,
                    "className": "set-center",
                },

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
                        { name: 'srchTxt', value: encodeURIComponent($('.dataTables_filter input').val() == '' ? '' : $('.dataTables_filter input').val()) },
                       // { name: 'srchBy', value: 'ALL' },


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
        $(CareerList.Controls.table).DataTable().ajax.reload();
    },
    
};

$(document).ready(function () {
    CareerList.IntializeTable();
});