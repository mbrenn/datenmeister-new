// Merge.ts
using BurnSystems.Make.BuildAgent;

await CopyCSS();

return 0;

// Compile.ts

async Task CopyCSS()
{
    Console.WriteLine("Copying CSS");
    
    System.IO.File.Copy(
        "../DatenMeister.WebServer/wwwroot/css/datenmeister-web.min.css", 
        "Assets/Css/datenmeister-web.min.css",
        true);       
}

