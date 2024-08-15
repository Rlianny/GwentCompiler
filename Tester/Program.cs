namespace GwentCompiler;
static class Program
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
        string path = GetFileContent("/Lianny/Proyectos/Gwent++/Compiler/TestFile/Input.txt");

        Effect effect = new Effect("Damage", new List<Parameter>(){new Parameter("Amount", ValueType.Number)});
        Effect.AllEffects.Add(effect.Name, effect);

        Effect effect1 = new Effect("ReturnToDeck", null);
        Effect.AllEffects.Add(effect1.Name, effect1);

        Compile(path);
    }

    private static string GetFileContent(string root) // método que devuleve el contenido del archivo
    {
        StreamReader reader = new StreamReader(root); // leemos el contenido del archivo
        string FileContent = reader.ReadToEnd();
        reader.Close();
        return FileContent;
    }

    private static void CompilationError()
    {
        System.Console.WriteLine("Compilation failed, fix all errorr and try again");
    }

    private static void Compile(string path)
    {
        Lexer lexer = new Lexer();
        List<Token> tokens = lexer.Tokenize(path);
        if(lexer.hadError)
        {
            CompilationError();
            return;
        }

        Parser parser = new Parser(tokens);
        //List<IStatement> program = parser.Parse();
        List<IProgramNode?> program = parser.Program();
        if(parser.hadError)
        {
            CompilationError();
            return;
        }

        
        // Interpreter interpreter = new Interpreter();
        // interpreter.Interpret(program);
        // if (interpreter.hadError)
        // {
        //     CompilationError();
        //     return;
        // }

        ObjectCompiler objectCompiller = new ObjectCompiler(program);
        List<CompiledObject> compiledObjects = objectCompiller.CompileObjects();
        foreach(var obj in compiledObjects)
        {
            if(obj is CompiledCard compiledCard)
            {
                System.Console.WriteLine(compiledCard.ToString());
            }
            if (obj is CompiledEffect compiledEffect)
            {
                System.Console.WriteLine(compiledEffect.ToString());
            }
        }
        
        
    }
}


// pendientes:

// el evaluador de cardComponent señala el error en la posicion errónea, guardar como token el :
//