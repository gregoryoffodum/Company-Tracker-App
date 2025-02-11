﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL.DTO;
using DAL;
using System.IO;

namespace PersonnelTracking
{
    public partial class FrmEmployee : Form
    {
        public FrmEmployee()
        {
            InitializeComponent();
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);

        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        EmployeeDTO dto = new EmployeeDTO();
        public EmployeeDetailDTO detail = new EmployeeDetailDTO();
        public bool isUpdate = false;
        string imagepath = "";
        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            dto = EmployeeBLL.GetAll();
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            if (isUpdate)
            {
                txtUserNo.Text = detail.UserNo.ToString();
                textPassword.Text=detail.Password;
                chAdmin.Checked=Convert.ToBoolean(detail.isAdmin);
                txtName.Text=detail.Name;
                txtSurname.Text=detail.Surname;
                txtSalary.Text=detail.Salary.ToString();
                cmbDepartment.SelectedValue=detail.DepartmentID;
                cmbPosition.SelectedValue=detail.PositionID;
                txtAddress.Text=detail.Address;
                dateTimePicker1.Value=Convert.ToDateTime(detail.Birthday);
                imagepath = Application.StartupPath+@"C:\Users\ooffodum001\Desktop\PersonnelTracking\PersonnelTracking\bin\Debug\\" + detail.ImagePath;
                textImagePath.Text = imagepath;
                pictureBox1.ImageLocation = imagepath;
                if (!UserStatic.isAdmin)
                {
                    txtUserNo.Enabled = false;
                    chAdmin.Enabled = false;
                    txtSalary.Enabled = false;
                    cmbDepartment.Enabled = false;
                    cmbPosition.Enabled = false;

                }
            }
        }
        bool combofull = false;
        private void cmbDepartment_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (combofull)
            {
                int departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == departmentID).ToList();
            }

        }
        string fileName = "";

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                textImagePath.Text = openFileDialog1.FileName;
                string Unique = Guid.NewGuid().ToString();
                fileName += Unique + openFileDialog1.SafeFileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("User No is Empty");
            else if (!EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text.Trim())))
                MessageBox.Show("This user no is used by another employee please change");
            else if (textPassword.Text.Trim() == "")
                MessageBox.Show("Password is Empty");
            else if (txtName.Text.Trim() == "")
                MessageBox.Show("Name is Empty");
            else if (txtSurname.Text.Trim() == "")
                MessageBox.Show("Surname is Empty");
            else if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Salary is Empty");
            else if (cmbDepartment.SelectedIndex == -1)
                MessageBox.Show("Select a department");
            else if (cmbPosition.SelectedIndex == -1)
                MessageBox.Show("Select a position");
            else
            {
                if (!isUpdate)
                {
                    if (!EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text.Trim())))
                        MessageBox.Show("This user no is used by another employee please change");
                    else
                    {
                        EMPLOYEE employee = new EMPLOYEE();
                        employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                        employee.Password = textPassword.Text;
                        employee.isAdmin = chAdmin.Checked;
                        employee.Name = txtName.Text;
                        employee.Surname = txtSurname.Text;
                        employee.Salary = Convert.ToInt32(txtSalary.Text);
                        employee.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                        employee.PositionID = Convert.ToInt32(cmbPosition.SelectedValue);
                        employee.Address = txtAddress.Text;
                        employee.Birthday = dateTimePicker1.Value;
                        employee.ImagePath = fileName;
                        EmployeeBLL.AddEmployee(employee);
                        File.Copy(textImagePath.Text, @"C:\Users\ooffodum001\Desktop\PersonnelTracking\PersonnelTracking\bin\Debug" + fileName);
                        MessageBox.Show("Employee was added");
                        txtUserNo.Clear();
                        textPassword.Clear();
                        chAdmin.Checked = false;
                        txtName.Clear();
                        txtSurname.Clear();
                        txtSalary.Clear();
                        txtAddress.Clear();
                        pictureBox1.Image = null;
                        combofull = false;
                        cmbDepartment.SelectedIndex = -1;
                        cmbPosition.DataSource = dto.Positions;
                        cmbPosition.SelectedIndex = -1;
                        combofull = true;
                        dateTimePicker1.Value = DateTime.Today;
                    }
                    
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure?", "Warning!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        EMPLOYEE employee = new EMPLOYEE();
                        if(textImagePath.Text != imagepath)
                        {
                            if (File.Exists(@"C:\Users\ooffodum001\Desktop\PersonnelTracking\PersonnelTracking\bin\Debug" + detail.ImagePath));
                                File.Delete(@"C:\Users\ooffodum001\Desktop\PersonnelTracking\PersonnelTracking\bin\Debug" + detail.ImagePath);

                            File.Copy(textImagePath.Text, @"C:\Users\ooffodum001\Desktop\PersonnelTracking\PersonnelTracking\bin\Debug" + fileName);
                            employee.ImagePath = fileName;
                        }
                        else
                        {
                            employee.ImagePath = detail.ImagePath;
                            employee.ID = detail.EmployeeID;
                            employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                            employee.Password = textPassword.Text;
                            employee.isAdmin = chAdmin.Checked;
                            employee.Name = txtName.Text;
                            employee.Surname = txtSurname.Text;
                            employee.Salary = Convert.ToInt32(txtSalary.Text);
                            employee.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                            employee.PositionID = Convert.ToInt32(cmbPosition.SelectedValue);
                            employee.Address = txtAddress.Text;
                            employee.Birthday = dateTimePicker1.Value;
                            employee.ImagePath = fileName;
                            EmployeeBLL.UpdateEmployee(employee);
                            MessageBox.Show("Employee was updated");
                            this.Close();
                               
                        }
                    }
                }


            }
        }

        bool isUnique = false;
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("User No is Empty");
            else
            {
                isUnique = EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text.Trim()));
                if (!isUnique)
                    MessageBox.Show("This user no is used by another employee please change");
                else
                    MessageBox.Show("This user no is usable");
            }
        }
    }
}
