﻿using Microsoft.Practices.Unity;
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

    //AddIssueCommand
    public class IssueManagerModuleViewModel : ViewModelBase, IModuleMainViewModel
    {
        IAPI _api;

        IUnityContainer _container;

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

        public ICommand AddIssueCommand { get; private set; }

        public ObservableCollection<IssueViewModel> AllIssues { get; private set; }

        private ObservableCollection<Issue>         _listAllIssues;

        private ObservableCollection<User>          _listAllUsers;

        private ObservableCollection<IssueType>     _listAllType;


        public IssueManagerModuleViewModel(IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;

            _listAllUsers = this.buildUserList();
            _container.RegisterInstance(typeof(object), "UserList", _listAllUsers);
            _listAllType = this.buildTypeList();
            _container.RegisterInstance(typeof(object), "TypeList", _listAllType);
            _listAllIssues = this.buildIssuesList();

            this.AddIssueCommand = new DelegateCommand((o) => this.AddIssue());

            this.AllIssues = new ObservableCollection<IssueViewModel>();
            foreach (Issue issue in this._listAllIssues)
            {
                this.AllIssues.Add(new IssueViewModel(issue, _listAllIssues, _api, _container));
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

        private ObservableCollection<User> buildUserList()
        {
            var res = _api.Orm.ObjectQuery<User>("select * from user");
            if (res == null)
                return new ObservableCollection<User>();
            return new ObservableCollection<User>(res);
        }

        private ObservableCollection<IssueType> buildTypeList()
        {
            var res = _api.Orm.ObjectQuery<IssueType>("select * from issue_type");
            if (res == null)
                return new ObservableCollection<IssueType>();
            return new ObservableCollection<IssueType>(res);
        }

        private ObservableCollection<Issue> buildIssuesList()
        {
            IEnumerable<dynamic> issueBrutList = _api.Orm.Query("select * from issue");
            if (issueBrutList == null)
            {
                issueBrutList = new Collection<dynamic>();
            }
            ObservableCollection<Issue> res = new ObservableCollection<Issue>();

            foreach (dynamic issueBrut in issueBrutList)
            {
                Issue newElem = new Issue();

                newElem.id = (int)issueBrut.id;
                newElem.title = issueBrut.title;
                newElem.description = issueBrut.description;
                newElem.creator = _listAllUsers.FirstOrDefault<User>(user => user.id == issueBrut.id_creator);
                newElem.assignee = _listAllUsers.FirstOrDefault<User>(user => user.id == issueBrut.id_assignee);
                newElem.type = _listAllType.FirstOrDefault<IssueType>(type => type.id == issueBrut.id_type);
                res.Add(newElem);
            }
            return res;
        }

        private void AddIssue()
        {
            Issue issue = new Issue();
            _api.Orm.Insert("insert into issue(id_creator, id_assignee, id_type) values (@id_creator, @id_assignee, @id_type)", new { id_creator = this._listAllUsers.First().id, id_assignee = this._listAllUsers.First().id, id_type = this._listAllType.First().id });
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from issue");
            if (res != null)
            {
                issue.id = (int)res.First().maxId;
                issue.assignee = this._listAllUsers.First();
                issue.creator = this._listAllUsers.First();
                issue.type = this._listAllType.First();
                IssueViewModel vm = new IssueViewModel(issue, this._listAllIssues, _api, _container);
                this.AllIssues.Add(vm);
                this.CurrentIssue = vm;
            }
            else
            {
            }
        }

        public void Refresh(){

        }
    }
}
