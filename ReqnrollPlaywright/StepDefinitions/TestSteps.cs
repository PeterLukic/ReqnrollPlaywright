
namespace ReqnrollPlaywright.StepDefinitions
{
    [Binding]
    public class Test
    {
        [Given("Write in console {string}")]
        public void GivenWriteInConsole(string p0)
        {
            Console.WriteLine(p0);
        }

    }
}
