using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IssueManagerModule.ViewModel
{
    public class IssueManagerModuleViewModel : ViewModelBase, IModuleMainViewModel
    {
        IAPI _api;

        private IssueViewModel _currentIssue;
        public IssueViewModel CurrentIssue
        {
            get
            {
                return _currentIssue;
            }

            set
            {
                _currentIssue = value;
                this.OnPropertyChanged("CurrentIssue");
            }
        }

        public ObservableCollection<IssueViewModel> AllIssues { get; private set; }
        private ObservableCollection<Issue>          _listAllIssues;


        public IssueManagerModuleViewModel(IAPI api)
        {
            _api = api;

            // Get list de la BD
            IEnumerable<Issue> listIssue = _api.Orm.ObjectQuery<Issue>("select * from issue");
            if (listIssue == null)
            {
                listIssue = new Collection<Issue>();                                
            }

            _listAllIssues = new ObservableCollection<Issue>(listIssue);
            this.AllIssues = new ObservableCollection<IssueViewModel>();
            foreach (Issue issue in listIssue)
            {
                this.AllIssues.Add(new IssueViewModel(issue, _listAllIssues, _api));
            }
            _currentIssue = AllIssues.Count > 0 ? AllIssues[0] : null;

            this.AllIssues.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null && e.OldItems.Contains(this.CurrentIssue))
                {
                    this.CurrentIssue = null;
                }
            };
        }
    }
}
