<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="project_edit.aspx.cs" Inherits="project_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        i.glyphicon-ok {
            color: #00a65a !important;
        }

        i[data-fv-icon-for*=ddlMonthStart] {
            left: 39%;
        }

        small[data-fv-for*=ddlMonthEnd] {
            display: table-footer-group;
        }

        i[data-fv-icon-for*=txtBudget] {
            padding-right: 75px;
        }

        i[data-fv-icon-for*=txtObjectiveD], i[data-fv-icon-for*=txtObjective], i[data-fv-icon-for*=txtBenefit], i[data-fv-icon-for*=txtRemark] {
            right: -30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">
            <div id="divForm">

                <div id="divData">

                    <div class="card collapHead mb-3">
                        <div class="card-header collap" data-toggle="collapse" aria-expanded="true" data-target="#collapse_info" aria-controls="collapse_info">
                            <div class="form-row">
                                <b>ข้อมูลทั่วไป</b>
                                <span class="ml-auto"></span>
                            </div>
                        </div>
                        <div class="collapse show" id="collapse_info">
                            <div class="card-body">

                                <div id="divInfo">
                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">ปี <span class="text-red">*</span></label>
                                        <div class="col-lg-auto">
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" MaxLength="4" disabled="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div id="divProjectCode" class="form-group row hide">
                                        <label class="col-lg-3 col-form-label">รหัสโครงการ</label>
                                        <div class="col-lg-auto">
                                            <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" disabled="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">ชื่อโครงการ <span class="text-red">*</span></label>
                                        <div class="col-lg-8">
                                            <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">ประเภท</label>
                                        <div class="col-lg-4">
                                            <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">หน่วยงาน <span class="text-red">*</span></label>
                                        <div class="col-lg-4">
                                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">ผู้รับผิดชอบโครงการ <span class="text-red">*</span></label>
                                        <div class="col-lg-4">
                                            <div class="input-group">
                                                <div class="input-group-append">
                                                    <label class="input-group-text"><i class="fa fa-search"></i></label>
                                                </div>
                                                <asp:TextBox runat="server" CssClass="form-control" type="text" ID="txtEmpName" placeholder="รหัส / ชื่อ-สกุล" />
                                                <asp:TextBox ID="txtEmpID" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">Cost Center <span class="text-red">*</span></label>
                                        <div class="col-lg-4">
                                            <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control" disabled="true">
                                                <asp:ListItem Value="">- Cost Center -</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">Order <span class="text-red">*</span></label>
                                        <div class="col-lg-5">
                                            <asp:DropDownList ID="ddlOrder" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">เดือนเริ่มต้น - สิ้นสุด <span class="text-red">*</span></label>
                                        <div class="col-lg-5">
                                            <div class="input-group">
                                                <asp:DropDownList ID="ddlMonthStart" runat="server" CssClass="form-control">
                                                    <asp:ListItem>- ระบุ เดือนเริ่มต้น -</asp:ListItem>
                                                </asp:DropDownList>
                                                <div class="input-group-append">
                                                    <label class="input-group-text">-</label>
                                                </div>
                                                <asp:DropDownList ID="ddlMonthEnd" runat="server" CssClass="form-control">
                                                    <asp:ListItem>- ระบุ เดือนสิ้นสุด -</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">งบโครงการ <span class="text-red">*</span></label>
                                        <div class="col-lg-4">
                                            <div class="input-group">

                                                <asp:TextBox ID="txtBudget" runat="server" CssClass="form-control pr-4"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <label class="input-group-text">บาท</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    

                                    <div id="divDimension">
                                        <div class="form-group row">
                                            <label class="col-lg-3 col-form-label">CSR Dimension <span class="text-red">*</span></label>
                                            <div class="col-lg-4">
                                                <asp:DropDownList ID="ddlDimension" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div id="divDimensionSub" class="form-group row">
                                            <label class="col-lg-3 col-form-label">CSR Dimension ย่อย <span class="text-red">*</span></label>
                                            <div class="col-lg-4">
                                                <asp:DropDownList ID="ddlDimensionSub" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">- CSR Dimension ย่อย -</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="divDonation">
                                        <div class="form-group row">
                                            <label class="col-lg-3 col-form-label">Internal</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtInternalD" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group row">
                                            <label class="col-lg-3 col-form-label">External</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtExternalD" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group row">
                                            <label class="col-lg-3 col-form-label">Objective <span class="text-red">*</span></label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtObjectiveD" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>

                    <div id="divHeadDetail" class="card collapHead mb-3">
                        <div class="card-header collap" data-toggle="collapse" aria-expanded="false" data-target="#collapse_detail" aria-controls="collapse_detail">
                            <div class="form-row">
                                <b>รายละเอียดเพิ่มเติม</b>
                                <span class="ml-auto"></span>
                            </div>
                        </div>
                        <div class="collapse" id="collapse_detail">
                            <div class="card-body">
                                <div id="divDetail">

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">วัตถุประสงค์ <span class="text-red">*</span></label>
                                        <div class="col-lg-8">
                                            <asp:TextBox ID="txtObjective" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">ประโยชน์ที่คาดว่าจะได้รับ <span class="text-red">*</span></label>
                                        <div class="col-lg-8">
                                            <asp:TextBox ID="txtBenefit" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-lg-3 col-form-label">หมายเหตุเพิ่มเติม</label>
                                        <div class="col-lg-8">
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="card collapHead mb-3">
                    <div class="card-header collap" data-toggle="collapse" aria-expanded="false" data-target="#collapse_File" aria-controls="collapse_File">
                        <div class="form-row">
                            <b>วิธีดำเนินงาน และแผนปฏิบัติงาน</b>
                            <span class="ml-auto"></span>
                        </div>
                    </div>
                    <div class="collapse" id="collapse_File">
                        <div class="card-body">
                            <div id="divFile">

                                <div class="form-group row">
                                    <label class="col-lg-3 col-form-label">ไฟล์ภาพ/เอกสาร</label>
                                    <div class="col-lg-8">
                                        <div id="divFilePic">
                                            <input type="file" name="files" id="txtFilePic" multiple="multiple" accept="
                                                .png,.jpg,.jpeg,.gif,application/msword,<%--image/*,--%>
                                                application/vnd.openxmlformats-officedocument.wordprocessingml.document
                                                " />
                                            <span class="text-muted">* ขนาดไฟล์ไม่เกิน 10MB., นามสกุล: .jpg .jpeg .png .doc .docx</span>
                                        </div>

                                        <div id="divTB_FilePic" class="pt-3 table-responsive">
                                            <table id="tbData_FilePic" class="table table-bordered">
                                                <thead class="pad-info">
                                                    <tr class="valign-middle">
                                                        <th class="text-center" style="width: 15%">
                                                            <asp:CheckBox ID="cbHead_FilePic" runat="server" CssClass="checkbox" Text="ที่" /></th>
                                                        <th class="text-center">ชื่อไฟล์</th>
                                                        <th class="text-center" style="width: 18%">วันที่อัพเดท</th>
                                                        <th class="text-center" style="width: 10%">ดู</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>

                                        <div id="divPaging_FilePic" class="form-row align-items-center pt-2">
                                            <div class="col-md-4 mb-3">
                                                <button type="button" id="btnDel_FilePic" class="btn btn-danger" title="ลบ"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp; ลบ</button>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label class="col-lg-3 col-form-label">ไฟล์อื่นๆ</label>
                                    <div class="col-lg-8">
                                        <div id="divFileOther">
                                            <input type="file" name="files" id="txtFileOther" multiple="multiple" accept="
                                            application/pdf,
                                            .png,.jpg,.jpeg,.gif,
                                            application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,
                                            application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,
                                            application/vnd.ms-powerpoint,
                                            application/vnd.openxmlformats-officedocument.presentationml.slideshow,
                                            application/vnd.openxmlformats-officedocument.presentationml.presentation" />
                                            <span class="text-muted">* ขนาดไฟล์ไม่เกิน 50MB., นามสกุล: .jpg .jpeg .png .doc .docx .xls .xlsx .pdf .ppt .pptx</span>
                                        </div>

                                        <div id="divTB_FileOther" class="pt-3 table-responsive">
                                            <table id="tbData_FileOther" class="table table-bordered">
                                                <thead class="pad-info">
                                                    <tr class="valign-middle">
                                                        <th class="text-center" style="width: 15%">
                                                            <asp:CheckBox ID="cbHead_FileOther" runat="server" CssClass="checkbox" Text="ที่" /></th>
                                                        <th class="text-center">ชื่อไฟล์</th>
                                                        <th class="text-center" style="width: 18%">วันที่อัพเดท</th>
                                                        <th class="text-center" style="width: 10%">ดู</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>

                                        <div id="divPaging_FileOther" class="form-row align-items-center pt-2">
                                            <div class="col-md-4 mb-3">
                                                <button type="button" id="btnDel_FileOther" class="btn btn-danger" title="ลบ"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp; ลบ</button>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div id="divHistory" class="card collapHead mb-3" style="display: none;">
                    <div class="card-header collap" data-toggle="collapse" aria-expanded="false" data-target="#collapse_Log" aria-controls="collapse_Log">
                        <div class="form-row">
                            <b>ประวัติการทำรายการ</b>
                            <span class="ml-auto"></span>
                        </div>
                    </div>
                    <div class="collapse" id="collapse_Log">
                        <div class="card-body">
                            <div id="divTB_Log" class="pt-3 table-responsive">
                                <table id="tbData_Log" class="table table-bordered">
                                    <thead class="pad-info">
                                        <tr class="valign-middle">
                                            <th class="text-center" style="width: 5%">ที่</th>
                                            <th class="text-center" style="width: 25%">รายการ</th>
                                            <th class="text-center" style="width: 25%">ผู้ทำรายการ</th>
                                            <th class="text-center" style="width: 15%">วันที่ทำรายการ</th>
                                            <th class="text-center">ข้อคิดเห็น</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="divComment" class="form-group row pl-3" style="display: none;">
                    <label class="col-lg-3 col-form-label">ข้อคิดเห็น <span class="text-red">*</span></label>
                    <div class="col-lg-8">
                        <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine" disabled="true"></asp:TextBox>
                    </div>
                </div>

            </div>

        </div>
        <div class="card-footer">
            <div class="col-12 text-center">
                <button id="btnBack" type="button" class="btn btn-secondary"><i class="fa fa-arrow-left"></i>&nbsp; กลับ</button>
                <button id="btnSave" type="button" class="btn btn-info" style="display: none;"><i class="fa fa-save"></i>&nbsp; บันทึก</button>
                <button id="btnSubmit" type="button" class="btn btn-primary" style="display: none;"><i class="fa fa-paper-plane"></i>&nbsp; ส่งอนุมัติ</button>
                <button id="btnApprove" type="button" class="btn btn-success" style="display: none;"><i class="fa fa-check"></i>&nbsp; อนุมัติ</button>
                <button id="btnRevisit" type="button" class="btn btn-warning" style="display: none;"><i class="fa fa-redo"></i>&nbsp; ส่งกลับแก้ไข</button>
                <button id="btnNotApprove" type="button" class="btn btn-danger" style="display: none;"><i class="fa fa-times"></i>&nbsp; ไม่อนุมัติ</button>
                <button id="btnClose1" type="button" class="btn btn-success" style="display: none;"><i class="fa fa-check"></i>&nbsp; ปิดโครงการ</button>
                <button id="btnClose2" type="button" class="btn btn-danger" style="display: none;"><i class="fa fa-times"></i>&nbsp; ระงับโครงการ</button>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hddUserID" runat="server" />
    <asp:HiddenField ID="hddProjectID" runat="server" />
    <asp:HiddenField ID="hddStatusID" runat="server" />
    <asp:HiddenField ID="hddPassApprove" runat="server" />
    <asp:HiddenField ID="hddIsOwner" runat="server" />
    <asp:HiddenField ID="hddIsAdmin" runat="server" />
    <asp:HiddenField ID="hddIsApprover" runat="server" />
    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <%--InputMask : Jquery for autoformat input--%>
    <script src="Scripts/jquery-inputmask/min/inputmask/inputmask.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-inputmask/min/inputmask/inputmask.numeric.extensions.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-inputmask/inputmask/jquery.inputmask.js"></script>

    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var $btnSave = $('button#btnSave');
        var $btnSubmit = $('button#btnSubmit');
        var $btnApprove = $('button#btnApprove');
        var $btnRevisit = $('button#btnRevisit');
        var $btnNotApprove = $('button#btnNotApprove');
        var $btnClose1 = $('button#btnClose1');
        var $btnClose2 = $('button#btnClose2');
        var $btnBack = $('button#btnBack');
        var nLength = 3;
        var IsEdit = false;

        var nStatusID = +GetValTextBox('hddStatusID');
        var IsAdmin = GetValTextBox('hddIsAdmin') == "Y";
        var IsOwner = GetValTextBox('hddIsOwner') == "Y";
        var IsApprover = GetValTextBox('hddIsApprover') == "Y";
        var IsPassApprove = GetValTextBox('hddPassApprove') == "Y";

        var lstCostCenter = [];
        var lstDimensionSub = [];
        var lstProjectName = [];
        var lstLog = [];

        $(document).on('keydown', 'input[pattern]', function (e) {
            var input = $(this);
            var oldVal = input.val();
            var regex = new RegExp(input.attr('pattern'), 'g');

            setTimeout(function () {
                var newVal = input.val();
                if (!regex.test(newVal)) {
                    input.val(oldVal);
                }
            }, 0);
        });

        $(function () {
            if (!isTimeOut) {
                if ($Permission === "N") {
                    PopupNoPermission();
                } else {
                    if (GetValTextBox('hddProjectID') != "") { $('#divHistory').show(); }

                    SetControl();
                    SetValidate();
                    SetDonation(true);
                    SetAutoComplete_Project();
                    SetAutoComplete_Owner();
                    SetData();
                }

                if ($Permission != "A") {
                    $('div#divForm').find('input,select,texarea').prop('disabled', true);
                }
            }
        });

        function SetControl() {
            SetMonthSE();

            if (GetValTextBox('txtProjectCode') != "") {
                $('#divProjectCode').removeClass('hide');
            }

            // #region Set Button
            $btnSave.click(function () { CheckSaveData(0); });
            $btnSubmit.click(function () { CheckSaveData(1); });
            $btnApprove.click(function () { CheckSaveData(2); });
            $btnNotApprove.click(function () { CheckSaveData(3); });
            $btnRevisit.click(function () { CheckSaveData(4); });
            $btnClose1.click(function () { CheckSaveData(5); });
            $btnClose2.click(function () {
                $('#divComment').show();
                $('textarea[id$=txtComment]').prop('disabled', false);
                CheckSaveData(6);
            });
            $btnBack.click(function () { window.Redirect('project.aspx'); });
            // #endregion

            // #region Set Input
            InputMaskDecimalMinMax_Align($('input[id$=txtBudget]'), 10, 0, false, false, 0, 9999999999, true);

            $('select[id$=ddlProjectType]').change(function () { SetDonation(false); });
            $('select[id$=ddlUnit]').change(function () {
                $('input[id$=txtEmpName],input[id$=txtEmpID]').val('');
                SetNotValidateTextbox('divData', 'txtEmpName');
                SetCostCenter("");
            });
            $('select[id$=ddlDimension]').change(function () { SetDimensionSub(''); });
            $('select[id$=ddlMonthStart],select[id$=ddlMonthEnd]').change(function () { SetMonthSE(); });
            $('input[id$=txtBudget]').on('keydown', function () {
                ReValidateFieldControl('divData', $('input[id$=txtBudget]').attr('name'));
            });
            // #endregion
        }

        function SetMonthSE() {
            var nStart = +GetValDropdown('ddlMonthStart');
            var nEnd = +GetValDropdown('ddlMonthEnd');

            $('select[id$=ddlMonthStart] option:not(:first)').each(function (i, el) {
                $(el).prop('disabled', (nEnd > 0 ? +el.value > nEnd : false));
            });

            $('select[id$=ddlMonthEnd] option:not(:first)').each(function (i, el) {
                $(el).prop('disabled', (nStart > 0 ? +el.value < nStart : false));
            });
        }

        function SetDonation(IsFirstTime) {
            $('select[id$=ddlDimension] option[value=18]').remove();//N/A

            var sVal = GetValDropdown('ddlProjectType');
            var IsDonation = sVal == "22";//Donation
            var IsNA = sVal == "22" || sVal == "24";//Donation, Advertising & PR
            var IsProject = sVal == "23";//CSR Expenses
            var IsEducation = sVal == "25" || sVal == "26";//KVIS,VISTEC
            if (!IsProject) {
                $('select[id$=ddlDimension],select[id$=ddlDimensionSub]').val('');
            }

            if (IsDonation) {
                $('#divDonation').show('fast');
            } else {
                $('#divDonation').hide('fast');
            }

            EnableValidateControl('divData', $('select[id$=ddlDimension]').attr('name'), IsProject);
            EnableValidateControl('divData', $('select[id$=ddlDimensionSub]').attr('name'), IsProject);

            if (IsNA) {
                $('select[id$=ddlDimension]').append('<option value="18">N/A</option>');
            }

            //Donation
            EnableValidateControl('divData', $('textarea[id$=txtObjectiveD]').attr('name'), IsDonation);

            if (!IsFirstTime) {
                $('#divDonation textarea').val('');
                $('select[id$=ddlDimension],select[id$=ddlDimensionSub]').val('');

                if (IsNA) { $('select[id$=ddlDimension]').val("18") }
                else if (IsEducation) {
                    $('select[id$=ddlDimension]').val('10');
                    $('select[id$=ddlDimensionSub] option:not(:first)').remove();
                    $('select[id$=ddlDimensionSub]').append('<option value="13">Education</option>');
                    $('select[id$=ddlDimensionSub]').val('13');
                }
            }

            var sDimension = GetValDropdown('ddlDimension');
            $('select[id$=ddlDimension]').prop('disabled', !IsProject);
            $('select[id$=ddlDimensionSub]').prop('disabled', !IsProject ? true : (sDimension == "9" || sDimension == "10" ? false : true));

            $('#divDonation textarea').prop('disabled', !IsDonation);
        }

        function SetCostCenter(sCostCenterID) {
            $('select[id$=ddlCostCenter] option:not(:first)').remove();
            var sOrgID = GetValTextBox('ddlUnit');
            var hasOrg = sOrgID != "";
            if (hasOrg) {
                $('select[id$=ddlCostCenter] option:not(:first)').remove();
                var sOrgName = $('select[id$=ddlUnit] option:selected').text();
                var qCC = Enumerable.From(lstCostCenter).FirstOrDefault(null, '$.sName == "' + sOrgName + '"');
                if (qCC != null) {
                    $('select[id$=ddlCostCenter]').append('<option value="' + qCC.sMainID + '">' + qCC.sMainID + " - " + qCC.sName + '</option>')
                    if (sCostCenterID == "") {
                        sCostCenterID = qCC.sMainID;
                    }
                }

                //$.each(lstCostCenter_, function (i, el) {
                //    $('select[id$=ddlCostCenter]').append('<option value="' + el.sMainID + '">' + el.sMainID + " - " + el.sName + '</option>')
                //});

                //if (IsOwner && sCostCenterID == "") {
                //    sCostCenterID = lstCostCenter_.length > 0 ? lstCostCenter_[0].sMainID : "";
                //}
            }

            //EnableValidateControl('divData', $('select[id$=ddlCostCenter]').attr('name'), hasOrg);
            $('select[id$=ddlCostCenter]').val(sCostCenterID);//.prop('disabled', !hasOrg);
            //SetNotValidateSelect('divData', 'ddlCostCenter');           
        }

        function SetDimensionSub(sDimensionSubID) {
            $('select[id$=ddlDimensionSub] option:not(:first)').remove();

            var sDimensionID = GetValDropdown('ddlDimension');
            if (sDimensionID == "9" || sDimensionID == "10") {
                //Environment 9, Social 10
                var lstDi = Enumerable.From(lstDimensionSub).Where('$.sMainID == "' + sDimensionID + '"').ToArray();
                $.each(lstDi, function (i, el) {
                    $('select[id$=ddlDimensionSub]').append('<option value="' + el.sSubID + '">' + el.sName + '</option>')
                });

                EnableValidateControl('divData', $('select[id$=ddlDimensionSub]').attr('name'), true);
                $('select[id$=ddlDimensionSub]').val(sDimensionSubID).prop('disabled', false);
                SetNotValidateSelect('divData', 'ddlDimensionSub');

            } else {
                EnableValidateControl('divData', $('select[id$=ddlDimensionSub]').attr('name'), false);
                $('select[id$=ddlDimensionSub]').val("").prop('disabled', true);
            }
        }

        function SetData() {
            BlockUI();
            var sProID = GetValTextBox('hddProjectID');
            AjaxWebMethod('GetData', { 'sProjectID': GetValTextBox('hddProjectID'), 'nYear': +GetValTextBox('txtYear') }, function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else {
                    UnblockUI();

                    var sCostCenterID = "";
                    var sDimensionID = "";
                    var sDimensionSubID = "";
                    if (sProID != "") {
                        sCostCenterID = data.d.sCostCenterID;
                        sDimensionID = data.d.sDimensionID;
                        sDimensionSubID = data.d.sDimensionSubID;
                        $('select[id$=ddlDimension]').val(sDimensionID);
                    }

                    lstProjectName = data.d.lstProjectName;
                    lstCostCenter = data.d.lstCostCenter;
                    lstDimensionSub = data.d.lstDimensionSub;

                    SetCostCenter(sCostCenterID);
                    SetDimensionSub(sDimensionSubID);

                    SetPermission();

                    //File Pic
                    if (data.d.lstFilePic !== null && data.d.lstFilePic.length > 0) {
                        arrData_FilePic = data.d.lstFilePic;
                        BindFilePic();
                    }

                    //File Other
                    if (data.d.lstFileOther !== null && data.d.lstFileOther.length > 0) {
                        arrData_FileOther = data.d.lstFileOther;
                        BindFileOther();
                    }

                    //File Other
                    if (data.d.lstLog !== null && data.d.lstLog.length > 0) {
                        lstLog = data.d.lstLog;
                        SetLog();
                    }
                }
            });
        }

        function SetPermission() {
            switch (nStatusID) {
                case 0://บันทึก                
                    if (IsOwner) {
                        IsEdit = true;

                        $('select[id$=ddlUnit],input[id$=txtEmpName]').prop('disabled', true);
                        $('#divComment').remove();

                        $btnSave.show();
                        $btnSubmit.show();
                    } else if (IsAdmin) {
                        IsEdit = true;
                        $btnSave.show();
                    } else { DisabledControl(); }
                    break;
                case 1://ส่งอนุมัติ
                    DisabledControl();
                    if (IsApprover) {
                        $('#divComment').show();
                        $btnApprove.show();
                        $btnRevisit.show();
                        if (!IsPassApprove) $btnNotApprove.show();

                        $('textarea[id$=txtComment]').prop('disabled', false);
                    }
                    break;
                case 2://อนุมัติ
                    if (IsOwner) {
                        $btnSave.show();
                        $btnSubmit.show();
                        $btnClose1.show();
                        $btnClose2.show();
                    } else { DisabledControl(); }
                    break;
                case 4://ส่งกลับแก้ไข
                    if (IsOwner) {
                        IsEdit = true;
                        $('select[id$=ddlUnit],input[id$=txtEmpName]').prop('disabled', true);
                        $btnSubmit.show();
                    } else { DisabledControl(); }
                    break;

                default: DisabledControl(); break;
            }

            if (IsPassApprove) {
                $('select[id$=ddlProjectType],select[id$=ddlUnit],input[id$=txtEmpName],select[id$=ddlOrder]').prop('disabled', true);

                if (GetValDropdown('ddlProjectType') != "23") {
                    $('select[id*=ddlDimension]').prop('disabled', true);
                }
            }
        }

        function DisabledControl() {
            $('#divForm').find('input,select,textarea').prop('disabled', true);

            $('#divFilePic,#divFileOther,divFileOther,#btnDel_FilePic,#btnDel_FileOther').hide();
            $('#tbData_FilePic th:first,#tbData_FileOther th:first').html('ที่');
        }

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("txtProjectName", objControl.txtbox)] = addValidate_notEmpty("ระบุ ชื่อโครงการ");
            objValidate[GetElementName("ddlUnit", objControl.dropdown)] = addValidate_notEmpty("ระบุ หน่วยงาน");
            //objValidate[GetElementName("ddlCostCenter", objControl.dropdown)] = addValidate_notEmpty("ระบุ Cost Center");
            objValidate[GetElementName("ddlOrder", objControl.dropdown)] = addValidate_notEmpty("ระบุ Order");
            objValidate[GetElementName("ddlMonthStart", objControl.dropdown)] = addValidate_notEmpty("ระบุ เดือนเริ่มต้น");
            objValidate[GetElementName("ddlMonthEnd", objControl.dropdown)] = addValidate_notEmpty("ระบุ เดือนสิ้นสุด");
            objValidate[GetElementName("txtBudget", objControl.txtbox)] = addValidate_notEmpty("ระบุ งบโครงการ");
            objValidate[GetElementName("ddlDimension", objControl.dropdown)] = addValidate_notEmpty("ระบุ CSR Dimension");
            objValidate[GetElementName("ddlDimensionSub", objControl.dropdown)] = addValidate_notEmpty("ระบุ CSR Dimension ย่อย");
            //objValidate[GetElementName("txtInternalD", objControl.txtarea)] = addValidate_notEmpty("ระบุ Internal");
            //objValidate[GetElementName("txtExternalD", objControl.txtarea)] = addValidate_notEmpty("ระบุ External");
            objValidate[GetElementName("txtObjectiveD", objControl.txtarea)] = addValidate_notEmpty("ระบุ Objective");
            objValidate[GetElementName("txtEmpName", objControl.txtbox)] = addValidate_notEmpty("ระบุ ผู้รับผิดชอบโครงการ");

            objValidate[GetElementName("txtObjective", objControl.txtarea)] = addValidate_notEmpty("ระบุ วัตถุประสงค์");
            objValidate[GetElementName("txtBenefit", objControl.txtarea)] = addValidate_notEmpty("ระบุ ประโยชน์ที่คาดว่าจะได้รับ");
            //objValidate[GetElementName("txtRemark", objControl.txtarea)] = addValidate_notEmpty("ระบุ หมายเหตุเพิ่มเติม");

            BindValidate("divData", objValidate);

            var objValidate2 = {};
            objValidate2[GetElementName("txtComment", objControl.txtarea)] = addValidate_notEmpty("ระบุ ข้อคิดเห็น");
            BindValidate("divComment", objValidate2);
        }

        function SetLog() {
            if (lstLog.length > 0) {
                $.each(lstLog, function (i, el) {
                    var sHTML = "";

                    sHTML += '<td class="text-center">' + (i + 1) + "." + '</td>';
                    sHTML += '<td>' + el.sAction + '</td>';
                    sHTML += '<td>' + el.sActionBy + '</td>';
                    sHTML += '<td class="text-center">' + el.sActionDate + '</td>';
                    sHTML += '<td>' + el.sComment + '</td>';

                    $('#tbData_Log tbody').append("<tr>" + sHTML + "</tr>");
                });
            } else {
                $('#tbData_Log tbody').append("<tr id='trFileNoData'><td colspan='5' class='dataNotFound' style='padding-top: 30px;'>ไม่พบข้อมูล</td></tr>");
            }
        }

        function CheckSaveData(nSaveType) {
            var sMsg = "";
            var IsApprove = true;
            var IsPass = true;
            switch (nSaveType) {
                case 0://บันทึก 
                    sMsg = AlertMsg.ConfirmSubmit;
                    IsPass = !IsPassApprove ? GetValTextBox('txtProjectName') : CheckValidate('divData');
                    IsApprove = false;
                    break;
                case 1://ส่งอนุมัติ
                    IsPass = CheckValidate('divData');
                    sMsg = AlertMsg.ConfirmSendApprove;
                    IsApprove = false;
                    break;
                case 2://อนุมัติ
                    sMsg = AlertMsg.ConfirmApprove;
                    break;
                case 3://ไม่อนุมัติ
                    IsPass = CheckValidate('divComment');
                    sMsg = AlertMsg.ConfirmNotApprove;
                    break;
                case 4://ส่งกลับแก้ไข
                    IsPass = CheckValidate('divComment');
                    sMsg = AlertMsg.ConfirmRevisit;
                    break;
                case 5://ปิดโครงการ
                    sMsg = AlertMsg.ConfirmCloseProject1;
                    IsApprove = false;
                    break;
                case 6://ระงับโครงการ
                    IsPass = CheckValidate('divComment');
                    sMsg = AlertMsg.ConfirmCloseProject2;
                    IsApprove = false;
                    break;
                default: break;
            }

            if (IsPass) {
                BBox.Confirm(AlertTitle.Confirm, sMsg, function () {
                    BBox.ButtonEnabled(false);
                    BlockUI();
                    if (!IsApprove) {
                        SaveData(nSaveType)
                    } else {
                        ApproveData(nSaveType)
                    }
                });
            } else if (nSaveType == 0) {
                DialogWarning("กรุณาระบุ ชื่อโครงการ");
            }
        }

        function SaveData(nSaveType) {
            var IsDonation = GetValDropdown('ddlProjectType') == "22";
            var itemSave = {
                'nProjectID': +GetValTextBox('hddProjectID'),
                'sProjectName': GetValTextBox('txtProjectName'),
                'nYear': +GetValTextBox('txtYear'),
                'nProjectType': GetValDropdown('ddlProjectType') != "" ? +GetValDropdown('ddlProjectType') : null,
                'sOrgID': GetValDropdown('ddlUnit'),
                'sCostCenterID': GetValDropdown('ddlCostCenter'),
                'sIOID': GetValDropdown('ddlOrder'),
                'nMonthStart': GetValDropdown('ddlMonthStart') != "" ? +GetValDropdown('ddlMonthStart') : null,
                'nMonthEnd': GetValDropdown('ddlMonthEnd') != "" ? +GetValDropdown('ddlMonthEnd') : null,
                'nBudget': GetValTextBox('txtBudget') != "" ? +GetValTextBox('txtBudget').replace(/\,/g, '') : null,
                'nDimensionID': GetValDropdown('ddlDimension') != "" ? +GetValDropdown('ddlDimension') : null,
                'nDimensionSubID': GetValDropdown('ddlDimensionSub') != "" ? +GetValDropdown('ddlDimensionSub') : null,
                'sInternalD': IsDonation ? GetValTextArea('txtInternalD') : "",
                'sExternalD': IsDonation ? GetValTextArea('txtExternalD') : "",
                'sObjectiveD': IsDonation ? GetValTextArea('txtObjectiveD') : "",
                'nOwnerID': GetValTextBox('txtEmpID') != "" ? +GetValTextBox('txtEmpID') : null,

                'sObjective': GetValTextArea('txtObjective'),
                'sBenefit': GetValTextArea('txtBenefit'),
                'sRemark': GetValTextArea('txtRemark'),

                'nSaveType': nStatusID == 2 && nSaveType == 0 ? nStatusID : nSaveType,
                'sComment': nSaveType == 6 ? GetValTextArea('txtComment') : ''
            };

            AjaxWebMethod("SaveData", { 'itemSave': itemSave, 'nStatusID': +GetValTextBox('hddStatusID'), 'lstFilePic': arrData_FilePic, 'lstFileOther': arrData_FileOther }, function (response) {
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
                    DialogSucessRedirect('project.aspx');
                }
            });
        }

        function ApproveData(nSaveType) {
            AjaxWebMethod("ApproveData", { 'nProjectID': +GetValTextBox('hddProjectID'), 'sComment': GetValTextArea('txtComment'), 'nSaveType': nSaveType }, function (response) {
                if (response.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else if (response.d.Status == SysProcess.SaveFail) {
                    UnblockUI();
                    DialogSaveFail(response.d.Msg);
                } else {
                    UnblockUI()
                    DialogSucessRedirect('project.aspx');
                }
            });
        }

        var sUserID = GetValTextBox('hddUserID');
        var urlAshx = "Ashx/Fileuploader.ashx";

        //#region Tab File Pic   
        var tbData_FilePic = $("table[id$=tbData_FilePic] tbody");
        var arrData_FilePic = [];
        var arrTypeFilePic = ['jpg', 'jpeg', 'png', 'doc', 'docx', 'gif'];

        var IsPassFilePic = true;
        $(function () {
            $("#tbData_FilePic").delegate('input[id^="cbRec_FilePic_"]:checkbox', 'change', function () {
                var $cbRec = $('input[id^="cbRec_FilePic_"]:checkbox');
                var $cbRec_FilePic_Checked = $cbRec.filter(':checked');
                var n_$cbRec = $cbRec.length;
                var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_FilePic_Checked.length : false;
                $("input[id$=cbHead_FilePic]").prop('checked', isCheckedAll);
            });

            $("input[id$=cbHead_FilePic]").change(function () {
                var isChecked = $(this).is(':checked');
                var $cbRec = $('input[id*=cbRec_FilePic_]:checkbox');
                $cbRec.prop('checked', isChecked);
            });

            $("button[id$=btnDel_FilePic]").click(function () {
                var dataHasUse = false;
                var $cbRec = $('input[id^="cbRec_FilePic_"]:checkbox');
                var $cbRec_FilePic_Checked = $cbRec.filter(':checked');
                if ($cbRec_FilePic_Checked.length > 0) {
                    DialogConfirmDel(function () {
                        BBox.ButtonEnabled(false);
                        var arrDel = $.map($cbRec_FilePic_Checked, function (cb) { return +$(cb).val(); });
                        $.each(arrDel, function (i, el) {
                            var $thisDel = Enumerable.From(arrData_FilePic).Where(function (w) { return w.nID == el }).FirstOrDefault();
                            if ($thisDel !== undefined) {
                                arrData_FilePic = Enumerable.From(arrData_FilePic).Where('$.nID != ' + el).ToArray();
                            }
                        });
                        BindFilePic();
                        BBox.Close();
                    });
                }
                else DialogDeleteError();
            });

            var filupload1 = $('input[id$="txtFilePic"]').fileuploader({
                fileMaxSize: 10,
                enableApi: true,
                thumbnails: false,
                extensions: arrTypeFilePic,
                upload: {
                    // upload URL {String}
                    url: urlAshx,

                    // upload data {null, Object}
                    // you can also change this Object in beforeSend callback
                    // example: {case savetoname = "" then generate format filename_DateTime("ddMMyyyyHHmmssff") }
                    data: { funcname: "UPLOAD", savetopath: '../UploadFiles/' + sUserID + '/Temp/', savetoname: '' },

                    // upload type {String}
                    type: 'POST',

                    // upload enctype {String}
                    enctype: 'multipart/form-data',

                    // auto-start file uploading {Boolean}
                    // if it will be false, you can use the API methods to start it (check options example)
                    start: true,

                    // upload the files synchron(อัพโหลดให้แล้วเสร็จทีละไฟล์) {Boolean}
                    synchron: true,

                    onProgress: function (data, item) {
                        //debugger
                        apiFile1.disable($('input[id$="txtFilePic"]'))
                        //if (item.size > 10 * 1000000) {
                        //    IsPassFilePic = false;
                        //    sMsgPic = 'ไฟล์ ' + item.name + ' ขนาดเกิน 10MB. <br/>กรุณาเลือกไฟล์ที่มีขนาดไม่เกิน 10MB.';
                        //    DialogWarning('ไฟล์ ' + item.name + ' ขนาดเกิน 10MB. <br/>กรุณาเลือกไฟล์ที่มีขนาดไม่เกิน 10MB.');
                        //} else if (arrTypeFilePic.indexOf(item.extension) == -1) {
                        //    IsPassFilePic = false;
                        //    sMsgPic = 'รองรับนามสกุลไฟล์ .jpg .jpeg .png .gif .doc .docx เท่านั้น'
                        //    DialogWarning('รองรับนามสกุลไฟล์ .jpg .jpeg .png .gif .doc .docx เท่านั้น');
                        //} else {
                        //    IsPassFilePic = true;
                        //    sMsgPic = "";
                        //}
                    },

                    // Callback fired if the uploading succeeds
                    // by default we will add a success icon and fadeOut the progressbar
                    // Remember that if you want so show the PHP errors, you will need to process them also here. To prevent it you will need to respond on the upload url with error code in header.
                    onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
                        //if (IsPassFilePic) {
                        AddFilePic(data);
                        RemoveFilePic(item);
                        //}
                    },

                    // Callback fired after all files were uploaded
                    onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                        var arrData_FilePic = apiFile1.getFiles();

                        BindFilePic();
                        apiFile1.enable();
                    }
                }
            });

            $(".fileuploader-input-button").html("<span>เลือกไฟล์<span>");
            $('.fileuploader-input-caption').text('เลือกไฟล์เพื่ออัพโหลด')

            var apiFile1 = $.fileuploader.getInstance(filupload1);

            function RemoveFilePic(item) {
                apiFile1.remove(item);
            }

            BindFilePic();
        });

        function AddFilePic(item) {
            var nID = arrData_FilePic.length > 0 ? Enumerable.From(arrData_FilePic).Max('$.nID') + 1 : 1;
            arrData_FilePic.push({
                nID: nID,
                sPath: item.SaveToPath,
                sSysFileName: item.SaveToFileName,
                sFilename: item.FileName,
                nUserID: +sUserID,
                sUpdate: item.sUpdate
            });
        }

        function BindFilePic() {
            tbData_FilePic.html("");
            var arrData_FilePic_ = Enumerable.From(arrData_FilePic).ToArray();

            if (arrData_FilePic_.length > 0) {
                var IsModeEdit = IsEdit;
                var Disabled = IsEdit ? "" : "disabled=''";

                $.each(arrData_FilePic_, function (i, el) {
                    var sHTML = "";

                    sHTML += '<td class="text-center">' + (IsModeEdit ? ('<div class="checkbox padT0"><input type="checkbox" name="cbRec_FilePic_' + el.nID + '" id="cbRec_FilePic_' + el.nID + '" value="' + el.nID + '" />' +
                              '<label for="cbRec_FilePic_' + el.nID + '">' + (i + 1) + '.</label></div>') : (i + 1) + ".") + '</td>';

                    sHTML += "<td id='tdFileName_" + el.nID + "' class=\"text-left\" style=\"word-break: break-all;\">" + el.sFilename + "</td>";

                    sHTML += "<td class='text-center'>" + el.sUpdate + "</td>";

                    var sFileURL = el.sPath.replace("../", "") + el.sSysFileName.toLowerCase();
                    var onclick = "FancyBox_ViewFile('" + sFileURL + "')";
                    var btn = "<button type=\"button\" class=\"btn btn-info btn-sm\" onclick=\"" + onclick + "\" title='ดู' data-toggle='tooltip'><i class=\"glyphicon glyphicon-zoom-in\"></i></button>";
                    sHTML += "<td class=\"text-center\">" + btn + "</td>";

                    tbData_FilePic.append("<tr>" + sHTML + "</tr>");
                });
            } else {
                tbData_FilePic.append("<tr id='trFileNoData'><td colspan='4' class='dataNotFound' style='padding-top: 30px;'>ไม่พบข้อมูล</td></tr>");
            }

            $("#divPaging_FilePic").toggle(arrData_FilePic_.length > 0); //have value is true=show
            $("input[id$=cbHead_FilePic]").prop("checked", false);

            SetTooltip_Control($("table#tbData_FilePic > tbody > tr > td > button:not(.dropdown-toggle)"));
        }

        //#endregion Tab File Pic

        //#region Tab File Other   

        var tbData_FileOther = $("table[id$=tbData_FileOther] tbody");
        var arrData_FileOther = [];
        var arrTypeFileOther = ['jpg', 'jpeg', 'png', 'gif', 'xls', 'xlsx', 'pdf', 'doc', 'docx', 'ppt', 'pptx'];

        var IsPassFileOther = true;
        $(function () {
            $("#tbData_FileOther").delegate('input[id^="cbRec_FileOther_"]:checkbox', 'change', function () {
                var $cbRec = $('input[id^="cbRec_FileOther_"]:checkbox');
                var $cbRec_FileOther_Checked = $cbRec.filter(':checked');
                var n_$cbRec = $cbRec.length;
                var isCheckedAll = n_$cbRec > 0 ? n_$cbRec == $cbRec_FileOther_Checked.length : false;
                $("input[id$=cbHead_FileOther]").prop('checked', isCheckedAll);
            });

            $("input[id$=cbHead_FileOther]").change(function () {
                var isChecked = $(this).is(':checked');
                var $cbRec = $('input[id*=cbRec_FileOther_]:checkbox');
                $cbRec.prop('checked', isChecked);
            });

            $("button[id$=btnDel_FileOther]").click(function () {
                var dataHasUse = false;
                var $cbRec = $('input[id^="cbRec_FileOther_"]:checkbox');
                var $cbRec_FileOther_Checked = $cbRec.filter(':checked');
                if ($cbRec_FileOther_Checked.length > 0) {
                    DialogConfirmDel(function () {
                        BBox.ButtonEnabled(false);
                        var arrDel = $.map($cbRec_FileOther_Checked, function (cb) { return +$(cb).val(); });
                        $.each(arrDel, function (i, el) {
                            var $thisDel = Enumerable.From(arrData_FileOther).Where(function (w) { return w.nID == el }).FirstOrDefault();
                            if ($thisDel !== undefined) {
                                arrData_FileOther = Enumerable.From(arrData_FileOther).Where('$.nID != ' + el).ToArray();
                            }
                        });
                        BindFileOther();
                        BBox.Close();
                    });
                }
                else DialogDeleteError();
            });

            var filupload1 = $('input[id$="txtFileOther"]').fileuploader({
                fileMaxSize: 50,
                enableApi: true,
                thumbnails: false,
                extensions: ['jpg', 'jpeg', 'png', 'xls', 'xlsx', 'pdf', 'doc', 'docx', 'ppt', 'pptx'],
                upload: {
                    // upload URL {String}
                    url: urlAshx,

                    // upload data {null, Object}
                    // you can also change this Object in beforeSend callback
                    // example: {case savetoname = "" then generate format filename_DateTime("ddMMyyyyHHmmssff") }
                    data: { funcname: "UPLOAD", savetopath: '../UploadFiles/' + sUserID + '/Temp/', savetoname: '' },

                    // upload type {String}
                    type: 'POST',

                    // upload enctype {String}
                    enctype: 'multipart/form-data',

                    // auto-start file uploading {Boolean}
                    // if it will be false, you can use the API methods to start it (check options example)
                    start: true,

                    // upload the files synchron(อัพโหลดให้แล้วเสร็จทีละไฟล์) {Boolean}
                    synchron: true,

                    onProgress: function (data, item) {
                        apiFile1.disable($('input[id$="txtFileOther"]'))
                        //if (item.size > 50 * 1000000) {
                        //    IsPassFileOther = false;
                        //    DialogWarning('ไฟล์ ' + item.name + ' ขนาดเกิน 50MB. <br/>กรุณาเลือกไฟล์ที่มีขนาดไม่เกิน 50MB.');
                        //    RemoveFileOther(item);
                        //} else if (arrTypeFileOther.indexOf(item.extension) == -1) {
                        //    IsPassFileOther = false;
                        //    DialogWarning('รองรับนามสกุลไฟล์ .jpg .jpeg .png .gif .doc .docx .xls .xlsx .pdf .ppt .pptx เท่านั้น');
                        //    RemoveFileOther(item);
                        //} else {
                        //    IsPassFileOther = true;
                        //}
                    },

                    // Callback fired if the uploading succeeds
                    // by default we will add a success icon and fadeOut the progressbar
                    // Remember that if you want so show the PHP errors, you will need to process them also here. To prevent it you will need to respond on the upload url with error code in header.
                    onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
                        //if (IsPassFileOther) {
                        AddFileOther(data);
                        RemoveFileOther(item);
                        //}
                    },

                    // Callback fired after all files were uploaded
                    onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                        var arrData_FileOther = apiFile1.getFiles();

                        BindFileOther();
                        apiFile1.enable();
                    }
                }
            });

            $(".fileuploader-input-button").html("<span>เลือกไฟล์<span>");
            $('.fileuploader-input-caption').text('เลือกไฟล์เพื่ออัพโหลด')

            var apiFile1 = $.fileuploader.getInstance(filupload1);

            function RemoveFileOther(item) {
                apiFile1.remove(item);
            }

            BindFileOther();
        });

        function AddFileOther(item) {
            var nID = arrData_FileOther.length > 0 ? Enumerable.From(arrData_FileOther).Max('$.nID') + 1 : 1;
            arrData_FileOther.push({
                nID: nID,
                sPath: item.SaveToPath,
                sSysFileName: item.SaveToFileName,
                sFilename: item.FileName,
                nUserID: +sUserID,
                sUpdate: item.sUpdate
            });
        }

        function BindFileOther() {
            tbData_FileOther.html("");
            var arrData_FileOther_ = Enumerable.From(arrData_FileOther).ToArray();

            if (arrData_FileOther_.length > 0) {
                var IsModeEdit = IsEdit;
                var Disabled = IsEdit ? "" : "disabled=''";

                $.each(arrData_FileOther_, function (i, el) {
                    var sHTML = "";

                    sHTML += '<td class="text-center">' + (IsModeEdit ? ('<div class="checkbox padT0"><input type="checkbox" name="cbRec_FileOther_' + el.nID + '" id="cbRec_FileOther_' + el.nID + '" value="' + el.nID + '" />' +
                              '<label for="cbRec_FileOther_' + el.nID + '">' + (i + 1) + '.</label></div>') : (i + 1) + ".") + '</td>';

                    sHTML += "<td id='tdFileName_" + el.nID + "' class=\"text-left\" style=\"word-break: break-all;\">" + el.sFilename + "</td>";

                    sHTML += "<td class='text-center'>" + el.sUpdate + "</td>";

                    var sFileURL = el.sPath.replace("../", "") + el.sSysFileName.toLowerCase();
                    var onclick = "FancyBox_ViewFile('" + sFileURL + "')";
                    var btn = "<button type=\"button\" class=\"btn btn-info btn-sm\" onclick=\"" + onclick + "\" title='ดู' data-toggle='tooltip'><i class=\"glyphicon glyphicon-zoom-in\"></i></button>";
                    sHTML += "<td class=\"text-center\">" + btn + "</td>";

                    tbData_FileOther.append("<tr>" + sHTML + "</tr>");
                });
            } else {
                tbData_FileOther.append("<tr id='trFileNoData'><td colspan='4' class='dataNotFound' style='padding-top: 30px;'>ไม่พบข้อมูล</td></tr>");
            }

            $("#divPaging_FileOther").toggle(arrData_FileOther_.length > 0); //have value is true=show
            $("input[id$=cbHead_FileOther]").prop("checked", false);

            SetTooltip_Control($("table#tbData_FileOther > tbody > tr > td > button:not(.dropdown-toggle)"));
        }

        //#endregion Tab File Other

        // #region Auto Complete Project Name
        function SetAutoComplete_Project() {
            $("input[id$=txtProjectName]")
           .autocomplete({
               source: function (request, response) {
                   var sSearch = request.term.replace(/\s/g, "").toLowerCase();
                   if (sSearch != "" && sSearch.length >= nLength) {
                       var lstProjectName_ = [];
                       $.each(lstProjectName, function (i, el) {
                           if (el.replace(/\s/g, "").toLowerCase().indexOf(sSearch) > -1) {
                               lstProjectName_.push(el);
                           }
                       });

                       response($.map(lstProjectName_, function (item) {
                           return {
                               value: item,
                               label: item
                           }
                       }));
                   }
               },
               minLength: nLength,
               select: function (event, ui) {
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
                       ReValidateFieldControl("divData", GetElementName('txtEmpName', objControl.txtbox));
                   }
               }).focus(function () {
                   IsSelectedtxtEmpName = false;
               })
           .autocomplete({
               source: function (request, response) {
                   IsSelectedtxtEmpName = false;
                   if (request.term.replace(/\s/g, "") != "" && request.term.replace(/\s/g, "").length >= nLength) {
                       var sOrgID = GetValDropdown('ddlUnit');
                       if (sOrgID != "") {
                           AjaxWebMethod(UrlSearchEmp(), { 'sSearch': request.term, 'sOrgID': sOrgID }, function (data) {
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
                           DialogWarning("กรุณาระบุ หน่วยงาน");
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
