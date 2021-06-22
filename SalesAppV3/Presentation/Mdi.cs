using Core.Poco;
using Presentation.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class Mdi : Form
    {
        public List<Product> Productos;
        public List<Client> Clientes;
        public List<Supplier> Proveedores;

        public Mdi()
        {
            InitializeComponent();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProducto frm = new FrmProducto();
            frm.Productos = Productos;
            frm.MdiParent = this;
            frm.Show();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCliente frm = new FrmCliente();
            frm.Clientes = Clientes;
            frm.MdiParent = this;
            frm.Show();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSupplier frm = new FrmSupplier();
            frm.Proveedores = Proveedores;
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
