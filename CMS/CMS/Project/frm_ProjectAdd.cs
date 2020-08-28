﻿using System;
using System.Data;
using System.Windows.Forms;
using DataControlsLib.DataModels;

namespace CMS
{
    public partial class frm_ProjectAdd : Form
    {
        /// <summary>
        /// Constructor - receives parameter ds_Project that contains the DataSources for ComboBox options, passes it 
        /// on to a method that generates a new project number and sets the ComboBox DataSources
        /// </summary>
        /// <param name="ds_Project"></param>
        public frm_ProjectAdd()
        {
            InitializeComponent();
            set_ProjectAdd();
        }

        DataSet ds_Project;
        public string pNumber;

        /// <summary>
        /// Uses methods in Project class to query SQL tblProjects for largest integer component of ProjectNumber 
        /// and increment it by 1 with a 'P' and leading zero prefix.
        /// </summary>
        /// <returns></returns>
        public string getNewProjectNumber()
        {
            Project Projects = new Project();
            int pNumInt = Projects.getLastProjectNumber() + 1;
            pNumber = Projects.getNewProjectNumber(pNumInt);
            return pNumber;
        }

        /// <summary>
        /// Method to fill class member DataSet (ds_Project) and assign the tables it contains to form controls
        /// </summary>
        private void set_ProjectAdd()
        {
            //instantiate new Project type object that contains project methods
            Project Projects = new Project();

            ds_Project = Projects.getProjectsDataSet();

            //bind DataSource to comboboxes
            cb_DATRAG.DataSource = ds_Project.Tables["tlkRAG"];
            cb_DATRAG.ValueMember = "ragID";
            cb_DATRAG.DisplayMember = "ragDescription";
            cb_DATRAG.SelectedIndex = -1;
            cb_pStage.DataSource = ds_Project.Tables["tlkStage"];
            cb_pStage.ValueMember = "StageID";
            cb_pStage.DisplayMember = "pStageDescription";
            cb_pStage.SelectedIndex = -1;
            cb_pClassification.DataSource = ds_Project.Tables["tlkClassification"];
            cb_pClassification.ValueMember = "classificationID";
            cb_pClassification.DisplayMember = "classificationDescription";
            cb_pClassification.SelectedIndex = -1;
            cb_LeadApplicant.DataSource = ds_Project.Tables["tlkLeadApplicant"];
            cb_LeadApplicant.ValueMember = "UserNumber";
            cb_LeadApplicant.DisplayMember = "FullName";
            cb_LeadApplicant.SelectedIndex = -1;
            cb_PI.DataSource = ds_Project.Tables["tlkPI"];
            cb_PI.ValueMember = "UserNumber";
            cb_PI.DisplayMember = "FullName";
            cb_PI.SelectedIndex = -1;           
            cb_Faculty.DataSource = ds_Project.Tables["tlkFaculty"];
            cb_Faculty.ValueMember = "facultyID";
            cb_Faculty.DisplayMember = "facultyDescription";
            cb_Faculty.SelectedIndex = -1;
        }

        /// <summary>
        /// Method to create a new project record using values entered in form.
        /// Assigns control values to ProjectModel class variables, 
        /// checks dates are dates and passes them as parameters to 
        /// the insertProject(...) method of the Projects class.
        /// </summary>
        private void insertNewProject()
        {
            //generate new pNumber and put it into class variable, can be used within 
            //this method/class but also to feed back to parent form.
            pNumber = getNewProjectNumber();
            ProjectModel mdl_Project = new ProjectModel();

            //populate ProjectModel class variables with values held in form controls
            mdl_Project.ProjectNumber      = pNumber;
            mdl_Project.ProjectName        = tb_pNameValue.Text;
            mdl_Project.DSPT               = chkb_DSPT.Checked;
            mdl_Project.ISO27001           = chkb_ISO27001.Checked;
            mdl_Project.Azure              = chkb_Azure.Checked;
            mdl_Project.IRC                = chkb_IRC.Checked;
            mdl_Project.SEED               = chkb_SEED.Checked;

            if (cb_pStage.SelectedIndex > -1)
                mdl_Project.Stage = int.Parse(cb_pStage.SelectedValue.ToString());
            if (cb_pClassification.SelectedIndex > -1)
                mdl_Project.Classification = int.Parse(cb_pClassification.SelectedValue.ToString());
            if (cb_DATRAG.SelectedIndex > -1)
                mdl_Project.DATRAG = int.Parse(cb_DATRAG.SelectedValue.ToString());
            if (cb_LeadApplicant.SelectedIndex > -1)
                mdl_Project.LeadApplicant = int.Parse(cb_LeadApplicant.SelectedValue.ToString());
            if (cb_PI.SelectedIndex > -1)
                mdl_Project.PI = int.Parse(cb_PI.SelectedValue.ToString());
            if (cb_Faculty.SelectedIndex > -1)
                mdl_Project.Faculty = int.Parse(cb_Faculty.SelectedValue.ToString());

            //dates are fuckey
            bool dateCheck = true;
            if (dateCheck == true & mtb_ProjectedStartDateValue.Text != "" & mtb_ProjectedStartDateValue.Text != "  /  /")
            {
                try
                {
                    mdl_Project.ProjectedStartDate = Convert.ToDateTime(mtb_ProjectedStartDateValue.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter valid Projected Start Date");
                    dateCheck = false;
                }
            }
            if (dateCheck == true & mtb_ProjectedEndDateValue.Text != "" & mtb_ProjectedEndDateValue.Text != "  /  /")
            {
                try
                {
                    mdl_Project.ProjectedEndDate = Convert.ToDateTime(mtb_ProjectedEndDateValue.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter valid Projected End Date");
                    dateCheck = false;
                }
            }
            if (dateCheck == true & mtb_pStartDateValue.Text != "" & mtb_pStartDateValue.Text != "  /  /")
            {
                try
                {
                    mdl_Project.StartDate = Convert.ToDateTime(mtb_pStartDateValue.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter valid Start Date");
                    dateCheck = false;
                }
            }
            if (dateCheck == true & mtb_pEndDateValue.Text != "" & mtb_pEndDateValue.Text != "  /  /")
            {
                try
                {
                    mdl_Project.EndDate = Convert.ToDateTime(mtb_pEndDateValue.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter valid End Date");
                    dateCheck = false;
                }
            }

            //instantiate new Project type object that contains methods to update db
            Project Projects = new Project();

            //Check required fields have an entry
            if (Projects.requiredFields(mdl_Project) == false)
            {
                return;
            }

            if (dateCheck == true)
            {
                //insert new record
                Projects.insertProject(mdl_Project);
                this.Close();
            }
        }

        /// <summary>
        /// Prevent the cursor from being positioned in the middle of a masked textbox when 
        /// the user clicks in it. Needs to be called by the OnClick event of the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enter_TextBox(object sender, EventArgs e)
        {
            MaskedTextBox textBox = sender as MaskedTextBox;
            if (textBox.Text == "  /  /")
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    textBox.Select(0, 0);
                });
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            insertNewProject();
        }
    }
}