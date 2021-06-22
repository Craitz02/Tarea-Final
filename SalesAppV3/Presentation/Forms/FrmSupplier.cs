using Core.Poco;
using Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Forms
{
    public partial class FrmSupplier : Form
    {
        public List<Supplier> Proveedores { get; set; }
        private SupplierRepository SRepo;
        private bool toUpdate = false;
        private Supplier sToUpdate;

        public FrmSupplier()
        {
            InitializeComponent();
            LoadComponents();
            SRepo = new SupplierRepository();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string Nombre = TxtNombre.Text;
                string Direccion = TxtDireccion.Text;
                string Email = TxtEmail.Text;
                string Telefono = TxtTelefono.Text;
                ValidateSupplier(Nombre, Direccion, Email, Telefono);

                Supplier S = new Supplier()
                {
                    Name = Nombre,
                    Address = Direccion,
                    Email = Email,
                    Phone = Telefono,
                };

                if (toUpdate)
                {
                    sToUpdate.Name = Nombre;
                    sToUpdate.Address = Direccion;
                    sToUpdate.Email = Email;
                    sToUpdate.Phone = Telefono;
                    SRepo.Update(sToUpdate);
                    toUpdate = false;
                    MessageBox.Show("Producto actualizado satisfactoriamente");
                }
                else
                {
                    SRepo.Create(S);
                    MessageBox.Show("Proveedor agregado satisfactoriamente");
                }
                ClearBox();
                Proveedores = SRepo.GetAll().ToList();
                reloadTable(Proveedores);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Se produjo un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        public void ValidateSupplier(string Nombre, string Direccion, string Email, string Telefono)
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                throw new ArgumentException("El Nombre es requerido");
            }
            if (string.IsNullOrWhiteSpace(Direccion))
            {
                throw new ArgumentException("El Apellido es requerido");
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new ArgumentException("El Email es requerido");
            }
            if (string.IsNullOrWhiteSpace(Telefono))
            {
                throw new ArgumentException("El Teléfono es requerido");
            }
            if (!ValidarEmail(Email))
            {
                throw new ArgumentException($"El Email '{Email}' no es válido");
            }
            if (!ValidarTelefono(Telefono))
            {
                throw new ArgumentException($"El Teléfono '{Telefono}' no es válido");
            }
        }

        public static bool ValidarEmail(string Correo)
        {
            return Correo != null && Regex.IsMatch(Correo, "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@(([a-zA-Z]+[\\w-]+\\.){1,2}[a-zA-Z]{2,4})$");
        }

        public static bool ValidarTelefono(string Telefono)
        {
            return Telefono != null && Regex.IsMatch(Telefono, "^\\d{8,10}$");
        }

        public void ClearBox()
        {
            TxtNombre.Clear();
            TxtDireccion.Clear();
            TxtEmail.Clear();
            TxtTelefono.Clear();
        }

        public void reloadTable(List<Supplier> Supplier)
        {
            if (SRepo.GetAll() == null)
            {
                return;
            }
            else
            {
                Supplier = SRepo.GetAll().ToList();
                DgvProveedores.DataSource = null;
                DgvProveedores.DataSource = Supplier;
            }
        }

        private void FrmSupplier_Load(object sender, EventArgs e)
        {
            reloadTable(Proveedores);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DgvProveedores.Rows.Count == 0 || DgvProveedores.CurrentCell.RowIndex < 0)
            {
                MessageBox.Show("Tabla sin datos o fila no seleccionada");
                return;
            }
            Supplier S = (Supplier)DgvProveedores.CurrentRow.DataBoundItem;
            SRepo.Delete(S);

            reloadTable(Proveedores);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (DgvProveedores.Rows.Count == 0 || DgvProveedores.CurrentCell.RowIndex < 0)
            {
                MessageBox.Show("Tabla sin datos o fila no seleccionada");
                return;
            }
            Supplier S = (Supplier)DgvProveedores.CurrentRow.DataBoundItem;
            LoadInfo(S);
            toUpdate = true;
            sToUpdate = S;
        }

        public void LoadInfo(Supplier S)
        {
            TxtNombre.Text = S.Name;
            TxtDireccion.Text = S.Address;
            TxtEmail.Text = S.Email;
            TxtTelefono.Text = S.Phone;
        }
        private void LoadComponents()
        {
            DgvProveedores.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DgvProveedores.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProveedores.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
    }
}
