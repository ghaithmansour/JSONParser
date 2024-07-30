// See https://aka.ms/new-console-template for more information
using JSONParser;


///Use this section to provide one file
#region OneTestFile
//string filePath = "C:\\Ghaith\\Coding challenges\\JSON Parser\\JSONParser\\TestFiles\\step3\\invalid.json"; // Replace with your file path
//Driver.Main(filePath);

#endregion

/// Use this section to provide a folder of test files
#region TestFolder

string rootPath = @"C:\Ghaith\Coding challenges\JSON Parser\JSONParser\TestFiles";
string[] files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);

foreach (string file in files)
{
    try
    {
        Driver.Main(file);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error at: {0}\n {1}", file, ex.Message);
        continue;
    }
}
#endregion


///Use this line to use provided hardecoded test case in code in Main function
#region HardcodedTest
// Driver.Main(null);
#endregion
