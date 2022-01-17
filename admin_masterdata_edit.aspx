<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_masterdata_edit.aspx.cs" Inherits="admin_masterdata_edit" %>

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
                    <label class="col-lg-3 col-form-label">ประเภท <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                            <asp:ListItem>- ระบุ ประเภท -</asp:ListItem>
                            <asp:ListItem>Social</asp:ListItem>
                            <asp:ListItem>Environment</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">ชื่อ <span class="text-red">*</span></label>
                    <div class="col-lg-4">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-lg-3 col-form-label">รายละเอียด</label>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="2000"></asp:TextBox>
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

    <asp:HiddenField ID="hddID" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSave = $('button#btnSave');
        var $btnBack = $('button#btnBack');

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {                    
                    SetControl();
                    SetValidate();
                }

                if ($Permission != "A") {
                    $('#divForm input, #divForm select, #divForm textarea').prop('disabled', true);
                    $btnSave.remove();
                }
            }
        });

        function SetControl() {
            $btnBack.click(function () {
                window.Redirect('admin_masterdata.aspx');
            });

            $btnSave.click(function () {
                SaveData();
            });
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("ddlType", objControl.dropdown)] = addValidate_notEmpty("ระบุ ประเภท");
            objValidate[GetElementName("txtName", objControl.txtbox)] = addValidate_notEmpty("ระบุ ชื่อ");
            BindValidate("divForm", objValidate);
        }

        function SaveData() {
            if (CheckValidate('divForm')) {
                DialogConfirmSubmit(function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();

                    var obj = {
                        'nSubID': +GetValTextBox('hddID'),
                        'nType': +GetValDropdown('ddlType'),
                        'sName': GetValTextBox('txtName'),
                        'sDesciption': GetValTextArea('txtDescription'),
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
                            UnblockUI()
                            DialogSucessRedirect('admin_masterdata.aspx');
                        }
                    });
                });
            }
        }
    </script>
</asp:Content>

