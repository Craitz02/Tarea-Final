using Core.Poco;
using Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Forms
{
    public partial class FrmProducto : Form
    {
        public List<Product> Productos { get; set; }
        private ProductRepository Prepo;
        private bool toUpdate = false;
        private Product pToUpdate;

        public FrmProducto()
        {
            InitializeComponent();
            LoadComponents();
            Prepo = new ProductRepository();
        }

        private void BtnSearchImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog Imagen = new OpenFileDialog();
            if (Imagen.ShowDialog() == DialogResult.OK)
            {
                TxtImagenUrl.Text = Imagen.FileName;
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string Nombre = TxtNombre.Text;
                string Descripcion = TxtDescripcion.Text;
                string Marca = TxtMarca.Text;
                string Modelo = TxtModelo.Text;
                string Imagen = TxtImagenUrl.Text;
                ValidateProduct(Nombre, out int Cantidad, out decimal Precio, Descripcion, Imagen, Marca, Modelo);


                Product P = new Product()
                {
                    Name = Nombre,
                    Description = Descripcion,
                    Brand = Marca,
                    ImageURL = Imagen,
                    Model = Modelo,
                    Price = Precio,
                    Stock = Cantidad,
                };

                if (toUpdate)
                {
                    pToUpdate.Name = Nombre;
                    pToUpdate.Description = Descripcion;
                    pToUpdate.Brand = Marca;
                    pToUpdate.ImageURL = Imagen;
                    pToUpdate.Model = Modelo;
                    pToUpdate.Price = Precio;
                    pToUpdate.Stock = Cantidad;
                    Prepo.Update(pToUpdate);
                    toUpdate = false;
                    MessageBox.Show("Producto actualizado satisfactoriamente");
                }
                else
                {
                    Prepo.Create(P);
                    MessageBox.Show("Producto agregado satisfactoriamente");
                }
                ClearBox();
                Productos = Prepo.GetAll().ToList();
                reloadTable(Productos);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        public void ValidateProduct(string nombre, out int cantidad, out decimal precio, string descripcion, string imagen, string marca, string modelo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre es requerido!");
            }
            if (!int.TryParse(TxtCantidad.Text, out int Cantidad))
            {
                throw new ArgumentException($"El valor \"{TxtCantidad.Text}\" es invalido!");
            }
            cantidad = Cantidad;
            if (!decimal.TryParse(TxtPrecio.Text, out decimal p))
            {
                throw new ArgumentException($"El valor \"{TxtPrecio.Text}\" es invalido!");
            }
            precio = p;
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                throw new ArgumentException("La descripción es requerida!");
            }
            if (string.IsNullOrWhiteSpace(imagen))
            {
                throw new ArgumentException("La imagen es requerida!");
            }
            if (string.IsNullOrWhiteSpace(marca))
            {
                throw new ArgumentException("La marca es requerida!");
            }
            if (string.IsNullOrWhiteSpace(modelo))
            {
                throw new ArgumentException("El modelo es requerido!");
            }
        }

        public void ClearBox()
        {
            TxtNombre.Clear();
            TxtCantidad.Clear();
            TxtMarca.Clear();
            TxtModelo.Clear();
            TxtPrecio.Clear();
            TxtDescripcion.Clear();
            TxtImagenUrl.Clear();
        }

        public void reloadTable(List<Product> prod)
        {
            
            if (Prepo.GetAll() == null)
            {
                return;
            }
            else
            {
                prod = Prepo.GetAll().ToList();
                DgvProductos.DataSource = null;
                DgvProductos.DataSource = prod;
            }
            
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {
            reloadTable(Productos);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DgvProductos.Rows.Count == 0 || DgvProductos.CurrentCell.RowIndex < 0)
            {
                MessageBox.Show("Tabla sin datos o fila no seleccionada");
                return;
            }
            Product p = (Product)DgvProductos.CurrentRow.DataBoundItem;
            Prepo.Delete(p);

            reloadTable(Productos);
        }

        public void LoadInfo(Product p)
        {
            TxtNombre.Text = p.Name;
            TxtDescripcion.Text = p.Description;
            TxtMarca.Text = p.Brand;
            TxtModelo.Text = p.Model;
            TxtCantidad.Text = p.Stock + "";
            TxtPrecio.Text = p.Price + "";
            TxtImagenUrl.Text = p.ImageURL;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (DgvProductos.Rows.Count == 0 || DgvProductos.CurrentCell.RowIndex < 0)
            {
                MessageBox.Show("Tabla sin datos o fila no seleccionada");
                return;
            }
            Product p = (Product)DgvProductos.CurrentRow.DataBoundItem;
            LoadInfo(p);
            toUpdate = true;
            pToUpdate = p;
        }

        private void LoadComponents()
        {
            DgvProductos.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DgvProductos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProductos.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
}