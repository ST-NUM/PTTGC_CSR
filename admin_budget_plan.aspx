<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_budget_plan.aspx.cs" Inherits="admin_budget_plan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .bg-lv1 {
            background-color: #7cbdff;
        }

        .bg-lv2 {
            background-color: #c9e2fd;
        }

        .bg-lv3 {
            /*background-color: #a8ccf1;*/
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
                <div class="col-lg-auto col-12">
                    <div class="form-group ">
                        <button id="btnSync" type="button" class="btn btn-info tooltipstered">
                            <i class="fa fa-upload"></i><span>&nbsp; ซิงค์งบประมาณจาก SAP</span>
                        </button>
                    </div>
                </div>

                <div id="divSearch" class="col">
                    <div class="form-row justify-content-end">
                        <div class="col-auto">
                            <div class="form-group">
                                <asp:TextBox ID="txtCostCenter" runat="server" CssClass="form-control" placeholder="รหัส / ชื่อ Cost Center"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" placeholder="รหัส / ชื่อ Order"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:TextBox ID="txtGL" runat="server" CssClass="form-control" placeholder="รหัส / ชื่อ GL"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <button id="btnSearch" type="button" class="btn btn-inverse">
                                    <i class="fa fa-search"></i><span>&nbsp; ค้นหา</span>
                                </button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="table-responsive">
                <table id="tbData" class="table table-bordered table-responsive-sm">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th class="text-center">Cost Center</th>
                            <%--<th class="text-center">Cost Center</th>
                            <th style="width: 23%" class="text-center">Order</th>
                            <th style="width: 23%" class="text-center">GL</th>--%>
                            <th style="width: 20%" class="text-center">งบประมาณ</th>
                            <th style="width: 20%" class="text-center">งบประมาณที่ใช้ไป</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">ไม่พบข้อมูล</div>
            </div>

        </div>
    </div>

    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <!-- Modal Total Used -->
    <div id="divModalGL" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="divModalGL" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-info">
                    <h5 id="hDTitel" class="modal-title">Budget by GL</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="table-responsive">
                        <table class="table table-bordered table-hover table-sm">
                            <thead>
                                <tr class="valign-middle pad-primary">
                                    <th style="width: 20%" class="text-center">GL Code</th>
                                    <th class="text-center">GL Name</th>
                                    <th style="width: 20%" class="text-center">Budget</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="text-center">62120200</td>
                                    <td class="text-left">Donation­Education X2 (Royal Decree 420)</td>
                                    <td class="text-right">10,00,000.00</td>
                                </tr>
                                <tr>
                                    <td class="text-center">62120300</td>
                                    <td class="text-left">Donation­Learning X2</td>
                                    <td class="text-right">8,000,000.00</td>
                                </tr>
                                <tr>
                                    <td class="text-center">62120100</td>
                                    <td class="text-left">Advertising and Public Relations Expense</td>
                                    <td class="text-right">7,000,000.00</td>
                                </tr>
                                <tr>
                                    <td class="text-center">62120400</td>
                                    <td class="text-left">Donation­Public Charities</td>
                                    <td class="text-right">5,000,000.00</td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--Bootstrap treegrid --%>
    <script src="Scripts/bootstrap-treegrid/js/jquery.treegrid.js"></script>
    <script src="Scripts/bootstrap-treegrid/js/jquery.treegrid.bootstrap3.js"></script>
    <link href="Scripts/bootstrap-treegrid/css/jquery.treegrid.css" rel="stylesheet" />

    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $divNoData = $('div#divNoData');
        var $btnSearch = $('button#btnSearch');
        var arrData = [];

        $(function () {
            if (!isTimeOut) {
                //$('#aGL').click(function () { $('#divModalGL').modal(); });
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SearchData();
                }

                if ($Permission != "A") {
                    $('#btnSync').remove();
                }
            }
        });

        function SetControl() {
            $btnSearch.click(function () { SearchData(); });

            $('#btnSync').click(function () {
                DialogConfirmSyncBudget(function () {
                    BlockUI();
                    AjaxWebMethod('SyncBudget', {}, function (data) {
                        if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                            DialogSyncSucess();
                            SearchData();
                        }
                    }, function () { }, function (xhr, status, err) {
                        var objResponse = eval('(' + xhr.responseText + ')');

                        var sNewLine = '<br />';
                        var sErrorMessage =
                            '<div class="text-left">' +
                            '<b>ExceptionType :</b>&nbsp;' + objResponse.ExceptionType + sNewLine +
                            '<b>Message :</b>&nbsp;' + objResponse.Message + sNewLine +
                            '<b>StackTrace :</b>' + sNewLine + objResponse.StackTrace.replace(/(?:\r\n|\r|\n)/g, sNewLine) +
                            '</div>';

                        DialogWarning(sErrorMessage)
                    }, true);
                });
            });
        }

        function SearchData() {
            BlockUI();
            var objSearch = { 'sCostCenter': GetValTextBox('txtCostCenter'), 'sOrder': GetValTextBox('txtOrder'), 'sGL': GetValTextBox('txtGL'), 'nYear': +GetValDropdown('ddlYear') };
            AjaxWebMethod('Search', objSearch, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    arrData = data.d.lstData;

                    var $tbData = $('table#tbData tbody');
                    $tbData.html('');

                    if (arrData.length > 0) {
                        $.each(arrData, (function (i, el) {
                            var IsLV1 = el.nLevel == 1;
                            var IsLV2 = el.nLevel == 2;
                            var IsLV3 = el.nLevel == 3;

                            var td = "<td>&emsp;" + el.sName + "</td>";
                            //var td = "<td>&emsp;" + (IsLV1 ? el.sName : '') + "</td>";
                            //td += "<td>" + (IsLV2 ? el.sName : '') + "</td>";
                            //td += "<td>" + (IsLV3 ? el.sName : '') + "</td>";
                            td += "<td class='text-right'>" + el.nBudget.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') + "</td>";
                            td += "<td class='text-right'>" + el.nBudgetUsed.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') + "</td>";

                            var sBG = (IsLV1 ? "bg-lv1" : (IsLV2 ? "bg-lv2" : "bg-lv3"));
                            $tbData.append('<tr class="' + sBG + ' treegrid-' + el.nID + ' ' + (!IsLV1 ? 'treegrid-parent-' + el.nParentID : '') + '">' + td + '</tr>');
                        }));

                        $('table#tbData').treegrid({
                            expanderExpandedClass: 'fa fa-minus-circle',
                            expanderCollapsedClass: 'fa fa-plus-circle'
                        });
                        $('table#tbData').treegrid('collapseAll');

                        $('#divNoData').hide();
                    } else {
                        $('#divNoData').show();
                    }
                }
            }, function () {
                UnblockUI();
                SetTooltip();
            });
        }
    </script>
</asp:Content>

