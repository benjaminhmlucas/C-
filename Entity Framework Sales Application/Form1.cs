using SalesApp.Data;
using SalesApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesApp {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        //loads Form Data using GetLists()
        private void Form1_Load(object sender, EventArgs e) {
            GetLists();
        }

        //Actually loads the form data
        private void GetLists() {
            //"using" wrapper makes context disposable
            using (var context = new SalesContext()) {
                //bind Sales Persons list
                salesPersonBindingSource.DataSource = context.People //use People context
                    .Where(e => e.Active) //where SalesPerson is Active
                    .OrderBy(e => e.FirstName) //orber by first name
                    .ThenBy(e => e.LastName) //then order by last name
                    .ToList(); //send to list
                //bind Sales Regions list
                salesRegionBindingSource.DataSource = context.Regions //use Region context
                    .Where(e => e.Active) //where SalesRegion is Active
                    .OrderBy(e => e.Name) //orber by name
                    .ToList(); //send to list
            }
        }

        //Create refresh button action
        private void RefreshSalesButton_Click(object sender, EventArgs e) {
            GetSales();
        }

        private void GetSales() {
            //get selected combobox id values
            var personId = (int)peopleComboBox.SelectedValue;
            var regionId = (int)RegionComboBox.SelectedValue;

            //"using" wrapper makes context disposable
            using (var context = new SalesContext()) {
                //bind Sale data to grid
                saleBindingSource.DataSource = context.Sales//use Sales context
                    .Where(o => o.PersonId == personId && //where region and slaeperson ID match
                                o.RegionId == regionId)
                    .OrderBy(o => o.Date)//order by date
                    .ToList();//send to list;
            }
        }

        //Displays a message box that tells the SalesPerson's TargetSales
        private void TargetButton_Click(object sender, EventArgs e) {
            var personId = (int)peopleComboBox.SelectedValue;
            using (var context = new SalesContext()) {
                var person = context.People.Single(p => p.Id == personId);
                //check for null on person object
                if (person != null) {
                    MessageBox.Show(string.Format("{0} has a sales target of {1:C}", person.FullName, person.SalesTarget));
                    //if null, tell user
                } else {
                    MessageBox.Show("Person Object is Null!");
                }
            }
        }

        private void NewSalesButton_Click(object sender, EventArgs e) {

            //grab person and region ids from comboboxes
            var personId = (int)peopleComboBox.SelectedValue;
            var regionId = (int)RegionComboBox.SelectedValue;

            //create new sale object
            var sale = new Sale {
                Amount = newAmountNumericUpDown.Value,
                Date = newDateDateTimePicker.Value,
                PersonId = personId,
                RegionId = regionId
            };

            //Add new sale object to context and save changes
            using (var context = new SalesContext()) {
                context.Sales.Add(sale);
                var result = context.SaveChanges();

                //notify user
                MessageBox.Show(string.Format("{0} sale created", result));
                GetSales();//refresh table
            }
        }

        private void SalesDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 1) {

                var salesId = (int)salesDataGridView.Rows[e.RowIndex].Cells[0].Value;
                var amount = (decimal)salesDataGridView.Rows[e.RowIndex].Cells[1].Value;

                using (var context = new SalesContext()) {
                    var sale = context.Sales.Single(p => p.Id == salesId);
                    //check for null on person object
                    if (sale != null) {
                        sale.Amount = amount;
                        var result = context.SaveChanges();
                        //notify user
                        MessageBox.Show(string.Format("{0} sale updated", result));
                        GetSales();//refresh table                        
                    } 
                }
            }
        }

        private void SalesDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {

            //check if user wants to delete
            if (MessageBox.Show("Are you sure you want to delete this sale?","Delete",
                MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) {
                e.Cancel = true;
                return;
            }

            var salesId = (int)e.Row.Cells[0].Value;

            using (var context = new SalesContext()) {
                var sale = context.Sales.Single(p => p.Id == salesId);
                //check for null on person object
                if (sale != null) {
                    context.Sales.Remove(sale);
                    var result = context.SaveChanges();
                    //notify user
                    MessageBox.Show(string.Format("{0} sale deleted", result));
                }
            }
        }
    }
}
