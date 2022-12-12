using System;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using FamilyMatters.Module.BusinessObjects;

namespace DevExpress.ExpressApp.SystemModule
{
    public class CurrentFamilyIdOperator : ICustomFunctionOperatorConvertibleToExpression
    {
        public const string OperatorName = "CurrentFamilyId";
        private static readonly CurrentFamilyIdOperator instance = new CurrentFamilyIdOperator();
        public static void Register()
        {
            CustomFunctionOperatorHelper.Register(instance);
        }
        public static object CurrentFamilyId(IObjectSpace objectSpace)
        {
            return instance.Evaluate(objectSpace);
        }
        public object Evaluate(params object[] operands)
        {
            var Os = (IObjectSpace)operands[0];
            var cuid = SecuritySystem.CurrentUserId;
            var user = Os.GetObjectsQuery<User>()
                .SingleOrDefault(u => u.Oid == (Guid)cuid);

            return user.CurrentFamily.Oid;
        }
        public string Name
        {
            get { return OperatorName; }
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(object);
        }
        Expression ICustomFunctionOperatorConvertibleToExpression.Convert(ICriteriaToExpressionConverter converter, params Expression[] operands)
        {
            var Os = (IObjectSpace)operands[0];
            var cuid = SecuritySystem.CurrentUserId;
            var user = Os.GetObjectsQuery<User>()
                .SingleOrDefault(u => u.Oid == (Guid)cuid);

            return Expression.Constant(user.CurrentFamily.Oid);
        }
    }
}