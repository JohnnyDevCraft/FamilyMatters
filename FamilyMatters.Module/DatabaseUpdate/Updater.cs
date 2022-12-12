using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using FamilyMatters.Module.BusinessObjects;

namespace FamilyMatters.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();

        var adminRole = ObjectSpace.GetObjectsQuery<Role>().SingleOrDefault(r => r.Name == "Admin Role");
        if (adminRole == null)
        {
            adminRole = ObjectSpace.CreateObject<Role>();
            adminRole.Name = "Admin Role";
            adminRole.IsAdministrative= true;
        }

        var adminUser = ObjectSpace.GetObjectsQuery<User>().SingleOrDefault(u => u.UserName == "admin@johnnydevcraft.com");
        if (adminUser == null)
        {
            adminUser= ObjectSpace.CreateObject<User>();
            adminUser.FirstName = "System";
            adminUser.LastName = "Admin";
            adminUser.Email= "admin@johnnydevcraft.com";
            adminUser.SetPassword("Snafu201");
        }

        adminUser.Roles.Add(adminRole);

        CreateFamilyAdminRole();

       
        ObjectSpace.CommitChanges(); //This line persists created object(s).

    }
    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
        //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
        //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
    private PermissionPolicyRole CreateFamilyAdminRole() {
        Role defaultRole = ObjectSpace.FirstOrDefault<Role>(role => role.Name == "Default");
        if(defaultRole == null) {
            defaultRole = ObjectSpace.CreateObject<Role>();
            defaultRole.Name = "Default";

			defaultRole.AddObjectPermissionFromLambda<User>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
			defaultRole.AddMemberPermissionFromLambda<User>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
			defaultRole.AddMemberPermissionFromLambda<User>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
			defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

            defaultRole.AddObjectPermissionFromLambda<Family>(SecurityOperations.ReadWriteAccess, f => f.Oid == (int)CurrentFamilyIdOperator.CurrentFamilyId(ObjectSpace), SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
}
