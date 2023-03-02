
namespace Game
{
  public class Stack
  {
    private int top = -1;
    private int maxSize = 64;
    private object[] array = new object[64];

    public void push(object val)
    {
      if (this.top == this.maxSize - 1)
        return;
      this.array[++this.top] = val;
    }

    public object pop() => this.top == -1 ? (object) null : this.array[this.top--];

    public object getTop() => this.top == -1 ? (object) null : this.array[this.top];

    public bool isEmpty() => this.top == -1;
  }
}