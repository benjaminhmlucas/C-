using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Models {

    //This class creates an observable list for Windows forms to track the DB object with
    //This is only needed in Windows Forms applications.
    class ObservableListSource<T> : ObservableCollection<T>, IListSource where T : BaseModel{
        private IBindingList _bindingList;
        bool IListSource.ContainsListCollection { get { return false; } }
        IList IListSource.GetList() {
            return _bindingList ?? (_bindingList = this.ToBindingList());
        }
            
    }
}
