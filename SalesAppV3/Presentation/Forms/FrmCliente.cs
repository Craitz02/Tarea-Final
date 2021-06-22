using Core.Poco;
using Infraestructure.Data;
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
    public partial class FrmCliente : Form
    {
        public List<Client> Clientes { get; set; }
        private ClientRepository CRepo;
        private bool toUpdate = false;
        private Client cToUpdate;

        public FrmCliente()
        {
            InitializeComponent();
            CRepo = new ClientRepository();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string Nombre = TxtNombre.Text;
                string Apellidos = TxtApellidos.Text;
                string Email = TxtEmail.Text;
                string Telefono = TxtTelefono.Text;
                ValidateClient(Nombre, Apellidos, Email, Telefono);

                Client C = new Client() {
                    Name = Nombre,
                    Lastname = Apellidos,
                    Email = Email,
                    Phone = Telefono,
                };

                if (toUpdate)
                {
                    cToUpdate.Name = Nombre;
                    cToUpdate.Lastname = Apellidos;
                    cToUpdate.Email = Email;
                    cToUpdate.Phone = Telefono;
                    CRepo.Update(cToUpdate);
                    MessageBox.Show("Producto actualizado satisfactoriamente");
                }
                else
                {
                    CRepo.Create(C);
                    MessageBox.Show("Cliente agregado satisfactoriamente");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ClearBox();
            Clientes = CRepo.GetAll().ToList();
            reloadTable(Clientes);
        }

        public void ValidateClient(string Nombre, string Apellidos, string Email, string Telefono)
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                throw new ArgumentException("El Nombre es requerido");
            }
            if (string.IsNullOrWhiteSpace(Apellidos))
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
            TxtApellidos.Clear();
            TxtEmail.Clear();
            TxtTelefono.Clear();
        }

        public void reloadTable(List<Client> Client)
        {

            if (CRepo.GetAll() == null)
            {
                return;
            }
            else
            {
                Client = CRepo.GetAll().ToList();
                DgvClientes.DataSource = null;
                DgvClientes.DataSource = Client;
            }
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            if (CRepo.GetAll() == null)
            {
                return;
            }
            else
            {
                reloadTable(Clientes);
                CRepo.Added = true;

                DgvClientes.DataSource = CRepo.GetAll().ToList();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DgvClientes.Rows.Count == 0 || DgvClientes.CurrentCell.RowIndex < 0)
            {
                MessageBox.Show("Tabla sin datos o fila no seleccionada");
                return;
            }
            Client C = (Client)DgvClientes.CurrentRow.DataBoundItem;
            CRepo.Delete(C);
            Clientes.Remove(C);
            if (Clientes.Count == 0)
            {
                CRepo.Added = false;
            }

            reloadTable(Clientes);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (DgvClientes.Rows.Count == 0 || DgvClientes.CurrentCell.RowIndex < 0)
            {
                MessageBox.Show("Tabla sin datos o fila no seleccionada");
                return;
            }
            Client C = (Client)DgvClientes.CurrentRow.DataBoundItem;
            LoadInfo(C);
            toUpdate = true;
            cToUpdate = C;
        }

        public void LoadInfo(Client C)
        {
            TxtNombre.Text = C.Name;
            TxtApellidos.Text = C.Lastname;
            TxtEmail.Text = C.Email;
            TxtTelefono.Text = C.Phone;
        }
    }
}
