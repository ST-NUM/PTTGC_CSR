<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_owner.aspx.cs" Inherits="admin_owner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <div id="divAdd" class="row">
                <div class="col">
                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">ชื่อโครงการ</label>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" MaxLength="250" placeholder="ชื่อโครงการ"></asp:TextBox>
                            <asp:TextBox ID="txtProjectID" runat="server" CssClass="form-control d-none"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">ผู้รับผิดชอบโครงการ(ปัจจุบัน)</label>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txtEmpOld" runat="server" CssClass="form-control" placeholder="ผู้รับผิดชอบโครงการ(ปัจจุบัน)" disabled="true"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3 col-form-label">ผู้รับผิดชอบโครงการ(ใหม่)</label>
                        <div class="col-lg-6">
                            <asp:TextBox runat="server" CssClass="form-control" type="text" ID="txtEmpName" placeholder="รหัส / ชื่อ-สกุล" />
                            <asp:TextBox ID="txtEmpID" runat="server" CssClass="form-control d-none"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-lg-1"></div>
                        <label class="col-lg-3"></label>
                        <div class="col-lg-6">
                            <button id="btnSave" type="button" class="btn btn-info" style=""><i class="fa fa-save"></i>&nbsp; บันทึก</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="table-responsive">
                <table id="tbData" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 8%" class="text-center">ที่</th>
                            <th class="text-center" data-sort="sProjectName">ชื่อโครงการ</th>
                            <th class="text-center" style="width: 20%" data-sort="sOwner">ผู้รับผิดชอบโครงการ</th>
                            <th class="text-center" style="width: 20%" data-sort="sUpdate">ผู้ปรับปรุง</th>
                            <th style="width: 20%" class="text-center" data-sort="sUpdateDate">ปรับปรุงล่าสุด</th>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var arrData = [];
        var arrCheckBox = [];

        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');
        function SortingEvent() { }

        var nLength = 3;
        var lstProjectName = [];

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SearchData();
                    SortingBind($tbData, SortingData);

                    if ($Permission == "A") {
                        SetValidate();
                        GetData();
                        SetAutoComplete_Project();
                        SetAutoComplete_Owner();
                    } else {
                        $('#divAdd').remove();
                    }

                }
            }
        });

        function GetData() {
            BlockUI();
            var sProID = GetValTextBox('hddProjectID');
            AjaxWebMethod('GetData', { 'sProjectID': GetValTextBox('hddProjectID'), 'nYear': +GetValTextBox('txtYear') }, function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else {
                    UnblockUI();
                    lstProjectName = data.d.lstProjectName;
                }
            });
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("txtProjectName", objControl.txtbox)] = addValidate_notEmpty("ระบุ ชื่อโครงการ");
            objValidate[GetElementName("txtEmpName", objControl.txtbox)] = addValidate_notEmpty("ระบุ ผู้รับผิดชอบโครงการ");
            BindValidate("divAdd", objValidate);
        }

        function SetControl() {
            $tbData
                .delegate('button[name=btnSearchRec]', 'click', function () {
                    if ($(this).attr('data-sub') != "0") {
                        var sOrgID = $(this).attr('data-orgid');
                        SearchData(sOrgID);
                    }
                });

            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind(nPageSize, nPageNo);
                SetTooltip();
            });

            $('#btnSave').click(function () {
                if (CheckValidate('divAdd')) {
                    DialogConfirmSubmit(function () {
                        AjaxWebMethod("SaveData", { 'nProjectID': +GetValTextBox('txtProjectID'), 'nOwnerID': +GetValTextBox('txtEmpID') }, function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupSessionTimeOut();
                            } else if (response.d.Status == SysProcess.SaveFail) {
                                UnblockUI();
                                DialogSaveFail(response.d.Msg);
                            } else {
                                UnblockUI()
                                $('#divAdd input').val('');
                                SetNotValidateTextbox('divAdd', 'txtProjectName');
                                SetNotValidateTextbox('divAdd', 'txtEmpName');

                                SearchData();
                            }
                        });
                    });
                }
                return false;
            });
        }

        function SearchData(pageThis) {
            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);

            BlockUI();
            AjaxWebMethod('Search', {}, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
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
                case 'sOwner':
                case 'sUpdate':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy('$.' + sExpression).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending('$.' + sExpression).ToArray(); })
                    break;
                case 'sUpdateDate':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending(function (o) { return DateForSort(o[sExpression], '/'); }).ToArray(); })
                    break;
            }
            ActiveDataBind($ddlPageSize.val(), $objPag.opts.page);
        }

        function CreateDataRow(objData, nRowNo) {
            var sHTML = "";
            sHTML += '<td class="text-center">' + nRowNo + '.' + '</td>';
            sHTML += "<td>" + objData.sProjectName + "</td>";
            sHTML += "<td>" + objData.sOwner + "</td>";
            sHTML += "<td>" + objData.sUpdate + "</td>";
            sHTML += "<td class='text-center'>" + objData.sUpdateDate + "</td>";

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();
        }

        // #region Auto Complete Project Name
        var IsSelectedtxtProjectName = false;
        function SetAutoComplete_Project() {
            $("input[id$=txtProjectName]")
            .on("change", function () {
                if (!IsSelectedtxtProjectName || !IsBrowserFirefox()) {
                    $("input[id$=txtProjectName]").val("");
                    $("input[id$=txtProjectID]").val("");
                    $("input[id$=txtEmpOld]").val("");
                    ReValidateFieldControl("divAdd", GetElementName('txtProjectName', objControl.txtbox));

                    $("input[id$=txtEmpName]").val("");
                    $("input[id$=txtEmpID]").val("");
                    ReValidateFieldControl("divAdd", GetElementName('txtEmpName', objControl.txtbox));
                }
            }).focus(function () {
                IsSelectedtxtProjectName = false;
            })
           .autocomplete({
               source: function (request, response) {
                   var sSearch = request.term.replace(/\s/g, "").toLowerCase();
                   if (sSearch != "" && sSearch.length >= nLength) {
                       var lstProjectName_ = [];
                       $.each(lstProjectName, function (i, el) {
                           if (el.sProjectName.replace(/\s/g, "").toLowerCase().indexOf(sSearch) > -1) {
                               lstProjectName_.push(el);
                           }
                       });

                       response($.map(lstProjectName_, function (item) {
                           return {
                               value: item.sProjectName,
                               label: item.sProjectName,
                               nProjectID: item.nProjectID,
                               sOwner: item.sOwner
                           }
                       }));
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
                   IsSelectedtxtProjectName = true;
                   $("input[id$=txtProjectID]").val(ui.item.nProjectID);
                   $("input[id$=txtEmpOld]").val(ui.item.sOwner);
                   if (IsBrowserFirefox()) {
                       $("input[id$=txtProjectName]").blur();;
                   }
               }
           });
        }
        // #endregion

        // #region Auto Complete ผู้รับผิดชอบโครงการ
        var IsSelectedtxtEmpName = false;
        function SetAutoComplete_Owner() {
            $("input[id$=txtEmpName]")
               .on("change", function () {
                   if (!IsSelectedtxtEmpName || !IsBrowserFirefox()) {
                       $("input[id$=txtEmpName]").val("");
                       $("input[id$=txtEmpID]").val("");
                       ReValidateFieldControl("divAdd", GetElementName('txtEmpName', objControl.txtbox));
                   }
               }).focus(function () {
                   IsSelectedtxtEmpName = false;
               })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtEmpName = false;
                   if (request.term.replace(/\s/g, "") != "" && request.term.replace(/\s/g, "").length >= nLength) {
                       var sProjectID = GetValTextBox('txtProjectID');
                       if (sProjectID != "") {
                           AjaxWebMethod(UrlSearchEmp(), { 'sSearch': request.term, 'nProjectID': +sProjectID }, function (data) {
                               if (data.d.Status == SysProcess.SessionExpired) {
                                   PopupSessionTimeOut();
                               } else {
                                   UnblockUI();
                                   response($.map(data.d.lstData, function (item) {
                                       return {
                                           value: item.sName,
                                           label: item.sName,
                                           nUserID: item.nUserID
                                       }
                                   }));
                               }
                           });
                       } else {
                           DialogWarning("กรุณาระบุ ชื่อโครงการ");
                       }
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
                   IsSelectedtxtEmpName = true;
                   $("input[id$=txtEmpID]").val(ui.item.nUserID);
                   if (IsBrowserFirefox()) {
                       $("input[id$=txtEmpName]").blur();;
                   }
               }
           });
        }

        function UrlSearchEmp() {
            BlockUI();
            return 'GetEmp';
        }
        // #endregion
    </script>
</asp:Content>

