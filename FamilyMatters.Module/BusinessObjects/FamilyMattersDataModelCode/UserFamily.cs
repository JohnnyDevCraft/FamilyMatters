using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace FamilyMatters.Module.BusinessObjects
{

    public partial class UserFamily
    {
        public UserFamily(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
