using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Web.SessionState;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mail;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections;
using System.Net.Mail;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Web.Configuration;

public class CommonFunction
{
    public CommonFunction() { }
    // Common Function

    public static string conn = WebConfigurationManager.ConnectionStrings["PTTGC_CSR_ConnectionString"].ConnectionString.ToString();
    public static string connBE = WebConfigurationManager.ConnectionStrings["ConnectionString_BE"].ConnectionString.ToString();

    public static DataTable ListToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        //Get all the properties
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name);
        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                //inserting property values to datatable rows
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        //put a breakpoint here and check datatable
        return dataTable;
    }

    public static string Get_Value(string _sql)
    {
        using (SqlConnection _conn = new SqlConnection(conn))
        {
            DataTable _dt = new DataTable();
            _conn.Open();
            new SqlDataAdapter(_sql, _conn).Fill(_dt);
            if (_dt.Rows.Count >= 1)
            {
                string _return = _dt.Rows[0][0].ToString();
                _dt.Dispose();
                return _return;
            }
            return "";
        }
    }

    public static string Get_Value(string _sconn, string _sql)
    {
        DataTable _dt = new DataTable();
        SqlConnection _conn = new SqlConnection(_sconn);
        if (_conn.State == ConnectionState.Closed) _conn.Open();
        new SqlDataAdapter(_sql, _conn).Fill(_dt);
        if (_dt.Rows.Count >= 1)
        {
            string _return = _dt.Rows[0][0].ToString();
            _dt.Dispose();
            return _return;
        }
        return "";
    }

    public static void ListDBToCheckBoxList(SqlConnection _conn, CheckBoxList _cbl, string _sql, string _label, string _datavalue, string _datatext)
    {
        DataTable _dt = CommonFunction.Get_Data(_conn, _sql);
        if (_dt.Rows.Count > 0)
        {
            _cbl.Items.Clear();
            _cbl.DataSource = _dt;
            _cbl.DataValueField = _datavalue;
            _cbl.DataTextField = _datatext;
            _cbl.DataBind();
            if (!_label.Equals("")) _cbl.Items.Insert(0, new ListItem(_label, ""));
        }
        else
        {
            _cbl.Items.Clear();
            if (!_label.Equals("")) _cbl.Items.Insert(0, new ListItem(_label, ""));
        }
    }
    public static void ListDBToRadioButtonList(SqlConnection _conn, RadioButtonList _cbl, string _sql, string _label, string _datavalue, string _datatext)
    {
        DataTable _dt = CommonFunction.Get_Data(_conn, _sql);
        if (_dt.Rows.Count > 0)
        {
            _cbl.Items.Clear();
            _cbl.DataSource = _dt;
            _cbl.DataValueField = _datavalue;
            _cbl.DataTextField = _datatext;
            _cbl.DataBind();
            if (!_label.Equals("")) _cbl.Items.Insert(0, new ListItem(_label, ""));
            _cbl.SelectedIndex = 0;
        }
        else
        {
            _cbl.Items.Clear();
            if (!_label.Equals("")) _cbl.Items.Insert(0, new ListItem(_label, ""));
        }
    }
    public static string Get_Value(SqlConnection _conn, string _sql)
    {
        DataTable _dt = new DataTable();
        new SqlDataAdapter(_sql, _conn).Fill(_dt);
        if (_dt.Rows.Count >= 1)
        {
            string _return = _dt.Rows[0][0].ToString();
            _dt.Dispose();
            return _return;
        }
        return "";
    }
    public static string LastDayOfMonth(int _month, int _year)
    {
        DateTime _date = new DateTime(_year, _month, 1);
        _date.AddDays(-1);
        return _date.Day.ToString();
    }
    public static DataTable Get_Data(string Conn, string _sql)
    {
        using (SqlConnection _conn = new SqlConnection(Conn))
        {
            DataTable _dt = new DataTable();
            _conn.Open();
            new SqlDataAdapter(_sql, _conn).Fill(_dt);

            return _dt;
        }
    }
    public static DataTable Get_Data(SqlConnection _conn, string _sortby, string _order, string _field, string _table, string _groupby, string _cond, int _pagesize, int _pageindex)
    {
        DataTable _dt = new DataTable();
        if (_conn.State == ConnectionState.Closed) _conn.Open();
        SqlCommand cmd = new SqlCommand("OpenDataByRang", _conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@SelectedPage", _pageindex);
        cmd.Parameters.AddWithValue("@PageSize", _pagesize);
        cmd.Parameters.AddWithValue("@BY", _order);
        cmd.Parameters.AddWithValue("@Order", _sortby);
        cmd.Parameters.AddWithValue("@Filed", _field);
        cmd.Parameters.AddWithValue("@Table", _table);
        cmd.Parameters.AddWithValue("@GroupBy", _groupby);
        cmd.Parameters.AddWithValue("@Condition", _cond);

        SqlDataReader dr = cmd.ExecuteReader();
        //new SqlDataAdapter(_sql, _conn).Fill(_dt);
        _dt.Load(dr);
        return _dt;
    }
    /// <summary>
    /// ฟังชั่นดึงข้อมูล
    /// </summary>
    /// <param name="Conn">SqlConnection</param>
    /// <param name="_sql">Query</param>
    /// <returns>DataTable</returns>
    public static DataTable Get_Data(SqlConnection Conn, string _sql)
    {

        DataTable _dt = new DataTable();
        if (Conn.State == ConnectionState.Closed) Conn.Open();
        new SqlDataAdapter(_sql, Conn).Fill(_dt);
        return _dt;

    }
    public static string FilterString(string _str, char[] _pattern)
    {
        string sresl = "";
        string[] _arkeyword = _str.Split(_pattern);
        foreach (string _keyword in _arkeyword)
        {
            if (_keyword.Trim().Equals("") || !sresl.IndexOf(", " + _keyword.Trim()).Equals(-1)) continue;
            sresl += ", " + _keyword.Trim();
        }
        if (!sresl.Equals("")) sresl = sresl.Remove(0, 2);
        return sresl;
    }
    public static string ExtensionToImages(string _extension)
    {
        string _imgofext = "";
        string[] _arext = new string[] { ".accdb", ".mdb", ".pdf", ".xlsx", ".xls", ".pptx", ".ppt", ".docx", ".doc", ".txt", ".zip", ".jpg", ".jpeg", ".bmp", ".gif" };
        string[] _arimg = new string[] { "access2007.gif", "access.gif", "acrobat.gif", "excel2007.gif", "excel.gif", "powerpoint2007.gif", "powerpoint.gif", "word2007.gif", "word.gif", "text.gif", "zip.gif", "image.gif", "image.gif", "image.gif", "image.gif" };
        sbyte _si = (sbyte)Array.IndexOf<string>(_arext, _extension);
        if (!_si.Equals(-1)) _imgofext = "images/iconfiletypewindow/" + _arimg[_si]; else _imgofext = "images/iconfiletypewindow/binary.gif";
        return _imgofext;
    }
    public static string InsertIPHostToText(Page _page, string _originaltext)
    {
        ListItemCollection _artagimgsrc = CommonFunction.ListTagimgsrc(_originaltext);
        foreach (ListItem _imgsrc in _artagimgsrc)
        {
            if (File.Exists(_page.Server.MapPath(_imgsrc.Value)))
            {
                _originaltext = _originaltext.Replace(_imgsrc.Value, "http://" + _page.Request.ServerVariables["LOCAL_ADDR"] + _page.Request.ApplicationPath + "/" + _imgsrc.Value);
            }
        }
        return _originaltext;
    }
    public static ListItemCollection ListTagimgsrc(string _originaltext)
    {
        int _ix = 0, _i = 0, _f = 0;
        ListItemCollection _lt = new ListItemCollection();
        while (true)
        {
            _i = _originaltext.IndexOf("<img", _ix);
            if (!_i.Equals(-1))
            {
                _f = _originaltext.IndexOf("/>", _i);
                if (!_f.Equals(-1))
                {
                    _ix = _f;
                    _i = _originaltext.IndexOf("src=\"", _i) + "src=\"".Length;
                    _f = _originaltext.IndexOf("\"", _i);
                    _lt.Add(_originaltext.Substring(_i, _f - _i));
                }
            }
            else break;
        }
        return _lt;
    }
    public static void UpdateImageSortColumnHeaders(DataGrid _dgd, ref Label _lfsort, string _lsort)
    {
        // กรณีที่ _lfsort เป็นค่าว่าง(ตอนเข้ามาครั้งแรกที่ยังไม่ได้กด sort ให้ default column แรก)
        if (_lfsort.Text.Equals("")) return;// _lfsort.Text = _dgd.Columns[0].SortExpression;
        //
        foreach (DataGridColumn _dgc in _dgd.Columns)
        {
            _dgc.HeaderText = Regex.Replace(_dgc.HeaderText, "\\s<.*>", String.Empty);  //Clear any <img> tags that might be present
        }
        foreach (DataGridColumn _dgc in _dgd.Columns)
        {
            if (_dgc.SortExpression.Equals(_lfsort.Text))
            {
                if (!_lsort.Equals("0"))
                {
                    _dgc.HeaderText += " <img src='../images/up.gif' border='0' width='11' height='7'>";
                }
                else
                {
                    _dgc.HeaderText += " <img src='../images/down.gif' border='0' width='11' height='7'>";
                }
                break;
            }
        }
    }
    public static void ClearControls(Control control)
    {
        for (int i = control.Controls.Count - 1; i >= 0; i--)
        {
            ClearControls(control.Controls[i]);
        }
        if (!(control is TableCell))
        {
            if (control.GetType().GetProperty("SelectedItem") != null)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                try
                {
                    literal.Text = (string)control.GetType().GetProperty("SelectedItem").GetValue(control, null);
                }
                catch
                {
                }
                control.Parent.Controls.Remove(control);
            }
            else
                if (control.GetType().GetProperty("Text") != null)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                literal.Text = (string)control.GetType().GetProperty("Text").GetValue(control, null);
                control.Parent.Controls.Remove(control);
            }
        }
        return;
    }
    public static string GetMonthName(string _nmonth)
    {
        string _monthname = "";
        switch (_nmonth.PadLeft(2, '0'))
        {
            case "01":
                _monthname = "มกราคม";
                break;
            case "02":
                _monthname = "กุมภาพันธ์";
                break;
            case "03":
                _monthname = "มีนาคม";
                break;
            case "04":
                _monthname = "เมษายน";
                break;
            case "05":
                _monthname = "พฤษภาคม";
                break;
            case "06":
                _monthname = "มิถุนายน";
                break;
            case "07":
                _monthname = "กรกฎาคม";
                break;
            case "08":
                _monthname = "สิงหาคม";
                break;
            case "09":
                _monthname = "กันยายน";
                break;
            case "10":
                _monthname = "ตุลาคม";
                break;
            case "11":
                _monthname = "พฤศจิกายน";
                break;
            case "12":
                _monthname = "ธันวาคม";
                break;
        }
        return _monthname;
    }
    public static string GetMonthShotName(string _nmonth)
    {
        string _monthname = "";
        switch (_nmonth.PadLeft(2, '0'))
        {
            case "01":
                _monthname = "ม.ค.";
                break;
            case "02":
                _monthname = "ก.พ.";
                break;
            case "03":
                _monthname = "มี.ค.";
                break;
            case "04":
                _monthname = "เม.ย.";
                break;
            case "05":
                _monthname = "พ.ค.";
                break;
            case "06":
                _monthname = "มิ.ย.";
                break;
            case "07":
                _monthname = "ก.ค.";
                break;
            case "08":
                _monthname = "ส.ค.";
                break;
            case "09":
                _monthname = "ก.ย.";
                break;
            case "10":
                _monthname = "ต.ค.";
                break;
            case "11":
                _monthname = "พ.ย.";
                break;
            case "12":
                _monthname = "ธ.ค.";
                break;
        }
        return _monthname;
    }
    public static sbyte FindIndexColumnOfDataFieldInDataTable(DataTable _dt, string _fieldname)
    {
        sbyte _i = 0; bool _c = false;
        for (; _i < _dt.Columns.Count; _i++)
        {
            if (_dt.Columns[_i].ColumnName.Equals(_fieldname)) { _c = true; break; }
        }
        if (!_c) _i = -1;
        return _i;
    }
    public static sbyte FindIndexColumnOfDataFieldInGrid(DataGrid _dgd, string _fieldname)
    {
        sbyte _i = 0; bool _c = false;
        for (; _i < _dgd.Columns.Count; _i++)
        {
            // ให้ข้ามฟิลที่มีชนิดเป็น TemplateColumn
            if (_dgd.Columns[_i].GetType().ToString().Equals("System.Web.UI.WebControls.TemplateColumn")) continue;
            //
            if (((BoundColumn)_dgd.Columns[_i]).DataField.Equals(_fieldname)) { _c = true; break; }
        }
        if (!_c) _i = -1;
        return _i;
    }
    public static void ListNumberPage(DropDownList _ddl, short _npage, short _ipage)
    {
        _ddl.Items.Clear();
        for (short _i = 0; _i < _npage; _i++)
        {
            _ddl.Items.Add(new ListItem("" + (_i + 1), "" + _i));
        }
        _ddl.SelectedValue = "" + _ipage;
    }
    public static void ListNumberPage(DropDownList _ddl, ushort _npage, ushort _ipage, string _format)
    {
        _ddl.Items.Clear();
        for (ushort _i = 0; _i < _npage; _i++)
        {
            _ddl.Items.Add(new ListItem(String.Format(_format, (_i + 1), _npage), "" + _i));
        }
        _ddl.SelectedValue = "" + _ipage;
    }
    public static void ListYears(DropDownList _ddl, string _label)
    {
        _ddl.Items.Clear();
        if (!_label.Equals("")) _ddl.Items.Add(new ListItem(_label, ""));
        for (int j = System.DateTime.Today.Year; j >= 2005; j--)
        {
            _ddl.Items.Add(new ListItem("" + (j + 543), "" + j));
        }
        _ddl.SelectedValue = "" + DateTime.Today.Year;
    }
    public static void ListDay(DropDownList _ddlday, string _month, string _year, string _label)
    {
        _ddlday.Items.Clear();
        if (_month.Equals("") || _year.Equals("")) return;

        short _dayofmonth = Convert.ToInt16(DateTime.DaysInMonth(Convert.ToInt16(_year), Convert.ToInt16(_month)));
        if (!_label.Equals("")) _ddlday.Items.Add(new ListItem(_label, ""));
        for (short _i = 1; _i <= _dayofmonth; _i++)
        {
            _ddlday.Items.Add(new ListItem("" + _i, "" + _i.ToString("00")));
        }
        if (_dayofmonth >= DateTime.Now.Day)
        {
            // วันปัจจุบันน้อยกว่าวันสุดท้ายของเดือนที่เลือก
            _ddlday.SelectedValue = DateTime.Now.Day.ToString("00");
        }
        else
        {
            // วันปัจจุบันมากกว่าวันสุดท้ายของเดือนที่เลือก
            _ddlday.SelectedValue = _dayofmonth.ToString("00");
        }
    }
    public static void ListWeekOfYear(DropDownList _ddlweek, string _month, string _year, string _label)
    {
        _ddlweek.Items.Clear();
        if (_month.Equals("") || _year.Equals("")) return;

        // วันที่1 ของเดือนปัจจุบัน
        DateTime _inow = new DateTime(Convert.ToInt16(_year), Convert.ToInt16(_month), 1);
        // วันสุดท้าย ของเดือนปัจจุบัน(เดือนบวก1 เพื่อที่จะลบ 1วัน หาวันสุกท้ายของเดือนปัจจุบัน)
        DateTime _fnow = new DateTime(Convert.ToInt16(_year), Convert.ToInt16(_month) + 1, 1).AddDays(-1);

        GregorianCalendar _cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
        short _iweek = (short)_cal.GetWeekOfYear(_inow, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        short _fweek = (short)_cal.GetWeekOfYear(_fnow, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        short _lweek = 1;

        if (!_label.Equals("")) _ddlweek.Items.Add(new ListItem(_label, ""));

        while (_fweek >= _iweek)
        {
            _ddlweek.Items.Add(new ListItem("" + _lweek++, "" + _iweek++));
        }
        // default current week
        if (DateTime.Now.ToString("MM").Equals(_month) && DateTime.Now.ToString("yyyy", new CultureInfo("en-US")).Equals(_year))
        {
            // เลือกวันเดือนปีปัจจุบัน
            _ddlweek.SelectedValue = "" + _cal.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        else
        {
            // เลือกวันเดือนปีอื่นๆ
            _ddlweek.SelectedValue = "" + _cal.GetWeekOfYear(_inow, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
    public static void ListWeek(DropDownList _ddlweek, string _month, string _year, string _label)
    {
        _ddlweek.Items.Clear();
        if (_month.Equals("") || _year.Equals("")) { _ddlweek.Items.Add(new ListItem(_label, "")); return; }
        // เก็บช่วงวันสัปดาห์ปัจจุบันและช่วงอื่นๆ
        string _nweek = "", _oweek = "";
        // คำนวนสัปดาห์(วันเริ่ม-วันสิ้นสุด) ref[http://stackoverflow.com/questions/672906/weeks-in-c]
        // วันที่1 ของเดือนปีปัจจุบัน
        DateTime _now = new DateTime(Convert.ToInt16(_year), Convert.ToInt16(_month), 1);
        // วันที่แรกของสัปดาห์แรกของเดือนปีปัจจุบัน
        DateTime _fdweek = _now.AddDays(-(_now.DayOfWeek - DayOfWeek.Monday));
        // วันที่สุดท้ายของสัปดาห์แรกของเดือนปีปัจจุบัน
        DateTime _edweek = _fdweek.AddDays(7).Date.AddMilliseconds(-1);
        // เก็บวันที่ช่วงสัปดาห์แรกของเดือนปีที่เลือก
        _oweek = "" + _fdweek.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "-" + _edweek.AddDays(1).ToString("MM/dd/yyyy", new CultureInfo("en-US"));
        // เริ่มที่สัปดาห์แรก
        short _lweek = 1;
        // เพิ่มใน Dropdownlist itemแรก
        if (!_label.Equals("")) _ddlweek.Items.Add(new ListItem(_label, ""));
        _ddlweek.Items.Add(new ListItem("สัปดาห์ที่ " + _lweek++ + " (" + _fdweek.ToString("dd/MM/yyyy") + "-" + _edweek.ToString("dd/MM/yyyy") + ")", "" + _fdweek.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "-" + _edweek.AddDays(1).ToString("MM/dd/yyyy", new CultureInfo("en-US"))));
        // เช็คว่าวันปัจจุบันอยู่ในสัปดาห์นั้นๆมะ
        if (DateTime.Compare(DateTime.Now, _fdweek) > 0 && DateTime.Compare(DateTime.Now, _edweek) < 0) _nweek = "" + _fdweek.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "-" + _edweek.AddDays(1).ToString("MM/dd/yyyy", new CultureInfo("en-US"));
        //
        while (_fdweek.AddDays(7).Month == _now.Month || _edweek.AddDays(7).Month == _now.Month)
        {
            _fdweek = _fdweek.AddDays(7);
            _edweek = _fdweek.AddDays(7).Date.AddMilliseconds(-1);
            _ddlweek.Items.Add(new ListItem("สัปดาห์ที่ " + _lweek++ + " (" + _fdweek.ToString("dd/MM/yyyy") + "-" + _edweek.ToString("dd/MM/yyyy") + ")", "" + _fdweek.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "-" + _edweek.AddDays(1).ToString("MM/dd/yyyy", new CultureInfo("en-US"))));
            // เช็คว่าวันปัจจุบันอยู่ในสัปดาห์นั้นๆมะ
            if (DateTime.Compare(DateTime.Now, _fdweek) > 0 && DateTime.Compare(DateTime.Now, _edweek) < 0) _nweek = "" + _fdweek.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "-" + _edweek.AddDays(1).ToString("MM/dd/yyyy", new CultureInfo("en-US"));
        }

        // default current week
        if (!_nweek.Equals(""))
        {
            // เซตช่วงสัปดาห์ปัจจุบันเมื่อเลือกวันเดือนปีปัจจุบัน
            _ddlweek.SelectedValue = _nweek;
        }
        else
        {
            // เซตช่วงสัปดาห์แรกเมื่อเลือกวันเดือนปีอื่นๆ
            _ddlweek.SelectedValue = _oweek;
        }
    }
    public static string CurrentWeek(DateTime _ndate)
    {
        // เก็บช่วงวันสัปดาห์ปัจจุบัน
        string _nweek = "";
        // คำนวนสัปดาห์(วันเริ่ม-วันสิ้นสุด) ref[http://stackoverflow.com/questions/672906/weeks-in-c]
        // วันที่1 ของเดือนปีปัจจุบัน
        DateTime _now = _ndate;
        // วันที่แรกของสัปดาห์แรกของเดือนปีปัจจุบัน
        DateTime _fdweek = _now.AddDays(-(_now.DayOfWeek - DayOfWeek.Monday));
        // วันที่สุดท้ายของสัปดาห์แรกของเดือนปีปัจจุบัน
        DateTime _edweek = _fdweek.AddDays(7).Date.AddMilliseconds(-1);
        // เก็บวันที่ช่วงสัปดาห์แรกของเดือนปีที่เลือก
        _nweek = "" + _fdweek.ToString("MM/dd/yyyy", new CultureInfo("en-US")) + "-" + _edweek.AddDays(1).ToString("MM/dd/yyyy", new CultureInfo("en-US"));

        return _nweek;
    }
    public static void ListMonth(DropDownList _ddlmonth, string _label)
    {
        _ddlmonth.Items.Clear();
        if (!_label.Equals("")) _ddlmonth.Items.Add(new ListItem(_label, ""));
        _ddlmonth.Items.Add(new ListItem("มกราคม", "1"));
        _ddlmonth.Items.Add(new ListItem("กุมภาพันธ์", "2"));
        _ddlmonth.Items.Add(new ListItem("มีนาคม", "3"));
        _ddlmonth.Items.Add(new ListItem("เมษายน", "4"));
        _ddlmonth.Items.Add(new ListItem("พฤษภาคม", "5"));
        _ddlmonth.Items.Add(new ListItem("มิถุนายน", "6"));
        _ddlmonth.Items.Add(new ListItem("กรกฎาคม", "7"));
        _ddlmonth.Items.Add(new ListItem("สิงหาคม", "8"));
        _ddlmonth.Items.Add(new ListItem("กันยายน", "9"));
        _ddlmonth.Items.Add(new ListItem("ตุลาคม", "10"));
        _ddlmonth.Items.Add(new ListItem("พฤศจิกายน", "11"));
        _ddlmonth.Items.Add(new ListItem("ธันวาคม", "12"));
    }
    public static void ListDays(DropDownList _ddlday, string _month, string _year, string _label, string _culture)
    {
        _ddlday.Items.Clear();
        if (_month.Equals("") || _year.Equals("")) return;
        if (!(Regex.IsMatch(_month, "^\\d{0,2}$") && Regex.IsMatch(_year, "^\\d{4}$"))) return;

        short _cul = 0;
        if (_culture != null && _culture.Equals("th-TH")) _cul = 543;
        if (Convert.ToInt16(_year) < _cul) return;
        if (!_label.Equals("")) _ddlday.Items.Add(new ListItem(_label, ""));

        byte _dayofmonth = Convert.ToByte(DateTime.DaysInMonth((Convert.ToInt16(_year) - _cul), Convert.ToInt16(_month)));
        for (byte _i = 1; _i <= _dayofmonth; _i++)
        {
            _ddlday.Items.Add(new ListItem("" + _i, "" + _i.ToString("00")));
        }
        if (_dayofmonth >= DateTime.Now.Day)
        {
            // วันปัจจุบันน้อยกว่าวันสุดท้ายของเดือนที่เลือก
            _ddlday.SelectedValue = DateTime.Now.Day.ToString("00");
        }
        else
        {
            // วันปัจจุบันมากกว่าวันสุดท้ายของเดือนที่เลือก
            _ddlday.SelectedValue = _dayofmonth.ToString("00");
        }
    }
    public static string ConvertEYearToTYear(string _datetime)
    {
        string _datec = "";
        if (!_datetime.Equals(""))
        {
            string[] _dmy = null;
            if (!_datetime.IndexOf(" ").Equals(-1))
            {
                string[] _dt = null;
                _dt = _datetime.Split(new char[] { ' ' });
                _dmy = _dt[0].Split(new char[] { '/' });
                _datec = _dmy[0] + "/" + _dmy[1] + "/" + (Convert.ToInt16(_dmy[2]) + 543) + " " + _dt[1];
            }
            else
            {
                _dmy = _datetime.Split(new char[] { '/' });
                _datec = _dmy[0] + "/" + _dmy[1] + "/" + (Convert.ToInt16(_dmy[2]) + 543);
            }
        }
        return _datec;
    }
    public static string Date_Format_Month_TH(object _date)
    {
        string[] ar_month = new string[] { "ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.", "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค." };
        string s_date = String.Format("{0:d/M/yy}", _date);
        string[] ar_date = s_date.Split(("/").ToCharArray());
        s_date = ar_date[0] + "-" + ar_month[(int.Parse(ar_date[1]) - 1)] + "-" + Convert.ToString(int.Parse(ar_date[2]) - 43).PadLeft(2, '0');
        return s_date;
    }
    public static string Date_Format_Month_TH(string _date)
    {
        string[] ar_month = new string[] { "ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.", "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค." };
        string s_date = _date.Split((" ").ToCharArray())[0];
        string[] ar_date = s_date.Split(("/").ToCharArray());
        s_date = ar_date[0] + "-" + ar_month[(int.Parse(ar_date[1]) - 1)] + "-" + Convert.ToString(int.Parse(ar_date[2]) - 543).Substring(2, 2);
        return s_date;
    }
    public static string Date_Format_Month_NUM(object _date)
    {
        string s_date = String.Format("{0:d/MM/yy}", _date);
        string[] ar_date = s_date.Split(("/").ToCharArray());
        s_date = ar_date[0] + "/" + ar_date[1] + "/" + Convert.ToString(int.Parse(ar_date[2]) - 43).PadLeft(2, '0');
        return s_date;
    }
    public static string Date_Format_Month_NUM(string _date)
    {
        string s_date = _date.Split((" ").ToCharArray())[0];
        string[] ar_date = s_date.Split(("/").ToCharArray());
        s_date = ar_date[0] + "/" + ar_date[1].PadLeft(2, '0') + "/" + Convert.ToString(int.Parse(ar_date[2]) - 543).Substring(2, 2);
        return s_date;
    }

    public static int ExecuteNonQuery(string _sql)
    {
        int result = -1;

        SqlConnection _conn = new SqlConnection(conn);
        if (_conn.State == ConnectionState.Closed) _conn.Open();
        SqlCommand cmd = new SqlCommand(_sql, _conn);
        result = cmd.ExecuteNonQuery();
        cmd.Dispose();
        return result;
    }

    public static int ExecuteNonQuery(SqlConnection _conn, string _sql)
    {
        int result = -1;

        if (_conn.State == ConnectionState.Closed) _conn.Open();
        SqlCommand cmd = new SqlCommand(_sql, _conn);
        result = cmd.ExecuteNonQuery();
        cmd.Dispose();
        return result;
    }

    public static int ExecuteNonQuery(string _strconn, string _sql)
    {
        int result = -1;
        SqlConnection _conn = new SqlConnection(_strconn);
        if (_conn.State == ConnectionState.Closed) _conn.Open();
        SqlCommand cmd = new SqlCommand(_sql, _conn);
        result = cmd.ExecuteNonQuery();
        cmd.Dispose();
        return result;
    }

    public static IList<T> ConvertDatableToList<T>(DataTable table)
    {
        if (table == null)
            return null;
        List<DataRow> rows = new List<DataRow>();
        foreach (DataRow row in table.Rows)
            rows.Add(row);
        return ConvertTo<T>(rows);
    }

    public static IList<T> ConvertTo<T>(IList<DataRow> rows)
    {
        IList<T> list = null;
        if (rows != null)
        {
            list = new List<T>();
            foreach (DataRow row in rows)
            {
                T item = CreateItem<T>(row);
                list.Add(item);
            }
        }
        return list;
    }

    public static T CreateItem<T>(DataRow row)
    {
        string columnName;
        T obj = default(T);
        if (row != null)
        {
            //Create the instance of type T
            obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in row.Table.Columns)
            {
                columnName = column.ColumnName.Replace(" ", "");
                //Get property with same columnName
                PropertyInfo prop = obj.GetType().GetProperty(columnName);
                try
                {
                    //Get value for the column
                    object value = (row[columnName].GetType() == typeof(DBNull)) ? null : row[columnName];
                    //Set property value
                    prop.SetValue(obj, value, null);
                }
                catch
                {
                    throw;
                    //Catch whatever here
                }
            }
        }
        return obj;
    }

    public static string encryptString(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
    public static string converttbl(DataTable tbl, string pvalue)
    {
        string sresult = "";
        for (int i = 0; i < tbl.Rows.Count; i++)
        {
            sresult += tbl.Rows[i][pvalue].ToString();
            if (i != tbl.Rows.Count - 1)
                sresult += ",";
        }
        return sresult;
    }
    public static string DataTableToString(DataTable _tb, string _column)
    {
        string sresult = "";
        bool blen = false;
        for (int i = 0; i < _tb.Rows.Count; i++)
        {
            sresult += "<br />" + (i + 1) + ". " + _tb.Rows[i][_column].ToString();
            if (!_tb.Rows[i][_column].ToString().Equals("")) blen = true;
        }
        _tb.Dispose();
        if (!sresult.Equals("")) sresult = sresult.Remove(0, 6);
        return (blen ? sresult : "");
    }
    public static string DataTableToString(DataTable _tb, string _column, bool _lnumber, string _sparate)
    {
        string sresult = "";
        for (int i = 0; i < _tb.Rows.Count; i++)
        {
            sresult += _sparate + (_lnumber ? (i + 1) + ". " : "") + _tb.Rows[i][_column].ToString();
        }
        _tb.Dispose();
        if (!sresult.Equals("")) sresult = sresult.Remove(0, _sparate.Length);
        return sresult;
    }
    public static int MonthDays(int month, int year)
    {

        switch (month)
        {
            case 2:
                if (IsLeapYear(year)) return (29);
                return (28);
            case 4:
            case 6:
            case 9:
            case 11:
                return (30);
            default:
                return (31);
        }
    } // method IndentFirstLine
    public static bool IsLeapYear(int year)
    {

        return (((year % 4 == 0) && (year % 100 != 0))
                 || (year % 400 == 0));

    }
    // DB
    public static String FormatDBNULL(string _str)
    {
        if (_str == null || _str.Trim() == "") return null; else return _str;
    }
    // Anti SQL Injection
    public static string ReplaceInjection(string str)
    {
        string[] _blacklist = new string[] { "'", "\\", "\"", "*/", ";", "--", "<script", "/*", "</script>" };
        string strRep = str;
        if (strRep == null || strRep.Trim().Equals(String.Empty))
            return strRep;
        foreach (string _blk in _blacklist) { strRep = strRep.Replace(_blk, ""); }

        return strRep;
    }
    public static string ReplaceInjectionAdvance(string str)
    {
        string[] _blacklist = new string[] { "'", "\\", "\"", "*/", ";", "--", "<script", "/*", "</script>" };
        string strRep = str;
        if (strRep == null || strRep.Trim().Equals(String.Empty))
            return strRep;
        foreach (string _blk in _blacklist)
        {
            if (_blk == "'") { strRep = strRep.Replace(_blk, "''"); } else { strRep = strRep.Replace(_blk, ""); }
        }

        return strRep;
    }
    // Anti SQL Injection Fpr Email
    public static string ReplaceInjectionForEmail(string str)
    {
        string[] _blacklist = new string[] { "'", "\\", "\"", "*/", ";", "--", "<script", "/*", "</script>" };
        string strRep = str;
        if (strRep == null || strRep.Trim().Equals(String.Empty))
            return strRep;
        foreach (string _blk in _blacklist) { strRep = strRep.Replace(_blk, ""); }

        return strRep;
    }
    // Anti SQL Injection Fpr Datetime
    public static string ReplaceInjectionForDate(string str)
    {
        string[] _blacklist = new string[] { "'", "\\", "\"", "*/", ";", "--", "<script", "/*", "</script>" };
        string strRep = str;
        if (strRep == null || strRep.Trim().Equals(String.Empty))
            return strRep;
        foreach (string _blk in _blacklist) { strRep = strRep.Replace(_blk, ""); }

        return strRep;
    }
    public static string Upload(FileUpload fluUpload, string path, string id)
    {
        string sFilename = fluUpload.FileName;
        string sFilepath = path;
        if (!id.Equals(""))
            id += "_";
        string msg;

        if (fluUpload.HasFile)
        {
            sFilepath += id + sFilename;
            int idxdot = sFilepath.LastIndexOf('.');
            if (File.Exists(sFilepath))
                File.Delete(sFilepath);
            if (Directory.Exists(sFilepath.Remove(idxdot) + "-small"))
                Directory.Delete(sFilepath.Remove(idxdot) + "-small", true);
            if (Directory.Exists(sFilepath.Remove(idxdot) + "-large"))
                Directory.Delete(sFilepath.Remove(idxdot) + "-large", true);
            fluUpload.SaveAs(sFilepath);
            msg = "";
        }
        else
            msg = "Can't upload this file";

        return msg;

    }
    public static DataTable LinqToDataTable<T>(IEnumerable<T> Data)
    {
        DataTable dtReturn = new DataTable();
        if (Data.ToList().Count == 0) return null;
        // Could add a check to verify that there is an element 0

        T TopRec = Data.ElementAt(0);

        // Use reflection to get property names, to create table

        // column names

        PropertyInfo[] oProps =
        ((Type)TopRec.GetType()).GetProperties();

        foreach (PropertyInfo pi in oProps)
        {

            Type colType = pi.PropertyType; if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {

                colType = colType.GetGenericArguments()[0];

            }

            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
        }

        foreach (T rec in Data)
        {

            DataRow dr = dtReturn.NewRow(); foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
            }
            dtReturn.Rows.Add(dr);

        }
        return dtReturn;
    }
    public static string ReplaceRegularExpressions(string str)
    {
        string[] _blacklist = new string[] { "\"p{Sc}", "\"s?", "(\"p{Sc}\"s?)?", "\"d+", "\".?", "((?<=\".)\"d+)?", "(\"d+\".?((?<=\".)\"d+)?)", "/*", "(?(1)|\"s?\"p{Sc})?" };
        string strRep = str;
        if (strRep == null || strRep.Trim().Equals(String.Empty))
            return strRep;
        foreach (string _blk in _blacklist) { strRep = strRep.Replace(_blk, ""); }

        return strRep;
    }
    /// <summary>
    ///  กรุ๊ป ดาต้าเทเบิล 
    /// </summary> 
    /// <param name="Table">Datatable ที่จะเก็บข้อมูล</param>  
    ///  <param name="group">ชื่อคอลัม ที่ต้องการกร๊ป  ex.  Field1,Field2,Field3...</param> 
    /// <returns>ดาต้าเทเบิลที่เลือกมาได้  หมายเหตุ: การเรียงลำดับต้อง จัดเรียงเอง</returns>
    public static DataTable GroupDataTable(DataTable dt, string groupColumn)
    {
        string[] arrColumn = groupColumn.Replace(" ", "").Split(',');

        DataTable newDataTable = dt.Copy();

        for (sbyte s = 0; s < newDataTable.Columns.Count; s++)
        {
            if (Array.IndexOf(arrColumn, newDataTable.Columns[s].ColumnName) == -1)
            {
                newDataTable.Columns.Remove(newDataTable.Columns[s]);
                s--;
            }
        }

        for (int i = 0; i < newDataTable.Rows.Count; i++)
        {
            for (int j = i + 1; j < newDataTable.Rows.Count; j++)
            {
                sbyte isSame = 0;
                for (sbyte s = 0; s < newDataTable.Columns.Count; s++)
                {
                    if (newDataTable.Rows[i][newDataTable.Columns[s]].ToString() == newDataTable.Rows[j][newDataTable.Columns[s]].ToString())
                    {
                        isSame++;
                    }
                }
                if (isSame == newDataTable.Columns.Count)
                {
                    newDataTable.Rows.Remove(newDataTable.Rows[j]);
                    j--;
                }
            }
        }

        return newDataTable;
    }

    public static int? ParseIntNull(string sVal)
    {
        int nTemp = 0;
        if (int.TryParse(sVal, out nTemp))
        {
            nTemp = int.TryParse(sVal, out nTemp) ? nTemp : 0;
            return nTemp;
        }
        else
        {
            return null;
        }
    }

    public static int GetIntNullToZero(string sVal)
    {
        int nTemp = 0;
        int nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = int.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = int.Parse(sVal);
            }
        }

        return nTemp;
    }

    public static string ConvertExponentialToString(string sVal)
    {
        string sRsult = "";
        try
        {
            decimal nTemp = 0;
            bool check = Decimal.TryParse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float, null, out nTemp);
            if (check)
            {
                decimal d = Decimal.Parse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float);
                sRsult = (d + "").Replace(",", "");
            }
            else
            {
                sRsult = sVal;
            }
        }
        catch
        {
            sRsult = sVal;
        }

        return sRsult != null ? (sRsult.Length < 50 ? sRsult : sRsult.Remove(50)) : ""; //เพื่อไม่ให้ตอน Save Error หากค่าที่เกิดจากผลการคำนวนเกิน Type ใน DB (varchar(50))
    }

    public static decimal? ParseDecimalNull(string sVal)
    {
        decimal nTemp = 0;
        if (decimal.TryParse(sVal, out nTemp))
        {
            nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
            return nTemp;
        }
        else
        {
            return null;
        }
    }

    public static decimal ParseDecimalZero(string sVal)
    {
        decimal nTemp = 0;
        if (decimal.TryParse(sVal, out nTemp))
        {
            nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
        }
        return nTemp;
    }

    public static void BindDdlPageSize(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 10, 30, 50, 100 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static void BindDdlPageSize20(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 20, 30, 50, 100 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static void BindDdlPageSizeSmall(DropDownList ddl)
    {
        ddl.Items.Clear();
        List<int> lstPageSize = new List<int>() { 5, 10, 20 };
        lstPageSize.ForEach(i => { ddl.Items.Add(i + ""); });
    }

    public static string Encrypt_UrlEncrypt(string sValue)
    {
        string result = "";
        if (!string.IsNullOrEmpty(sValue))
        {
            result = HttpUtility.UrlEncode(STCrypt.Encrypt(sValue));
        }
        return result;
    }

    public static string Decrypt_UrlDecode(string sValue)
    {
        string result = "";
        if (!string.IsNullOrEmpty(sValue))
        {
            result = STCrypt.Decrypt(HttpUtility.UrlDecode(sValue));
        }
        return result;
    }

    public static DateTime ConvertStringToDateTime(string strDate, string strTime)
    {
        DateTime dTemp;
        bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
        if (!checkDate)
        {
            if (strTime.Length < 5)
            {
                dTemp = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? "0" + strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : DateTime.Now;
            }
        }
        else
        {
            dTemp = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : DateTime.Now;
        }

        return dTemp;
    }

    public static DateTime? ConvertStringToDateTimeExcel(string strDate)
    {
        var arr = strDate.Split('/');
        string sDate = arr.Length == 3 ? arr[1].PadLeft(2, '0') + "/" + arr[0].PadLeft(2, '0') + "/" + (arr[2].Length >= 4 ? arr[2].PadLeft(4, '0').Substring(0, 4) : "0000") : "";

        DateTime? dResult = null;
        DateTime dTemp;
        bool checkDate = DateTime.TryParseExact(sDate + " 00.00", "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
        if (checkDate)
        {
            dResult = DateTime.TryParseExact(sDate + " 00.00", "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
        }

        return dResult;
    }

    public static DateTime? ConvertStringToDateTime(string strDate)
    {
        string strTime = "";
        DateTime? dResult = null;
        DateTime dTemp;
        bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
        if (!checkDate)
        {
            if (strTime.Length < 5)
            {
                dResult = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? "0" + strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
            }
        }
        else
        {
            dResult = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
        }

        return dResult;
    }

    public static DateTime? ConvertStringToDateTime_MMddyyyy(string strDate, string strTime)
    {
        DateTime dTemp;
        DateTime? dTempre = null;
        try
        {
            if (!string.IsNullOrEmpty(strDate))
            {
                string[] arrDate = strDate.Split(' ')[0].Split('/');
                strDate = arrDate[1].PadLeft(2, '0') + "/" + arrDate[0].PadLeft(2, '0') + "/" + arrDate[2];

                bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
                if (!checkDate)
                {
                    if (strTime.Length < 5)
                    {
                        dTempre = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? "0" + strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
                    }
                }
                else
                {
                    dTempre = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
                }
            }
            else
            {
                dTempre = null;
            }
        }
        catch
        {
            dTempre = null;
        }

        return dTempre;
    }

    public static string ConvertDatetimeToString(DateTime? dDate)
    {
        string result = "";

        if (dDate.HasValue)
        {
            result = dDate.Value.Year + "-" + dDate.Value.Month.ToString().PadLeft(2, '0') + "-" + dDate.Value.Day.ToString().PadLeft(2, '0');
        }

        return result;
    }

    public static string ConvertDatetimeToString2(DateTime? dDate)
    {
        string result = "";

        if (dDate.HasValue)
        {
            result = dDate.Value.Day.ToString().PadLeft(2, '0') + "/" + dDate.Value.Month.ToString().PadLeft(2, '0') + "/" + dDate.Value.Year;
        }

        return result;
    }

    public static string ConvertDatetimeToString_Month_name(DateTime? dDate)
    {
        string result = "";

        if (dDate.HasValue)
        {
            result = dDate.Value.Day.ToString().PadLeft(2, '0') + "-" + GetMonth(dDate.Value.Month) + "-" + dDate.Value.Year;
        }

        return result;
    }

    private static string GetMonth(int month)
    {
        try
        {
            DateTime date = new DateTime(1900, month, 1);

            return date.ToString("MMM");

        }
        catch (Exception ex)
        {

            return string.Empty;
        }
    }

    public static TimeSpan? ConvertStringToTimespan(string sTime)
    {
        return !string.IsNullOrEmpty(sTime) ? TimeSpan.Parse(sTime) : (TimeSpan?)null;
    }

    public static string ConvertStringSQLNULL(string sVal)
    {
        return !string.IsNullOrEmpty(sVal) ? "'" + sVal + "'" : "NULL";
    }

    public static string Ob2Json(object ob)
    {
        try
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 2147483644 };
            string res = serializer.Serialize(ob);//new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ob);

            return res;
        }
        catch
        {
            return "";
        }
    }

    public static void ListDBToDropDownList(DropDownList _ddl, string _sql, string _label, string _datavalue, string _datatext)
    {
        DataTable _dt = Get_Data(_sql);
        if (_dt.Rows.Count > 0)
        {
            _ddl.Items.Clear();
            _ddl.DataSource = _dt;
            _ddl.DataValueField = _datavalue;
            _ddl.DataTextField = _datatext;
            _ddl.DataBind();
            if (!_label.Equals("")) _ddl.Items.Insert(0, new ListItem(_label, ""));
        }
        else
        {
            _ddl.Items.Clear();
            if (!_label.Equals("")) _ddl.Items.Insert(0, new ListItem(_label, ""));
        }
    }

    public static DataTable Get_Data(string _sql)
    {
        using (SqlConnection _conn = new SqlConnection(conn))
        {
            DataTable _dt = new DataTable();
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }

            SqlCommand com = new SqlCommand(_sql, _conn);
            com.CommandTimeout = 6000;
            new SqlDataAdapter(com).Fill(_dt);

            return _dt;
        }
    }

    #region File
    public static void CheckPathAndMoveFile(string sysFileName, string FileName, string sUploadPath, string sUploadPath_Temp)
    {
        HttpContext context = HttpContext.Current;
        string sMapPath = context.Server.MapPath("./");
        string sPathSave = sMapPath + sUploadPath;
        if (!Directory.Exists(sPathSave))
        {
            Directory.CreateDirectory(sPathSave);
        }
        if (File.Exists(sMapPath + sUploadPath_Temp + sysFileName))
        {
            string currentPath = context.Server.MapPath("./" + sUploadPath_Temp);
            File.Move(currentPath + "/" + sysFileName, sPathSave + "/" + sysFileName);
        }
    }

    public static void RemoveFile(string sPathAndFileName)
    {
        HttpContext context = HttpContext.Current;
        string sMapPath = context.Server.MapPath("./");
        if (File.Exists(sPathAndFileName))
        {
            File.Delete(sPathAndFileName);
        }
    }

    public static void DeleteAllFile(string _pathFile)
    {
        if (Directory.Exists(_pathFile.Replace("/", "\\")))
        {
            DirectoryInfo di = new DirectoryInfo(_pathFile.Replace("/", "\\"));
            FileInfo[] fi = di.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo f in fi)
            {
                try
                {
                    f.Delete();
                }
                finally
                {

                }
            }
        }
    }

    public static void RemoveFileAllInFolfer(string sPath)
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(sPath);
        if (di.Exists)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
    #endregion

    public static string GetURL(string Url)
    {
        string[] arrUrl = Url.ToLower().Replace("~/", "").Split('-');
        return arrUrl.Length == 1 ? arrUrl[0] : arrUrl[0] + (arrUrl[1].Substring(4)); //กรณีเข้าหน้า Edit ให้ตั้งชื่อ Url เป็น xxxxx-edit.aspx 
    }

    public static string GetPageModeSubName(string Url, string[] arUrlEdit)
    {
        string Result = "";

        if (arUrlEdit.Length == 1)
        {
            string[] arrUrl = Url.ToLower().Replace("~/", "").Split('-'); //กรณีเข้าหน้า Edit ให้ตั้งชื่อ Url เป็น xxxxx-edit.aspx 
            if (arrUrl.Length == 1)
            {

            }
            else
            {
                // ถ้า Split ได้ แสดงว่าเป็นหน้าเพิ่มข้อมูล
                Result = " > เพิ่มข้อมูล";
            }
        }
        else
        {
            //ถ้ามี String Query แสดงว่าเป็น Edit
            if (arUrlEdit.Length >= 2)
            {
                Result = " > แก้ไขข้อมูล";
            }
        }

        return Result;
    }

    public static void ListddlOrder(DropDownList ddlOrder, int nCountOrder, int? nOrder)
    {
        if (nCountOrder == 0)
        {
            ddlOrder.Items.Insert(0, new ListItem(1 + "", 1 + ""));
        }
        else
        {
            for (int i = 0; i < nCountOrder; i++)
            {
                ddlOrder.Items.Insert(i, new ListItem((i + 1) + "", (i + 1) + ""));
            }

            //Add Order ตัวสุดท้าย
            string sOrderNew = nOrder == 0 ? (nCountOrder + 1) + "" : nOrder + "";
            if (nOrder != null)
            {
                ddlOrder.Items.Insert(nCountOrder, new ListItem(sOrderNew, sOrderNew));
            }
        }
    }

    /********************************/

    public static void SetLog(int? nMenuID, string sMenuName, string sEvent)
    {
        //PTTEP_TMS_Entities db = new PTTEP_TMS_Entities();
        //string sUserID = "System";
        //try
        //{
        //    sUserID = !UserAccount.IsExpired ? UserAccount.SessionInfo.sUserID : "";
        //}
        //catch
        //{
        //    sUserID = "System";
        //}

        //db.TLog.Add(new TLog()
        //{
        //    dLog = DateTime.Now,
        //    sUserID = sUserID,
        //    nMenuID = nMenuID,
        //    sMenuName = sMenuName,
        //    sEvent = sEvent
        //});
        //db.SaveChanges();
    }

    public static string GetFileType(string sFileName)
    {
        string sType = "";
        string[] arr = (sFileName + "").Split('.');
        sType = arr[arr.Length - 1];
        return sType;
    }

    public static bool isNumeric(string sVal)
    {
        int n;
        return int.TryParse(sVal, out n);
    }

    public static bool isNumeric_Decimal(string sVal)
    {
        decimal n;
        return decimal.TryParse(sVal, out n);
    }

    public static bool isDatetime(string sVal)
    {
        DateTime n;
        return DateTime.TryParse(sVal, out n);
    }

    /// <summary>
    /// Encrypt ค่าสำหรับส่งผ่าน URL
    /// </summary>
    /// <param name="s">ค่าที่ต้องการ Encrypt</param>
    /// <returns></returns>
    public static string EncryptParameter(string s)
    {
        return HttpUtility.UrlEncode(STCrypt.Encrypt(s));
    }

    /// <summary>
    /// Encrypt ค่าสำหรับส่งผ่าน URL
    /// </summary>
    /// <param name="n">ค่าที่ต้องการ Encrypt</param>
    /// <returns></returns>
    public static string EncryptParameter(int n)
    {
        return EncryptParameter(n.ToString());
    }

    #region Send Mail    
    public static bool IsUseRealMail()
    {
        PTTGC_CSREntities db = new PTTGC_CSREntities();
        return db.TM_MasterData.FirstOrDefault(w => w.nMainID == 9 && w.sDescription == "Y") != null;
    }

    public static string GET_TemplateEmail()
    {
        return @"<div id=':km' class='ii gt adP adO'>
                <div id=':l9' class='a3s aXjCH m15f05c377e26ea4b'>
                    <u></u>
                    <div style='background: #f9f9f9'>
                        <div style='background-color: #f9f9f9'>

                            <div style='margin: 0px auto; /* max-width: 630px; */background: transparent;'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent;' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; /* padding: 40px 0px */'>
                                                <div style='font-size: 1px; line-height: 12px'>&nbsp;</div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div style='max-width: 640px; margin: 0 auto; border-radius: 4px; overflow: hidden'>
                                <div style='margin: 0px auto; max-width: 640px; background: #ffffff'>
                                    <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: #ffffff' align='center' border='0'>
                                        <tbody>
                                            <tr>
                                                <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 40px 70px'>
                                                    <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                        <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #737f8d; font-family: Tahoma; font-size: 12px; line-height: 24px; text-align: left'>
                                                                            {0}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='text-align: left;'>
                                                                            {1}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div style='margin: 0px auto; max-width: 640px; background: transparent'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 0px'>
                                                <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                    <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                        <tbody>
                                                            <tr>
                                                                <td style='word-break: break-word; font-size: 0px'>
                                                                    <div style='font-size: 1px; line-height: 12px'>&nbsp;</div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>";
    }

    //public static string GetEmail(string sUserID)
    //{
    //    var qUser = new PTTEP_SmartPathEntities().T_User.FirstOrDefault(w => w.sUserID == sUserID && w.IsEmailNotification);
    //    return qUser != null ? qUser.sEmail : "";
    //}

    public static bool SendNetMail(string sfrom, string sto, string sCc, string subject, string message)
    {
        bool IsSend = false;
        string sMessage_Error = "";

        try
        {
            if (!IsUseRealMail())
            {//Demo
                sfrom = ConfigurationSettings.AppSettings["DemoMail_Sender"].ToString();
                sto = ConfigurationSettings.AppSettings["DemoMail_Reciever"].ToString();
                sCc = "";
            }

            System.Net.Mail.MailMessage oMsg = new System.Net.Mail.MailMessage();

            // TODO: Replace with sender e-mail address. 
            oMsg.From = new System.Net.Mail.MailAddress(sfrom, "PTTEP SmartPATH");

            // TODO: Replace with recipient e-mail address.
            string[] Arrsto = sto.Split(',');
            for (int i = 0; i < Arrsto.Length; i++)
            {
                if (!string.IsNullOrEmpty(Arrsto[i]))
                {
                    oMsg.To.Add(Arrsto[i]);
                }
            }

            // Add CC Mail
            string[] ArrsCc = sCc.Split(',');
            for (int i = 0; i < ArrsCc.Length; i++)
            {
                if (!string.IsNullOrEmpty(ArrsCc[i]))
                {
                    oMsg.CC.Add(ArrsCc[i]);
                }
            }

            // TODO: Replace with subject.
            oMsg.Subject = subject;

            // SEND IN HTML FORMAT (comment this line to send plain text).
            oMsg.IsBodyHtml = true;

            oMsg.Body = "<HTML><BODY>" + message + "</BODY></HTML>";

            // TODO: Replace with the name of your remote SMTP server.
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Port = 25;
            smtp.Host = ConfigurationSettings.AppSettings["smtpmail"];
            smtp.Send(oMsg);
            oMsg = null;
            IsSend = true;//return true;
        }
        catch (Exception e)
        {
            IsSend = false; //return false;
            sMessage_Error = e.Message;
        }

        string Status = IsSend == true ? "T" : "F";
        LogMailSend(sto, sCc, subject, message, Status, sMessage_Error);

        return IsSend;
    }

    public static void LogMailSend(string sTo, string sCc, string sSubject, string sMessage, string sStatus, string sMessage_Error)
    {
        //// Encode the content for storing in Sql server.
        //string htmlEncoded = WebUtility.HtmlEncode(sMessage);

        //// Decode the content for showing on Web page.
        ////string original = WebUtility.HtmlDecode(htmlEncoded);

        //PTTEP_SmartPathEntities db = new PTTEP_SmartPathEntities();

        //var tb = new TLog_Mail();
        //tb.sTo = sTo;
        //tb.sCc = sCc;
        //tb.sSubject = sSubject;
        //tb.sMessage = sMessage;
        //tb.sStatus = sStatus;
        //tb.sMessage_Error = !string.IsNullOrEmpty(sMessage_Error) ? sMessage_Error : null;
        //tb.dSend = DateTime.Now;

        //db.TLog_Mail.Add(tb);
        //db.SaveChanges();
    }

    #endregion
    public static string getPathIfFileExists(string sysFileNameAndPath)
    {
        string strRe = "";
        HttpContext context = HttpContext.Current;
        string mapPh = context.Server.MapPath("./");
        if (File.Exists(mapPh + sysFileNameAndPath))
        {
            strRe = mapPh + sysFileNameAndPath;
        }
        return strRe;
    }
}