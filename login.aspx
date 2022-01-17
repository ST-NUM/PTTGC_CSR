<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_AllSource.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="Images/ico/ptt_weblogo-144.png" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="Images/ico/ptt_weblogo-114.png" />
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="Images/ico/ptt_weblogo-72.png" />
    <link rel="apple-touch-icon-precomposed" sizes="57x57" href="Images/ico/ptt_weblogo-57.png" />
    <link rel="shortcut icon" href="Images/ico/ptt_weblogo-favicon.png" />

    <%--Form Validate--%>
    <script src="Scripts/FormValidation/formValidation.min.js"></script>
    <script src="Scripts/FormValidation/bootstrap.min.js"></script>
    <link href="Scripts/FormValidation/formValidation.min.css" rel="stylesheet" />

    <script src="Scripts/System/sysFunction.js"></script>
    <script src="Scripts/CommonScript.js"></script>

    <link href="Styles/_MP_Front.css" rel="stylesheet" />

    <style type="text/css">
        /*@import "bourbon";*/

        body {
            /*font-family: 'Tahoma';
            font-size: 14px;
            font-weight: 300;
            margin: 0 auto;
            min-height: 375px;*/
            background-image: url(Images/bg-main.jpg);
            /*background-position: center center;*/
            background-repeat: no-repeat;
            background-size: cover;
        }

        .wrapper {
            margin-top: 80px;
            margin-bottom: 80px;
        }

        .form-signin {
            max-width: 450px;
            padding: 15px 35px 45px;
            margin: 0 auto;
            background-color: #cfdae2;
            /*border: 1px solid rgba(0,0,0,0.1);*/
            border-radius: 25px;
            background: rgba(212,228,239,1);
            background: -moz-linear-gradient(left, rgba(212,228,239,1) 0%, rgba(134,174,204,0.5) 100%);
            background: -webkit-gradient(left top, right top, color-stop(0%, rgba(212,228,239,1)), color-stop(100%, rgba(134,174,204,0.5)));
            background: -webkit-linear-gradient(left, rgba(212,228,239,1) 0%, rgba(134,174,204,0.5) 100%);
            background: -o-linear-gradient(left, rgba(212,228,239,1) 0%, rgba(134,174,204,0.5) 100%);
            background: -ms-linear-gradient(left, rgba(212,228,239,1) 0%, rgba(134,174,204,0.5) 100%);
            background: linear-gradient(to right, rgba(212,228,239,1) 0%, rgba(134,174,204,0.5) 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#d4e4ef', endColorstr='#86aecc', GradientType=1 );
        }

        .form-signin-heading, .checkbox {
            margin-bottom: 30px;
        }

        .checkbox {
            font-weight: normal;
        }

        .form-control {
            position: relative;
            font-size: 16px;
            height: auto;
            padding: 10px;
        }

        i.form-control-feedback {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="SITE_CONTENT" style="padding-top: 0!important">
        <div class="wrapper">
            <div class="form-signin">
                <img src="Images/ico-pttgc.png" />
                <h3 class="form-signin-heading pt-2" style="color: #fff05a">Corporate Social Responsibility</h3>
                <div id="divLogin">
                    <div class="form-group">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtUsername" name="txtUsername" placeholder="รหัสพนักงาน" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPassword" name="txtPassword" placeholder="รหัสผ่าน" TextMode="Password" autocomplete="off"></asp:TextBox>
                    </div>
                </div>

                <button id="btnLogin" type="button" class="btn btn-outline-primary"><i class="fa fa-sign-in-alt" aria-hidden="true"></i>&nbsp;เข้าสู่ระบบ</button>
                <a class="btn btn-outline-dark" style="float: right;" href="#exampleModalCenter" data-toggle="modal" data-target="#exampleModalCenter"><i class="fa fa-key" aria-hidden="true"></i>&nbsp;ลืมรหัสผ่าน</a>
                <%--<button id="btnCancel" type="button" class="btn btn-outline-dark"><i class="fa fa-minus-circle" aria-hidden="true"></i>&nbspล้างข้อมูล</button>--%>
            </div>
        </div>

        <div id="FOOT">
            <div class="container">
                <div class="footer-area">
                    <div class="footer-pttgc">
                        <div class="footer-pttgc-icon">
                            <img src="Images/ico-pttgc.png" />
                        </div>
                        <div class="footer-pttgc-info">
                            <%--                            <div class="footer-pttgc-copyright">
                                Copyright &copy; 2020,<br />
                                PTT Global Chemical Public Company Limited All rights reserved
                            </div>--%>
                            <div class="footer-pttgc-about">
                                <div class="footer-pttgc-about-title">Copyright &copy; 2020,</div>
                                <div class="footer-pttgc-about-title">PTT Global Chemical Public Company Limited</div>
                                <div class="footer-pttgc-about-address">
                                    555/1 Energy Complex Building A, 14th - 18th Floor, Vibhavadi Rangsit Road, Chatuchak, Chatuchak, Bangkok 10900 Thailand.
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hddUserAD" />
    <asp:HiddenField runat="server" ID="hddPath" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">

    <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header ">
                    <div class="text-center col-12">
                        <h3><i class="fa fa-lock fa-4x"></i></h3>
                        <h2 class="text-center">ลืมรหัสผ่าน?</h2>
                        <p>คุณสามารถรีเซ็ตรหัสผ่านได้ที่นี่</p>
                    </div>

                </div>
                <div id="divForgot" class="modal-body">
                    <div class="form-group row">
                        <label class="col-lg-3 control-label">อีเมล์ <span class="text-red">*</span></label>
                        <div class="col-lg-9" style="padding-bottom: 7px">
                            <div class="input-group">
                                <div class="input-group-append">
                                    <label class="input-group-text"><i class="fa fa-pencil-alt"></i></label>
                                </div>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="40" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-lg-3 control-label">ชื่อผู้ใช้ <span class="text-red">*</span></label>
                        <div class="col-lg-9" style="padding-bottom: 7px">
                            <div class="input-group">
                                <div class="input-group-append">
                                    <label class="input-group-text"><i class="fa fa-pencil-alt"></i></label>
                                </div>
                                <asp:TextBox ID="txtUserNameForgot" runat="server" CssClass="form-control" MaxLength="20" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <button class="btn btn-lg btn-primary btn-block" type="button" onclick="ForgotPassword()" id="btnForgot">ส่งรหัสผ่าน</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- The Modal -->
    </div>

    <script type="text/javascript">
        var $txtUsername = $('input[id$=txtUsername]');
        var $txtPassword = $('input[id$=txtPassword]');

        $(function () {
            $(this).delegate('input:not([type=submit], [type=button])', 'keydown', function (e) {
                if (e.which == 13)  //e.which = keyCode (ENTER : 13)
                    return false;
            });

            $('input[id$=txtPassword]').attr('autocomplete', 'false');

            var UserAD = GetValTextBox('hddUserAD');
            if (UserAD != "") {
                Login('AD');
            }

            SetValidate();
            SetControl();
        });

        function SetValidate() {
            var objValidate = {};
            objValidate[GetElementName("txtUsername", objControl.txtbox)] = addValidate_notEmpty("ระบุ รหัสพนักงาน");
            objValidate[GetElementName("txtPassword", objControl.txtbox)] = addValidate_notEmpty("ระบุ รหัสผ่าน");
            BindValidate("divLogin", objValidate);

            var objValidate = {};
            objValidate[GetElementName("txtEmail", objControl.txtbox)] = addValidateEmail_notEmpty();
            objValidate[GetElementName("txtUserNameForgot", objControl.txtbox)] = addValidate_notEmpty("ระบุ ชื่อผู้ใช้");
            BindValidate("divForgot", objValidate);
        }

        function SetControl() {
            $txtUsername.keydown(function (e) {
                //if press ENTER then SEARCH
                if (e.which == 13) { //keyCode - ENTER : 13
                    $txtPassword.focus();
                    return false;
                }
                else if (e.which == 220) return false; //keyCode - BACKSLASH : 220
            });

            $txtPassword.keydown(function (e) {
                //if press ENTER then SEARCH
                if (e.which == 13) { //keyCode - ENTER : 13
                    $('button[id$=btnLogin]').click();
                    return false;
                }
                else if (e.which == 220) return false; //keyCode - BACKSLASH : 220
            });

            $('button[id$=btnLogin]').click(function () {
                Login('');
                return false;
            });
        }

        function Login(sMode) {
            var UserAD = GetValTextBox('hddUserAD');
            var sUsername = GetValTextBox('txtUsername')
            if ((sMode == "" ? CheckValidate("divLogin") : true)) {
                BlockUI();
                AjaxWebMethod("Login", { 'sUserName': sUsername, 'sPassword': $txtPassword.val(), 'sMode': sMode }, function (response) {
                    if (response.d.Msg == "") {
                        var sPath = GetValTextBox('hddPath');
                        window.location.href = sPath != "" ? sPath : "index.aspx";
                    } else {
                        UnblockUI();
                        DialogWarning(response.d.Msg);
                    }
                });
            }
        }

        function ForgotPassword() {
            if (CheckValidate("divForgot")) {
                BlockUI();
                AjaxWebMethod("ForgetPassword", { 'sEmail': GetValTextBox('txtEmail'), 'sUserName': GetValTextBox('txtUserNameForgot') }, function (data) {
                    UnblockUI();
                    if (data.d.Status == SysProcess.Success) {
                        $('#exampleModalCenter').modal('hide');
                        $('#divForgot input').val('');
                        SetNotValidateTextbox('divForgot', 'txtEmail');
                        SetNotValidateTextbox('divForgot', 'txtUserNameForgot');
                        BBox.Success(AlertTitle.Info, data.d.Content);
                    }
                    else {
                        DialogWarning(data.d.Msg);
                    }
                });
            }
        }
    </script>
</asp:Content>

