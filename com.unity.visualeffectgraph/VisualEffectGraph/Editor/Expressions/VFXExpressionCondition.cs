using System;
using UnityEngine.VFX;

namespace UnityEditor.VFX
{
    class VFXExpressionCondition : VFXExpression
    {
        public VFXExpressionCondition()
            : this(VFXCondition.Equal, VFXValue.Constant(0.0f), VFXValue.Constant(0.0f))
        {}

        public VFXExpressionCondition(VFXCondition cond, VFXExpression left, VFXExpression right) : base(VFXExpression.Flags.None, new VFXExpression[] { left, right })
        {
            condition = cond;
        }

        public override VFXExpressionOp operation
        {
            get
            {
                return VFXExpressionOp.kVFXCondition;
            }
        }

        sealed protected override VFXExpression Evaluate(VFXExpression[] constParents)
        {
            bool res = false;
            float left = constParents[0].Get<float>();
            float right = constParents[1].Get<float>();

            switch (condition)
            {
                case VFXCondition.Equal:            res = left == right;    break;
                case VFXCondition.NotEqual:         res = left != right;    break;
                case VFXCondition.Less:             res = left < right;     break;
                case VFXCondition.LessOrEqual:      res = left <= right;    break;
                case VFXCondition.Greater:          res = left > right;     break;
                case VFXCondition.GreaterOrEqual:   res = left >= right;    break;
            }

            return VFXValue.Constant<bool>(res);
        }

        public override string GetCodeString(string[] parents)
        {
            string comparator = null;
            switch (condition)
            {
                case VFXCondition.Equal:            comparator = "==";  break;
                case VFXCondition.NotEqual:         comparator = "!=";  break;
                case VFXCondition.Less:             comparator = "<";   break;
                case VFXCondition.LessOrEqual:      comparator = "<=";  break;
                case VFXCondition.Greater:          comparator = ">";   break;
                case VFXCondition.GreaterOrEqual:   comparator = ">=";  break;
            }

            return string.Format("({0} {1} {2})", parents[0], comparator, parents[1]);
        }

        protected override VFXExpression Reduce(VFXExpression[] reducedParents)
        {
            var newExpression = (VFXExpressionCondition)base.Reduce(reducedParents);
            newExpression.condition = condition;
            return newExpression;
        }

        protected override int[] additionnalOperands { get { return new int[] { (int)condition }; } }
        private VFXCondition condition;
    }
}
