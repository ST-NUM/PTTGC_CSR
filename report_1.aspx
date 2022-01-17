<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="report_1.aspx.cs" Inherits="report_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .cPointer {
            cursor: pointer !important;
        }

        .circle {
            display: inline-block;
            width: 25px;
            height: 25px;
            border-radius: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <div class="row">

                <div id="divSearch" class="col">
                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">รหัส/ชื่อ โครงการ</label>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" placeholder="รหัส/ชื่อ โครงการ"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">ประเภท</label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">Dimension</label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlDimension" runat="server" CssClass="form-control" disabled="true"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">ปี</label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">สถานะ</label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3"></label>
                        <div class="col-lg-6">
                            <button id="btnSearch" type="button" class="btn btn-inverse">
                                <i class="fa fa-search"></i><span>&nbsp; ค้นหา</span>
                            </button>
                            <button id="btnExportProject" type="button" class="btn btn-success" style="display: none">
                                <i class="fa fa-file-excel"></i><span>&nbsp; พิมพ์เอกสาร</span>
                            </button>
                            <asp:Button ID="btnExportProject_" runat="server" OnClick="btnExportProject_Click" CssClass="hide" />
                        </div>
                    </div>

                </div>
            </div>

            <div id="divTable" class="table-responsive" style="display: none;">
                <table id="tbData" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 7%" class="text-center">
                                <asp:CheckBox ID="cbHead" runat="server" CssClass="checkbox" Text="ที่" />
                            </th>
                            <th class="text-center" data-sort="sProjectName">ชื่อโครงการ</th>
                            <th style="width: 10%" class="text-center" data-sort="sType">ประเภท</th>
                            <th style="width: 12%" class="text-center" data-sort="sDimension">Dimension</th>
                            <th style="width: 12%" class="text-center" data-sort="nBudget">งบโครงการ</th>
                            <th style="width: 12%" class="text-center" data-sort="nBudgetUsed">งบโครงการ<br />
                                ที่ใช้ไป</th>
                            <%--<th style="width: 10%" class="text-center" data-sort="sBudgetStatus">สถานะงบ</th>--%>
                            <th style="width: 10%" class="text-center" data-sort="sStatus">สถานะ</th>
                            <th style="width: 5%" class="text-center"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">ไม่พบข้อมูล</div>
            </div>

            <%--<div class="form-row pt-3">
                <div class="col-auto">
                    <div class="circle bg-success"></div>
                    ใช้งบต่ำกว่า 80%
                </div>
                <div class="col-auto">
                    <div class="circle bg-warning"></div>
                    ใช้งบระหว่าง 80-100% 
                </div>
                <div class="col-auto">
                    <div class="circle bg-danger"></div>
                    ใช้งบมากกว่า 100% 
                </div>
            </div>--%>

            <div id="divPaging" class="form-row align-items-center pt-3" style="display: none;">
                <div class="col-lg-2 mb-3">
                    <%--<button type="button" id="btnExport" class="btn btn-danger" title="ลบ"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp; ลบ</button>--%>
                </div>
                <div class="col-lg-8 mb-3 text-center">
                    <ul id="pagData" class="pagination small d-inline-flex"></ul>
                </div>
                <div class="col-lg-2 mb-3">
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fa fa-table"></i></span>
                        </div>
                        <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control height-custom"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:Button ID="btnExportTransaction" Text="text" runat="server" OnClick="btnExportTransaction_Click" CssClass="hide" />

    <asp:HiddenField ID="hddPermission" runat="server" />
    <asp:HiddenField ID="hddProjectID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <!-- Modal Total Used -->
    <div id="divModalGL" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="divModalGL" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-info">
                    <h5 id="hDTitel" class="modal-title">งบประมาณที่ใช้ไปแต่ละ GL</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="table-responsive">
                        <table id="tbGL" class="table table-bordered table-hover table-sm">
                            <thead>
                                <tr class="valign-middle pad-primary">
                                    <th style="width: 20%" class="text-center">รหัส GL</th>
                                    <th class="text-center">ชื่อ GL</th>
                                    <th style="width: 20%" class="text-center">งบที่ใช้ไป</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var sPageEdit = "project_edit.aspx";
        var $Permission = GetValTextBox('hddPermission');
        var $cbHead = $('input[id$=cbHead]');
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var $btnSearch = $('button#btnSearch');
        var $btnAdd = $('a#btnAdd');
        var $btnExportProject = $('button#btnExportProject');
        var arrData = [];
        var arrCheckBox = [];

        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');
        function SortingEvent() { }

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    //SearchData();
                    SortingBind($tbData, SortingData);
                }
            }
        });

        function SetControl() {
            $cbHead.change(function () {
                var isChecked = $(this).is(':checked');
                var $cbRec = $('input[id*=cbRec_]:checkbox');
                $cbRec.prop('checked', isChecked);

                $('input[id*=cbRec_]:checkbox').each(function () {
                    var sVal = $(this).attr('id').split('_')[1];
                    if ($(this).is(":checked")) {
                        var obj = {
                            nID: sVal,
                        };
                        arrCheckBox.push(obj);
                    } else {
                        arrCheckBox = Enumerable.From(arrCheckBox).Where(function (w) { return w.nID != sVal }).ToArray();
                    }
                })
            });

            $tbData
                .delegate('input[id^="cbRec_"]:checkbox', 'change', function () {
                    var $cbRec = $('input[id^="cbRec_"]:checkbox');
                    var $cbRec_Checked = $cbRec.filter(':checked');
                    var n_$cbRec = $cbRec.length;
                    var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
                    $cbHead.prop('checked', isCheckedAll);
                    var sVal = $(this).attr('id').split('_')[1];
                    if ($(this).is(":checked")) {
                        var obj = {
                            nID: sVal,
                        };
                        arrCheckBox.push(obj);
                    } else {
                        arrCheckBox = Enumerable.From(arrCheckBox).Where(function (w) { return w.nID != sVal }).ToArray();
                    }
                })
                .delegate('button[name=btnSearchRec]', 'click', function () {
                    if ($(this).attr('data-sub') != "0") {
                        var sOrgID = $(this).attr('data-orgid');
                        SearchData(sOrgID);
                    }
                })
                .delegate('span[id*=spGL_]', 'click', function () {
                    var nProID = +$(this).attr('id').replace('spGL_', '');
                    var qPro = Enumerable.From(arrData).FirstOrDefault(null, '$.nProjectID == ' + nProID);
                    if (qPro != null) {
                        var lstGL = qPro.lstGL;
                        if (lstGL.length > 0) {
                            $('#tbGL tbody tr').remove();

                            $.each(lstGL, function (i, el) {
                                var td = '<td class="text-center">' + el.sGLID + '</td>';
                                td += '<td>' + el.sGLName + '</td>';
                                td += '<td class="text-right">' + el.sBudget + '</td>';
                                $('#tbGL tbody').append('<tr>' + td + '</tr>');
                            });

                            $('#divModalGL').modal();
                        }
                    }
                });

            $btnSearch.click(function () { SearchData(); });

            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind(nPageSize, nPageNo);
                SetTooltip();
            });

            $btnExportProject.click(function () {
                var $cbRec = $('input[id^="cbRec_"]:checkbox');
                var $cbRec_Checked = $cbRec.filter(':checked');
                if ($cbRec_Checked.length > 0) {
                    DialogConfirmExport(function () {
                        var arrExport = $.map($cbRec_Checked, function (cb) { return +$(cb).val(); });
                        $('input[id$=hddProjectID]').val(arrExport.join());
                        $("input[id$=btnExportProject_]").click();
                    });
                }
                else DialogExportError();
            });

            $('select[id$=ddlProjectType]').change(function () {
                var sVal = GetValDropdown('ddlProjectType');
                var IsEnabled = sVal == "23";
                $('select[id$=ddlDimension]').prop('disabled', !IsEnabled);

                $('select[id$=ddlDimension] option[value=18]').remove();
                switch (sVal) {
                    case '22'://Donation
                    case '24'://Advertising & PR
                        $('select[id$=ddlDimension]').append('<option value="18">N/A</option>');
                        $('select[id$=ddlDimension]').val('18');
                        break;
                    case '25'://KVIS         
                    case '26'://VISTEC          
                        $('select[id$=ddlDimension]').val('10');
                        break;
                    case '23'://CSR Expenses
                        $('select[id$=ddlDimension]').val('');
                        break;
                    default: $('select[id$=ddlDimension]').val(''); break;
                }
            });
        }

        function SearchData(pageThis) {
            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);
            BlockUI();
            var objSearch = {
                'sProjectName': GetValTextBox('txtProjectName'),
                'sProjectType': GetValDropdown('ddlProjectType'),
                'sDimension': GetValDropdown('ddlDimension'),
                'sYear': GetValDropdown('ddlYear'),
                'sStatus': GetValDropdown('ddlStatus')
            };
            AjaxWebMethod('Search', objSearch, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    $('#divTable').show('fast');
                    arrData = data.d.lstData;
                    SortingClear($tbData);
                    $objPag = $('ul#pagData').paging(arrData.length, {
                        format: '[< nnncnnn >]',
                        onFormat: EasyPaging_OnFormat,
                        perpage: $ddlPageSize.val(),
                        onSelect: function (nPageNo) { //1,2,3,4,5,...
                            var nPageSize = $ddlPageSize.val();
                            ActiveDataBind(nPageSize, nPageNo);
                            SetTooltip();
                        },
                    });

                    if (arrData.length > 0) { $('#btnExportProject').show('fast'); } else { $('#btnExportProject').hide('fast'); }
                }
            }, function () {
                $("#pagData a[data-page=" + pageThis + "]").not(".next").click();
                UnblockUI();
                SetTooltip();
            });
        }

        function SortingData(sExpression, sDirection) {
            switch (sExpression) {
                case 'sProjectName':
                case 'nYear':
                case 'sType':
                case 'sDimension':
                case 'nBudget':
                case 'nBudgetUsed':
                case 'sBudgetStatus':
                case 'sStatus':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy('$.' + sExpression).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending('$.' + sExpression).ToArray(); })
                    break;
            }
            ActiveDataBind($ddlPageSize.val(), $objPag.opts.page);
        }

        function CreateDataRow(objData, nRowNo) {
            var sHTML = "";
            var isChecked = Enumerable.From(arrCheckBox).Where(function (w) { return w.nID == objData.nProjectID }).ToArray().length > 0 ? "checked=''" : "";
            sHTML += '<td class="text-center">' +
        '<div class="checkbox"><input type="checkbox" name="cbRec_' + objData.nProjectID + '" id="cbRec_' + objData.nProjectID + '" value="' + objData.nProjectID + '" ' + isChecked + '/>' +
        '<label for="cbRec_' + objData.nProjectID + '">' + nRowNo + '.</label></div>' + '</td>';

            sHTML += "<td>" + (objData.sProjectCode != "" ? "(" + objData.sProjectCode + ") " : "") + objData.sProjectName + "</td>";
            sHTML += "<td class='text-center'>" + objData.sType + "</td>";
            sHTML += "<td class='text-center'>" + objData.sDimension + "</td>";

            var HasBudget = objData.sBudget != "";
            sHTML += "<td class='text-" + (HasBudget ? "right" : "center") + "'>" + (HasBudget ? objData.sBudget : "-") + "</td>";

            var HasBudgetUsed = objData.sBudgetUsed != "";
            sHTML += "<td class='text-" + (HasBudgetUsed ? "right" : "center") + "'>" + (HasBudgetUsed ? "<span id='spGL_" + objData.nProjectID + "' class='cPointer'>" + objData.sBudgetUsed + '</span>' : "-") + "</td>";

            //sHTML += "<td class='text-center'>" + objData.sBudgetStatus + "</td>";
            sHTML += "<td class='text-center'>" + objData.sStatus + "</td>";

            sHTML += '<td class="text-center" valign="top">' +
                    '<button type="button" class="btn btn-sm btn-outline-success"  title="Transaction" onclick="ExportTransaction(' + objData.nProjectID + ')"><i class="fa fa-file-excel' + '"></i>&nbsp;</button>' +
                    '</td>';

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();

            var $cbRec = $('input[id^="cbRec_"]:checkbox');
            var $cbRec_Checked = $cbRec.filter(':checked');
            var n_$cbRec = $cbRec.length;
            var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
            $cbHead.prop('checked', isCheckedAll);
        }

        function ExportTransaction(nProjectID) {
            $('input[id$=hddProjectID]').val(nProjectID);
            $("input[id$=btnExportTransaction]").click();
        }
    </script>
</asp:Content>

