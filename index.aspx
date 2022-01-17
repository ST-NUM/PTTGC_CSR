<%@ Page Title="" Language="C#" MasterPageFile="~/_MP_Front.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <style type="text/css">
        .pad-data {
            padding: 10px;
        }

            .pad-data a.lnk-chart {
                display: inline-block;
                width: 21px;
                height: 21px;
                text-align: center;
                color: #666666;
                background-color: #ffffff;
                border-radius: 4px;
            }

                .pad-data a.lnk-chart:before {
                    content: "\f080";
                    font-family: "Font Awesome 5 Free";
                    font-weight: 900;
                    line-height: 21px;
                }

                .pad-data a.lnk-chart:hover {
                    color: #ffffff;
                    background-color: #999999;
                    text-decoration: none;
                }

        .page-header {
            padding-bottom: 9px;
            margin: 40px 0 20px;
            border-bottom: 1px solid #eee;
        }

        .table td, .table th {
            font-size: 14px !important;
            padding: 1px !important;
            color: #ffffff;
        }

        #divDimension, #divDimensionSub, #divTotalUseSub, #divGLSub {
            width: 100%;
            height: 350px;
        }

        h5 {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="card">
        <div class="card-header bg-info1">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </div>
        <div class="card-body">

            <h3 style="margin-top: 0; margin-bottom: 5px; padding-bottom: 5px; border-bottom: 1px solid #eee;">งบประจำปี
                <asp:Label ID="lbYear" runat="server"></asp:Label></h3>
            <div class="small text-right text-muted" style="margin-bottom: 20px;">
                <asp:Label ID="lbUpdate" runat="server"></asp:Label>
            </div>

            <div class="row">

                <%--รวมทั้งหมด--%>
                <div class="col-lg-5">
                    <div class="pad-data pad-primary form-group">
                        <h5 class="page-header" style="margin: 0;">รวมทั้งหมด</h5>
                        <div class="media pt-3">
                            <div class="text-center" style="min-width: 100px;">
                                <div style="font-size: 300%; line-height: 1;"><span id="spOrder"></span></div>
                                Order(s)<br />
                                (<span id="spProject"></span> โครงการ)
                            </div>
                            <div class="media-body">
                                <table id="tbOrder" class="table table-sm">
                                    <tbody>
                                        <tr class="valign-middle">
                                            <th class="border-no-top" colspan="2">Total Budget</th>
                                            <td id="tdBudget" class="border-no-top text-right"></td>
                                            <td class="border-no-top">บาท</td>
                                            <td class="border-no-top"></td>
                                        </tr>
                                        <tr class="valign-middle">
                                            <th>Total Used</th>
                                            <td>
                                                <div id="tdBudgetUsedPer" class="badge pad-warning pull-right"></div>
                                            </td>
                                            <td id="tdBudgetUsed" class="text-right"></td>
                                            <td>บาท</td>
                                            <td class="text-center"><a id="aTotalUsed" class="lnk-chart"></a></td>
                                        </tr>
                                        <tr class="valign-middle">
                                            <th>Available</th>
                                            <td>
                                                <div id="tdAvailablePer" class="badge pad-success pull-right"></div>
                                            </td>
                                            <td id="tdAvailable" class="text-right">120,000,000</td>
                                            <td>บาท</td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <%--GL Total Use--%>
                <div class="col-lg-7">
                    <%--GL Total Use--%>
                    <div class="pad-data pad-warning form-group">
                        <h5 class="page-header" style="margin: 0;">GL Total Used</h5>
                        <div class="table-responsive">
                            <table id="tbGL" class="table table-sm">
                                <thead>
                                    <tr>
                                        <th class="text-center border-right">GL</th>
                                        <th class="text-center border-right" style="width: 22%">Used (บาท)</th>
                                        <th class="text-center border-right" style="width: 22%">Available (บาท)</th>
                                        <td style="width: 5%"></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="valign-middle">
                                        <th colspan="4" class="text-center" style="height: 87px;">ไม่พบข้อมูล</th>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            </div>

            <div class="row">

                <%--Group--%>
                <div class="col-lg-12">
                    <%--Group--%>
                    <div class="pad-data pad-info form-group">
                        <h5 class="page-header" style="margin: 0;">Group</h5>
                        <div class="table-responsive">
                            <table id="tbGroup" class="table">
                                <thead>
                                    <tr>
                                        <th class="border-right">Unit</th>
                                        <th class="text-center border-right" style="width: 15%">Donation</th>
                                        <th class="text-center border-right" style="width: 15%">CSR Expenses</th>
                                        <th class="text-center border-right" style="width: 15%">Advertising & PR</th>
                                        <th class="text-center border-right" style="width: 15%">KVIS</th>
                                        <th class="text-center" style="width: 15%">VISTEC</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="valign-middle">
                                        <th colspan="6" class="text-center" style="height: 87px;">ไม่พบข้อมูล</th>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <%--Dimension Group--%>
                <div class="col-lg-12">
                    <%--Dimation Group--%>
                    <div class="pad-data pad-muted form-group">
                        <h4 class="page-header" style="margin: 0;">Dimension Group</h4>
                        <div id="divDimension" class="table-responsive"></div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <asp:HiddenField ID="hddPermission" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">

    <!-- Modal Graph Dimension -->
    <div id="divModalDimension" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="divModalDimension" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-info">
                    <h5 id="hDTitel" class="modal-title"></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <%--<h5 id="hGL" class="text-right"></h5>--%>
                    <div id="divDimensionSub">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Total Used -->
    <div id="divModalTotalUse" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="divModalTotalUse" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-info">
                    <h5 id="hTTitel" class="modal-title"></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <h5 id="hOrder" class="text-right"></h5>
                    <div id="divTotalUseSub">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal GL -->
    <div id="divModalGL" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="divModalGL" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-info">
                    <h5 class="modal-title">งบประมาณที่ใช้ไป(GL)</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <h5 id="hGL" class="text-right"></h5>
                    <div id="divGLSub">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="Scripts/amcharts_3.20.3/amcharts/amcharts.js"></script>
    <script src="Scripts/amcharts_3.20.3/amcharts/pie.js"></script>
    <script src="Scripts/amcharts_3.20.3/amcharts/serial.js"></script>
    <script src="Scripts/amcharts_3.20.3/amcharts/themes/light.js"></script>

    <script type="text/javascript">
        var $Permission = GetValTextBox('hddPermission');
        var objData = {};

        $(function () {
            if (!isTimeOut) {
                if ($Permission == "N") {
                    PopupNoPermission();
                } else {
                    SetControl();
                    GetData();
                }
            }
        });

        function SetControl() {
            $('#tbGL tbody').delegate('a', 'click', function () {
                var sGLID = $(this).attr('id').replace('a_', '');
                var qGL = Enumerable.From(objData.lstGL).FirstOrDefault(null, '$.sGLID == "' + sGLID + '"');
                if (qGL != null) {
                    $('#divModalGL').modal();
                    BuildBarChart('divGLSub', qGL.lstData);
                    $('#hGL').text(qGL.sName + ' : ' + qGL.sBudget1 + 'บาท');
                }
            });
        }

        function GetData() {
            BlockUI();
            AjaxWebMethod('GetData', {}, function (data) {
                UnblockUI();
                if (data.d == SysProcess.SessionExpired) { PopupSessionTimeOut(); } else {
                    objData = data.d;
                    SetData();
                }
            }, function () {
                UnblockUI();
                SetTooltip();
            });
        }

        function SetData() {
            //รวมทั้งหมด
            $('#spOrder').text(objData.nOrder);
            $('#spProject').text(objData.nProject);

            $('#tdBudget').text(objData.sBudget);
            $('#tdBudgetUsed').text(objData.sBudgetUsed);
            $('#tdAvailable').text(objData.sAvailable);
            $('#tdBudgetUsedPer').text(objData.sBudgetUsedPer + '%');
            $('#tdAvailablePer').text(objData.sAvailablePer + '%');

            $('#aTotalUsed').click(function () {
                $('#hTTitel').text('งบประมาณที่ใช้ไป(Order)');
                $('#hOrder').text('งบที่ใช้ไป : ' + objData.sBudgetUsed + 'บาท');
                $('#divModalTotalUse').modal();
                BuildBarChart('divTotalUseSub', objData.lstOrder);
            });

            //GL Total Used
            var lstGL = objData.lstGL;
            if (lstGL.length > 0) {
                $('#tbGL tbody tr').remove();
                $.each(lstGL, function (i, el) {
                    var td = '<th class="border-right">' + el.sName + '</th>';
                    td += '<td class="text-right border-right">' + el.sBudget1 + '</td>';
                    td += '<td class="text-right border-right">' + el.sBudget2 + '</td>';
                    td += '<td class="text-center"><a id="a_' + el.sGLID + '" class="lnk-chart"></a></td>';

                    $('#tbGL tbody').append('<tr class="valign-middle">' + td + '</tr>');
                });
            }

            //Group
            var lstHeadGroup = objData.lstHeadGroup;
            if (lstHeadGroup.length > 0) {
                $('#tbGroup thead tr th:not(:first)').each(function (i, el) {
                    $(el).text(lstHeadGroup[i]);
                });
            }

            var lstGroup = objData.lstGroup;
            if (lstGroup.length > 0) {
                $('#tbGroup tbody tr').remove();
                $.each(lstGroup, function (i, el) {
                    var td = '<th class="text-nowrap border-right">' + el.sName + '</th>';
                    var nCountBudget = el.lstBudget.length;
                    if (nCountBudget > 0) {
                        $.each(el.lstBudget, function (ii, ell) {
                            td += '<td class="text-right ' + (nCountBudget != (ii + 1) ? 'border-right' : '') + '">' + ell + '</td>';
                        });
                    }
                    $('#tbGroup tbody').append('<tr class="valign-middle">' + td + '</tr>');
                });
            }

            //Dimension Group
            if (objData.lstDimension.length > 0) {
                BuildPieChart('divDimension', objData.lstDimension, '', 'sName', 'nValue', '');
            } else {
                $('#divDimension').css('height', '80px').append('<div id="divNoData" class="dataNotFound">ไม่พบข้อมูล</div>');
            }
        }

        function BuildBarChart(dvChartID, datasource) {
            var chart = AmCharts.makeChart(dvChartID, {
                "type": "serial",
                "theme": "light",
                "dataProvider": datasource,
                //"valueAxes": [{
                //    "title": "All Order"
                //}],
                "graphs": [{
                    "balloonText": "[[category]] : [[value]]บาท",
                    "fillAlphas": 1,
                    "lineAlpha": 0.2,
                    "title": "nValue",
                    "type": "column",
                    "valueField": "nValue"
                }],
                //"angle": 30,
                //"rotate": true,
                "categoryField": "month",
                "categoryAxis": {
                    "gridPosition": "start",
                    "fillAlpha": 0.05,
                    "position": "left"
                }
            });
        }

        //Pie Chart
        function BuildPieChart(dvChartID, datasource, chartTitle, titleField, valueField, colorField) {
            var chart = AmCharts.makeChart(dvChartID, {
                "type": "pie",
                "theme": "light",
                "startDuration": 1,
                "addClassNames": true,
                "radius": "42%",
                "innerRadius": "40%",
                "labelRadius": 20,
                "labelText": '[[title]]: [[percents]]% ([[value]])',
                "dataProvider": datasource,
                "valueField": valueField,
                "titleField": titleField,
                "colorField": colorField,
                "titles": [{ text: chartTitle, size: 12 }],
                "marginTop": 0,
                "marginBottom": 20,
                "labelsEnabled": true,
                //"legend": {},
                "listeners": [{
                    event: 'clickSlice',
                    method: function (event) {
                        event.chart.validateData();
                        var lstDMSub = event.dataItem.dataContext.lstDimensionSub;
                        if (lstDMSub.length > 0) {
                            var nDimensionID = event.dataItem.dataContext.nDimensionID;
                            $('#hDTitel').text(event.dataItem.dataContext.sName)
                            $("#divModalDimension").modal();

                            BuildPieChartSub('divDimensionSub', lstDMSub, '', 'sName', 'nValue', '');
                        }
                    }
                }]
            });
        }

        function BuildPieChartSub(dvChartID, datasource, chartTitle, titleField, valueField, colorField) {
            var chart = AmCharts.makeChart(dvChartID, {
                "type": "pie",
                "theme": "light",
                "startDuration": 1,
                "addClassNames": true,
                "radius": "42%",
                "innerRadius": "40%",
                "labelRadius": 10,
                "labelText": '[[title]]: [[percents]]% ([[value]])',
                "dataProvider": datasource,
                "valueField": valueField,
                "titleField": titleField,
                //"colorField": colorField,
                "titles": [{ text: chartTitle, size: 12 }],
                //"marginTop": 50,
                "marginBottom": 50,
                "maxLabelWidth": 300,
                "labelsEnabled": true,
                "listeners": [{
                    event: 'clickSlice',
                    method: function (event) {
                        event.chart.validateData();
                    }
                }],
            });
        }

    </script>
</asp:Content>
