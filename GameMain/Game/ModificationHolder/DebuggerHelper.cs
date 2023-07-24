using Engine;
using Game;
using Color = Engine.Color;

namespace ModificationHolder {

    public class DebuggerHelper {
        
        private Action<String, Color, bool, bool> doMessageFunction;
        private string debug_str;

        public DebuggerHelper(Action<String, Color, bool, bool> doMessageFunction, ComponentMiner componentMiner) {
            this.doMessageFunction = doMessageFunction;
            Point3 point  = Datahandle.Coordbodyhandle(componentMiner.ComponentCreature.ComponentBody.Position);
            this.debug_str = "XYZ:(" + point.X.ToString() + "," + point.Y.ToString() + "," + point.Z.ToString() + "); ";
        }

        public DebuggerHelper AddToDebugger(string txt) {
            this.debug_str += txt + ";\n";
            return this;
        }

        public void Print() {
            this.doMessageFunction(this.debug_str, Color.Yellow, false, false);
        }

        public void Print(Color color) {

        }

    }

}