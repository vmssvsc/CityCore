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
                },
                {
                    "aTargets": [2],
                },
                {
                    "aTargets": [3],
                },
                {
                    "aTargets": [4],
                },
                {
                    "aTargets": [5],
                },

                {
                    "aTargets": [6],
                    "mRender": function (data, type, full) {
                        return "<a href='" + full[8] +"' ><i class='fa fa-pdf'></i></a>";
                    },
                    "sortable": false,
                    "className": "text-center",
                },
                {
                    "aTargets": [7],
                    "mRender": function (data, type, full) {
                        return "<a href='" + full[9] + "' ><i class='fa fa-pdf'></i></a>";
                    },
                    "sortable": false,
                    "className": "text-center",
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