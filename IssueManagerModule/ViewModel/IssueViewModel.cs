using Microsoft.Practices.Unity;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IssueManagerModule.ViewModel
{
    public class IssueViewModel : ViewModelBase
    {
        Issue           _issue;
        
        public Issue    Model;

        ObservableCollection<Issue> _listIssue;
        
        ObservableCollection<User> _listUsers;

        ObservableCollection<IssueType> _listType;

        public ICommand ValidateIssueCommand { get; private set; }

        IAPI _api;

        IUnityContainer _container;

        public IssueViewModel(Issue issue, ObservableCollection<Issue> listIssue, IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;
            if (issue == null)
            {
                throw new ArgumentNullException("issue");
            }
            _issue = Model = issue;
            _listIssue = listIssue;
            _listUsers = _container.Resolve(typeof(object), "UserList") as ObservableCollection<User>;
            _listType = _container.Resolve(typeof(object), "TypeList") as ObservableCollection<IssueType>;
            ValidateIssueCommand = new DelegateCommand((o) => this.ValidateIssue());
        }

        public ObservableCollection<User> AllUsers
        {
            get
            {
                return this._listUsers;
            }
            set
            {
                if (this._listUsers == value) return;
                this._listUsers = value;
                this.OnPropertyChanged("AllUsers");
            }
        }

        public ObservableCollection<IssueType> AllTypes
        {
            get
            {
                return this._listType;
            }
            set
            {
                if (this._listType == value) return;
                this._listType = value;
                this.OnPropertyChanged("AllTypes");
            }
        }

        public int Id 
        {
            get 
            {
                return this.Model.id;
            }
            
            set
            {
                this.Model.id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public User Creator 
        {
            get 
            {
                return this.Model.creator;
            }
            set
            {
                if (this.Model.creator == value) return;
                this.Model.creator = value;
            }
        }
        
        public User Assignee 
        {
            get 
            {
                return this.Model.assignee;
            }
            set
            {
                if (this.Model.assignee == value) return;
                this.Model.assignee = value;
            }
        }

        public string Title
        {
            get
            {
                return this.Model.title;
            }
            set
            {
                this.Model.title = value;
            }
        }

        public string Description
        {
            get
            {
                return this.Model.description;
            }
            set
            {
                this.Model.description = value;
            }
        }

        public IssueType Type
        {
            get
            {
                return this.Model.type;
            }
            set
            {
                if (this.Model.type == value) return;
                this.Model.type = value;
            }
        }

        private void ValidateIssue()
        {
            this.OnPropertyChanged("Title");
            _api.Orm.UpdateObject<Issue>(@"update issue set title = @title where Id = @Id", Model);
            this.OnPropertyChanged("Description");
            _api.Orm.UpdateObject<Issue>(@"update issue set description = @description where Id = @Id", Model);
            this.OnPropertyChanged("Creator");
            _api.Orm.Update(@"update issue set id_creator = @creator where id = @Id", new { creator = this.Model.creator.id, Id = this.Model.id });
            this.OnPropertyChanged("Assignee");
            _api.Orm.Update(@"update issue set id_assignee = @assignee where id = @Id", new { assignee = this.Model.assignee.id, Id = this.Model.id });
            this.OnPropertyChanged("Type");
            _api.Orm.Update(@"update issue set id_type = @type where id = @Id", new { type = this.Model.type.id, Id = this.Model.id });
        }
    }
}
