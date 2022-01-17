<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_budget_upload_edit.aspx.cs" Inherits="admin_budget_upload_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        #tbData {
            font-size: 14px;
        }

        i[data-fv-icon-for*=txtPostingDate] {
            padding-right: 65px;
        }

        i[data-fv-icon-for*=txtValInRepCur] {
            padding-right: 75px;
        }

        .bg-grey {
            background-color: #b3b1b1 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <div id="divEdit" style="display: none;">

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">ชื่อโครงการ</label>
                    <div class="col-lg-9">
                        <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" MaxLength="250" disabled="true"></asp:TextBox>
                        <input id="txtProjectID" type="text" class="hide" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Pay To</label>
                    <div class="col-lg-9">
                        <asp:TextBox ID="txtNameOffsetting" runat="server" CssClass="form-control" disabled="true" MaxLength="400"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-6">
                        <div class="form-group row">
                            <label class="col-lg-6 col-form-label">Period</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="form-control" disabled="true">
                                    <asp:ListItem Value="">- ระบุ Period -</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="form-group row">
                            <label class="col-lg-6 col-form-label">Posting Date</label>
                            <div class="col-lg-6">
                                <div class="input-group">
                                    <asp:TextBox ID="txtPostingDate" runat="server" CssClass="form-control" placeholder="--/--/----" MaxLength="10" disabled="true"></asp:TextBox>
                                    <div class="input-group-append">
                                        <label class="input-group-text"><i class="fa fa-calendar-alt"></i></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-6">
                        <div class="form-group row">
                            <label class="col-lg-6 col-form-label">Order</label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlOrder" runat="server" CssClass="form-control" disabled="true">
                                    <asp:ListItem Value="">- Order -</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="form-group row">
                            <label class="col-lg-6 col-form-label">Amount</label>
                            <div class="col-lg-6">
                                <div class="input-group">
                                    <asp:TextBox ID="txtValInRepCur" runat="server" CssClass="form-control pr-4" disabled="true"></asp:TextBox>
                                    <div class="input-group-append">
                                        <label class="input-group-text">บาท</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group row">
                            <label class="col-lg-3 col-form-label">Cost Element</label>
                            <div class="col-lg-9">
                                <asp:DropDownList ID="ddlGL" runat="server" CssClass="form-control" disabled="true">
                                    <asp:ListItem Value="">- Cost Element -</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <%-- <div class="col-6">
                        <div class="form-group row">
                            <label class="col-lg-6 col-form-label">Cost Element Name</label>
                            <div class="col-lg-6">
                                <asp:TextBox ID="txtGLName" runat="server" CssClass="form-control" disabled></asp:TextBox>
                            </div>
                        </div>
                    </div>--%>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Description <span class="text-red">*</span></label>
                    <div class="col-lg-9">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" MaxLength="400"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">Internal <span class="text-red">*</span></label>
                    <div class="col-lg-9">
                        <asp:TextBox ID="txtInternal" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">External <span class="text-red">*</span></label>
                    <div class="col-lg-9">
                        <asp:TextBox ID="txtExternal" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-6">
                        <div class="form-group row">
                            <label class="col-lg-6 col-form-label">Objective <span class="text-red">*</span></label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlObjective" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="form-group row">
                            <label class="col-lg-6 col-form-label">Area <span class="text-red">*</span></label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-6">
                        <div class="form-group row">
                            <label class="col-lg-6 col-form-label">Philanthropic Activities <span class="text-red">*</span></label>
                            <div class="col-lg-6">
                                <asp:DropDownList ID="ddlPA" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <div class="col-lg-3"></div>
                    <div class="col-lg-auto">
                        <button id="btnSave" type="button" class="btn btn-info tooltipstered"><i class="fa fa-save"></i>&nbsp; บันทึก</button>
                        <button id="btnCancelData" type="button" class="btn btn-secondary tooltipstered"><i class="fa fa-times"></i>&nbsp; ยกเลิก</button>
                    </div>
                </div>

            </div>

            <div class="table-responsive" style="max-height: 400px;">
                <%----%>
                <table id="tbData" class="table table-bordered table-hovertable-sm" style="width: 2500px">
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
                            <th style="width: 50px"></th>
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
        <div class="card-footer">
            <div class="col-12 text-center">
                <button id="btnBack" type="button" class="btn btn-secondary"><i class="fa fa-arrow-left"></i>&nbsp; กลับ</button>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hddPermission" runat="server" />
    <asp:HiddenField ID="hddnID" runat="server" />
    <input id="txtItemID" type="text" class="hide" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="Scripts/tableHeadFixer.js"></script>

    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnBack = $('button#btnBack');

        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var $btnSearch = $('button#btnSearch');
        var arrData = [];

        var lstProject = [];
        var lstOrder = [];
        var lstGL = [];
        var nYear = 0;

        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');
        function SortingEvent() { }

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SetValidate();
                    SearchData();
                    SortingBind($tbData, SortingData);
                    SetAutoComplete_Project();
                    //$('#tbData').tableHeadFixer({ head: true, left: 2 })
                }

                if ($Permission != "A") {
                    $('#divEdit input, #divEdit select, #divEdit textarea').prop('disabled', true);
                    $('#btnSave').remove();
                }
            }
        });

        function SetControl() {
            SetDatePickerValidate('divEdit', $('input[id$=txtPostingDate]'));

            $('input[id$=txtPostingDate]').keydown(function () {
                var sDate = GetValTextBox('txtPostingDate');
                var arrDate = sDate.split('/');
                if (arrDate.length == 2) {
                    var nYear_ = +arrDate[2];
                    if (nYear != nYear_) {
                        SetIO("", "");
                    }
                } else {
                    $('select[id$=ddlOrder] option:not(:first)').remove();
                    $('select[id$=ddlGL] option:not(:first)').remove();
                    SetNotValidateSelect('divEdit', 'ddlOrder');
                    SetNotValidateSelect('divEdit', 'ddlGL');
                }
            });

            $('select[id$=ddlOrder]').change(function () {
                SetIO(GetValDropdown('ddlOrder'), "");
            });

            InputMaskDecimalMinMax_Align($('input[id$=txtValInRepCur]'), 10, 0, false, false, 0, 9999999999, true);
            $('input[id$=txtValInRepCur]').on('keydown', function () {
                ReValidateFieldControl('divEdit', $('input[id$=txtValInRepCur]').attr('name'));
            });

            $('#btnCancelData').click(function () {
                $('#divEdit').hide('fast');
                //$('#tbData tr').removeClass('bg-grey');
                ClearData();
                return false;
            });

            $('#btnSave').click(function () {
                SaveData();
                return false;
            });

            $btnBack.click(function () {
                window.Redirect('admin_budget_upload.aspx');
            });

            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน
                ActiveDataBind(nPageSize, nPageNo);
                SetTooltip();
            });
        }

        function SearchData(pageThis) {
            var chkPageThis = pageThis != undefined ? pageThis : "1";
            pageThis = IsNullOrEmptyString(chkPageThis);

            BlockUI();
            AjaxWebMethod('Search', { 'nID': +GetValTextBox('hddnID') }, function (data) {
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

            sHTML += '<td class="text-center" valign="top">' +
                    '<a class="btn btn-sm btn-outline-info" href="#"  onclick="GetData(' + objData.nItem + ')" title="' + ($Permission == "A" ? 'แก้ไข' : 'ดูรายละเอียด') + '">' +
                    '<i class="fa fa-' + ($Permission == "A" ? "edit" : "eye") + '"></i>&nbsp;' +
                    '</a>' +
                    '</td>';

            var IsSelectID = +GetValTextBox('txtItemID') == objData.nItem;
            var tr = "<tr id='tr_" + objData.nItem + "' class='" + (IsSelectID != "" ? "bg-grey" : "") + "'>" + sHTML + "</tr>";
            return tr;
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("txtProjectName", objControl.txtbox)] = addValidate_notEmpty("ระบุ ชื่อโครงการ");
            objValidate[GetElementName("txtDescription", objControl.txtbox)] = addValidate_notEmpty("ระบุ Description");
            objValidate[GetElementName("txtNameOffsetting", objControl.txtbox)] = addValidate_notEmpty("ระบุ Pay To");
            objValidate[GetElementName("txtInternal", objControl.txtbox)] = addValidate_notEmpty("ระบุ Internal");
            objValidate[GetElementName("txtExternal", objControl.txtbox)] = addValidate_notEmpty("ระบุ External");
            objValidate[GetElementName("ddlPeriod", objControl.dropdown)] = addValidate_notEmpty("ระบุ Period");
            objValidate[GetElementName("txtPostingDate", objControl.txtbox)] = addValidate_notEmpty("ระบุ Posting Date");
            objValidate[GetElementName("ddlOrder", objControl.dropdown)] = addValidate_notEmpty("ระบุ Order");
            objValidate[GetElementName("txtValInRepCur", objControl.txtbox)] = addValidate_notEmpty("ระบุ Amount");
            objValidate[GetElementName("ddlGL", objControl.dropdown)] = addValidate_notEmpty("ระบุ Cost Element");
            objValidate[GetElementName("ddlObjective", objControl.dropdown)] = addValidate_notEmpty("ระบุ Objective");
            objValidate[GetElementName("ddlArea", objControl.dropdown)] = addValidate_notEmpty("ระบุ Area");
            //objValidate[GetElementName("ddlPortion", objControl.dropdown)] = addValidate_notEmpty("ระบุ Portion");
            objValidate[GetElementName("ddlPA", objControl.dropdown)] = addValidate_notEmpty("ระบุ Philanthropic Activities");
            BindValidate("divEdit", objValidate);
        }

        function GetData(nID) {
            ClearData();
            var qData = Enumerable.From(arrData).FirstOrDefault(null, '$.nItem == ' + nID);
            if (qData != null) {
                $('#divEdit').show('fast');
                ScrollTopToElements('divEdit')
                $('#tbData tr').removeClass('bg-grey');
                $('tr[id$=tr_' + nID + ']').addClass('bg-grey');

                $('#txtItemID').val(nID);
                $('input[id$=txtProjectName]').val(qData.sProjectName);
                $('input[id$=txtProjectID]').val(qData.sProjectID);
                $('select[id$=ddlPeriod]').val(qData.nPeriod);

                nYear = +qData.sPostingDate.split('/')[2];
                SetIO(qData.sIOID, qData.sGLID);

                $('input[id$=txtPostingDate]').val(qData.sPostingDate);
                $('input[id$=txtDescription]').val(qData.sDescription);
                $('input[id$=txtValInRepCur]').val(qData.nValInRepCur);
                $('input[id$=txtNameOffsetting]').val(qData.sNameOffsetting);

                if (qData.nProjectType == 22 || qData.nProjectType == 24) {
                    //Donation,Advertising & PR
                    $('select[id$=ddlObjective]').val(qData.nObjective).prop('disabled', false);
                } else {
                    $('select[id$=ddlObjective]').val("").prop('disabled', true);
                }

                $('select[id$=ddlArea]').val(qData.nArea);
                $('input[id$=txtInternal]').val(qData.sInternal);
                $('input[id$=txtExternal]').val(qData.sExternal);
                $('select[id$=ddlPA]').val(qData.nPA);
                $('select[id$=ddlPortion]').val(qData.nPortion);
            } else {
                DialogWarning("ไม่พบข้อมูล");
            }
        }

        function SetIO(sIOID, sGLID) {
            SetNotValidateSelect('divEdit', 'ddlOrder');
            SetNotValidateSelect('divEdit', 'ddlGL');

            var lstIO = Enumerable.From(lstOrder).Where('$.nYear == ' + nYear).ToArray();
            $('select[id$=ddlOrder] option:not(:first)').remove();
            $.each(lstIO, function (i, el) {
                $('select[id$=ddlOrder]').append('<option value="' + el.sIOID + '">' + el.sIOID + '</option>');
            });

            $('select[id$=ddlOrder]').val(sIOID);

            $('select[id$=ddlGL] option:not(:first)').remove();
            if (sIOID != "") {
                var lstGL_ = Enumerable.From(lstGL).Where('$.nYear == ' + nYear + ' && $.sIOID == "' + sIOID + '"').ToArray();
                $.each(lstGL_, function (i, el) {
                    $('select[id$=ddlGL]').append('<option value="' + el.sGLID + '">' + el.sGLID + ' - ' + el.sGLName + '</option>');
                });

                $('select[id$=ddlGL]').val(sGLID);
                //$('input[id$=txtGLName]').val(qData.sGLName);
            }
        }

        function ClearData() {
            $('#divEdit input, #divEdit select, divEdit').val('');

            SetNotValidateTextbox('divEdit', 'txtProjectName');
            SetNotValidateTextbox('divEdit', 'txtDescription');
            SetNotValidateTextbox('divEdit', 'txtNameOffsetting');
            SetNotValidateTextbox('divEdit', 'txtInternal');
            SetNotValidateTextbox('divEdit', 'txtExternal');
            SetNotValidateSelect('divEdit', 'ddlPeriod');
            SetNotValidateTextbox('divEdit', 'txtPostingDate');
            SetNotValidateSelect('divEdit', 'ddlOrder');
            SetNotValidateTextbox('divEdit', 'txtValInRepCur');
            SetNotValidateSelect('divEdit', 'ddlGL');
            SetNotValidateSelect('divEdit', 'ddlObjective');
            SetNotValidateSelect('divEdit', 'ddlArea');
            //SetNotValidateSelect('divEdit', 'ddlPortion');
            SetNotValidateSelect('divEdit', 'ddlPA');
        }

        // #region Auto Complete Project Name
        var IsSelectedtxtProject = false;
        function SetAutoComplete_Project() {
            $("input[id$=txtProjectName]")
            .on("change", function () {
                if (!IsSelectedtxtProject || !IsBrowserFirefox()) {
                    $("input[id$=txtProjectName]").val("");
                    $("input[id$=txtProjectID]").val("");
                    ReValidateFieldControl("divEdit", GetElementName('txtProjectName', objControl.txtbox));
                }
            }).focus(function () {
                IsSelectedtxtProject = false;
            })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtProject = false;
                   var sSearch = request.term.replace(/\s/g, "").toLowerCase();
                   if (sSearch != "" && sSearch.length >= 3) {
                       var lstProject_ = [];
                       $.each(lstProject, function (i, el) {
                           if (el.sProjectName.replace(/\s/g, "").toLowerCase().indexOf(sSearch) > -1) {
                               lstProject_.push(el);
                           }
                       });

                       response($.map(lstProject_, function (item) {
                           return {
                               value: item.sProjectName,
                               label: item.sProjectName,
                               nProjectID: item.nProjectID
                           }
                       }));
                   }
               },
               minLength: 3,
               select: function (event, ui) {
                   IsSelectedtxtProject = true;
                   $("input[id$=txtProjectID]").val(ui.item.nProjectID);
                   if (IsBrowserFirefox()) {
                       $("input[id$=txtProjectName]").blur();;
                   }
               }
           });
        }
        // #endregion

        function SaveData() {
            if (CheckValidate('divEdit')) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();

                    var nID = +GetValTextBox('hddnID');
                    var nProjectID = +GetValTextBox('txtProjectID');
                    //var sProjectName = GetValTextBox('txtProjectName');
                    //var nPeriod = +GetValDropdown('ddlPeriod');
                    //var sIOID = GetValDropdown('ddlOrder');
                    //var sPostingDate = GetValTextBox('txtPostingDate');
                    var sDescription = GetValTextBox('txtDescription');
                    //var nValInRepCur = +GetValTextBox('txtValInRepCur').replace(/\,/g, '');
                    //var sNameOffsetting = GetValTextBox('txtNameOffsetting');
                    //var sGLID = GetValDropdown('ddlGL');
                    //var sGLName = $('select[id$=ddlGL] :selected').text().split(' - ')[1];
                    var nObjective = +GetValDropdown('ddlObjective');
                    var sObjective = $('select[id$=ddlObjective] :selected').text();
                    var nArea = +GetValDropdown('ddlArea');
                    var sArea = $('select[id$=ddlArea] :selected').text();
                    var sInternal = GetValTextBox('txtInternal');
                    var sExternal = GetValTextBox('txtExternal');
                    var nPA = +GetValDropdown('ddlPA');
                    var sPA = $('select[id$=ddlPA] :selected').text();

                    var qData = Enumerable.From(arrData).FirstOrDefault(null, '$.nItem == ' + nID);
                    if (qData != null) {
                        //qData.nProjectID = nProjectID;
                        //qData.sProjectName = sProjectName;
                        //qData.nPeriod = nPeriod;
                        //qData.sIOID = sIOID;
                        //qData.sPostingDate = sPostingDate;
                        qData.sDescription = sDescription;
                        //qData.nValInRepCur = nValInRepCur;
                        //qData.sNameOffsetting = sNameOffsetting;
                        //qData.sGLID = sGLID;
                        //qData.sGLName = sGLName;
                        qData.nObjective = nObjective;
                        qData.sObjective = sObjective;
                        qData.nArea = nArea;
                        qData.sArea = sArea;
                        qData.sInternal = sInternal;
                        qData.sExternal = sExternal;
                        qData.nPA = nPA;
                        qData.sPA = sPA;
                    }

                    var obj = {
                        'nID': nID,
                        'nItem': +GetValTextBox('txtItemID'),
                        'nProjectID': nProjectID,
                        //'nPeriod': nPeriod,
                        //'sIOID': sIOID,
                        //'sPostingDate': sPostingDate,
                        'sDescription': sDescription,
                        //'nValInRepCur': nValInRepCur,
                        //'sNameOffsetting': sNameOffsetting,
                        //'sGLID': sGLID,
                        'nObjective': nObjective,
                        'nArea': nArea,
                        'sInternal': sInternal,
                        'sExternal': sExternal,
                        'nPA': nPA
                    }

                    AjaxWebMethod("SaveData", { 'itemSave': obj }, function (response) {
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupSessionTimeOut();
                        } else if (response.d.Status == SysProcess.Duplicate) {
                            UnblockUI();
                            DialogDuplicate();
                        } else if (response.d.Status == SysProcess.SaveFail) {
                            UnblockUI();
                            DialogSaveFail(response.d.Msg);
                        } else {
                            //UnblockUI()
                            DialogSucess();
                            ClearData();
                            SearchData();//SearchData($('#pagData li.active a').text());
                            $('#divEdit').hide('fast');
                        }
                    });
                });
            }
        }
    </script>
</asp:Content>

