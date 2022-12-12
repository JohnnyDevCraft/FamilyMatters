﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace FamilyMatters.Module.BusinessObjects
{

    public partial class User : DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser
    {
        string fFirstName;
        public string FirstName
        {
            get { return fFirstName; }
            set { SetPropertyValue<string>(nameof(FirstName), ref fFirstName, value); }
        }
        string fLastName;
        public string LastName
        {
            get { return fLastName; }
            set { SetPropertyValue<string>(nameof(LastName), ref fLastName, value); }
        }
        DateTime fDateOfBirth;
        public DateTime DateOfBirth
        {
            get { return fDateOfBirth; }
            set { SetPropertyValue<DateTime>(nameof(DateOfBirth), ref fDateOfBirth, value); }
        }
        Family fCurrentFamily;
        public Family CurrentFamily
        {
            get { return fCurrentFamily; }
            set { SetPropertyValue<Family>(nameof(CurrentFamily), ref fCurrentFamily, value); }
        }
        [PersistentAlias("Concat([FirstName], ' ', [LastName])")]
        public string FullName
        {
            get { return (string)(EvaluateAlias(nameof(FullName))); }
        }
        [Association(@"UserFamilyReferencesUser")]
        public XPCollection<UserFamily> UserFamilies { get { return GetCollection<UserFamily>(nameof(UserFamilies)); } }
    }

}