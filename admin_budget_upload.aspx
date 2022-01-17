<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="admin_budget_upload.aspx.cs" Inherits="admin_budget_upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <div class="form-group row">
                <div class="col-lg-12">
                    <div class="float-right"><a href="UploadFiles/Template_Import_Budget.xlsx" class="btn btn-info">ดาวน์โหลดไฟล์ Template</a></div>
                </div>
            </div>

            <div id="divShowFile" class="form-group row">
                <label class="col-lg-2 col-form-label">นำเข้า Template</label>
                <div class="col-lg-5">
                    <div id="divFile">
                        <input type="file" name="files" id="txtFile" accept="application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                        <span class="text-muted">* ขนาดไฟล์ไม่เกิน 10MB., นามสกุล: .xls .xlsx</span>
                    </div>
                </div>
                <div id="divFileBtn" class="col-lg-auto" style="display: none;">
                    <button id="btnViewFile" type="button" class="btn btn-info" title="ดาวน์โหลด"><span class="glyphicon glyphicon-zoom-in"></span></button>
                    <button id="btnDelFile" type="button" class="btn btn-danger" title="ลบ" data-toggle="tooltip"><span class="fa fa-trash"></span></button>
                </div>
                <div id="divBtnFile" class="col-lg-auto"></div>
            </div>

            <div id="divError" class="table-responsive pb-3" style="display: none;">
                <table id="tbDataError" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 8%" class="text-center">แถวที่</th>
                            <th class="text-center">รายละเอียด</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>

            <div class="table-responsive">
                <table id="tbData" class="table table-bordered table-hover table-responsive-sm table-responsive-md">
                    <thead>
                        <tr class="valign-middle pad-primary">
                            <th style="width: 8%" class="text-center">ที่</th>
                            <th class="text-center" data-sort="sFileName">ชื่อไฟล์</th>
                            <th style="width: 20%" class="text-center" data-sort="nRow">จำนวนข้อมูล(รายการ)</th>
                            <th style="width: 15%" class="text-center" data-sort="sUpdateDate">วันที่นำเข้า</th>
                            <th style="width: 15%" class="text-center" data-sort="sUpdateBy">ผู้นำเข้า</th>
                            <th style="width: 8%"></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div id="divNoData" class="dataNotFound">ไม่พบข้อมูล</div>
            </div>

            <div id="divPaging" class="form-row align-items-center pt-3">
                <div class="col-lg-2 mb-3">
                    <%--<button type="button" id="btnDel" class="btn btn-danger" title="ลบ"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp; ลบ</button>--%>
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
    <asp:HiddenField ID="hddUserID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var sPageEdit = "admin_budget_upload_edit.aspx";
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
                    $('#btnSave,#divShowFile,#btnDel').remove();
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
                case 'sFileName':
                case 'nRow':
                case 'sUpdateBy':
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

            sHTML += "<td class='text-center'>" + nRowNo + ".</td>";
            sHTML += "<td>" + objData.sFileName + "</td>";
            sHTML += "<td class='text-center'>" + objData.nRow + "</td>";
            sHTML += "<td class='text-center'>" + objData.sUpdateDate + "</td>";
            sHTML += "<td>" + objData.sUpdateBy + "</td>";

            sHTML += '<td class="text-center" valign="top">' +
                    '<a class="btn btn-sm btn-outline-info"  href="' + sPageEdit + '?str=' + objData.sIDEncrypt + '" title="' + ($Permission == "A" ? 'แก้ไข' : 'ดูรายละเอียด') + '">' +
                    '<i class="fa fa-' + ($Permission == "A" ? "edit" : "eye") + '"></i>&nbsp;' +
                    '</a>' +
                    '</td>';

            var tr = "<tr>" + sHTML + "</tr>";
            return tr;
        }

        function OnDataBound() {
            CheckDataFound();
            SortingEvent();
        }

        //#region File    
        var sUserID = GetValTextBox('hddUserID');
        var urlAshx = "Ashx/Fileuploader.ashx";

        var tbData_File = $("table[id$=tbData_File] tbody");
        var arrData_Error = [];
        var obj_File = {};
        var arrTypeFile = ['xls', 'xlsx'];
        var IsPassFile = true;

        $(function () {
            $('#btnDelFile').click(function () {
                arrData_Error = [];
                obj_File = {};
                $('#divFileBtn').hide();
                $('#divError').hide();
                $("input[id$=txtFile]").parent().removeClass('fileuploader-disabled');
                return false;
            });

            var filupload1 = $('input[id$="txtFile"]').fileuploader({
                fileMaxSize: 10,
                enableApi: true,
                thumbnails: false,
                extensions: arrTypeFile,
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
                        //if (item.size > 10 * 1000000) {
                        //    IsPassFile = false;
                        //    DialogWarning('ไฟล์ ' + item.name + ' ขนาดเกิน 10MB. <br/>กรุณาเลือกไฟล์ที่มีขนาดไม่เกิน 10MB.');
                        //} else if (arrTypeFile.indexOf(item.extension) == -1) {
                        //    IsPassFile = false;
                        //    DialogWarning('รองรับนามสกุลไฟล์ .xls .xlsx เท่านั้น');
                        //} else {
                        //    IsPassFile = true;
                        //}

                        apiFile1.disable($('input[id$="txtFile"]'))
                    },

                    // Callback fired if the uploading succeeds
                    // by default we will add a success icon and fadeOut the progressbar
                    // Remember that if you want so show the PHP errors, you will need to process them also here. To prevent it you will need to respond on the upload url with error code in header.
                    onSuccess: function (data, item, listEl, parentEl, newInputEl, inputEl, textStatus, jqXHR) {
                        //if (IsPassFile) {
                        obj_File = {
                            sPath: data.SaveToPath,
                            sSysFileName: data.SaveToFileName,
                            sFileName: data.FileName
                        };
                        CheckFile();
                        //}
                        RemoveFile(item);
                    },

                    // Callback fired after all files were uploaded
                    onComplete: function (listEl, parentEl, newInputEl, inputEl, jqXHR, textStatus) {
                        //obj_File = apiFile1.getFiles();
                        apiFile1.enable();
                    }
                }
            });

            $(".fileuploader-input-button").html("<span>เลือกไฟล์<span>");
            $('.fileuploader-input-caption').text('เลือกไฟล์เพื่ออัพโหลด')

            var apiFile1 = $.fileuploader.getInstance(filupload1);

            function RemoveFile(item) {
                apiFile1.remove(item);
            }
        });

        function BindFile() {
            $('#divFileBtn').show();
            var sFileURL = obj_File.sPath.replace("../", "") + 'Error_' + obj_File.sSysFileName;
            var onclick = "FancyBox_ViewFile('" + sFileURL + "')";
            $('#btnViewFile').attr('onclick', onclick);
            $('#btnViewFile').text('').append('<span class="glyphicon glyphicon-zoom-in"></span> ' + Sub_string(obj_File.sFileName, 20));

            //$('#divFileBtn').addClass('padT10');

            $("input[id$=txtFile]").parent().addClass('fileuploader-disabled');
        }

        function CheckFile() {
            BlockUI();

            AjaxWebMethod("CheckFile", { 'sPath': obj_File.sPath, 'sSysFileName': obj_File.sSysFileName, 'sFileName': obj_File.sFileName }, function (data) {
                if (data.d.Status == SysProcess.SessionExpired) {
                    PopupSessionTimeOut();
                } else if (data.d.Status == SysProcess.Failed) {
                    UnblockUI();
                    arrData_Error = data.d.lstData;
                    BindFile();
                    BindError();
                    $("input[id$=txtFile]").parent().addClass('fileuploader-disabled');
                    DialogWarning(data.d.Msg);
                } else {
                    UnblockUI();
                    debugger
                    if (data.d.IsOrderNotMatch) {
                        arrData_Error = data.d.lstData;
                        BindFile();
                        BindError();
                        $("input[id$=txtFile]").parent().addClass('fileuploader-disabled');
                    } else {
                        obj_File = {};
                        arrData_Error = [];
                        $("input[id$=txtFile]").parent().removeClass('fileuploader-disabled');
                    }

                    DialogSucess();
                    SearchData();
                }
            }, function () { UnblockUI(); });
        }

        function BindError() {
            var lstError = Enumerable.From(arrData_Error).Where('$.IsPassMatch == false').ToArray();
            if (lstError.length > 0) {
                $('#divError').show();
                $('#tbDataError tbody tr').remove();

                $.each(lstError, function (i, el) {
                    var td = '<td class="text-center">' + el.nRow + '</td>';
                    td += '<td>' + el.sMsg + '</td>';
                    $('#tbDataError tbody').append('<tr>' + td + '</tr>');
                });

            } else {
                $('#divError').hide();
            }
        }

        //#endregion Tab File 
    </script>
</asp:Content>

