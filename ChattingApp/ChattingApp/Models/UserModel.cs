using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChattingApp.Enums;

using Prism.Mvvm;

namespace ChattingApp.Models
{
    public class UserModel: BindableBase
    {
        private string _displayName;
        private string _email;
        private string _linkFb;
        private string _firstName;
        private string _lastName;
        private string _localId;
        private string _photoUrl;
        private StateEnum _state;
        private GenderEnum _gender;
        private int _age;
        private GenderEnum _targetGender;

        public GenderEnum TargetGender
        {
            get
            {
                return _targetGender;
            }
            set
            {
                SetProperty(ref _targetGender, value);
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                SetProperty ( ref _displayName, value );
            }
        }

        public StateEnum State
        {
            get
            {
                return _state;
            }
            set
            {
                SetProperty ( ref _state, value );
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                SetProperty ( ref _email, value );
            }
        }
        public string LinkFb
        {
            get
            {
                return _linkFb;
            }
            set
            {
                SetProperty(ref _linkFb, value);
            }
        }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                SetProperty(ref _firstName, value);
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                SetProperty(ref _lastName, value);
            }
        }
        public string LocalId
        {
            get
            {
                return _localId;
            }
            set
            {
                SetProperty(ref _localId, value);
            }
        }
        public string PhotoUrl
        {
            get
            {
                return _photoUrl;
            }
            set
            {
                SetProperty(ref _photoUrl, value);
            }
        }

        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                SetProperty ( ref _age, value );
            }
        }

        public GenderEnum Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                SetProperty ( ref _gender, value );
            }
        }

        
    }
}
