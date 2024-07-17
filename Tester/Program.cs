namespace GwentCompiler;
class Program
{
    // Para probar el código puede escribir en el archivo Input dentro de la carpeta TestFile de este repositorio, solo cambie el valor de path
    // por la ruta real del repositorio en su equipo. 

    // Falta muuucho trabajo en la detección y reporte de errores y el manejo de valores null, así que las excepciones de cualquier tipo por ahora son normales. Sin embargo, 
    // creo que hay un buen "esqueleto" de lo que será el compilador por ahora.

    // Agregué nuevos elementos al lenguaje como "!" (no lógico), "!=" (desigualdad), if y else, y una instrucción print que necesité para probar
    // el avance en los statementes y que usted también puede probar.

    // Dentro de los archivos de código hay más aclaraciones como estas y algunas dudas, cualquier sugerencia es bienvenida.

    static void Main()
    {
        string path = GetFileContent("/home/lianny/$/home/lianny/Proyecto/ProyectoGwent++/Compiler/TestFile/Input");

        Lexer lexer = new Lexer();
        List<Token> tokens = lexer.Tokenize(path);

        Parser parser = new Parser(tokens);
        List<IStatement> program = parser.Parse();


        Interpreter interpreter = new Interpreter();
        interpreter.Interpret(program);
    }

    private static string GetFileContent(string root) // método que devuleve el contenido del archivo
    {
        StreamReader reader = new StreamReader(root); // leemos el contenido del archivo
        string FileContent = reader.ReadToEnd();
        reader.Close();

        return FileContent;
    }
}