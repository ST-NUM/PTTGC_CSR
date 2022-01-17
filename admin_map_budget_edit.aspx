<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_map_budget_edit.aspx.cs" Inherits="admin_map_budget_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <div id="divForm">

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">ปี <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" MaxLength="4" placeholder="ปปปป"></asp:TextBox>
                    </div>
                </div>

            </div>

            <div id="divSub" class="card collapHead mb-3">
                <div class="card-header collap" data-toggle="collapse" aria-expanded="true" data-target="#collapse_data" aria-controls="collapse_data">
                    <div class="form-row">
                        <b>รายการ</b>
                        <span class="ml-auto"></span>
                    </div>
                </div>
                <div class="collapse show" id="collapse_data">
                    <div class="card-body">

                        <div id="divEdit">
                            <div class="form-group row">
                                <label class="col-lg-3 col-form-label">ประเภท <span class="text-red">*</span></label>
                                <div class="col-lg-6">
                                    <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label class="col-lg-3 col-form-label">Order <span class="text-red">*</span></label>
                                <div class="col-lg-6">
                                    <asp:DropDownList ID="ddlOrder" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label class="col-lg-3 col-form-label">GL <span class="text-red">*</span></label>
                                <div class="col-lg-6">
                                    <asp:DropDownList ID="ddlGL" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="col-lg-3"></div>
                                <div class="col-lg-auto">
                                    <button id="btnSaveData" type="button" class="btn btn-info tooltipstered"><i class="fa fa-save"></i>&nbsp; บันทึก</button>
                                    <button id="btnCancelData" type="button" class="btn btn-secondary tooltipstered"><i class="fa fa-times"></i>&nbsp; ยกเลิก</button>
                                </div>
                            </div>
                        </div>

                        <div id="divTable" class="table-responsive">
                            <table id="tbData" class="table table-bordered table-hovertable-sm">
                                <thead>
                                    <tr class="valign-middle pad-primary">
                                        <th class="text-center" style="width: 8%">
                                            <asp:CheckBox ID="cbHead" runat="server" CssClass="checkbox" Text="ที่" />
                                        </th>
                                        <th style="width: 15%" class="text-center" data-sort="sProjectType">ประเภท</th>
                                        <th class="text-center" data-sort="sIOID">Order</th>
                                        <th style="width: 30%" class="text-center" data-sort="sGLID">GL</th>
                                        <%--<th style="width: 5%"></th>--%>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <div id="divNoData" class="dataNotFound">ไม่พบข้อมูล</div>
                        </div>

                        <div id="divPaging" class="form-row align-items-center pt-3">
                            <div class="col-lg-2 mb-3">
                                <button type="button" id="btnDel" class="btn btn-danger" title="ลบ"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp; ลบ</button>
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
            </div>

        </div>
        <div class="card-footer">
            <div class="col-12 text-center">
                <button id="btnBack" type="button" class="btn btn-secondary"><i class="fa fa-arrow-left"></i>&nbsp; กลับ</button>
                <button id="btnSave" type="button" class="btn btn-info"><i class="fa fa-save"></i>&nbsp; บันทึก</button>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hddID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSave = $('button#btnSave');
        var $btnBack = $('button#btnBack');

        var lstGL = [];

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    SetValidate();

                    if (GetValTextBox('hddID') == "") {
                        $('#divSub').remove();
                    } else {
                        GetData();
                        SetControl_Sub();
                        SearchData();
                        SortingBind($tbData, SortingData);
                    }
                }

                if ($Permission != "A") {
                    $('#divForm input, #divForm select, #divForm textarea').prop('disabled', true);
                    $btnSave.remove();

                    $('#divEdit, #btnDel').remove();
                    $('#tbData thead th:first').html('ที่');
                    $('#tbData tr th:nth-child(5)').remove();
                }
            }
        });

        function SetControl() {
            SetYearPickerValidate('divForm', $('input[id$=txtYear]'));

            $btnBack.click(function () {
                window.Redirect('admin_map_budget.aspx');
            });

            $btnSave.click(function () {
                SaveData();
            });

            $('select[id$=ddlOrder]').change(function () {
                SetGL('');
            });
        }

        function SetGL(sGLID) {

            $('select[id$=ddlGL] option:not(:first)').remove();

            var sIOID = GetValDropdown('ddlOrder');
            if (sIOID != "") {
                var lstGL_ = Enumerable.From(lstGL).Where('$.sIOID == "' + sIOID + '"').ToArray();
                $.each(lstGL_, function (i, el) {
                    $('select[id$=ddlGL]').append('<option value="' + el.sGLID + '">' + el.sGLID + ' - ' + el.sGLName + '</option>');
                });

                $('select[id$=ddlGL]').val(sGLID);
            }
            SetNotValidateSelect('divEdit', 'ddlGL');
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("txtYear", objControl.txtbox)] = addValidate_notEmpty("ระบุ ปี");
            BindValidate("divForm", objValidate);

            objValidate = {};
            objValidate[GetElementName("ddlProjectType", objControl.dropdown)] = addValidate_notEmpty("ระบุ ประเภท");
            objValidate[GetElementName("ddlOrder", objControl.dropdown)] = addValidate_notEmpty("ระบุ Order");
            objValidate[GetElementName("ddlGL", objControl.dropdown)] = addValidate_notEmpty("ระบุ GL");
            BindValidate("divEdit", objValidate);
        }

        function GetData() {
            BlockUI();

            AjaxWebMethod('GetData', { 'nID': +GetValTextBox('hddID') }, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    arrData = data.d.lstData;
                    lstGL = data.d.lstGL;
                    SearchData();
                }
            });
        }

        function SaveData() {
            if (CheckValidate('divForm')) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();

                    AjaxWebMethod("SaveData", { 'nID': +GetValTextBox('hddID'), 'nYear': +GetValTextBox('txtYear'), 'lstData': arrData }, function (response) {
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupSessionTimeOut();
                        } else if (response.d.Status == SysProcess.Duplicate) {
                            UnblockUI();
                            DialogDuplicate();
                        } else if (response.d.Status == SysProcess.SaveFail) {
                            UnblockUI();
                            DialogSaveFail(response.d.Msg);
                        } else {
                            UnblockUI()
                            var sPath = GetValTextBox('hddID') != "" ? "admin_map_budget.aspx" : "admin_map_budget_edit.aspx?str=" + response.d.Msg;
                            DialogSucessRedirect(sPath);
                        }
                    });
                });
            }
        }

        //#region Sub
        var sIconPlus = '<i class="fa fa-plus" aria-hidden="true"></i>&nbsp;';
        var isAdd = true;
        var nEdit_ID = "";
        var $cbHead = $('input[id$=cbHead]');
        var $tbData = $('table#tbData');
        var $divNoData = $('div#divNoData');
        var $divPaging = $('div#divPaging');
        var $btnDel = $('button#btnDel');
        var arrData = [];
        var $objPag = {};
        var $ddlPageSize = $('select[id$=ddlPageSize]');

        function SortingEvent() { }

        function SetControl_Sub() {
            $cbHead.change(function () {
                var isChecked = $(this).is(':checked');
                var $cbRec = $('input[id*=cbRec_]:checkbox');
                $cbRec.prop('checked', isChecked);
            });

            $tbData.delegate('input[id^="cbRec_"]:checkbox', 'change', function () {
                var $cbRec = $('input[id^="cbRec_"]:checkbox');
                var $cbRec_Checked = $cbRec.filter(':checked');
                var n_$cbRec = $cbRec.length;
                var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_Checked.length : false;
                $cbHead.prop('checked', isCheckedAll);
            });

            $ddlPageSize.change(function () {
                var nPageSize = $(this).val();
                $objPag.setOptions({ page: 1, perpage: nPageSize }).setPage(); //set PageSize and Refresh
                var nPageNo = $objPag.opts.page; //เลขหน้าปัจจุบัน

                ActiveDataBind_Rev(nPageSize, nPageNo, arrData, $tbData, CreateDataRow, OnDataBound());
            });

            $btnDel.click(function () {
                var $cbRec = $('input[id^="cbRec_"]:checkbox');
                var $cbRec_Checked = $cbRec.filter(':checked');
                if ($cbRec_Checked.length > 0) {
                    DialogConfirmDel(function () {
                        BBox.ButtonEnabled(false);
                        var arrDel = $.map($cbRec_Checked, function (cb) { return +$(cb).val(); });
                        arrData = Enumerable.From(arrData).Where(function (w) { return arrDel.indexOf(w.nItem) == -1 }).ToArray();
                        SearchData();
                        BBox.Close();
                        UnblockUI();
                    });
                }
                else DialogDeleteError();
            });

            $('button[id$=btnSaveData]').click(function () {
                Add_Data();
                return false;
            });

            $('button#btnCancelData').click(function () {
                isAdd = true;
                SetContent(isAdd);
                return false;
            });

            SetContent(true);
        }

        function SearchData() {
            BlockUI();
            var arrData_ = Enumerable.From(arrData).OrderByDescending('$.nItem').ToArray();
            $objPag = $('ul#pagData').paging(arrData.length, {
                format: '[< nnncnnn >]',
                onFormat: EasyPaging_OnFormat,
                perpage: $ddlPageSize.val(),
                onSelect: function (nPageNo) { //1,2,3,4,5,...
                    ActiveDataBind_Rev($ddlPageSize.val(), nPageNo, arrData, $tbData, CreateDataRow, OnDataBound());
                },
            });

            SetTooltip();
            UnblockUI();
        }

        function SortingData(sExpression, sDirection) {
            switch (sExpression) {
                case 'sProjectType':
                case 'sIOID':
                case 'sGLID':
                    DataSort(sDirection,
                        function () { arrData = Enumerable.From(arrData).OrderBy('$.' + sExpression).ToArray(); },
                        function () { arrData = Enumerable.From(arrData).OrderByDescending('$.' + sExpression).ToArray(); })
                    break;
            }

            ActiveDataBind_Rev($ddlPageSize.val(), $objPag.opts.page, arrData, $tbData, CreateDataRow, OnDataBound());
            SetTooltip();
        }

        function CreateDataRow(objData, nRowNo) {
            var sHTML = "";
            var isCanDel = objData.IsCanDel;

            sHTML += '<td class="text-center">' +
        ($Permission == "A" && isCanDel ?
        ('<div class="checkbox"><input type="checkbox" name="cbRec_' + objData.nItem + '" id="cbRec_' + objData.nItem + '" value="' + objData.nItem + '" />' +
        '<label for="cbRec_' + objData.nItem + '">' + nRowNo + '.</label></div>') : nRowNo + '.') + '</td>';

            sHTML += '<td class="text-center">' + objData.sProjectType + '</td>';
            sHTML += '<td>' + objData.sIOName + '</td>';
            sHTML += '<td>' + objData.sGLName + '</td>';

            //sHTML += $Permission == "A" ? '<td class="text-center" valign="top">' +
            //       '<button type="button" class="btn btn-sm btn-outline-info" title="' + ($Permission == "A" ? 'แก้ไข' : 'ดูรายละเอียด') + '" onclick="Edit_Data(\'' + objData.nItem + '\')">' +
            //       '<i class="fa fa-' + ($Permission == "A" ? "edit" : "eye") + '"></i>&nbsp;' +
            //       '</button>' +
            //       '</td>' : '';

            return '<tr id="' + objData.nItem + '">' + sHTML + '</tr>';
        }

        function OnDataBound() {
            CheckDataFound_Rev(arrData, $divNoData, $divPaging);
            SortingEvent();
            $cbHead.prop('checked', false);
        }

        function Add_Data() {
            BlockUI();
            if (CheckValidate('divEdit')) {
                var nProjectType = +GetValDropdown('ddlProjectType');
                var sProjectType = $('select[id$=ddlProjectType] :selected').text();
                var sIOID = GetValDropdown("ddlOrder");
                var sIOName = $('select[id$=ddlOrder] :selected').text();
                var sGLID = GetValDropdown('ddlGL');
                var sGLName = $('select[id$=ddlGL] :selected').text();
                var IsPass = true;

                if (arrData.length > 0) {
                    if (isAdd) {
                        IsPass = Enumerable.From(arrData).FirstOrDefault(null, '$.sIOID =="' + sIOID + '" && $.sGLID == "' + sGLID + '"') == null;
                    } else {
                        IsPass = Enumerable.From(arrData).FirstOrDefault(null, '$.nItem != ' + nEdit_ID + ' && $.sIOID =="' + sIOID + '" && $.sGLID == "' + sGLID + '"') == null;
                    }
                }

                if (IsPass) {
                    if (isAdd) {
                        var nItem = Enumerable.From(arrData).LastOrDefault() !== undefined ? Enumerable.From(arrData).LastOrDefault().nItem + 1 : 1;
                        arrData.push({
                            nItem: nItem,
                            nProjectType: nProjectType,
                            sProjectType: sProjectType,
                            sIOID: sIOID,
                            sIOName: sIOName,
                            sGLID: sGLID,
                            sGLName: sGLName,
                            IsCanDel: true
                        });

                    } else {
                        var qThis = Enumerable.From(arrData).FirstOrDefault(null, function (w) { return w.nItem == nEdit_ID });
                        if (qThis != null) {
                            qThis.nProjectType = nProjectType;
                            qThis.sProjectType = sProjectType;
                            qThis.sIOID = sIOID;
                            qThis.sGLID = sGLID;
                        }
                    }

                    SearchData();
                    isAdd = true;
                    SetContent(isAdd);

                    nEdit_ID = "";

                    DialogSucess();
                } else {
                    DialogDuplicate();
                }
            }

            UnblockUI();
        }

        function Edit_Data(nItem) {
            BlockUI();
            var qThis = Enumerable.From(arrData).FirstOrDefault(null, function (w) { return w.nItem == nItem });
            if (qThis != null) {
                $('select[id$=ddlProjectType]').val(qThis.nProjectType)
                $('select[id$=ddlOrder]').val(qThis.sIOID);

                SetGL(qThis.sGLID);

                nEdit_ID = qThis.nItem;
            }

            isAdd = false;
            SetContent(isAdd);
            SetTooltip();
            UnblockUI();
        }

        function SetContent(bType) {
            if (bType) {
                //ADD
                $('button#btnSaveData').html(sIconPlus + ' เพิ่ม');
                $('button#btnCancelData').addClass('hidden');

                $('select[id$=ddlProjectType]').val("")
                $('select[id$=ddlOrder]').val("");
                $('select[id$=ddlGL]').val("");
                nEdit_ID = "";
            } else {
                //EDIT
                $('button#btnSaveData').html(sIconPlus + ' อัพเดต');
                $('button#btnCancelData').removeClass('hidden');
            }

            $('#btnDel, #divTable .btn-warning').attr('disabled', !bType);

            VALIDATED("divEdit", "select", "ddlProjectType");
            VALIDATED("divEdit", "select", "ddlOrder");
            VALIDATED("divEdit", "select", "ddlGL");
        }

        //#endregion Sub
    </script>
</asp:Content>

