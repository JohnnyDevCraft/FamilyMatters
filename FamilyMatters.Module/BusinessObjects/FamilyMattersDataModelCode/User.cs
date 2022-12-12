using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Validation;

namespace FamilyMatters.Module.BusinessObjects
{
    [DefaultProperty(nameof(FullName))]

    public partial class User
    {
        public const string EmailRegularExpression = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";

        public User(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override IEnumerable<ISecurityRole> GetSecurityRoles()
        {
            var roles = Session.Query<UserFamily>()
                .Where(uf => uf.User.Oid == Oid && uf.Family.Oid == CurrentFamily.Oid)
                .Select(uf => uf.Role)
                .ToList();

            return roles;
        }

        [NonPersistent]
        [RuleRegularExpression(DefaultContexts.Save, User.EmailRegularExpression, CustomMessageTemplate = "Invalid email format!")]
        public string Email
        {
            get => UserName;
            set => UserName = value;
        }
    }

}
