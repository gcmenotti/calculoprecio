// Decompiled with JetBrains decompiler
// Type: CalculoPrecio.Form1
// Assembly: CalculoPrecio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 28462C77-BFAC-4649-A3D2-416C8EAD4C66
// Assembly location: C:\Users\gmenotti\Desktop\CalculoPrecio.exe

using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CalculoPrecio
{
  public class Form1 : Form
  {
    public SqlConnection Sqlcnn = new SqlConnection("Data Source=10.4.8.10;Initial Catalog=LEEPharmacySoftBackOfficeE;User ID=sa; Password=ClubFarmaSQLAdmin12.");
    public string TipoCompra;
    private IContainer components = (IContainer) null;
    private Label label3;
    private Button button2;
    private SplitContainer splitContainer1;
    private DataGridView dataGridView1;
    private ListBox listBox1;
    private PictureBox pictureBox1;
    private TextBox txtCodigo;
    private Button button1;
    private SaveFileDialog saveFileDialog1;
    private Label label6;
    private Label label4;
    private Label label5;
    private Label label2;
    private Label label1;
    private RadioButton radioButton2;
    private RadioButton radioButton1;
    private Label label7;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem actualizarToolStripMenuItem;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel toolStripStatusLabel1;
    private GroupBox groupBox1;
    private Label lblTablaCompra;
    private Label label8;
    private Button btnBuscarFolio;
    private ComboBox ddBuscarFolio;

    public Form1()
    {
      this.InitializeComponent();
      this.TipoCompra = "M";
      this.toolStripStatusLabel1.Text = "ver 1.0.0.5";
      this.splitContainer1.IsSplitterFixed = true;
      this.label6.Text = DateTime.Today.ToString("D", (IFormatProvider) new CultureInfo("es-PA"));
      this.Sqlcnn.Close();
    }

    public void ListarBox1()
    {
      string str = "SELECT    gpc.codigoProducto FROM dbo.genProductosCat AS gpc INNER JOIN " + this.lblTablaCompra.Text + " AS cdd ON gpc.codigoProducto = cdd.codigoProducto INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto INNER JOIN dbo.cmrCambioPreciosCnf AS ccp ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4 WHERE     (cdd.folioCompra = '" + this.txtCodigo.Text + "') AND (ice.existencia > 0)AND  (gpcr.pedir = 1) group by gpc.codigoProducto UNION ALL SELECT gpc.codigoProducto FROM dbo.genProductosCat AS gpc INNER JOIN  dbo.cmpComprasPastillerosReg  AS cdpr  ON  gpc.codigoProducto = cdpr.codigoProducto INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto INNER JOIN dbo.cmrCambioPreciosCnf AS ccp ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4 WHERE cdpr.tipoCompra = '" + this.TipoCompra + "' AND cdpr.folioCompra = '" + this.txtCodigo.Text + "' AND  (gpcr.pedir = 1) group by gpc.codigoProducto ORDER BY gpc.codigoProducto";
      this.Sqlcnn.Open();
      DataSet dataSet = new DataSet();
      SqlCommand sqlCommand1 = new SqlCommand();
      sqlCommand1.Connection = this.Sqlcnn;
      sqlCommand1.CommandText = str;
      SqlCommand sqlCommand2 = sqlCommand1;
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
      {
        SelectCommand = sqlCommand2
      };
      sqlDataAdapter.TableMappings.Add("codigoProducto", "codigoProducto");
      sqlDataAdapter.Fill(dataSet);
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        this.listBox1.Items.Add(row["codigoProducto"]);
      this.Sqlcnn.Close();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(this.txtCodigo.Text))
        {
          int num = (int) MessageBox.Show("Debe ingresar primero el Folio de Compra");
        }
        else
        {
          this.Limpiar();
          this.ListarBox1();
          foreach (object obj in this.listBox1.Items)
            this.ProductosClass(obj.ToString());
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("Error al conectarse a la Base de Datos");
      }
    }

    private void ProductosClass(string numero)
    {
      string selectCommandText = "SELECT    gpc.codigoProducto,gpc.descripcion,gpc.precioFarmacia [precioFarmaciaCat], ice.codigoSucursal, convert(int, ice.existencia) as existencia,ice.precioFarmacia, ccp.margen, gpcr.codigoRelacionado, 0 AS cantidadPastillero, gpc.precioPublico [PrecioActual] FROM         dbo.genProductosCat AS gpc INNER JOIN " + this.lblTablaCompra.Text + " AS cdd ON gpc.codigoProducto = cdd.codigoProducto INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto INNER JOIN dbo.cmrCambioPreciosCnf AS ccp ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4 WHERE     (cdd.folioCompra = '" + this.txtCodigo.Text + "') AND (ice.existencia > 0) AND gpc.codigoProducto = '" + numero + "' AND (gpcr.pedir = 1) UNION ALL SELECT gpc.codigoProducto, gpc.descripcion, gpc.precioFarmacia [precioFarmaciaCat],ice.codigoSucursal, convert(int, ice.existencia) as existencia ,ice.precioFarmacia, ccp.margen, gpcr.codigoRelacionado, CAST (gpasc.cantidad as int) as cantidadPastillero, gpc.precioPublico [PrecioActual] FROM dbo.genProductosCat AS gpc  INNER JOIN  dbo.cmpComprasPastillerosReg  AS cdpr  ON  gpc.codigoProducto = cdpr.codigoProducto INNER JOIN  dbo.genPastillerosCat AS gpasc ON gpc.codigoProducto = gpasc.codigoPastillero INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto  INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto INNER JOIN dbo.cmrCambioPreciosCnf AS ccp ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4 WHERE cdpr.tipoCompra = '" + this.TipoCompra + "' AND cdpr.folioCompra = '" + this.txtCodigo.Text + "' AND gpc.codigoProducto = '" + numero + "' AND  (gpcr.pedir = 1)  ORDER BY gpc.codigoProducto,ice.codigoSucursal";
      this.Sqlcnn.Open();
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommandText, this.Sqlcnn);
      DataSet dataSet = new DataSet();
      sqlDataAdapter.Fill(dataSet);
      List<Form1.GridTemp> source = new List<Form1.GridTemp>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        Form1.GridTemp gridTemp = new Form1.GridTemp()
        {
          CodigoProducto = row[0].ToString(),
          Descripcion = row[1].ToString(),
          PrecioFarmaciaCat = row[2].ToString(),
          CodigoSucursal = row[3].ToString(),
          Existencia = row[4].ToString(),
          PrecioFarmacia = row[5].ToString(),
          Margen = row[6].ToString(),
          CodigoRelacionado = row[7].ToString(),
          CantidadPastillero = row[8].ToString(),
          PrecioActual = row[9].ToString()
        };
        source.Add(gridTemp);
      }
      int num1 = 0;
      foreach (Form1.GridTemp gridTemp in source)
        num1 += Convert.ToInt32(gridTemp.Existencia);
      string str = num1.ToString();
      for (int index = 0; index < 5; ++index)
      {
        foreach (Form1.GridTemp gridTemp in source)
          gridTemp.SumaExistencia = str;
      }
      Decimal num2 = 0M;
      for (int index = 0; index < 5; ++index)
      {
        foreach (Form1.GridTemp gridTemp in source)
          gridTemp.PorcentajeFinal = ((Decimal) Convert.ToInt32(gridTemp.Existencia) * Convert.ToDecimal(gridTemp.PrecioFarmacia) / (Decimal) Convert.ToInt32(num1)).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
      foreach (Form1.GridTemp gridTemp in source)
        num2 += Convert.ToDecimal(gridTemp.PorcentajeFinal);
      Decimal num3 = num2;
      for (int index = 0; index < 5; ++index)
      {
        foreach (Form1.GridTemp gridTemp in source)
        {
          Decimal num4 = num3 * 100M / Convert.ToDecimal(gridTemp.PrecioFarmaciaCat) - 100M;
          string precioFarmaciaCat = gridTemp.PrecioFarmaciaCat;
          gridTemp.PrecioFinal = !(num4 > 0.6M) ? Convert.ToDecimal(precioFarmaciaCat) : Convert.ToDecimal(num3);
        }
      }
      for (int index = 0; index < 5; ++index)
      {
        foreach (Form1.GridTemp gridTemp in source)
          gridTemp.PrecioVenta = gridTemp.PrecioFinal / (1M - Convert.ToDecimal(gridTemp.Margen) / 100M);
      }
      foreach (Form1.GridTemp gridTemp in source.GroupBy(ac => new
      {
        CodigoProducto = ac.CodigoProducto,
        CodigoRelacionado = ac.CodigoRelacionado,
        Descripcion = ac.Descripcion,
        PrecioFinal = ac.PrecioFinal,
        PrecioVenta = ac.PrecioVenta,
        Margen = ac.Margen,
        CantidadPastillero = ac.CantidadPastillero,
        PrecioActual = ac.PrecioActual
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType0<string, string, string, Decimal, Decimal, string, string, string>, Form1.GridTemp>, Form1.GridTemp>(ac => new Form1.GridTemp()
      {
        CodigoProducto = ac.Key.CodigoProducto,
        CodigoRelacionado = ac.Key.CodigoRelacionado,
        Descripcion = ac.Key.Descripcion,
        PrecioFinal = ac.Key.PrecioFinal,
        PrecioVenta = ac.Key.PrecioVenta,
        Margen = ac.Key.Margen,
        CantidadPastillero = ac.Key.CantidadPastillero,
        PrecioActual = ac.Key.PrecioActual
      }))
      {
        this.dataGridView1.ColumnCount = 11;
        this.dataGridView1.Columns[0].Name = "Codigo Producto";
        this.dataGridView1.Columns[1].Name = "Codigo Relacionado";
        this.dataGridView1.Columns[2].Name = "Descripcion";
        this.dataGridView1.Columns[3].Name = "Precio Venta Actual";
        this.dataGridView1.Columns[4].Name = "Precio Farmacia Nuevo";
        this.dataGridView1.Columns[5].Name = "Precio Venta Nuevo";
        this.dataGridView1.Columns[6].Name = "Margen";
        this.dataGridView1.Columns[7].Name = "Cantidad Pastillero";
        this.dataGridView1.Columns[8].Name = "Precio Venta Caja Actual";
        this.dataGridView1.Columns[9].Name = "Costo Caja";
        this.dataGridView1.Columns[10].Name = "Precio Venta Caja Nuevo";
        object[] objArray = new object[11];
        objArray[0] = (object) gridTemp.CodigoProducto;
        objArray[1] = (object) gridTemp.CodigoRelacionado;
        objArray[2] = (object) gridTemp.Descripcion;
        Decimal num5 = Convert.ToDecimal(gridTemp.PrecioActual);
        objArray[3] = (object) num5.ToString("0.00");
        num5 = gridTemp.PrecioFinal;
        objArray[4] = (object) num5.ToString("0.00");
        num5 = gridTemp.PrecioVenta;
        objArray[5] = (object) num5.ToString("0.00");
        num5 = Convert.ToDecimal(gridTemp.Margen);
        objArray[6] = (object) num5.ToString("0.00");
        objArray[7] = (object) Convert.ToInt32(gridTemp.CantidadPastillero);
        num5 = Convert.ToDecimal(gridTemp.PrecioActual) * (Decimal) Convert.ToInt32(gridTemp.CantidadPastillero);
        objArray[8] = (object) num5.ToString("0.00");
        num5 = gridTemp.PrecioFinal * (Decimal) Convert.ToInt32(gridTemp.CantidadPastillero);
        objArray[9] = (object) num5.ToString("0.00");
        num5 = gridTemp.PrecioVenta * (Decimal) Convert.ToInt32(gridTemp.CantidadPastillero);
        objArray[10] = (object) num5.ToString("0.00");
        this.dataGridView1.Rows.Add(objArray);
        this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      }
      this.Sqlcnn.Close();
    }

    private void GcExport()
    {
      this.saveFileDialog1.InitialDirectory = "C:";
      this.saveFileDialog1.Title = "Salvar como archivo Excel";
      this.saveFileDialog1.FileName = "";
      this.saveFileDialog1.DefaultExt = "xlsx";
      this.saveFileDialog1.Filter = "Excel Files(2003) | .*xls |Excel Files(2007)| *.xlsx";
      if (this.saveFileDialog1.ShowDialog() == DialogResult.Cancel)
        return;
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Application instance = (Microsoft.Office.Interop.Excel.Application) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
      // ISSUE: reference to a compiler-generated method
      instance.Application.Workbooks.Add(System.Type.Missing);
      for (int ColumnIndex = 1; ColumnIndex < this.dataGridView1.Columns.Count + 1; ++ColumnIndex)
        instance.Cells[(object) 1, (object) ColumnIndex] = (object) this.dataGridView1.Columns[ColumnIndex - 1].HeaderText;
      for (int index1 = 0; index1 < this.dataGridView1.Rows.Count - 1; ++index1)
      {
        for (int index2 = 0; index2 < this.dataGridView1.Columns.Count; ++index2)
          instance.Cells[(object) (index1 + 2), (object) (index2 + 1)] = (object) this.dataGridView1.Rows[index1].Cells[index2].Value.ToString();
      }
      // ISSUE: reference to a compiler-generated method
      instance.Columns.AutoFit();
      // ISSUE: reference to a compiler-generated method
      instance.Rows.AutoFit();
      object row = instance.Rows[(object) 1, System.Type.Missing];
      // ISSUE: reference to a compiler-generated field
      if (Form1.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Form1.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Bold", typeof (Form1), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool, object> target = Form1.\u003C\u003Eo__7.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool, object>> p1 = Form1.\u003C\u003Eo__7.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Form1.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Form1.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Font", typeof (Form1), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Form1.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) Form1.\u003C\u003Eo__7.\u003C\u003Ep__0, row);
      object obj2 = target((CallSite) p1, obj1, true);
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Range cells = instance.Cells;
      cells.NumberFormat = (object) "@";
      cells.HorizontalAlignment = (object) XlHAlign.xlHAlignRight;
      // ISSUE: reference to a compiler-generated method
      instance.ActiveWorkbook.SaveCopyAs((object) this.saveFileDialog1.FileName);
      instance.ActiveWorkbook.Saved = true;
      // ISSUE: reference to a compiler-generated method
      instance.Quit();
      int num = (int) MessageBox.Show("Archivo guardado.");
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.dataGridView1.Rows.Count == 0)
      {
        int num = (int) MessageBox.Show("Debe generar el reporta primero!");
      }
      else
        this.GcExport();
    }

    public void Limpiar()
    {
      this.dataGridView1.Columns.Clear();
      this.dataGridView1.DataSource = (object) null;
      this.dataGridView1.Refresh();
      this.listBox1.Items.Clear();
    }

    private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
    {
    }

    private void label8_Click(object sender, EventArgs e)
    {
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
      this.lblTablaCompra.Text = (string) null;
      this.lblTablaCompra.Text = "dbo.cmpComprasDirectasDet";
      this.TipoCompra = "D";
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
      this.lblTablaCompra.Text = (string) null;
      this.lblTablaCompra.Text = "dbo.cmpComprasDet";
      this.TipoCompra = "M";
    }

    private void actualizarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!ApplicationDeployment.IsNetworkDeployed)
        return;
      ApplicationDeployment currentDeployment = ApplicationDeployment.CurrentDeployment;
      UpdateCheckInfo updateCheckInfo;
      try
      {
        updateCheckInfo = currentDeployment.CheckForDetailedUpdate();
      }
      catch (DeploymentDownloadException ex)
      {
        int num = (int) MessageBox.Show("No se puede descargar la ultima version en estos momentos. \\n\\nVerifica tu Internet.");
        return;
      }
      catch (InvalidDeploymentException ex)
      {
        int num = (int) MessageBox.Show("No se puede verificar. Volver a instalar");
        return;
      }
      if (updateCheckInfo.UpdateAvailable)
      {
        if (MessageBox.Show("Una version nueva esta disponible. Quiere descargarla?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        try
        {
          currentDeployment.Update();
          System.Windows.Forms.Application.Restart();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else
      {
        int num1 = (int) MessageBox.Show("Ya tienes la ultima version.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void btnBuscarFolio_Click(object sender, EventArgs e)
    {
      SqlCommand sqlCommand = new SqlCommand("Select foliocompra from " + this.lblTablaCompra.Text + " group by foliocompra order by folioCompra", this.Sqlcnn);
      this.Sqlcnn.Open();
      this.ddBuscarFolio.Items.Clear();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      while (sqlDataReader.Read())
        this.ddBuscarFolio.Items.Add((object) sqlDataReader["folioCompra"].ToString());
      sqlDataReader.Close();
      this.Sqlcnn.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.label3 = new Label();
      this.button2 = new Button();
      this.splitContainer1 = new SplitContainer();
      this.label8 = new Label();
      this.btnBuscarFolio = new Button();
      this.ddBuscarFolio = new ComboBox();
      this.lblTablaCompra = new Label();
      this.groupBox1 = new GroupBox();
      this.radioButton1 = new RadioButton();
      this.radioButton2 = new RadioButton();
      this.label7 = new Label();
      this.button1 = new Button();
      this.pictureBox1 = new PictureBox();
      this.txtCodigo = new TextBox();
      this.listBox1 = new ListBox();
      this.menuStrip1 = new MenuStrip();
      this.actualizarToolStripMenuItem = new ToolStripMenuItem();
      this.statusStrip1 = new StatusStrip();
      this.toolStripStatusLabel1 = new ToolStripStatusLabel();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.dataGridView1 = new DataGridView();
      this.saveFileDialog1 = new SaveFileDialog();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(9, 36);
      this.label3.Name = "label3";
      this.label3.Size = new Size(203, 17);
      this.label3.TabIndex = 6;
      this.label3.Text = "Escribir el número de Folio";
      this.button2.Location = new Point(242, 48);
      this.button2.Name = "button2";
      this.button2.Size = new Size(98, 34);
      this.button2.TabIndex = 8;
      this.button2.Text = "Generar Reporte";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.FixedPanel = FixedPanel.Panel1;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.BackColor = SystemColors.Window;
      this.splitContainer1.Panel1.Controls.Add((Control) this.label8);
      this.splitContainer1.Panel1.Controls.Add((Control) this.btnBuscarFolio);
      this.splitContainer1.Panel1.Controls.Add((Control) this.ddBuscarFolio);
      this.splitContainer1.Panel1.Controls.Add((Control) this.lblTablaCompra);
      this.splitContainer1.Panel1.Controls.Add((Control) this.groupBox1);
      this.splitContainer1.Panel1.Controls.Add((Control) this.label7);
      this.splitContainer1.Panel1.Controls.Add((Control) this.button1);
      this.splitContainer1.Panel1.Controls.Add((Control) this.pictureBox1);
      this.splitContainer1.Panel1.Controls.Add((Control) this.txtCodigo);
      this.splitContainer1.Panel1.Controls.Add((Control) this.listBox1);
      this.splitContainer1.Panel1.Controls.Add((Control) this.button2);
      this.splitContainer1.Panel1.Controls.Add((Control) this.label3);
      this.splitContainer1.Panel1.Controls.Add((Control) this.menuStrip1);
      this.splitContainer1.Panel1.Paint += new PaintEventHandler(this.splitContainer1_Panel1_Paint);
      this.splitContainer1.Panel2.BackColor = SystemColors.Window;
      this.splitContainer1.Panel2.Controls.Add((Control) this.statusStrip1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label4);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label5);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label6);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label2);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.dataGridView1);
      this.splitContainer1.Size = new Size(1231, 492);
      this.splitContainer1.SplitterDistance = 101;
      this.splitContainer1.TabIndex = 9;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(448, 40);
      this.label8.Name = "label8";
      this.label8.Size = new Size(65, 13);
      this.label8.TabIndex = 20;
      this.label8.Text = "Buscar Folio";
      this.btnBuscarFolio.Location = new Point(619, 54);
      this.btnBuscarFolio.Name = "btnBuscarFolio";
      this.btnBuscarFolio.Size = new Size(75, 23);
      this.btnBuscarFolio.TabIndex = 19;
      this.btnBuscarFolio.Text = "Buscar";
      this.btnBuscarFolio.UseVisualStyleBackColor = true;
      this.btnBuscarFolio.Click += new EventHandler(this.btnBuscarFolio_Click);
      this.ddBuscarFolio.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
      this.ddBuscarFolio.FormattingEnabled = true;
      this.ddBuscarFolio.Location = new Point(451, 55);
      this.ddBuscarFolio.Name = "ddBuscarFolio";
      this.ddBuscarFolio.Size = new Size(162, 21);
      this.ddBuscarFolio.TabIndex = 18;
      this.lblTablaCompra.AutoSize = true;
      this.lblTablaCompra.ForeColor = Color.DarkGreen;
      this.lblTablaCompra.Location = new Point(242, 32);
      this.lblTablaCompra.Name = "lblTablaCompra";
      this.lblTablaCompra.Size = new Size(145, 13);
      this.lblTablaCompra.TabIndex = 15;
      this.lblTablaCompra.Text = "dbo.cmpComprasDirectasDet";
      this.lblTablaCompra.Visible = false;
      this.lblTablaCompra.Click += new EventHandler(this.label8_Click);
      this.groupBox1.Controls.Add((Control) this.radioButton1);
      this.groupBox1.Controls.Add((Control) this.radioButton2);
      this.groupBox1.Location = new Point(700, 37);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(239, 50);
      this.groupBox1.TabIndex = 17;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Seleccionar tipo de Compra";
      this.radioButton1.AutoSize = true;
      this.radioButton1.Location = new Point(17, 22);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new Size(98, 17);
      this.radioButton1.TabIndex = 13;
      this.radioButton1.TabStop = true;
      this.radioButton1.Text = "Compra Directa";
      this.radioButton1.UseVisualStyleBackColor = true;
      this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
      this.radioButton2.AutoSize = true;
      this.radioButton2.Location = new Point(121, 22);
      this.radioButton2.Name = "radioButton2";
      this.radioButton2.Size = new Size(106, 17);
      this.radioButton2.TabIndex = 14;
      this.radioButton2.TabStop = true;
      this.radioButton2.Text = "Compra Sugerida";
      this.radioButton2.UseVisualStyleBackColor = true;
      this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(466, 37);
      this.label7.Name = "label7";
      this.label7.Size = new Size(0, 13);
      this.label7.TabIndex = 15;
      this.button1.Location = new Point(346, 48);
      this.button1.Name = "button1";
      this.button1.Size = new Size(99, 34);
      this.button1.TabIndex = 11;
      this.button1.Text = "Exportar a Excel";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(1066, 25);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(153, 74);
      this.pictureBox1.TabIndex = 10;
      this.pictureBox1.TabStop = false;
      this.txtCodigo.Location = new Point(12, 56);
      this.txtCodigo.Name = "txtCodigo";
      this.txtCodigo.Size = new Size(224, 20);
      this.txtCodigo.TabIndex = 9;
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new Point(12, 81);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new Size(117, 17);
      this.listBox1.TabIndex = 7;
      this.listBox1.Visible = false;
      this.menuStrip1.BackColor = SystemColors.HotTrack;
      this.menuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.actualizarToolStripMenuItem
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(1231, 24);
      this.menuStrip1.TabIndex = 16;
      this.menuStrip1.Text = "menuStrip1";
      this.actualizarToolStripMenuItem.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
      this.actualizarToolStripMenuItem.ForeColor = SystemColors.ButtonHighlight;
      this.actualizarToolStripMenuItem.Name = "actualizarToolStripMenuItem";
      this.actualizarToolStripMenuItem.Size = new Size(74, 20);
      this.actualizarToolStripMenuItem.Text = "Actualizar";
      this.actualizarToolStripMenuItem.Click += new EventHandler(this.actualizarToolStripMenuItem_Click);
      this.statusStrip1.BackColor = SystemColors.InactiveCaptionText;
      this.statusStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.toolStripStatusLabel1
      });
      this.statusStrip1.Location = new Point(0, 365);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new Size(1231, 22);
      this.statusStrip1.TabIndex = 7;
      this.statusStrip1.Text = "statusStrip1";
      this.toolStripStatusLabel1.ForeColor = SystemColors.ButtonHighlight;
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new Size(59, 17);
      this.toolStripStatusLabel1.Text = "ver 1.0.0.1";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.ForeColor = Color.Black;
      this.label4.Location = new Point(463, 337);
      this.label4.Name = "label4";
      this.label4.Size = new Size(422, 15);
      this.label4.TabIndex = 6;
      this.label4.Text = "* Si otra sucursal realiza una compra el precio mostrado podría ser diferente";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.ForeColor = Color.Black;
      this.label5.Location = new Point(463, 319);
      this.label5.Name = "label5";
      this.label5.Size = new Size(382, 15);
      this.label5.TabIndex = 5;
      this.label5.Text = "* El cambio de precio en el sistema se verá reflejado al día siguiente.";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.ForeColor = SystemColors.HotTrack;
      this.label6.Location = new Point(966, 337);
      this.label6.Name = "label6";
      this.label6.Size = new Size(262, 17);
      this.label6.TabIndex = 12;
      this.label6.Text = "Vienres, 30 de diciembre del 2016.";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.ForeColor = Color.Black;
      this.label2.Location = new Point(12, 337);
      this.label2.Name = "label2";
      this.label2.Size = new Size(403, 15);
      this.label2.TabIndex = 4;
      this.label2.Text = "* Solo se le cambiaran los precios a los productos que tengan inventario.";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.Black;
      this.label1.Location = new Point(12, 319);
      this.label1.Name = "label1";
      this.label1.Size = new Size(396, 15);
      this.label1.TabIndex = 3;
      this.label1.Text = "* El cálculo de precio solo sirve para los Folios generados el día de hoy.";
      this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Location = new Point(12, 7);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.Size = new Size(1207, 304);
      this.dataGridView1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1231, 492);
      this.Controls.Add((Control) this.splitContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MainMenuStrip = this.menuStrip1;
      this.MaximizeBox = false;
      this.Name = nameof (Form1);
      this.Text = "Calculo de Precios";
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
    }

    public class GridTemp
    {
      public string CodigoSucursal { get; set; }

      public string CodigoProducto { get; set; }

      public string PrecioActual { get; set; }

      public string Descripcion { get; set; }

      public string CodigoRelacionado { get; set; }

      public string PrecioFarmaciaCat { get; set; }

      public string PrecioFarmacia { get; set; }

      public string Existencia { get; set; }

      public string Margen { get; set; }

      public string SumaExistencia { get; set; }

      public string PorcentajeFinal { get; set; }

      public Decimal PrecioFinal { get; set; }

      public Decimal PrecioVenta { get; set; }

      public string CantidadPastillero { get; set; }
    }
  }
}
