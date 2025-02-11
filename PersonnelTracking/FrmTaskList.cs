﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DAL.DTO;
using BLL;

namespace PersonnelTracking
{
    public partial class FrmTaskList : Form
    {
        public FrmTaskList()
        {
            InitializeComponent();
        }

        TaskDTO dto = new TaskDTO();
        private bool combofull = false;
        void FillAllData()
        {
            dto = TaskBLL.GetAll();
            if(!UserStatic.isAdmin)
                dto.Tasks = dto.Tasks.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();
            dataGridView1.DataSource = dto.Tasks;
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            cmbTaskState.DataSource = dto.TaskStates;
            cmbTaskState.DisplayMember = "StateName";
            cmbTaskState.ValueMember = "ID";
            cmbTaskState.SelectedIndex = -1;
        }

        TaskDetailDTO detail = new TaskDetailDTO();
        private void FrmTaskList_Load(object sender, EventArgs e)
        {
            FillAllData();

            dataGridView1.Columns[0].HeaderText = "Task Title";
            dataGridView1.Columns[1].HeaderText = "User no";
            dataGridView1.Columns[2].HeaderText = "Name";
            dataGridView1.Columns[3].HeaderText = "Surname";
            dataGridView1.Columns[4].HeaderText = "Start Date";
            dataGridView1.Columns[5].HeaderText = "Delivery Date";
            dataGridView1.Columns[6].HeaderText = "Task State";
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;

            if (!UserStatic.isAdmin)
            {
                btnNew.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnClose.Location = btnUpdate.Location;
                btnApprove.Location = btnDelete.Location;
                pnlForAdmin.Hide();
                btnApprove.Text = "Delivery";

            }

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbDelivery_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmTask frm = new FrmTask();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.TaskID == 0)
                MessageBox.Show("Please select a task on table");
            FrmTask frm = new FrmTask();
            this.Hide();
            frm.isUpdate = true;
            frm.detail = detail;
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<TaskDetailDTO> list = dto.Tasks;
            if (txtUserNo.Text.Trim() != "")
            {
                list = list.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            }
            if (txtName.Text.Trim() != "")
            {
                list = list.Where(x => x.Name.Contains(txtName.Text)).ToList();
            }
            if (txtSurname.Text.Trim() != "")
            {
                list = list.Where(x => x.Name.Contains(txtSurname.Text)).ToList();
            }
            if (cmbDepartment.SelectedIndex != -1)
            {
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
            if (cmbPosition.SelectedIndex != -1)
            {
                list = list.Where(x => x.PositionID == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            }
            if (rbStart.Checked)
                list = list.Where(x => x.TaskStartDate > Convert.ToDateTime(dpStart.Value)
                && x.TaskStartDate < Convert.ToDateTime(dpEnd.Value)).ToList();
            if (rbDelivery.Checked)
                list = list.Where(x => x.TaskDeliveryDate > Convert.ToDateTime(dpStart.Value)
                && x.TaskDeliveryDate < Convert.ToDateTime(dpEnd.Value)).ToList();
            if (cmbTaskState.SelectedIndex != -1)
                list = list.Where(x => x.TaskStateID == Convert.ToInt32(cmbTaskState.SelectedValue)).ToList();
            dataGridView1.DataSource = list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanFilters();
        }

        private void CleanFilters()
        {
            txtUserNo.Clear();
            txtName.Clear();
            txtSurname.Clear();
            combofull = false;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            rbDelivery.Checked = false;
            rbStart.Checked = false;
            cmbTaskState.SelectedIndex = -1;
            dataGridView1.DataSource = dto.Tasks;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.Title = (dataGridView1.Rows[e.RowIndex].Cells[0]).Value.ToString();
            detail.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.Name = (dataGridView1.Rows[e.RowIndex].Cells[2]).Value.ToString();
            detail.Surname = (dataGridView1.Rows[e.RowIndex].Cells[3]).Value.ToString();
            detail.TaskStartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            detail.TaskDeliveryDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            detail.TaskID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
            detail.TaskStateID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
            detail.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[13].Value);
            detail.Content = (dataGridView1.Rows[e.RowIndex].Cells[14]).Value.ToString();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this task?", "Warning!!", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                TaskBLL.DeleteTask(detail.TaskID);
                MessageBox.Show("Task was deleted");
                FillAllData();
                CleanFilters();
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (UserStatic.isAdmin && detail.TaskStateID == TaskStates.OnEmployee && detail.EmployeeID != UserStatic.EmployeeID)
                MessageBox.Show("Before approving a task, employee has to deliver task");
            else if (UserStatic.isAdmin && detail.TaskStateID == TaskStates.Approved)
                MessageBox.Show("This task is already approved");
            else if (!UserStatic.isAdmin && detail.TaskStateID == TaskStates.Delivered)
                MessageBox.Show("This task is already delivered");
            else if (!UserStatic.isAdmin && detail.TaskStateID == TaskStates.Approved)
                MessageBox.Show("This task is already approved");
            else
            {
                TaskBLL.ApproveTask(detail.TaskID, UserStatic.isAdmin);
                MessageBox.Show("Task was updated");
                FillAllData();
                CleanFilters();
            }

        }

        private void txtExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel.ExcelExport(dataGridView1);
        }
    }
}
