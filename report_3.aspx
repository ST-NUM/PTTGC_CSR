<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="report_3.aspx.cs" Inherits="report_3" EnableEventValidation="false" %>

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
                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">รหัส/ชื่อ โครงการ</label>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" placeholder="รหัส/ชื่อ โครงการ"></asp:TextBox>
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
                        <label class="col-lg-3 col-form-label">Cost Center</label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">- Cost Center -</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">Order</label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlOrder" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">- Order -</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">GL</label>
                        <div class="col-lg-6">
                            <asp:DropDownList ID="ddlGL" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">- GL -</asp:ListItem>
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
                            <button id="btnExport" type="button" class="btn btn-success" style="display: none;">
                                <i class="fa fa-file-excel"></i><span>&nbsp; พิมพ์เอกสาร</span>
                            </button>
                            <asp:Button ID="btnExport_" runat="server" OnClick="btnExport_Click" CssClass="hide" />
                        </div>
                    </div>

                </div>
            </div>

            <div id="divTable" class="table-responsive" style="max-height: 400px; display: none;">
                <table id="tbData" class="table table-bordered table-hovertable-sm" style="width: 2550px">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 70px" class="text-center">ที่</th>
                            <th style="width: 100px" class="text-center" data-sort="nPeriod">Period</th>
                            <th style="width: 120px" class="text-center" data-sort="sPostingDate">Posting Date</th>
                            <th style="width: 250px" class="text-center" data-sort="sProjectName">ชื่อโครงการ</th>
                            <th style="width: 250px" class="text-center" data-sort="sDescription">Description</th>
                            <th style="width: 130px" class="text-center" data-sort="nValInRepCur">Amount</th>
                            <th style="width: 250px" class="text-center" data-sort="sNameOffsetting">Pay To</th>
                            <th style="width: 170px" class="text-center" data-sort="sCostCenter">Cost Center</th>
                            <th style="width: 120px" class="text-center" data-sort="sIOID">Order</th>
                            <th style="width: 125px" class="text-center" data-sort="sGLID">Cost Element</th>
                            <th style="width: 200px" class="text-center" data-sort="sGLName">Cost Element Name</th>
                            <th style="width: 150px" class="text-center" data-sort="sObjective">Objective</th>
                            <th style="width: 95px" class="text-center" data-sort="sArea">Area</th>
                            <th style="width: 150px" class="text-center" data-sort="sInternal">Internal</th>
                            <th style="width: 150px" class="text-center" data-sort="sExternal">External</th>
                            <th style="width: 220px" class="text-center" data-sort="sPA">Philanthropic Activities</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">ไม่พบข้อมูล</div>
            </div>

            <div id="divPaging" class="form-row align-items-center pt-3" style="display: none;">
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

    <asp:HiddenField ID="hddProjectName" runat="server" />
    <asp:HiddenField ID="hddYear" runat="server" />
    <asp:HiddenField ID="hddCCID" runat="server" />
    <asp:HiddenField ID="hddIOID" runat="server" />
    <asp:HiddenField ID="hddGLID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $cbHead = $('input[id$=cbHead]');
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var $btnSearch = $('button#btnSearch');
        var $btnAdd = $('a#btnAdd');
        var $btnExport = $('button#btnExport');
        var arrData = [];
        var arrCheckBox = [];
        var lstCC = [];
        var lstIO = [];
        var lstGL = [];

        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');
        function SortingEvent() { }

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    GetData();
                    //SearchData();
                    SortingBind($tbData, SortingData);
                }
            }
        });

        function SetControl() {
            $tbData
                //.delegate('input[id^="cbRec_"]:checkbox', 'change', function () {
                //    var $cbRec = $('input[id^="cbRec_"]:checkbox');
                //    var $cbRec_Checked = $cbRec.filter(':checked');
                //    var n_$cbRec = $cbRec.length;
                //    var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
                //    $cbHead.prop('checked', isCheckedAll);
                //    var sVal = $(this).attr('id').split('_')[1];
                //    if ($(this).is(":checked")) {
                //        var obj = {
                //            nID: sVal,
                //        };
                //        arrCheckBox.push(obj);
                //    } else {
                //        arrCheckBox = Enumerable.From(arrCheckBox).Where(function (w) { return w.nID != sVal }).ToArray();
                //    }
                //})
                .delegate('button[name=btnSearchRec]', 'click', function () {
                    if ($(this).attr('data-sub') != "0") {
                        var sOrgID = $(this).attr('data-orgid');
                        SearchData(sOrgID);
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

            $btnExport.click(function () {
                $("input[id$=btnExport_]").click();
            });

            $('select[id$=ddlYear]').change(function () {
                SetDDL();
            });
        }

        function GetData() {
            AjaxWebMethod('GetData', { 'sYear': GetValDropdown('ddlYear') }, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    lstCC = data.d.lstCC;
                    lstIO = data.d.lstIO;
                    lstGL = data.d.lstGL;
                    SetDDL();
                }
            });
        }

        function SetDDL() {
            var nYear = +GetValDropdown('ddlYear');

            var sCC = GetValDropdown('ddlCostCenter');
            $('select[id$=ddlCostCenter] option:not(:first)').remove();
            var lstCC_ = Enumerable.From(lstCC).Where('$.nYear == ' + nYear).ToArray();
            $.each(lstCC_, function (i, el) {
                $('select[id$=ddlCostCenter]').append('<option value="' + el.sCCID + '">' + el.sName + '</option>');
                if (el.sCCID == sCC) $('select[id$=ddlCostCenter]').val(sCC);
            });

            var sIO = GetValDropdown('ddlOrder');
            $('select[id$=ddlOrder] option:not(:first)').remove();
            var lstIO_ = Enumerable.From(lstIO).Where('$.nYear == ' + nYear).ToArray();
            $.each(lstIO_, function (i, el) {
                $('select[id$=ddlOrder]').append('<option value="' + el.sIOID + '">' + el.sName + '</option>');
                if (el.sIOID == sIO) $('select[id$=ddlOrder]').val(sIO);
            });

            var sGL = GetValDropdown('ddlGL');
            $('select[id$=ddlGL] option:not(:first)').remove();
            var lstGL_ = Enumerable.From(lstGL).Where('$.nYear == ' + nYear).GroupBy(function (item) { return item.sGLID, item.sName }).Select(function (item) { return { 'sGLID': item.source[0].sGLID, 'sName': item.source[0].sName } }).ToArray();
            $.each(lstGL_, function (i, el) {
                $('select[id$=ddlGL]').append('<option value="' + el.sGLID + '">' + el.sName + '</option>');
                if (el.sGLID == sGL) $('select[id$=ddlGL]').val(sGL);
            });
        }

        function SearchData(pageThis) {
            $('input[id$=hddProjectName]').val(GetValTextBox('txtProjectName'));
            $('input[id$=hddYear]').val(GetValDropdown('ddlYear'));
            $('input[id$=hddCCID]').val(GetValDropdown('ddlCostCenter'));
            $('input[id$=hddIOID]').val(GetValDropdown('ddlOrder'));
            $('input[id$=hddGLID]').val(GetValDropdown('ddlGL'));

            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);
            BlockUI();
            var objSearch = {
                'sProjectName': GetValTextBox('txtProjectName'),
                'sCCID': GetValDropdown('ddlCostCenter'),
                'sIOID': GetValDropdown('ddlOrder'),
                'sGLID': GetValDropdown('ddlGL'),
                'sYear': GetValDropdown('ddlYear')
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

                    if (arrData.length > 0) { $('#btnExport').show('fast'); } else { $('#btnExport').hide('fast'); }
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
                case 'sCostCenter':
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
            sHTML += "<td>" + objData.sDescription + "</td>";
            sHTML += "<td class='text-right'>" + objData.nValInRepCur.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,') + "</td>";
            sHTML += "<td>" + objData.sNameOffsetting + "</td>";
            sHTML += "<td class='text-center'>" + objData.sCostCenter + "</td>";           
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

            //var $cbRec = $('input[id^="cbRec_"]:checkbox');
            //var $cbRec_Checked = $cbRec.filter(':checked');
            //var n_$cbRec = $cbRec.length;
            //var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
            //$cbHead.prop('checked', isCheckedAll);
        }
    </script>
</asp:Content>

