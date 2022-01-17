<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="budget_transaction.aspx.cs" Inherits="budget_transaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        #tbData {
            font-size: 14px;
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
                    <div class="form-row justify-content-end">
                        <div class="col-auto">
                            <div class="form-group">
                                <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" placeholder="รหัส/ชื่อ โครงการ"></asp:TextBox>
                            </div>
                        </div>

                         <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-auto">
                            <div class="form-group">
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                                </asp:DropDownList>
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

            <div class="table-responsive" style="max-height: 400px;">
                <table id="tbData" class="table table-bordered table-hovertable-sm" style="width: 2450px">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 70px" class="text-center">ที่</th>
                             <th style="width: 100px" class="text-center" data-sort="nPeriod">Period</th>
                             <th style="width: 120px" class="text-center" data-sort="sPostingDate">Posting Date</th>
                            <th style="width: 250px" class="text-center" data-sort="sProjectName">ชื่อโครงการ</th>
                            <th style="width: 90px" class="text-center" data-sort="sProjectType">ประเภท</th>
                            <th style="width: 250px" class="text-center" data-sort="sDescription">Description</th>
                            <th style="width: 130px" class="text-center" data-sort="nValInRepCur">Amount</th>
                            <th style="width: 250px" class="text-center" data-sort="sNameOffsetting">Pay To</th>
                            <th style="width: 120px" class="text-center" data-sort="sIOID">Order</th>          
                            <th style="width: 125px" class="text-center" data-sort="sGLID">Cost Element</th>
                            <th style="width: 200px" class="text-center" data-sort="sGLName">Cost Element Name</th>
                            <th style="width: 150px" class="text-center" data-sort="sObjective">Objective</th>
                            <th style="width: 95px" class="text-center" data-sort="sArea">Area</th>
                            <th style="width: 150px" class="text-center" data-sort="sInternal">Internal</th>
                            <th style="width: 150px" class="text-center" data-sort="sExternal">External</th>
                            <th style="width: 200px" class="text-center" data-sort="sPA">Philanthropic Activities</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">ไม่พบข้อมูล</div>
            </div>

            <div id="divPaging" class="form-row align-items-center pt-3">
                <div class="col-lg-2 mb-3">
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
    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="Scripts/tableHeadFixer.js"></script>

    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');

        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var $btnSearch = $('button#btnSearch');
        var arrData = [];

        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');
        function SortingEvent() { }

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SearchData();
                    SortingBind($tbData, SortingData);
                }

                if ($Permission != "A") {
                    $('#divForm input, #divForm select, #divForm textarea').prop('disabled', true);
                }
            }
        });

        function SetControl() {
            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind(nPageSize, nPageNo);
                SetTooltip();
            });

            $btnSearch.click(function () { SearchData(); });
        }

        function SearchData(pageThis) {
            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);

            BlockUI();
            AjaxWebMethod('Search', { 'sProjectName': GetValTextBox('txtProjectName'), 'sProjectType': GetValDropdown('ddlProjectType'), 'sYear': GetValDropdown('ddlYear') }, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    lstProject = data.d.lstProject;
                    lstOrder = data.d.lstOrder;
                    lstGL = data.d.lstGL;

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
                case 'nPeriod':
                case 'sDescription':
                case 'nValInRepCur':
                case 'sNameOffsetting':
                case 'sGLName':
                case 'sObjective':
                case 'sArea':
                case 'sInternal':
                case 'sExternal':
                case 'sPA':
                case 'sProjectType':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy('$.' + sExpression).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending('$.' + sExpression).ToArray(); })
                    break;
                case 'sPostingDate':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); })
                    break;
            }
            ActiveDataBind($ddlPageSize.val(), $objPag.opts.page);
        }

        function CreateDataRow(objData, nRowNo) {
            var sHTML = "";

            sHTML += "<td class='text-center'>" + nRowNo + ".</td>";
            sHTML += "<td class='text-center'>" + objData.nPeriod + "</td>";
            sHTML += "<td class='text-center'>" + objData.sPostingDate + "</td>";
            sHTML += "<td>" + objData.sProjectName + "</td>";
            sHTML += "<td class='text-center'>" + objData.sProjectType + "</td>";
            sHTML += "<td>" + objData.sDescription + "</td>";
            sHTML += "<td class='text-right'>" + objData.nValInRepCur.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') + "</td>";
            sHTML += "<td>" + objData.sNameOffsetting + "</td>";
            sHTML += "<td class='text-center'>" + objData.sIOID + "</td>";    
            sHTML += "<td class='text-center'>" + objData.sGLID + "</td>";
            sHTML += "<td>" + objData.sGLName + "</td>";
            sHTML += "<td class='text-center'>" + objData.sObjective + "</td>";
            sHTML += "<td class='text-center'>" + objData.sArea + "</td>";
            sHTML += "<td>" + objData.sInternal + "</td>";
            sHTML += "<td>" + objData.sExternal + "</td>";
            sHTML += "<td class='text-center'>" + objData.sPA + "</td>";

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();
        }
    </script>
</asp:Content>

