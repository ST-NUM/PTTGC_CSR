<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_permission_edit.aspx.cs" Inherits="admin_permission_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        #tbData tbody tr td span {
            display: inline-flex;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <input id="username" style="display: none" type="text" name="fakeusernameremembered">
    <input id="password" style="display: none" type="password" name="fakepasswordremembered">

    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <div id="divForm">

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">ประเภทพนักงาน</label>
                    <div class="col-lg-4">
                        <asp:RadioButtonList ID="rdlUserType" runat="server" CssClass="radio radio-inline" RepeatLayout="Flow" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Selected="true">พนักงาน GC</asp:ListItem>
                            <asp:ListItem Value="0" class="pl-4">พนักงานภายนอก</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">กลุ่มผู้ใช้งาน <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>

                <div id="divGC" class="form-group row">
                    <label class="col-lg-3 col-form-label">ชื่อ - นามสกุล <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-search"></i></span>
                            </div>
                            <asp:TextBox ID="txtEmpName" runat="server" CssClass="form-control" placeholder="รหัส / ชื่อ-นามสกุล" />
                            <asp:TextBox ID="txtEmpID" runat="server" CssClass="form-control hide"></asp:TextBox>
                            <asp:TextBox ID="txtFName" runat="server" CssClass="form-control hide"></asp:TextBox>
                            <asp:TextBox ID="txtLName" runat="server" CssClass="form-control hide"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div id="divNonGC">
                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">ชื่อ <span class="text-red">*</span></label>
                        <div class="col-lg-4">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100" />
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">นามสกุล <span class="text-red">*</span></label>
                        <div class="col-lg-4">
                            <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control" MaxLength="100" />
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">ชื่อผู้ใช้ <span class="text-red">*</span></label>
                        <div class="col-lg-4">
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="20" />
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 col-form-label">รหัสผ่าน <span class="text-red">*</span></label>
                        <div class="col-lg-4">
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" MaxLength="20" />
                        </div>
                        <div class="col-auto">
                            <button type="button" id="btnResetPass" class="btn btn-primary hide"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; รีเซ็ตรหัสผ่าน</button>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">อีเมล์ <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="40" />
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">สิทธิ์ผู้ใช้งาน</label>
                    <div class="col-lg-9">
                        <div id="divData" class="col-sm-12" style="margin-left: -15px;">
                            <div class="table-responsive">
                                <table id="tbData" class="table table-bordered table-hover">
                                    <thead class="pad-info">
                                        <tr class="valign-middle">
                                            <th class="text-center" data-sort="">ชื่อเมนู</th>
                                            <th class="text-center" data-sort="" style="width: 20%">ไม่มีสิทธิ์</th>
                                            <th class="text-center" data-sort="" style="width: 20%">ดูรายละเอียด</th>
                                            <th class="text-center" data-sort="" style="width: 20%">เพิ่ม/แก้ไข/ลบ</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <div id="divNoData" class="dataNotFound">No Data</div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">สถานะ</label>
                    <div class="col-lg-auto">
                        <asp:RadioButtonList ID="rdlActive" runat="server" CssClass="radio" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1" Selected="True">ใช้งาน</asp:ListItem>
                            <asp:ListItem Value="0" class="pl-4">ไม่ใช้งาน</asp:ListItem>
                        </asp:RadioButtonList>
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

    <asp:HiddenField ID="hddnUserID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
    <asp:HiddenField ID="hddCSR" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSave = $('button#btnSave');
        var $btnBack = $('button#btnBack');
        var arrNotAdd = [1, 3, 6, 7, 8];
        var arrMenuBackend = [9, 10, 11, 12, 13, 14];
        var arrMenuFront = [1, 2, 3, 6, 7, 8];
        var nLength = 3;

        $(function () {
            if (!isTimeOut) {
                if ($Permission === "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    GetMenu();
                    SetValidate();
                    SetUserType(true);
                    SetAutoComplete();
                }

                if ($Permission != "A") {
                    $('div#divForm').find('input,select').prop('disabled', true)
                    $btnSave.remove();
                    $('#btnResetPass').remove();
                }
            }
        });

        function SetControl() {
            if (GetValTextBox('hddnUserID') != "") {
                $('input[name*=rdlUserType]').prop('disabled', true);
                $('#btnResetPass').removeClass('hide');
            }

            $('input[name*=rdlUserType]').on('change', function () {
                SetUserType(false);
            });

            $('select[id$=ddlRole]').change(function () {
                if (GetValRadioListNotValidate('rdlUserType') == "1") {
                    $('input[id$=txtEmail]').val('');
                }

                $('input[id$=hddCSR]').val(GetValDropdown('ddlRole') == "2" || GetValDropdown('ddlRole') == "3" ? "Y" : "N")
                SetMenuByRole(false);
            });

            $btnBack.click(function () {
                window.Redirect('admin_permission.aspx');
            });

            $('#btnResetPass').click(function () {
                var nUserID = +GetValTextBox('hddnUserID');
                if (nUserID > 0) {
                    DialogConfirmResetPassword(function () {
                        BlockUI();
                        AjaxWebMethod('ResetPassword', { 'nUserID': nUserID }, function (response) {
                            if (response.d.Status == SysProcess.SessionExpired) {
                                PopupSessionTimeOut();
                            } else if (response.d.Status == SysProcess.SaveFail) {
                                UnblockUI();
                                DialogSaveFail(response.d.Msg);
                            } else {
                                UnblockUI();
                                DialogSucessRedirect('admin_permission.aspx');
                            }
                        });
                    });
                }

                return false;
            });

            $btnSave.click(function () {
                SaveData();
                return false;
            });
        }

        function SetUserType(IsFirst) {
            var IsEdit = GetValTextBox('hddnUserID') != "";
            var IsGC = GetValRadioList('rdlUserType') == "1";
            var nRole = +GetValDropdown('ddlRole');
            var arrRole = [];
            $('select[id$=ddlRole] option:not(:first)').remove();
            if (IsGC) {
                $('#divGC').show();
                $('#divNonGC').hide();

                if (IsEdit) {
                    if (GetValTextBox('hddCSR') == "Y") {
                        arrRole = [1, 2, 3, 4];
                        $('select[id$=ddlRole]').append(
                            '<option value="1">Administrator</option>' +
                            '<option value="2">CSR Staff</option>' +
                            '<option value="3">CSR Approver</option>' +
                            '<option value="4">Viewer</option>');
                    } else {
                        arrRole = [1, 4];
                        $('select[id$=ddlRole]').append(
                          '<option value="1">Administrator</option>' +
                          '<option value="4">Viewer</option>');
                    }
                } else {
                    arrRole = [1, 2, 3, 4];
                    $('select[id$=ddlRole]').append(
                        '<option value="1">Administrator</option>' +
                        '<option value="2">CSR Staff</option>' +
                        '<option value="3">CSR Approver</option>' +
                        '<option value="4">Viewer</option>');
                }
            } else {
                $('#divGC').hide();
                $('#divNonGC').show();
                arrRole = [1, 4];
                $('select[id$=ddlRole]').append(
                     '<option value="1">Administrator</option>' +
                     '<option value="4">Viewer</option>');
            }
            
            $('select[id$=ddlRole]').val(arrRole.indexOf(nRole) > -1 ? nRole + "" : "");
            SetNotValidateSelect('divForm', 'ddlRole');

            EnableValidateControl('divForm', $('input[id$=txtEmpName]').attr('name'), IsGC);

            EnableValidateControl('divForm', $('input[id$=txtName]').attr('name'), !IsGC);
            EnableValidateControl('divForm', $('input[id$=txtSurname]').attr('name'), !IsGC);
            EnableValidateControl('divForm', $('input[id$=txtUsername]').attr('name'), !IsGC);
            EnableValidateControl('divForm', $('input[id$=txtPassword]').attr('name'), !IsGC);

            SetNotValidateTextbox('divForm', 'txtEmail');
            EnableValidateControl('divForm', $('input[id$=txtEmail]').attr('name'), !IsGC);
            $('input[id$=txtEmail]').prop('disabled', IsGC);

            if (!IsFirst) {
                $('#divGC input,#divNonGC input,input[id$=txtEmail]').val('');
            }
        }

        function GetMenu() {
            BlockUI();
            var nUserID = GetValTextBox('hddnUserID') != "" ? +GetValTextBox('hddnUserID') : null;
            AjaxWebMethod("GetMenu", { 'nUserID': nUserID }, function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else {
                    arrData = response.d.lstData;
                    BindTable_Menu();
                    SetMenuByRole(true);
                }
            }, function () {
                UnblockUI();
                if ($Permission == "V") {
                    $('#tbData').find('input,select').prop('disabled', true);
                }
            });
        }

        function SetMenuByRole(IsFirst) {
            var sVal = GetValDropdown('ddlRole');
            var IsAdmin = sVal == "1";
            var IsViewer = sVal == "4";
            $.each($('#tbData input[id*=rdoAll_]'), function (i, el) {
                if (sVal == "4") {
                    $(el).prop('checked', false);
                    $(el).parent().hide();
                } else {
                    $(el).parent().show();
                }
            });

            if (!IsFirst && sVal != "") {
                if (GetValTextBox('hddnUserID') == "") {
                    $("input[id$=txtEmpName]").val("");
                    $("input[id$=txtEmpID]").val("");
                    $("input[id$=txtFName]").val("");
                    $("input[id$=txtLName]").val("");
                    SetNotValidateTextbox("divForm", 'txtEmpName');
                }

                for (var i = 0; i < arrMenuFront.length; i++) {
                    $('input[id*=rdo' + (!IsViewer && arrMenuFront[i] == "2" ? 'All' : 'View') + '_' + arrMenuFront[i] + ']').prop('checked', true);
                }

                for (var i = 0; i < arrMenuBackend.length; i++) {
                    $('input[id*=rdo' + (IsAdmin ? 'All' : 'NotView') + '_' + arrMenuBackend[i] + ']').prop('checked', true);
                }
            }
        }

        function BindTable_Menu() {
            var $Table = $('#tbData tbody');
            $Table.html('');
            if (arrData.length > 0) {
                var lstData = arrData;

                for (var i = 0; i < lstData.length; i++) {
                    var sID = lstData[i].nMenuID;
                    var CanAdd = arrNotAdd.indexOf(sID) > -1;
                    var IsHead = lstData[i].IsHead;
                    var nLevel = lstData[i].nLevel;

                    var sTD = '<td>' + (nLevel == 1 ? "" : "&emsp;-") + lstData[i].sMenuName + '</td>';
                    sTD += !IsHead ? ('<td class="text-center"><span class="radio"><input id="rdoNotView_' + sID + '" type="radio" name="rdoPer_' + sID + '" value="0"><label for="rdoNotView_' + sID + '"></span></td>') : '<td colspan="3"></td>';
                    sTD += !IsHead ? ('<td class="text-center"><span class="radio"><input id="rdoView_' + sID + '" type="radio" name="rdoPer_' + sID + '" value="1"><label for="rdoView_' + sID + '"></span></td>') : '';
                    sTD += !IsHead ? (!CanAdd ? ('<td class="text-center"><span class="radio"><input id="rdoAll_' + sID + '" type="radio" name="rdoPer_' + sID + '" value="2"><label for="rdoAll_' + sID + '"></span></td>') : '<td></td>') : '';

                    $Table.append('<tr>' + sTD + '</tr>');

                    var nPermission = lstData[i].nPermission;
                    if (nPermission != null) {
                        var sType = nPermission == 2 ? "All" : (nPermission == 1 ? "View" : "NotView");
                        $('input[id$=rdo' + sType + '_' + sID + ']').prop('checked', true);
                    }
                }
                $('#divNoData').hide("fast");
            } else {
                $('#divNoData').show("fast");
            }
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("ddlRole", objControl.dropdown)] = addValidate_notEmpty("ระบุ กลุ่มผู้ใช้งาน");
            objValidate[GetElementName("txtEmpName", objControl.txtbox)] = addValidate_notEmpty("ระบุ ชื่อ - นามสกุล");
            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty("ระบุ ชื่อ");
            objValidate[GetElementName("txtSurname", objControl.txtbox)] = addValidate_notEmpty("ระบุ นามสกุล");
            objValidate[GetElementName("txtUsername", objControl.txtbox)] = addValidate_notEmpty("ระบุ ชื่อผู้ใช้");
            objValidate[GetElementName("txtPassword", objControl.txtbox)] = addValidatePassword_notEmpty(20, '');
            objValidate[GetElementName("txtEmail", objControl.txtbox)] = addValidateEmail_notEmpty();

            BindValidate("divForm", objValidate);

            SetNotValidateTextbox('divForm', 'txtPassword')
        }

        var xxx = [];
        var IsSelectedtxtEmpName = false;
        function SetAutoComplete() {
            $("input[id$=txtEmpName]")
               .on("change", function () {
                   if (!IsSelectedtxtEmpName || !IsBrowserFirefox()) {
                       $("input[id$=txtEmpName]").val("");
                       $("input[id$=txtEmpID]").val("");
                       $("input[id$=txtFName]").val("");
                       $("input[id$=txtLName]").val("");
                       ReValidateFieldControl("divForm", GetElementName('txtEmpName', objControl.txtbox));
                   }
               }).focus(function () {
                   IsSelectedtxtEmpName = false;
               })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtEmpName = false;
                   if (request.term.replace(/\s/g, "") != "" && request.term.replace(/\s/g, "").length >= nLength) {
                       var IsCSR = GetValDropdown('ddlRole') == "2" || GetValDropdown('ddlRole') == "3";
                       AjaxWebMethod(UrlSearchEmp(), { 'sSearch': request.term, 'IsCSR': IsCSR }, function (data) {
                           if (data.d.Status == SysProcess.SessionExpired) {
                               PopupSessionTimeOut();
                           } else {
                               UnblockUI();
                               xxx = data.d.lstData.d.results;
                               response($.map(data.d.lstData.d.results, function (item) {
                                   return {
                                       value: item.EmployeeID + ' - ' + item.NameTH,
                                       label: item.EmployeeID + ' - ' + item.NameTH,
                                       sUserID: item.EmployeeID,
                                       FName: item.NameTH.split(' ')[0] + " " + item.THFirstName,
                                       LName: item.THLastName,
                                       Email: item.EmailAddress,
                                   }
                               }));
                           }
                       });
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
                   IsSelectedtxtEmpName = true;
                   $("input[id$=txtEmpID]").val(ui.item.sUserID);
                   $("input[id$=txtFName]").val(ui.item.FName);
                   $("input[id$=txtLName]").val(ui.item.LName);
                   $("input[id$=txtEmail]").val(ui.item.Email);
                   SetNotValidateTextbox("divForm", 'txtEmail');
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

        function SaveData() {
            if (CheckValidate('divForm')) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();

                    var lstPermission = [];
                    $.each($('#tbData input[id*=rdoNotView_]'), function (i, el) {
                        var sID = el.id.replace('rdoNotView_', '')
                        var sPer = GetValRadioListNotValidate('rdoPer_' + sID);
                        lstPermission.push({ 'nMenuID': +sID, 'nPermission': (sPer != "" ? +sPer : null) });
                    });

                    var IsGC = GetValRadioList('rdlUserType') == "1";
                    var obj = {
                        'nUserID': +GetValTextBox('hddnUserID'),
                        'sUserID': IsGC ? GetValTextBox('txtEmpID') : GetValTextBox('txtUsername'),
                        'sPassword': !IsGC ? GetValTextBox('txtPassword') : '',
                        'sFirstname': !IsGC ? GetValTextBox('txtName') : GetValTextBox('txtFName'),
                        'sLastname': !IsGC ? GetValTextBox('txtSurname') : GetValTextBox('txtLName'),
                        'sEmail': GetValTextBox('txtEmail'),
                        'IsGC': IsGC,
                        'nRole': +GetValDropdown('ddlRole'),
                        'lstPermission': lstPermission,
                        'IsActive': Boolean(+GetValRadioList('rdlActive')),
                    }

                    AjaxWebMethod("SaveData", obj, function (response) {
                        if (response.d.Status == SysProcess.SessionExpired) {
                            PopupSessionTimeOut();
                        } else if (response.d.Status == SysProcess.Duplicate) {
                            UnblockUI();
                            DialogDuplicate();
                        } else if (response.d.Status == SysProcess.SaveFail) {
                            UnblockUI();
                            DialogSaveFail(response.d.Msg);
                        } else {
                            UnblockUI();
                            DialogSucessRedirect('admin_permission.aspx');
                        }
                    });
                });
            }
        }
    </script>
</asp:Content>

