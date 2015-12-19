using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManagerModule.ViewModel
{
    public class IssueViewModel : ViewModelBase
    {
        Issue           _issue;
        public Issue    Model;
        ObservableCollection<Issue> _listIssue;
        IAPI _api;

        public IssueViewModel(Issue issue, ObservableCollection<Issue> listUser, IAPI api)
        {
            _api = api;
            if (issue == null)
            {
                throw new ArgumentNullException("issue");
            }
            _issue = Model = issue;
            _listIssue = listUser;
        }
    }
}
